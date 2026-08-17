mod application;
mod window;
mod arg_handler;
mod handler_installer;
mod tokio_runtime;
mod models;

use std::path::Path;

use self::application::OneClickModInstallerApplication;
use self::window::OneClickModInstallerWindow;
use gtk::{gio, glib};
use adw::prelude::ApplicationExtManual;
use self::arg_handler::ArgHandler;

const APP_ID: &str = "Sonic4ModLoader.OneClickModInstaller";

fn main() -> glib::ExitCode {
    // This will either install and exit, or prepare the config for the mod installer
    ArgHandler::init(std::env::args());

    // This thing is needed when launching from URI handler on Windows
    if let Ok(current_directory) = std::env::current_dir()
        && current_directory == Path::new("C:\\WINDOWS\\system32") {
        let current_exe = std::env::current_exe()
            .expect("Couldn't get current exe path to change current path from system's one, exiting...");
        let actual_directory = current_exe.parent()
            .expect("Couldn't get directory of current exe to change current path from system's one, exiting...");
        std::env::set_current_dir(actual_directory)
            .expect("Couldn't change current working directory from the system's one, exiting...");
    }

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
