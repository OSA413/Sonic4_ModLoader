use adw::subclass::prelude::*;
use gtk::{gio::{self, ActionEntry}, glib, prelude::ActionMapExtManual};
use std::process;

mod imp {
    use super::*;

    #[derive(Debug, Default, gtk::CompositeTemplate)]
    #[template(resource = "/Sonic4ModLoader/ManagerLauncher/window.ui")]
    pub struct ManagerLauncherWindow {}

    #[glib::object_subclass]
    impl ObjectSubclass for ManagerLauncherWindow {
        const NAME: &'static str = "ManagerLauncher";
        type Type = super::ManagerLauncherWindow;
        type ParentType = gtk::ApplicationWindow;

        fn class_init(klass: &mut Self::Class) {
            klass.bind_template();
        }

        fn instance_init(obj: &glib::subclass::InitializingObject<Self>) {
            obj.init_template();
        }
    }

    impl ObjectImpl for ManagerLauncherWindow {
        fn constructed(&self) {
            self.parent_constructed();
            self.obj().setup_actions();
        }
    }

    impl WidgetImpl for ManagerLauncherWindow {}
    impl WindowImpl for ManagerLauncherWindow {}
    impl ApplicationWindowImpl for ManagerLauncherWindow {}
    impl AdwApplicationWindowImpl for ManagerLauncherWindow {}
}

glib::wrapper! {
    pub struct ManagerLauncherWindow(ObjectSubclass<imp::ManagerLauncherWindow>)
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

impl ManagerLauncherWindow {
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
                    Err(e) => println!("{e}"),
                }
            })
            .build();

        let action_launch_config_tool = ActionEntry::builder("launch_config_tool")
            .activate(move |_window: &Self, _action, _parameter| {
                let res = common::Launcher::launch_config();
                match res {
                    Ok(_) => process::exit(0),
                    Err(e) => println!("{e}"),
                }
            })
            .build();

        let action_launch_mod_manager = ActionEntry::builder("launch_mod_manager")
            .activate(move |_window: &Self, _action, _parameter| {
                let res = common::Launcher::launch_mod_manager(vec![]);
                match res {
                    Ok(_) => process::exit(0),
                    Err(e) => println!("{e}"),
                }
            })
            .build();

        self.add_action_entries([action_play, action_launch_config_tool, action_launch_mod_manager]);
    }
}
