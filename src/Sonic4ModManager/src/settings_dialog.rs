use std::{fs, path::Path};

use adw::subclass::prelude::*;
use common::{settings::{amb_patcher::AMBPatcherConfig, csb_editor::CSBEditorConfig}, Launcher};
use gtk::{gio::{self, prelude::ActionMapExtManual}, glib::{self, clone}, prelude::{ButtonExt, CheckButtonExt, EditableExt, GtkWindowExt, WidgetExt}};

use crate::installation::{self, get_installation_status, InstallationStatus, UninstallationOptions};

mod imp {
    use super::*;

    #[derive(Debug, Default, gtk::CompositeTemplate)]
    #[template(resource = "/Sonic4ModLoader/Sonic4ModManager/window_settings.ui")]
    pub struct SettingsWindow {
        // Installation
        #[template_child]
        pub label_installation_status: TemplateChild<gtk::Label>,
        #[template_child]
        pub button_un_install: TemplateChild<gtk::Button>,
        #[template_child]
        pub checkbutton_force_uninstall: TemplateChild<gtk::CheckButton>,
        #[template_child]
        pub label_uninstallation_options: TemplateChild<gtk::Label>,
        #[template_child]
        pub checkbutton_rename_files_back: TemplateChild<gtk::CheckButton>,
        #[template_child]
        pub checkbutton_delete_all_mod_loader_files: TemplateChild<gtk::CheckButton>,
        #[template_child]
        pub checkbutton_keep_configs: TemplateChild<gtk::CheckButton>,
        #[template_child]
        pub checkbutton_uninstall_and_delete_ocmi: TemplateChild<gtk::CheckButton>,
        #[template_child]
        pub checkbutton_recover_original_files: TemplateChild<gtk::CheckButton>,

        // AMBPatcher
        #[template_child]
        pub checkbutton_progress_bar: TemplateChild<gtk::CheckButton>,
        #[template_child]
        pub checkbutton_check_sha_of_files: TemplateChild<gtk::CheckButton>,

        // CSBEditor
        #[template_child]
        pub entry_buffer_size: TemplateChild<gtk::Entry>,
        #[template_child]
        pub checkbutton_enable_threading: TemplateChild<gtk::CheckButton>,
        #[template_child]
        pub entry_max_threads: TemplateChild<gtk::Entry>,
    }

    #[glib::object_subclass]
    impl ObjectSubclass for SettingsWindow {
        const NAME: &'static str = "Settings";
        type Type = super::SettingsWindow;
        type ParentType = adw::ApplicationWindow;

        fn class_init(klass: &mut Self::Class) {
            klass.bind_template();
        }

        fn instance_init(obj: &glib::subclass::InitializingObject<Self>) {
            obj.init_template();
        }
    }

    impl ObjectImpl for SettingsWindow {
        fn constructed(&self) {
            self.parent_constructed();
            self.obj().setup_actions();
            self.obj().startup();
        }
    }

    impl WidgetImpl for SettingsWindow {}
    impl WindowImpl for SettingsWindow {}
    impl ApplicationWindowImpl for SettingsWindow {}
    impl AdwApplicationWindowImpl for SettingsWindow {}
}

