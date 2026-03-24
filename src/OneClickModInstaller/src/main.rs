mod application;
mod window;
mod arg_handler;
mod handler_installer;

use self::application::OneClickModInstallerApplication;
use self::window::OneClickModInstallerWindow;
use gtk::{gio, glib};
use adw::prelude::ApplicationExtManual;
use self::arg_handler::ArgHandler;

const APP_ID: &str = "Sonic4ModLoader.OneClickModInstaller";

fn main() -> glib::ExitCode {
    // This will either install and exit, or prepare the config for the mod installer
    ArgHandler::init(std::env::args());

    common_gtk4::set_gsk_renderer_from_config();

    // Load resources
    gio::resources_register_include!("OneClickModInstaller.gresource")
        .expect("Failed to register resources.");

    // Create a new GtkApplication. The application manages our main loop,
    // application windows, integration with the window manager/compositor, and
    // desktop features such as file opening and single-instance applications.
    let app = OneClickModInstallerApplication::new(APP_ID, &gio::ApplicationFlags::empty());

    // Run the application. This function will block until the application
    // exits. Upon return, we have our exit code to return to the shell. (This
    // is the code you see when you do `echo $?` after running a command in a
    // terminal.
    let args: [&str; 0] = [];
    app.run_with_args(&args)
}
