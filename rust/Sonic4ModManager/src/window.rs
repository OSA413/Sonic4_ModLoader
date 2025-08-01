use adw::subclass::prelude::*;
use gtk::{gio::{self, ActionEntry}, glib, prelude::ActionMapExtManual};
use std::process;

mod imp {
    use super::*;

    #[derive(Debug, Default, gtk::CompositeTemplate)]
    #[template(resource = "/Sonic4ModLoader/Sonic4ModManager/window.ui")]
    pub struct Sonic4ModManagerWindow {
        // #[template_child]
        // pub button_play: TemplateChild<gtk::Button>,
        // #[template_child]
        // pub button_launch_config_tool: TemplateChild<gtk::Button>,
        // #[template_child]
        // pub button_launch_mod_manager: TemplateChild<gtk::Button>,
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
