use adw::subclass::prelude::*;
use gtk::{gio, glib};

mod imp {
    use super::*;

    #[derive(Debug, Default, gtk::CompositeTemplate)]
    #[template(resource = "/Sonic4ModLoader/OneClickModInstaller/window.ui")]
    pub struct OneClickModInstallerWindow {
        #[template_child]
        pub logo: TemplateChild<gtk::Picture>,
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
        
    }

    fn startup(&self) {
        self.imp().logo.set_resource(Some("/Sonic4ModLoader/OneClickModInstaller/logo.svg"));
    }
}
