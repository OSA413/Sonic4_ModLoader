use std::{collections::HashSet, fs::{self, File}, io::Write, ops::Deref, path::{self, Path}};
use async_channel::Sender;
use futures_util::StreamExt;
use adw::subclass::prelude::*;
use gtk::{gio::{self, prelude::ActionMapExtManual}, glib::{self, clone}, prelude::{CheckButtonExt, EditableExt, WidgetExt}};

use crate::{arg_handler::{ArgHandler, InitialArgs}, tokio_runtime};

use common::Launcher;

async fn download_mod(url: String, progress_bar: Sender<f64>, progress_bar_text: Sender<String>, file_path: Sender<String>) {
    progress_bar_text.send_blocking("Connecting to the server...".to_string()).unwrap();

    let response = reqwest::Client::new()
        .get(url)
        .send()
        .await
        .unwrap();
    let total_size = response.content_length().unwrap();
    let file_name = response.url().path_segments().unwrap().last().unwrap().to_owned();

    progress_bar_text.send_blocking(format!("Downloading {file_name}...")).unwrap();
    progress_bar.send_blocking(0.0).unwrap();

    match fs::create_dir_all("downloaded_mods") {
        Ok(()) => println!("Alan please write description"),
        Err(e) => println!("Alan please write error: {e}"),
    };
    let to = Path::new("downloaded_mods").join(&file_name);
    let mut file = File::create(&to).unwrap();
    let mut downloaded = 0;
    let mut stream = response.bytes_stream();

    while let Some(item) = stream.next().await {
        let chunk = item.unwrap();
        file.write_all(&chunk).unwrap();
        downloaded += chunk.len();
        progress_bar.send_blocking(downloaded as f64 / total_size as f64).unwrap();
    }

    progress_bar_text.send_blocking(format!("Finished downloading {file_name}!")).unwrap();
    progress_bar.send_blocking(1.0).unwrap();
    file_path.send_blocking(path::absolute(to).unwrap().display().to_string()).unwrap();
}

mod imp {
    use super::*;

    #[derive(Debug, Default, gtk::CompositeTemplate)]
    #[template(resource = "/Sonic4ModLoader/OneClickModInstaller/window.ui")]
    pub struct OneClickModInstallerWindow {
        #[template_child]
        pub logo: TemplateChild<gtk::Picture>,
        #[template_child]
        pub stack: TemplateChild<adw::ViewStack>,

        // Mod Installation
        #[template_child]
        pub mod_path_entry: TemplateChild<gtk::Entry>,
        #[template_child]
        pub mod_path_button: TemplateChild<gtk::Button>,
        #[template_child]
        pub exit_after_install_checkbutton: TemplateChild<gtk::CheckButton>,
        #[template_child]
        pub install_button: TemplateChild<gtk::Button>,
        #[template_child]
        pub progress_bar: TemplateChild<gtk::ProgressBar>,
    }

    #[glib::object_subclass]
    impl ObjectSubclass for OneClickModInstallerWindow {
        const NAME: &'static str = "OneClickModInstaller";
        type Type = super::OneClickModInstallerWindow;
        type ParentType = adw::ApplicationWindow;

        fn class_init(klass: &mut Self::Class) {
            klass.bind_template();
        }

        fn instance_init(obj: &glib::subclass::InitializingObject<Self>) {
            obj.init_template();
        }
    }

    impl ObjectImpl for OneClickModInstallerWindow {
        fn constructed(&self) {
            self.parent_constructed();
            self.obj().setup_actions();
            self.obj().startup();
        }
    }

    impl WidgetImpl for OneClickModInstallerWindow {}
    impl WindowImpl for OneClickModInstallerWindow {}
    impl ApplicationWindowImpl for OneClickModInstallerWindow {}
    impl AdwApplicationWindowImpl for OneClickModInstallerWindow {}
}

