use adw::subclass::prelude::*;
use common::settings::{amb_patcher::AMBPatcherConfig, csb_editor::CSBEditorConfig};
use gtk::{gio::{self, prelude::ActionMapExtManual}, glib, prelude::{CheckButtonExt, EditableExt, GtkWindowExt}};

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
        let amb_patcher_config = common::settings::amb_patcher::load();
        let csb_editor_config = common::settings::csb_editor::load();

        self.imp().checkbutton_progress_bar.set_active(amb_patcher_config.progress_bar);
        self.imp().checkbutton_check_sha_of_files.set_active(amb_patcher_config.sha_check);

        self.imp().entry_buffer_size.set_text(csb_editor_config.buffer_size.to_string().as_str());
        self.imp().checkbutton_enable_threading.set_active(csb_editor_config.enable_threading);
        self.imp().entry_max_threads.set_text(csb_editor_config.max_threads.to_string().as_str());
    }

    fn save(&self) {
        let amb_patcher_config_progress_bar = self.imp().checkbutton_progress_bar.is_active();
        let amb_patcher_config_sha_check = self.imp().checkbutton_check_sha_of_files.is_active();
        common::settings::amb_patcher::save(&AMBPatcherConfig{ progress_bar: amb_patcher_config_progress_bar, sha_check: amb_patcher_config_sha_check});

        let current_or_default_config_csb_editor = common::settings::csb_editor::load();
        let csb_editor_config_buffer_size = self.imp().entry_buffer_size.text().parse().unwrap_or(current_or_default_config_csb_editor.buffer_size);
        let csb_editor_config_enable_threading = self.imp().checkbutton_enable_threading.is_active();
        let csb_editor_config_max_threads = self.imp().entry_max_threads.text().parse().unwrap_or(current_or_default_config_csb_editor.max_threads);
        common::settings::csb_editor::save(&CSBEditorConfig { buffer_size: csb_editor_config_buffer_size, enable_threading: csb_editor_config_enable_threading, max_threads: csb_editor_config_max_threads } );
    }

    fn setup_actions(&self) {
        let close_action = gio::ActionEntry::builder("close")
            .activate(move |app: &Self, _, _| {app.close();})
            .build();

        let save_action = gio::ActionEntry::builder("save")
            .activate(move |app: &Self, _, _| {app.save();})
            .build();

        self.add_action_entries([
            close_action,
            save_action,
        ]);
    }
}
