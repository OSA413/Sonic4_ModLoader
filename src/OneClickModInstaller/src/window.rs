use std::{cmp, collections::HashSet, ffi::OsString, fs::{self, File}, io::Write, ops::Deref, path::{self, Path, PathBuf}, time::Duration};
use async_channel::Sender;
use common_binary::error::CommonBinaryError;
use common_gtk4::show_error_dialog;
use futures_util::StreamExt;
use adw::{ActionRow, prelude::{AdwDialogExt, AlertDialogExt}, subclass::prelude::*};
use gtk::{Widget, gio::{self, Cancellable, prelude::{ActionMapExtManual, FileExt}}, glib::{self, clone, object::{Cast, ObjectExt}}, prelude::{BoxExt, ButtonExt, CheckButtonExt, EditableExt, WidgetExt}};

use crate::{arg_handler::{ArgHandler, InitialArgs}, handler_installer, models::my_g_string::MyGString, tokio_runtime};

use common_modloader::{Game, Launcher, config::OneClickModInstallerConfig};

#[derive(PartialEq, Eq, Hash, Clone)]
pub enum ModType {
    PC,
    ModLoader,
}

pub enum SuspiciousResolution {
    Cancel,
    Continue,
    RemoveSuspiciousFilesAndContinue,
}

async fn download_mod(
    url: String,
    progress_bar: Sender<f64>,
    progress_bar_text: Sender<String>,
    file_path: Sender<String>,
    critical_error_sender: Sender<String>,
) {
    if let Err(e) = progress_bar_text.send_blocking("Connecting to the server...".to_string()) {
        eprintln!("Error sending progress bar text: {e}");
    }

    let response = match reqwest::Client::new()
        .get(url)
        .timeout(Duration::from_mins(15))
        .send()
        .await {
        Ok(response) => response,
        Err(e) => {
            if let Err(e) = critical_error_sender.send_blocking(e.to_string()) {
                eprintln!("Critical error downloading the mod: {e}");
            }
            return;
        }
    };
    let total_size = response.content_length();
    let final_url = response.url().clone();
    let file_name = match final_url.path_segments().ok_or("Couldn't determine name of the file from the url") {
        Ok(mut segments) => match segments.next_back().ok_or("Couldn't determine name of the file from the url") {
            Ok(filename) => filename,
            Err(e) => {
                if let Err(e) = critical_error_sender.send_blocking(e.to_string()) {
                    eprintln!("Critical error downloading the mod: {e}");
                }
                return;
            }
        }
        Err(e) => {
            if let Err(e) = critical_error_sender.send_blocking(e.to_string()) {
                eprintln!("Critical error downloading the mod: {e}");
            }
            return;
        }
    };

    if let Err(e) = progress_bar_text.send_blocking(format!("Downloading {file_name}...")) {
        eprintln!("Error sending progress bar text: {e}");
    }
    if let Err(e) = progress_bar.send_blocking(0.0) {
        eprintln!("Error sending progress bar: {e}");
    }

    // TODO: Redo to non-expect
    fs::create_dir_all("downloaded_mods")
        .expect("Couldn't create a directory for downloaded mods, can't continue.");
    let to = Path::new("downloaded_mods").join(file_name);
    let mut file = match File::create(&to) {
        Ok(file) => file,
        Err(e) => {
            if let Err(e) = critical_error_sender.send_blocking(e.to_string()) {
                eprintln!("Critical error downloading the mod: {e}");
            }
            return;
        }
    };
    let mut downloaded = 0;
    let mut stream = response.bytes_stream();

    while let Some(item) = stream.next().await {
        let chunk = match item {
            Ok(chunk) => chunk,
            Err(e) => {
                if let Err(e) = critical_error_sender.send_blocking(e.to_string()) {
                    eprintln!("Critical error downloading the mod: {e}");
                }
                return;
            }
        };
        if let Err(e) = file.write_all(&chunk) {
            if let Err(e) = critical_error_sender.send_blocking(e.to_string()) {
                eprintln!("Critical error downloading the mod: {e}");
            }
            return;
        };
        let send_signal = match total_size {
            Some(total_size) => {
                downloaded += chunk.len();
                progress_bar.send_blocking(downloaded as f64 / total_size as f64)
            },
            // If we don't have the total size, then we turn the progress into a speedometer
            None => {
                downloaded = cmp::max(downloaded, chunk.len());
                progress_bar.send_blocking(chunk.len() as f64 / downloaded as f64)
            },
        };
        if let Err(e) = send_signal {
            eprintln!("Error sending progress bar progress: {e}");
        };
    }

    if let Err(e) = progress_bar_text.send_blocking(format!("Finished downloading {file_name}!")) {
        eprintln!("Error sending progress bar progress text: {e}");
    }
    if let Err(e) = progress_bar.send_blocking(1.0) {
        eprintln!("Error sending progress bar progress: {e}");
    }
    
    match path::absolute(to) {
        Ok(absolute_path) => if let Err(e) = file_path.send_blocking(absolute_path.display().to_string())
        && let Err(e) = critical_error_sender.send_blocking(e.to_string()) {
            eprintln!("Critical error downloading the mod: {e}");
        }
        Err(e) => if let Err(e) = critical_error_sender.send_blocking(e.to_string()) {
            eprintln!("Critical error downloading the mod: {e}");
        }
    }
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
        pub exit_on_install_checkbutton: TemplateChild<gtk::CheckButton>,
        #[template_child]
        pub launch_mod_manager_on_exit_checkbutton: TemplateChild<gtk::CheckButton>,
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
                if let Ok(dir) = self.unpack_archive(path) {
                    self.check_suspicious_files(&dir);
                }
            },
            InitialArgs::FromDir(dir) => {
                self.check_suspicious_files(&dir)
            },
            InitialArgs::None => {},
        }
    }

    fn place_mod_in_mods_folder(&self, root: &String) -> Result<String, Vec<CommonBinaryError>> {
        let root_path = Path::new(&root);
        let root_file_name = root_path.file_name().ok_or_else(|| [CommonBinaryError::Description(format!("Can not get filename from [{root}]"))])?;
        common_utils::copy_dir::copy_dir(&root_path.to_path_buf(), &Path::new("mods").join(root_file_name))?;
        Ok(root_file_name.to_string_lossy().to_string())
    }

    fn launch_mod_manager_if_needed(&self, mod_path: String) {
        if self.imp().exit_on_install_checkbutton.is_active() {
            if self.imp().launch_mod_manager_on_exit_checkbutton.is_active()
            && let Err(e) = Launcher::launch_mod_manager(vec![mod_path]) {
                show_error_dialog(self, "Error launching Mod Manager", &e.to_string().into_boxed_str());
                return;
            }
            std::process::exit(0);
        }
    }

    fn download_mod(&self, url: String) {
        let (progress_bar_sender, progress_bar_receiver) = async_channel::bounded(1);
        let (progress_bar_text_sender, progress_bar_text_receiver) = async_channel::bounded(1);
        let (archive_path_sender, archive_path_receiver) = async_channel::bounded(1);
        let (critical_error_sender, critical_error_receiver) = async_channel::bounded(1);

        glib::spawn_future_local(clone!(
            #[weak (rename_to = this)]
            self,
            async move {
                this.imp().install_button.set_sensitive(false);
                if let Err(e) = tokio_runtime::get().spawn(
                    download_mod(
                        url,
                        progress_bar_sender,
                        progress_bar_text_sender,
                        archive_path_sender,
                        critical_error_sender,
                    )
                )
                .await {
                    show_error_dialog(&this, "Error downloading the mod", &e.to_string().into_boxed_str());
                    this.imp().install_button.set_sensitive(true);
                }
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
                    if let Ok(dir) = this.unpack_archive(text) {
                        this.check_suspicious_files(&dir);
                    }
                }
            }
        ));

        glib::spawn_future_local(clone!(
            #[weak (rename_to = this)]
            self,
            async move {
                while let Ok(text) = critical_error_receiver.recv().await {
                    show_error_dialog(&this, "Critical error downloading the mod.
Please try again later.", text.as_str());
                    this.imp().install_button.set_sensitive(true);
                }
            }
        ));
    }

    fn show_suspicious_dialog(&self, suspicios_files: &[PathBuf]) -> adw::AlertDialog {
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
        let list_entries = suspicios_files.iter().map(|x| MyGString::from_string(&x.to_string_lossy())).collect::<Vec<_>>();
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
        // TODO: move to a separate file
        let good_formats: [OsString; 33] = ["TXT","INI","DDS","TXB","AMA","AME","ZNO","ZNM","ZNV","DC","EV","RG","MD","MP","AT","DF","DI","PSH","VSH","LTS","XNM","MFS","SSS","GPB","MSG","AYK","ADX","AMB","CPK","CSB","PNG","CT","TGA"]
        .map(|x| x.into());

        let dir_path_path = Path::new(&dir_path);

        let all_files = common_utils::walk_dir::walk_dir(dir_path_path, None);
        let mut suspicious_files = Vec::<PathBuf>::new();

        for file in all_files {
            let short_file = file.to_string_lossy().chars().skip(dir_path.len() + 1).collect::<String>();
            let short_file = Path::new(&short_file);

            let short_file_path =  Path::new(&short_file);

            if let Some(short_file_name) = short_file_path.file_name()
            && let Some(short_file_name_str) = short_file_name.to_str()
            && short_file_name_str.parse::<u32>().is_ok()
                && short_file.to_string_lossy().contains(
                    &Path::new("DEMO").join("WORLDMAP").join("WORLDMAP.AMB").to_string_lossy().to_string()
                ) {
                continue;
            }

            if let Some(file_short_extension) = Path::new(&short_file).extension()
            && good_formats.contains(&file_short_extension.to_ascii_uppercase()) {
                continue;
            }

            suspicious_files.push(short_file.to_path_buf());
        }

        let global_dir = dir_path.clone();
        let global_files = suspicious_files.clone();

        if suspicious_files.is_empty() {
            let root = &self.find_mod_roots(&global_dir)[0];
            match self.place_mod_in_mods_folder(&root.1) {
                Ok(mod_path) => self.launch_mod_manager_if_needed(mod_path),
                Err(e) => show_error_dialog(self, "Error placing mod files in the mods folder", &format!("{e:?}").into_boxed_str()),
            }
            return;
        }

        let dialog = self.show_suspicious_dialog(&suspicious_files);

        let closure = clone!(
            #[weak (rename_to = this)]
            self,
            move |response: &str| {
                let resolution = match response {
                    "cancel" => SuspiciousResolution::Cancel,
                    "continue" => SuspiciousResolution::Continue,
                    "remove" => SuspiciousResolution::RemoveSuspiciousFilesAndContinue,
                    _ => SuspiciousResolution::Cancel,
                };

                match resolution {
                    SuspiciousResolution::Cancel => {},
                    SuspiciousResolution::Continue => {
                        let root = &this.find_mod_roots(&global_dir)[0];
                        match this.place_mod_in_mods_folder(&root.1) {
                            Ok(mod_path) => this.launch_mod_manager_if_needed(mod_path),
                            Err(e) => show_error_dialog(&this, "Error placing mod files in the mods folder", &format!("{e:?}").into_boxed_str()),
                        }

                    },
                    SuspiciousResolution::RemoveSuspiciousFilesAndContinue => {
                        if let Err(e) = try_delete_files_or_fail(&global_dir, &global_files) {
                            show_error_dialog(&this, "Error deleting suspicious files", &e.to_string().into_boxed_str());
                            return;
                        }
                        let root = &this.find_mod_roots(&global_dir)[0];
                        match this.place_mod_in_mods_folder(&root.1) {
                            Ok(mod_path) => this.launch_mod_manager_if_needed(mod_path),
                            Err(e) => show_error_dialog(&this, "Error placing mod files in the mods folder", &format!("{e:?}").into_boxed_str()),
                        }
                    },
                };
            }
        );

        dialog.connect_response(None, move |_, response| closure(response));
    }

    // It probably should be async
    // TODO: redo for more smooth error handling
    fn unpack_archive(&self, url: String) -> Result<String, ()> {
        self.imp().progress_bar.set_text(Some(&format!("Extracting {url}...")));
        self.imp().progress_bar.set_fraction(0.0);

        let dir_path = format!("{url}_extracted");

        let _ = fs::remove_dir_all(&dir_path);

        self.imp().progress_bar.set_fraction(0.5);

        let process = Launcher::launch_7zip(vec![
            "x".to_string(),
            url.clone(),
            format!("-o{dir_path}"),
        ]);

        match process {
            Ok(mut process) => {
                if let Err(e) = process.wait() {
                    show_error_dialog(self, "Error extracting archive", &e.to_string().into_boxed_str());
                    return Err(());
                }
            }
            Err(e) => {
                show_error_dialog(self, "Error launching 7-Zip", &e.to_string().into_boxed_str());
                return Err(());
            }
        }

        self.imp().progress_bar.set_fraction(1.0);
        self.imp().progress_bar.set_text(Some("Archive extraction complete!"));

        Ok(dir_path)
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

        let downloaded_mod_folders = common_utils::walk_dir::walk_dir_for_dirs(Path::new(&dir_path));

        for downloaded_mod_folder in downloaded_mod_folders {
            for game_folders in &game_folders_array {
                for game_folder in &game_folders.1 {
                    if downloaded_mod_folder.ends_with(game_folder)
                    && let Some(parent) = downloaded_mod_folder.parent() {
                        result.insert((game_folders.0.clone(), parent.display().to_string()));
                        break;
                    }
                }
            }
        }
        
        Vec::from_iter(result)
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
        if current_installation_info.0 == Game::Unknown {
            self.imp().current_install_button.set_sensitive(false);
            self.imp().current_uninstall_button.set_sensitive(false);
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

    fn load_config(&self) {
        match OneClickModInstallerConfig::load_config() {
            Ok(config) => {
                self.imp().exit_on_install_checkbutton.set_active(config.exit_on_install);
                self.imp().launch_mod_manager_on_exit_checkbutton.set_active(config.launch_mod_manager_on_exit_on_install);
            },
            Err(_) => {
                self.imp().exit_on_install_checkbutton.set_active(true);
                self.imp().launch_mod_manager_on_exit_checkbutton.set_active(true);
            }
        }
    }

    fn save_config(&self) {
        let config = OneClickModInstallerConfig {
            exit_on_install: self.imp().exit_on_install_checkbutton.is_active(),
            launch_mod_manager_on_exit_on_install: self.imp().launch_mod_manager_on_exit_checkbutton.is_active(),
        };

        if let Err(e) = config.save_config() {
            show_error_dialog(self, "Error saving config", &e.to_string().into_boxed_str());
        }
    }

    fn setup_actions(&self) {
        let initialize_installation_action = gio::ActionEntry::builder("initialize_installation")
            .activate(move |app: &Self, _, _| app.initialize_installation())
            .build();

        let exit_on_install_action = gio::ActionEntry::builder("exit_on_install_toggle")
            .activate(move |app: &Self, _, _| {
                app.imp().exit_on_install_checkbutton.set_active(!app.imp().exit_on_install_checkbutton.is_active());
                if !app.imp().exit_on_install_checkbutton.is_active() {
                    app.imp().launch_mod_manager_on_exit_checkbutton.set_active(false);
                }
                app.save_config();
            })
            .build();

        let launch_mod_manager_on_exit_action = gio::ActionEntry::builder("launch_mod_manager_on_exit_toggle")
            .activate(move |app: &Self, _, _| {
                app.imp().launch_mod_manager_on_exit_checkbutton.set_active(
                    app.imp().launch_mod_manager_on_exit_checkbutton.is_active()
                );
                app.save_config();
            })
            .build();

        let select_mod_clicked_action = gio::ActionEntry::builder("select_mod_clicked")
            .activate(move |app: &Self, _, _| {
                let file_dialog = gtk::FileDialog::builder()
                    .title("Select a mod to install (archive or folder, for folder select any file in the root of the mod folder)")
                    .build();

                gtk::FileDialog::open(&file_dialog, None::<&gtk::Window>, None::<&Cancellable>, clone!(
                    #[weak (rename_to = this)]
                    app,
                    move |result| {
                        if let Ok(file) = result 
                        && let Some(path) = extract_preferred_path_from_selected_file(file).path() {
                            this.imp().mod_path_entry.set_text(path.display().to_string().as_str())
                        }
                    }
                ));
            })
            .build();

        let install_or_fix_path_to_current_game_action = gio::ActionEntry::builder("install_or_fix_path_to_current_game")
            .activate(move |window: &Self, _, _| {
                match handler_installer::get_info(None) {
                    (_, handler_installer::InstallationInfo::Installed(_)) => {},
                    (_, handler_installer::InstallationInfo::AnotherInstallationPresent(_)) => {
                        if let Err(e) = handler_installer::fix(None) {
                            let error_message = match e {
                                handler_installer::HadnlerInstallationError::Io(io_error) => &format!("IO Error: {io_error}").into_boxed_str(),
                                handler_installer::HadnlerInstallationError::UnknownGame => "You can't install OCMI to the unknown game!",
                            };
                            show_error_dialog(window, "Error fixing current installation", error_message);
                        }
                    }
                    (_, handler_installer::InstallationInfo::NotInstalled) => {
                        if let Err(e) = handler_installer::install(None) {
                            let error_message = match e {
                                handler_installer::HadnlerInstallationError::Io(io_error) => &format!("IO Error: {io_error}").into_boxed_str(),
                                handler_installer::HadnlerInstallationError::UnknownGame => "You can't install OCMI to the unknown game!",
                            };
                            show_error_dialog(window, "Error installaing for current game", error_message);
                        }
                    }
                };
                window.load_current_installation();
                window.load_other_installations();
            })
            .build();

        let uninstall_current_game_action = gio::ActionEntry::builder("uninstall_current_game")
            .activate(move |window: &Self, _, _| {
                if let Err(e) = handler_installer::uninstall(None) {
                    let error_message = match e {
                        handler_installer::HadnlerInstallationError::Io(io_error) => &format!("IO Error: {io_error}").into_boxed_str(),
                        handler_installer::HadnlerInstallationError::UnknownGame => "You can't install OCMI to the unknown game!",
                    };
                    show_error_dialog(window, "Error uninstalling current installation", error_message);
                }
                window.load_current_installation();
                window.load_other_installations();
            })
            .build();

        let open_episode1_action = gio::ActionEntry::builder("open_episode1")
            .activate(move |window, _, _| open_ocmi_for_game(window, Game::Episode1))
            .build();

        let open_episode2_action = gio::ActionEntry::builder("open_episode2")
            .activate(move |window, _, _| open_ocmi_for_game(window, Game::Episode2))
            .build();

        self.add_action_entries([
            initialize_installation_action,
            exit_on_install_action,
            launch_mod_manager_on_exit_action,
            select_mod_clicked_action,
            install_or_fix_path_to_current_game_action,
            uninstall_current_game_action,
            open_episode1_action,
            open_episode2_action,
        ]);
    }

    fn startup(&self) {
        self.imp().logo.set_resource(Some("/Sonic4ModLoader/OneClickModInstaller/logo.svg"));
        common_modloader::Launcher::where_in_the_world_am_i();
        common_gtk4::show_admin_warning(self);
        self.load_config();

        // HACK FIXME
        glib::spawn_future_local(clone!(
            #[weak(rename_to = this)]
            self,
            async move {
                this.handle_initial_args();
                this.load_current_installation();
                this.load_other_installations();
            }
        ));
    }
}

fn open_ocmi_for_game<W: glib::prelude::IsA<gtk::Widget>>(window: &W, game: Game) {
    match handler_installer::get_info(Some(game)) {
        (_, handler_installer::InstallationInfo::Installed(path)) => open_directory(window, &path),
        (_, handler_installer::InstallationInfo::AnotherInstallationPresent(path)) => open_directory(window, &path),
        (_, handler_installer::InstallationInfo::NotInstalled) => {},
    };
}

fn open_directory<W: glib::prelude::IsA<gtk::Widget>>(window: &W, path: &String) {
    match Path::new(&path).parent() {
        Some(parent) => {
            match Launcher::open_folder(parent) {
                Ok(mut process) => {
                    if let Ok(e) = process.wait() {
                        show_error_dialog(
                            window,
                            "Error opening OCMI folder",
                            e.to_string().as_str()
                        );
                    }
                }
                Err(e) => show_error_dialog(
                    window,
                    "Error opening OCMI folder",
                    e.to_string().as_str()
                ),
            };
        }
        None => show_error_dialog(
            window,
            "Error opening OCMI folder",
            "It looks like you installed OCMI in the root of your file system or drive. I refuse to open it."
        ),
    };
}

fn extract_preferred_path_from_selected_file(file: gio::File) -> gio::File {
    if let Some(basename) = file.basename() 
        && let Some(extension) = basename.extension()
        && (extension == "7z"
        || extension == "zip"
        || extension == "rar") {
        return file;
    };
    if let Some(parent) = file.parent() {
        if let Some(granddad) = parent.parent() {
            return granddad;
        }
        return parent;
    }
    file
}

fn try_delete_files_or_fail(global_dir: &str, global_files: &Vec<PathBuf>) -> Result<(), std::io::Error> {
    for file in global_files {
        fs::remove_file(Path::new(&global_dir).join(file))?;
    }
    Ok(())
}