glib::wrapper! {
    pub struct OneClickModInstallerWindow(ObjectSubclass<imp::OneClickModInstallerWindow>)
        @extends gtk::Widget, gtk::Window, gtk::ApplicationWindow, adw::ApplicationWindow,
        @implements 
            gio::ActionGroup,
            gio::ActionMap,
            gtk::ConstraintTarget,
            gtk::Buildable,
            gtk::Accessible,
            gtk::ShortcutManager,
            gtk::Root,
            gtk::Native;
}

impl OneClickModInstallerWindow {
    pub fn new<P: glib::prelude::IsA<gtk::Application>>(application: &P) -> Self {
        glib::Object::builder()
            .property("application", application)
            .build()
    }

    fn setup_actions(&self) {
        let initialize_installation_action = gio::ActionEntry::builder("initialize_installation")
            .activate(move |app: &Self, _, _| app.initialize_installation())
            .build();

        self.add_action_entries([initialize_installation_action]);
    }

    fn initialize_installation(&self) {
        let args = ArgHandler::convert_url_to_args(self.imp().mod_path_entry.text().to_string());
        match args {
            InitialArgs::FromGameBanana { url, type_, id } => {
                self.download_mod(url);
            },
            InitialArgs::FromInternet(url) => {
                self.download_mod(url);
            },
            InitialArgs::FromArchive(path) => {
                let dir = self.unpack_archive(path);
                let root = &self.find_mod_roots(dir)[0];
                let mod_path = self.place_mod_in_mods_folder(root);
                self.launch_mod_manager_if_needed(mod_path);
            },
            InitialArgs::FromDir(dir) => {
                let root = &self.find_mod_roots(dir)[0];
                let mod_path = self.place_mod_in_mods_folder(root);
                self.launch_mod_manager_if_needed(mod_path);
            },
            InitialArgs::None => {},
        }
    }

    fn place_mod_in_mods_folder(&self, root: &String) -> String {
        let root = Path::new(&root);
        let root_file_name = root.file_name().unwrap();
        common::copy_dir::copy_dir(&root.to_path_buf(), &Path::new("mods").join(root_file_name));
        return root_file_name.to_str().unwrap().to_owned();
    }

    fn launch_mod_manager_if_needed(&self, mod_path: String) {
        if self.imp().exit_after_install_checkbutton.is_active() {
            Launcher::launch_mod_manager(vec![mod_path]).unwrap();
            std::process::exit(0);
        }
    }

    fn download_mod(&self, url: String) {
        let (progress_bar_sender, progress_bar_receiver) = async_channel::bounded(1);
        let (progress_bar_text_sender, progress_bar_text_receiver) = async_channel::bounded(1);
        let (archive_path_sender, archive_path_receiver) = async_channel::bounded(1);

        glib::spawn_future_local(clone!(
            #[weak (rename_to = this)]
            self,
            async move {
                this.imp().install_button.set_sensitive(false);
                tokio_runtime::tokio_runtime().spawn(
                    download_mod(
                        url,
                        progress_bar_sender,
                        progress_bar_text_sender,
                        archive_path_sender,
                    )
                )
                .await
                .unwrap();
                // this.imp().install_button.set_sensitive(true);
            }
        ));

        glib::spawn_future_local(clone!(
            #[weak (rename_to = this)]
            self,
            async move {
                while let Ok(fraction) = progress_bar_receiver.recv().await {
                    this.imp().progress_bar.set_fraction(fraction);
                }
            }
        ));
        
        glib::spawn_future_local(clone!(
            #[weak (rename_to = this)]
            self,
            async move {
                while let Ok(text) = progress_bar_text_receiver.recv().await {
                    this.imp().progress_bar.set_text(Some(&text));
                }
            }
        ));

        glib::spawn_future_local(clone!(
            #[weak (rename_to = this)]
            self,
            async move {
                while let Ok(text) = archive_path_receiver.recv().await {
                    let dir = this.unpack_archive(text);
                    let root = &this.find_mod_roots(dir)[0];
                    let mod_path = this.place_mod_in_mods_folder(root);
                    this.launch_mod_manager_if_needed(mod_path);
                }
            }
        ));
    }

