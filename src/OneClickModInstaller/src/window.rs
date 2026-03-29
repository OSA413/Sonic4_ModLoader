use std::{collections::HashSet, ffi::OsStr, fs::{self, File}, io::Write, ops::{Deref}, path::{self, Path}};
use async_channel::Sender;
use futures_util::StreamExt;
use adw::{ActionRow, prelude::{AdwDialogExt, AlertDialogExt}, subclass::prelude::*};
use gtk::{Widget, gio::{self, prelude::ActionMapExtManual}, glib::{self, clone, object::{Cast, ObjectExt}}, prelude::{BoxExt, ButtonExt, CheckButtonExt, EditableExt, WidgetExt}};

use crate::{arg_handler::{ArgHandler, InitialArgs}, handler_installer, models::my_g_string::MyGString, tokio_runtime};

use common::{Game, Launcher};

#[derive(PartialEq, Eq, Hash, Clone)]
pub enum ModType {
    PC,
    ModLoader,
}

pub enum SuspiciousResolution {
    CancelInstallation,
    ContinueInstallation,
    RemoveSuspiciousFilesAndContinueInstallation,
}

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

        // Current Installation
        #[template_child]
        pub current_game_label: TemplateChild<gtk::Label>,
        #[template_child]
        pub current_installation_status_label: TemplateChild<gtk::Label>,
        #[template_child]
        pub current_install_button: TemplateChild<gtk::Button>,
        #[template_child]
        pub current_uninstall_button: TemplateChild<gtk::Button>,

        // Installation Locations
        #[template_child]
        pub episode1_status_label: TemplateChild<gtk::Label>,
        #[template_child]
        pub episode1_open_button: TemplateChild<gtk::Button>,
        #[template_child]
        pub episode1_path_label: TemplateChild<gtk::Label>,
        #[template_child]
        pub episode2_status_label: TemplateChild<gtk::Label>,
        #[template_child]
        pub episode2_open_button: TemplateChild<gtk::Button>,
        #[template_child]
        pub episode2_path_label: TemplateChild<gtk::Label>,

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

    fn initialize_installation(&self) {
        let args = ArgHandler::convert_url_to_args(self.imp().mod_path_entry.text().to_string());
        match args {
            InitialArgs::FromGameBanana { url, type_: _, id: _ } => {
                self.download_mod(url);
            },
            InitialArgs::FromInternet(url) => {
                self.download_mod(url);
            },
            InitialArgs::FromArchive(path) => {
                let dir = self.unpack_archive(path);
                self.check_suspicious_files(&dir);
            },
            InitialArgs::FromDir(dir) => {
                self.check_suspicious_files(&dir)
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
                tokio_runtime::get().spawn(
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
                    this.check_suspicious_files(&dir);
                }
            }
        ));
    }

    fn show_suspicious_dialog(&self, suspicios_files: &Vec<String>) -> adw::AlertDialog {
        // Maybe redo that as a .ui file and class?
        let dialog = adw::AlertDialog::new(Some("Suspicious files found"), None);
        // dialog.set_title(Some("Suspicious files found"));

        let root = gtk::Box::new(gtk::Orientation::Vertical, 4);
        let button_root = gtk::Box::new(gtk::Orientation::Horizontal, 4);

        let cancel_button = gtk::Button::builder()
            .label("Cancel Installation")
            .width_request(128)
            .height_request(64)
            .build();
        cancel_button.connect_clicked(clone!(
            #[weak]
            dialog,
            move |_| {
                dialog.close();
                dialog.emit_by_name("response", &[&"cancel".to_string()])
            }
        ));
        button_root.insert_child_after(&cancel_button, None::<&Widget>);

        let continue_button = gtk::Button::builder()
            .label("Continue installation as is")
            .width_request(128)
            .height_request(64)
            .build();
        continue_button.connect_clicked(clone!(
            #[weak]
            dialog,
            move |_| {
                dialog.close();
                dialog.emit_by_name("response", &[&"continue".to_string()])
            }
        ));
        button_root.insert_child_after(&continue_button, None::<&Widget>);

        let remove_button = gtk::Button::builder()
            .label("Remove suspicious files\nand continue installation")
            .width_request(128)
            .height_request(64)
            .build();
        remove_button.connect_clicked(clone!(
            #[weak]
            dialog,
            move |_| {
                dialog.close();
                dialog.emit_by_name("response", &[&"remove".to_string()])
            }
        ));
        button_root.insert_child_after(&remove_button, None::<&Widget>);

        root.insert_child_after(&button_root, None::<&Widget>);

        let list = gtk::ListBox::new();
        let list_store = gio::ListStore::new::<MyGString>();
        let list_entries = suspicios_files.iter().map(MyGString::from_string).collect::<Vec<_>>();
        list_store.extend_from_slice(&list_entries);
        list.bind_model(Some(&list_store), |obj | {
            let g_mod_entry = obj
                .downcast_ref::<MyGString>()
                .expect("The object should be of type `MyGString`.");

            let row = ActionRow::builder()
                .title(g_mod_entry.value())
                .use_markup(false)
                .build();

            row.upcast::<gtk::Widget>()
        });

        let scrollable_wrapper = gtk::ScrolledWindow::new();
        scrollable_wrapper.set_child(Some(&list));
        scrollable_wrapper.set_width_request(380);
        scrollable_wrapper.set_height_request(300);
        root.insert_child_after(&scrollable_wrapper, None::<&Widget>);

        let label = gtk::Label::new(Some("Please review the list and select what to do with them."));
        root.insert_child_after(&label, None::<&Widget>);

        let label = gtk::Label::new(Some("Suspicious files found!"));
        root.insert_child_after(&label, None::<&Widget>);

        dialog.set_child(Some(&root));
        dialog.present(None::<&Widget>);

        dialog
    }

    fn check_suspicious_files(&self, dir_path: &String) {
        let good_formats = "TXT,INI,DDS,TXB,AMA,AME,ZNO,ZNM,ZNV,DC,EV,RG,MD,MP,AT,DF,DI,PSH,VSH,LTS,XNM,MFS,SSS,GPB,MSG,AYK,ADX,AMB,CPK,CSB,PNG,CT,TGA";

        let good_formats = good_formats.split(',').collect::<Vec<_>>();

        let dir_path_path = Path::new(&dir_path);

        let all_files = common::walk_dir::walk_dir(dir_path_path, None);
        let mut suspicious_files = Vec::<String>::new();

        for file in all_files {
            let file_short = file.to_str().unwrap().chars().skip(dir_path.len() + 1).collect::<String>();

            let file_short_name = Path::new(&file_short).file_name().unwrap();
            let file_short_extension = Path::new(&file_short).extension().unwrap_or(OsStr::new(""));

            if file_short_name.to_str().unwrap().parse::<u32>().is_ok()
                && file_short.contains(Path::new("DEMO").join("WORLDMAP").join("WORLDMAP.AMB").to_str().unwrap()) {
                continue;
            }

            if good_formats.contains(&file_short_extension.to_ascii_uppercase().to_str().unwrap()) {
                continue;
            }

            suspicious_files.push(file_short);
        }

        let global_dir = dir_path.clone();
        let global_files = suspicious_files.clone();
        let dialog = self.show_suspicious_dialog(&suspicious_files);

        let closure = clone!(
            #[weak (rename_to = this)]
            self,
            move |response: &str| {
                let resolution = match response {
                    "cancel" => SuspiciousResolution::CancelInstallation,
                    "continue" => SuspiciousResolution::ContinueInstallation,
                    "remove" => SuspiciousResolution::RemoveSuspiciousFilesAndContinueInstallation,
                    _ => SuspiciousResolution::CancelInstallation,
                };

                match resolution {
                    SuspiciousResolution::CancelInstallation => {},
                    SuspiciousResolution::ContinueInstallation => {
                        let root = &this.find_mod_roots(&global_dir)[0];
                        let mod_path = this.place_mod_in_mods_folder(&root.1);
                        this.launch_mod_manager_if_needed(mod_path);
                    },
                    SuspiciousResolution::RemoveSuspiciousFilesAndContinueInstallation => {
                        for file in &global_files {
                            fs::remove_file(Path::new(&global_dir).join(&file)).unwrap();
                        }
                        let root = &this.find_mod_roots(&global_dir)[0];
                        let mod_path = this.place_mod_in_mods_folder(&root.1);
                        this.launch_mod_manager_if_needed(mod_path);
                    },
                };
            }
        );

        dialog.connect_response(None, move |_, response| closure(&response));
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

    fn find_mod_roots(&self, dir_path: &String) -> Vec<(ModType, String)> {
        let mut result = HashSet::<(ModType, String)>::new();

        let game_folders_array = [
            "CUTSCENE,DEMO,G_COM,G_SS,G_EP1COM,G_EP1ZONE2,G_EP1ZONE3,G_EP1ZONE4,G_ZONE1,G_ZONE2,G_ZONE3,G_ZONE4,G_ZONEF,MSG,NNSTDSHADER,SOUND",
            //"WSNE8P,WSNP8P,WSNJ8P"
            "Sonic4ModLoader",
        ];

        let game_folders_array = [
            (ModType::PC, game_folders_array[0].split(',').map(|x| x.to_owned()).collect::<Vec<String>>()),
            (ModType::ModLoader, game_folders_array[1].split(',').map(|x| x.to_owned()).collect::<Vec<String>>()),
        ];

        let downloaded_mod_folders = common::walk_dir::walk_dir_for_dirs(Path::new(&dir_path));

        for downloaded_mod_folder in downloaded_mod_folders {
            for game_folders in &game_folders_array {
                for game_folder in &game_folders.1 {
                    if downloaded_mod_folder.ends_with(&game_folder) {
                        result.insert((game_folders.0.clone(), downloaded_mod_folder.parent().unwrap().display().to_string()));
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

    fn load_current_installation(&self) {
        let current_installation_info = handler_installer::get_info(None);
        self.imp().current_game_label.set_text(match current_installation_info.0 {
            Game::Episode1 => "Episode 1",
            Game::Episode2 => "Episode 2",
            Game::Unknown => "Unknown game (you are probabaly not in the game directory)",
        });
        match current_installation_info.1 {
            handler_installer::InstallationInfo::Installed(_) => {
                self.imp().current_install_button.set_label("Install");
                self.imp().current_install_button.set_sensitive(false);
                self.imp().current_uninstall_button.set_sensitive(true);
                self.imp().current_installation_status_label.set_text("Installed");
            },
            handler_installer::InstallationInfo::AnotherInstallationPresent(_) => {
                self.imp().current_install_button.set_label("Fix/change path to current OCMI");
                self.imp().current_install_button.set_sensitive(true);
                self.imp().current_uninstall_button.set_sensitive(true);
                self.imp().current_installation_status_label.set_text("Another installation present");
            },
            handler_installer::InstallationInfo::NotInstalled => {
                self.imp().current_install_button.set_label("Install");
                self.imp().current_install_button.set_sensitive(true);
                self.imp().current_uninstall_button.set_sensitive(false);
                self.imp().current_installation_status_label.set_text("Not installed");
            },
        }
    }

    fn load_other_installations(&self) {
        let episode1_installation_info = handler_installer::get_info(Some(Game::Episode1));
        assert_eq!(episode1_installation_info.0, Game::Episode1);
        match episode1_installation_info.1 {
            handler_installer::InstallationInfo::Installed(path) => {
                self.imp().episode1_status_label.set_text("Installed");
                self.imp().episode1_path_label.set_text(path.as_str());
                self.imp().episode1_open_button.set_sensitive(true);
            },
            handler_installer::InstallationInfo::AnotherInstallationPresent(path) => {
                self.imp().episode1_status_label.set_text("Installed");
                self.imp().episode1_path_label.set_text(path.as_str());
                self.imp().episode1_open_button.set_sensitive(true);
            },
            handler_installer::InstallationInfo::NotInstalled => {
                self.imp().episode1_status_label.set_text("Not installed");
                self.imp().episode1_path_label.set_text("");
                self.imp().episode1_open_button.set_sensitive(false);
            },
        }

        let episode2_installation_info = handler_installer::get_info(Some(Game::Episode2));
        assert_eq!(episode2_installation_info.0, Game::Episode2);
        match episode2_installation_info.1 {
            handler_installer::InstallationInfo::Installed(path) => {
                self.imp().episode2_status_label.set_text("Installed");
                self.imp().episode2_path_label.set_text(path.as_str());
                self.imp().episode2_open_button.set_sensitive(true);
            },
            handler_installer::InstallationInfo::AnotherInstallationPresent(path) => {
                self.imp().episode2_status_label.set_text("Installed");
                self.imp().episode2_path_label.set_text(path.as_str());
                self.imp().episode2_open_button.set_sensitive(true);
            },
            handler_installer::InstallationInfo::NotInstalled => {
                self.imp().episode2_status_label.set_text("Not installed");
                self.imp().episode2_path_label.set_text("");
                self.imp().episode2_open_button.set_sensitive(false);
            },
        }
    }

    fn setup_actions(&self) {
        let initialize_installation_action = gio::ActionEntry::builder("initialize_installation")
            .activate(move |app: &Self, _, _| app.initialize_installation())
            .build();

        // TODO connect all the buttons

        self.add_action_entries([initialize_installation_action]);
    }

    fn startup(&self) {
        self.imp().logo.set_resource(Some("/Sonic4ModLoader/OneClickModInstaller/logo.svg"));
        common::Launcher::where_in_the_world_am_i();
        common_gtk4::show_admin_warning(self);
        // TODO load config
        self.handle_initial_args();
        self.load_current_installation();
        self.load_other_installations();
    }
}
