use adw::subclass::prelude::*;
use gtk::{gio::{self, ActionEntry}, glib, prelude::ActionMapExtManual};
use std::process;

mod imp {
    use super::*;

    #[derive(Debug, Default, gtk::CompositeTemplate)]
    #[template(resource = "/Sonic4ModLoader/ManagerLauncher/window.ui")]
    pub struct ModloaderWindow {
        #[template_child]
        pub button_play: TemplateChild<gtk::Button>,
        #[template_child]
        pub button_launch_config_tool: TemplateChild<gtk::Button>,
        #[template_child]
        pub button_launch_mod_manager: TemplateChild<gtk::Button>,
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

    impl ObjectImpl for ModloaderWindow {
        fn constructed(&self) {
            self.parent_constructed();
            self.obj().setup_actions();
        }
    }

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

    fn setup_actions(&self) {
        let action_play = ActionEntry::builder("play")
            .activate(move |_window: &Self, _action, _parameter| {
                let res = common::Launcher::launch_game();
                match res {
                    Ok(_) => process::exit(0),
                    Err(_) => {},
                }
            })
            .build();

        let action_launch_config_tool = ActionEntry::builder("launch_config_tool")
            .activate(move |_window: &Self, _action, _parameter| {
                let res = common::Launcher::launch_config();
                match res {
                    Ok(_) => process::exit(0),
                    Err(_) => {},
                }
            })
            .build();

        let action_launch_mod_manager = ActionEntry::builder("launch_mod_manager")
            .activate(move |_window: &Self, _action, _parameter| {
                let res = common::Launcher::launch_mod_manager(vec![]);
                match res {
                    Ok(_) => process::exit(0),
                    Err(_) => {},
                }
            })
            .build();

        self.add_action_entries([action_play, action_launch_config_tool, action_launch_mod_manager]);
    }
}
