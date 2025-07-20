#![windows_subsystem = "windows"]
mod application;
mod window;

use self::application::ModloaderApplication;
use self::window::ModloaderWindow;
use gtk::{gio, glib};
use adw::prelude::ApplicationExtManual;

const APP_ID: &str = "Sonic4ModLoader.ManagerLauncher";

fn main() -> glib::ExitCode {

    let gtk4_config = common::config::config::GTKConfig::load_config();
    match gtk4_config {
        Ok(config) => {
            println!("GTK4 config loaded");
            println!("Applying GSK_RENDERER variable to {} (without overwriting)", config.gsk_renderer);
            glib::setenv("GSK_RENDERER", config.gsk_renderer, false).expect("Failed to set GSK_RENDERER");
        }
        Err(err) => {
            println!("Error loading GTK4 config: {}", err);
            println!("Continuing without GTK4 config");
        }
    }

    // Load resources
    gio::resources_register_include!("ManagerLauncher.gresource")
        .expect("Failed to register resources.");

    // Create a new GtkApplication. The application manages our main loop,
    // application windows, integration with the window manager/compositor, and
    // desktop features such as file opening and single-instance applications.
    let app = ModloaderApplication::new(APP_ID, &gio::ApplicationFlags::empty());

    // Run the application. This function will block until the application
    // exits. Upon return, we have our exit code to return to the shell. (This
    // is the code you see when you do `echo $?` after running a command in a
    // terminal.
    let args: [&str; 0] = [];
    app.run_with_args(&args)
}