    fn unpack_archive(&self, url: String) -> String {
        self.imp().progress_bar.set_text(Some(&format!("Extracting {url}...")));
        self.imp().progress_bar.set_fraction(0.0);

        let dir_path = format!("{url}_extracted");

        match fs::remove_dir_all(&dir_path) {
            Ok(()) => (),
            Err(e) => eprintln!("Error removing directory: {e}"),
        };

        self.imp().progress_bar.set_fraction(0.5);

        Launcher::launch_7zip(vec![
            "x".to_string(),
            url.clone(),
            format!("-o{dir_path}"),
        ]).unwrap().wait().unwrap();

        self.imp().progress_bar.set_fraction(1.0);
        self.imp().progress_bar.set_text(Some(&format!("Archive extraction complete!")));

        return dir_path;
    }

    fn find_mod_roots(&self, dir_path: String) -> Vec<String> {
        let mut result = HashSet::<String>::new();

        let game_folders_array = [
            "CUTSCENE,DEMO,G_COM,G_SS,G_EP1COM,G_EP1ZONE2,G_EP1ZONE3,G_EP1ZONE4,G_ZONE1,G_ZONE2,G_ZONE3,G_ZONE4,G_ZONEF,MSG,NNSTDSHADER,SOUND",
            //"WSNE8P,WSNP8P,WSNJ8P"
            "Sonic4ModLoader",
        ];

        let game_folders_array = [
            game_folders_array[0].split(',').map(|x| x.to_owned()).collect::<Vec<String>>(),
            game_folders_array[1].split(',').map(|x| x.to_owned()).collect::<Vec<String>>(),
        ];

        let downloaded_mod_folders = common::walk_dir::walk_dir_for_dirs(Path::new(&dir_path));

        for downloaded_mod_folder in downloaded_mod_folders {
            for game_folders in &game_folders_array {
                for game_folder in game_folders {
                    if downloaded_mod_folder.ends_with(&game_folder) {
                        result.insert(downloaded_mod_folder.parent().unwrap().display().to_string());
                        break;
                    }
                }
            }
        }
        
        return Vec::from_iter(result);
    }
    

    fn handle_initial_args(&self) {
        let initial_args = ArgHandler::get();
        let initial_args = initial_args.deref();
        self.imp().mod_path_entry.set_sensitive(false);
        self.imp().mod_path_button.set_sensitive(false);
        self.imp().stack.set_visible_child_name("mod_installation");
        match initial_args {
            InitialArgs::FromDir(dir) => {
                println!("Provied args for mod from directory: {dir}");        
                self.imp().mod_path_entry.set_text(dir);
            },
            InitialArgs::FromArchive(archive) => {
                println!("Provied args for mod from archive: {archive}");
                self.imp().mod_path_entry.set_text(archive);
            },
            InitialArgs::FromGameBanana { url, type_, id } => {
                println!("Provied args for mod from GameBanana: {url}, {type_}, {id}");
                self.imp().mod_path_entry.set_text(url);
            },
            InitialArgs::FromInternet(url) => {
                println!("Provied args for mod from the internet: {url}");
                self.imp().mod_path_entry.set_text(url);
            },
            InitialArgs::None => {
                println!("No initial args provided");
                self.imp().mod_path_entry.set_sensitive(true);
                self.imp().mod_path_button.set_sensitive(true);
                self.imp().stack.set_visible_child_name("current_installation");
            },
        };
    }

    fn startup(&self) {
        self.imp().logo.set_resource(Some("/Sonic4ModLoader/OneClickModInstaller/logo.svg"));
        common::Launcher::where_in_the_world_am_i();
        common_gtk4::show_admin_warning(self);
        self.handle_initial_args();
    }
}
