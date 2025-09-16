use adw::subclass::prelude::*;
use gtk::{gio::{self, prelude::ActionMapExtManual}, glib, prelude::GtkWindowExt};

mod imp {
    use super::*;

    #[derive(Debug, Default, gtk::CompositeTemplate)]
    #[template(resource = "/Sonic4ModLoader/Sonic4ModManager/window_settings.ui")]
    pub struct SettingsWindow {
        // #[template_child]
        // pub mod_list: TemplateChild<gtk::ListBox>,
        // #[template_child]
        // pub description: TemplateChild<gtk::TextView>,
        // #[template_child]
        // pub refresh_button: TemplateChild<gtk::Button>,
        
        // #[template_child]
        // pub mod_store: TemplateChild<gio::ListStore>,
        // pub selected_mod_index: RefCell<Option<String>>,
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

    fn setup_actions(&self) {
        let close_action = gio::ActionEntry::builder("close")
            .activate(move |app: &Self, _, _| {app.close();})
            .build();

        self.add_action_entries([
            close_action
        ]);
    }
}
