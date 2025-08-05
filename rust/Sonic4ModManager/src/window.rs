use adw::{prelude::{ActionRowExt}, subclass::prelude::*, ActionRow};
use gtk::{gio::{self}, glib, prelude::{ActionMapExtManual}, Align, CheckButton};

mod imp {
    use super::*;

    #[derive(Debug, Default, gtk::CompositeTemplate)]
    #[template(resource = "/Sonic4ModLoader/Sonic4ModManager/window.ui")]
    pub struct Sonic4ModManagerWindow {
        #[template_child]
        pub mod_list: TemplateChild<gtk::ListBox>,
    }

    #[glib::object_subclass]
    impl ObjectSubclass for Sonic4ModManagerWindow {
        const NAME: &'static str = "Sonic4ModManager";
        type Type = super::Sonic4ModManagerWindow;
        type ParentType = gtk::ApplicationWindow;

        fn class_init(klass: &mut Self::Class) {
            klass.bind_template();
        }

        fn instance_init(obj: &glib::subclass::InitializingObject<Self>) {
            obj.init_template();
        }
    }

    impl ObjectImpl for Sonic4ModManagerWindow {
        fn constructed(&self) {
            self.parent_constructed();
            self.obj().setup_actions();
        }
    }

    impl WidgetImpl for Sonic4ModManagerWindow {}
    impl WindowImpl for Sonic4ModManagerWindow {}
    impl ApplicationWindowImpl for Sonic4ModManagerWindow {}
    impl AdwApplicationWindowImpl for Sonic4ModManagerWindow {}
}

glib::wrapper! {
    pub struct Sonic4ModManagerWindow(ObjectSubclass<imp::Sonic4ModManagerWindow>)
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

impl Sonic4ModManagerWindow {
    pub fn new<P: glib::prelude::IsA<gtk::Application>>(application: &P) -> Self {
        glib::Object::builder()
            .property("application", application)
            .build()
    }

     fn create_mod_row(&self, title: &str, subtitle: &str) -> () {
        let check_button = CheckButton::builder()
            .valign(Align::Center)
            .can_focus(false)
            .build();

        let row = ActionRow::builder()
            .title(title)
            .subtitle(subtitle)
            .build();
        row.add_prefix(&check_button);
        
        self.imp().mod_list.append(&row);
    }

    fn setup_actions(&self) {
        let mod_check_action = gio::ActionEntry::builder("check_mod")
            .activate(move |app: &Self, _, _| app.create_mod_row("test", "Version 1.0.1 by OSA413"))
            .build();

        self.add_action_entries([mod_check_action]);
    }
}
