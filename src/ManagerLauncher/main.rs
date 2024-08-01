mod window;

use gtk::{gio, glib};
use window::Window;
use adw::prelude::*;

const APP_ID: &str = "Sonic4ModLoader.ManagerLauncher";

fn main() -> glib::ExitCode {
    gio::resources_register_include!("ManagerLauncher.gresource")
        .expect("Failed to register resources.");
    
    let app = adw::Application::builder().application_id(APP_ID).build();
    app.connect_activate(build_ui);
    app.run()
}

fn build_ui(app: &adw::Application) {
    Window::new(app).present()
}
