use gtk::{prelude::*, License};
use adw::{prelude::AdwDialogExt, subclass::prelude::*};
use gtk::{gio, glib};

use crate::ManagerLauncherWindow;

mod imp {
    use super::*;

    #[derive(Debug, Default)]
    pub struct ManagerLauncherApplication {}

    #[glib::object_subclass]
    impl ObjectSubclass for ManagerLauncherApplication {
        const NAME: &'static str = "ManagerLauncherApplication";
        type Type = super::ManagerLauncherApplication;
        type ParentType = adw::Application;
    }

    impl ObjectImpl for ManagerLauncherApplication {
        fn constructed(&self) {
            self.parent_constructed();
            let obj = self.obj();
            obj.setup_gactions();
            obj.set_accels_for_action("app.quit", &["<primary>q"]);
        }
    }

    impl ApplicationImpl for ManagerLauncherApplication {
        // We connect to the activate callback to create a window when the application
        // has been launched. Additionally, this callback notifies us when the user
        // tries to launch a "second instance" of the application. When they try
        // to do that, we'll just present any existing window.
        fn activate(&self) {
            let application = self.obj();
            // Get the current window or create one if necessary
            let window = if let Some(window) = application.active_window() {
                window
            } else {
                let window = ManagerLauncherWindow::new(&*application);
                window.upcast()
            };

            let icon_theme = gtk::IconTheme::for_display(&gtk::gdk::Display::default().unwrap());
            gtk::IconTheme::add_resource_path(&icon_theme, "/Sonic4ModLoader/ManagerLauncher/");
            window.set_icon_name(Some("icon"));
            
            // Ask the window manager/compositor to present the window
            window.present();
        }
    }

    impl GtkApplicationImpl for ManagerLauncherApplication {}
    impl AdwApplicationImpl for ManagerLauncherApplication {}
}

glib::wrapper! {
    pub struct ManagerLauncherApplication(ObjectSubclass<imp::ManagerLauncherApplication>)
        @extends gio::Application, gtk::Application, adw::Application,
        @implements gio::ActionGroup, gio::ActionMap;
}

impl ManagerLauncherApplication {
    pub fn new(application_id: &str, flags: &gio::ApplicationFlags) -> Self {
        glib::Object::builder()
            .property("application-id", application_id)
            .property("flags", flags)
            .build()
    }

    fn setup_gactions(&self) {
        let quit_action = gio::ActionEntry::builder("quit")
            .activate(move |app: &Self, _, _| app.quit())
            .build();
        let about_action = gio::ActionEntry::builder("about")
            .activate(move |app: &Self, _, _| app.show_about())
            .build();
        self.add_action_entries([quit_action, about_action]);
    }

    fn show_about(&self) {
        let window = self.active_window();                
        let about = adw::AboutDialog::builder()
            .application_name("Manager Launcher")
            .license_type(License::MitX11)
            .application_icon("icon")
            .developer_name("Oleg \"OSA413\" Sokolov")
            .version(common::global::VERSION)
            .developers(vec!["Oleg \"OSA413\" Sokolov"])
            .artists(vec!["Oleg \"OSA413\" Sokolov"])
            .website("https://github.com/OSA413/Sonic4_ModLoader")
            .issue_url("https://github.com/OSA413/Sonic4_ModLoader/issues")
            .support_url("https://discord.gg/WCp8BFyFxN")
            .copyright("© 2018-2025 Oleg \"OSA413\" Sokolov")
            .build();

        about.present(window.as_ref());
    }
}
