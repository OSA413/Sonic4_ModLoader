use adw::subclass::prelude::*;
use gtk::{gio, glib};

mod imp {
    use super::*;

    #[derive(Debug, Default, gtk::CompositeTemplate)]
    #[template(resource = "/Sonic4ModLoader/ManagerLauncher/window.ui")]
    pub struct ModloaderWindow {
        #[template_child]
        pub gtk_box: TemplateChild<gtk::Box>,
        #[template_child]
        pub button: TemplateChild<gtk::Button>,
    }

    #[glib::object_subclass]
    impl ObjectSubclass for ModloaderWindow {
        const NAME: &'static str = "ManagerLauncher";
        type Type = super::ModloaderWindow;
        type ParentType = gtk::ApplicationWindow;

        fn class_init(klass: &mut Self::Class) {
            klass.bind_template();
        }

        fn instance_init(obj: &glib::subclass::InitializingObject<Self>) {
            obj.init_template();
        }
    }

    impl ObjectImpl for ModloaderWindow {}
    impl WidgetImpl for ModloaderWindow {}
    impl WindowImpl for ModloaderWindow {}
    impl ApplicationWindowImpl for ModloaderWindow {}
    impl AdwApplicationWindowImpl for ModloaderWindow {}
}

glib::wrapper! {
    pub struct ModloaderWindow(ObjectSubclass<imp::ModloaderWindow>)
        @extends gtk::Widget, gtk::Window, gtk::ApplicationWindow, adw::ApplicationWindow,
        @implements gio::ActionGroup, gio::ActionMap;
}

impl ModloaderWindow {
    pub fn new<P: glib::prelude::IsA<gtk::Application>>(application: &P) -> Self {
        glib::Object::builder()
            .property("application", application)
            .build()
    }
}
