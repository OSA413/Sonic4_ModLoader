mod application;
mod window;

use self::application::ManagerLauncherApplication;
use self::window::ManagerLauncherWindow;
use gtk::{gio, glib};
use adw::prelude::ApplicationExtManual;

const APP_ID: &str = "Sonic4ModLoader.ManagerLauncher";

fn main() -> glib::ExitCode {
    common_gtk4::set_gsk_renderer_from_config();

    // Load resources
    gio::resources_register_include!("ManagerLauncher.gresource")
        .expect("Failed to register resources.");

    // Create a new GtkApplication. The application manages our main loop,
    // application windows, integration with the window manager/compositor, and
    // desktop features such as file opening and single-instance applications.
    let app = ManagerLauncherApplication::new(APP_ID, &gio::ApplicationFlags::empty());

    // Run the application. This function will block until the application
    // exits. Upon return, we have our exit code to return to the shell. (This
    // is the code you see when you do `echo $?` after running a command in a
    // terminal.
    let args: [&str; 0] = [];
    app.run_with_args(&args)
}