glib::wrapper! {
    pub struct SettingsWindow(ObjectSubclass<imp::SettingsWindow>)
        @extends gtk::Widget, gtk::Window, adw::Window, gtk::ApplicationWindow, adw::ApplicationWindow,
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

impl SettingsWindow {
    pub fn new() -> Self {
        glib::Object::builder().build()
    }

    fn startup(&self) {
        self.load_settings();
        self.update_installation_status();

        let closure = {
            clone!(
                #[strong (rename_to = this)] self,
                move || this.update_installation_status()
            )
        };

        self.imp().checkbutton_force_uninstall.connect_active_notify(move |_| closure());
    }

    fn un_install(&self) {
        match self.imp().button_un_install.label() {
            Some(label) => {
                let label = label.to_string();
                if label == "Install" {
                    installation::install();
                } else if label == "Uninstall" {
                    installation::uninstall(UninstallationOptions {
                        recover_original_files: self.imp().checkbutton_recover_original_files.is_active(),
                        delete_all_mod_loader_files: self.imp().checkbutton_delete_all_mod_loader_files.is_active(),
                        keep_configs: self.imp().checkbutton_keep_configs.is_active(),
                        uninstall_and_delete_ocmi: self.imp().checkbutton_uninstall_and_delete_ocmi.is_active()
                            && self.imp().checkbutton_delete_all_mod_loader_files.is_active(),
                    });
                }
            }
            None => (),
        }

        self.update_installation_status();
    }

    fn update_installation_status(&self) {
        let status = get_installation_status();
        let force_uninstall = self.imp().checkbutton_force_uninstall.is_active();
        
        self.imp().label_uninstallation_options.set_sensitive(false);
        self.imp().checkbutton_rename_files_back.set_sensitive(false);
        self.imp().checkbutton_delete_all_mod_loader_files.set_sensitive(false);
        self.imp().checkbutton_keep_configs.set_sensitive(false);
        self.imp().checkbutton_uninstall_and_delete_ocmi.set_sensitive(false);
        self.imp().checkbutton_recover_original_files.set_sensitive(false);
        self.imp().button_un_install.set_label("Install");

        self.imp().checkbutton_rename_files_back.set_active(true);
        self.imp().checkbutton_delete_all_mod_loader_files.set_active(false);
        self.imp().checkbutton_recover_original_files.set_active(false);
        self.imp().checkbutton_uninstall_and_delete_ocmi.set_active(false);
        self.imp().button_un_install.set_sensitive(true);

        match status {
            InstallationStatus::Installed => {
                self.imp().label_uninstallation_options.set_sensitive(true);
                self.imp().checkbutton_rename_files_back.set_sensitive(true);
                self.imp().checkbutton_delete_all_mod_loader_files.set_sensitive(true);
                self.imp().checkbutton_keep_configs.set_sensitive(true);
                self.imp().checkbutton_uninstall_and_delete_ocmi.set_sensitive(true);
                self.imp().button_un_install.set_label("Uninstall");
                self.imp().label_installation_status.set_text("Installed");
            }
            InstallationStatus::NotInstalled => {
                self.imp().button_un_install.set_sensitive(true);
                self.imp().button_un_install.set_label("Install");
                self.imp().label_installation_status.set_text("Not installed");
            }
            InstallationStatus::FirstLaunch => {
                self.imp().button_un_install.set_sensitive(true);
                self.imp().button_un_install.set_label("Install");
                self.imp().label_installation_status.set_text("Not installed");
            }
            InstallationStatus::NotGameDirectory => {
                self.imp().label_installation_status.set_text("Current directory is not the game directory");
                self.imp().button_un_install.set_sensitive(false);
                self.imp().checkbutton_force_uninstall.set_sensitive(false);
                self.imp().label_uninstallation_options.set_sensitive(false);
            }
        }
        if force_uninstall {
            self.imp().label_uninstallation_options.set_sensitive(true);
            self.imp().checkbutton_rename_files_back.set_sensitive(true);
            self.imp().checkbutton_delete_all_mod_loader_files.set_sensitive(true);
            self.imp().checkbutton_keep_configs.set_sensitive(true);
            self.imp().checkbutton_recover_original_files.set_sensitive(true);
            self.imp().checkbutton_uninstall_and_delete_ocmi.set_sensitive(true);
            self.imp().button_un_install.set_sensitive(true);
            self.imp().button_un_install.set_label("Uninstall");
            self.imp().checkbutton_rename_files_back.set_active(false);
            self.imp().checkbutton_delete_all_mod_loader_files.set_active(true);            
        }
    }

    fn load_settings(&self) {
        let amb_patcher_config = common::settings::amb_patcher::load();
        let csb_editor_config = common::settings::csb_editor::load();

        self.imp().checkbutton_progress_bar.set_active(amb_patcher_config.progress_bar);
        self.imp().checkbutton_check_sha_of_files.set_active(amb_patcher_config.sha_check);

        self.imp().entry_buffer_size.set_text(csb_editor_config.buffer_size.to_string().as_str());
        self.imp().checkbutton_enable_threading.set_active(csb_editor_config.enable_threading);
        self.imp().entry_max_threads.set_text(csb_editor_config.max_threads.to_string().as_str());
    }

    fn recover_files(&self) {
        match Path::new("AMBPatcher.exe").exists() {
            true => {
                match Launcher::launch_amb_patcher(vec!["recover".to_string()]) {
                    Ok(_) => println!("Recovered files with AMBPatcher"),
                    Err(e) => println!("Error recovering files: {}", e)
                }
                match fs::remove_file("mods/mods_prev") {
                    Ok(_) => println!("Removed mods_prev"),
                    Err(e) => println!("Error removing mods_prev: {}", e)
                }
                match fs::remove_file("mods/mods_sha") {
                    Ok(_) => println!("Removed mods_sha"),
                    Err(e) => println!("Error removing mods_sha: {}", e)
                }
            },
            false => println!("AMBPatcher.exe not found"),
        };
    }

    fn save(&self) {
        let amb_patcher_config_progress_bar = self.imp().checkbutton_progress_bar.is_active();
        let amb_patcher_config_sha_check = self.imp().checkbutton_check_sha_of_files.is_active();
        let result = common::settings::amb_patcher::save(&AMBPatcherConfig{
            progress_bar: amb_patcher_config_progress_bar,
            sha_check: amb_patcher_config_sha_check
        });


        let mut can_close = false;

        match result {
            Ok(_) => can_close = true,
            Err(e) => println!("Error saving settings: {}", e)
        }

        let current_or_default_config_csb_editor = common::settings::csb_editor::load();
        let csb_editor_config_buffer_size = self.imp().entry_buffer_size.text().parse().unwrap_or(current_or_default_config_csb_editor.buffer_size);
        let csb_editor_config_enable_threading = self.imp().checkbutton_enable_threading.is_active();
        let csb_editor_config_max_threads = self.imp().entry_max_threads.text().parse().unwrap_or(current_or_default_config_csb_editor.max_threads);
        let result = common::settings::csb_editor::save(&CSBEditorConfig {
            buffer_size: csb_editor_config_buffer_size,
            enable_threading: csb_editor_config_enable_threading,
            max_threads: csb_editor_config_max_threads
        });

        match result {
            Ok(_) => if can_close {
                self.close()
            },
            Err(e) => println!("Error saving settings: {}", e)
        }
    }

    fn setup_actions(&self) {
        let close_action = gio::ActionEntry::builder("close")
            .activate(move |app: &Self, _, _| {app.close();})
            .build();

        let save_action = gio::ActionEntry::builder("save")
            .activate(move |app: &Self, _, _| {app.save();})
            .build();

        let un_install_action = gio::ActionEntry::builder("un_install")
            .activate(move |app: &Self, _, _| {app.un_install();})
            .build();

        let recover_files_action = gio::ActionEntry::builder("recover_files")
            .activate(move |app: &Self, _, _| {app.recover_files();})
            .build();

        self.add_action_entries([
            close_action,
            save_action,
            un_install_action,
            recover_files_action,
        ]);
    }
}
