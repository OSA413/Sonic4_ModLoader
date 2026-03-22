use gtk::glib;
use adw::{prelude::{AdwDialogExt, AlertDialogExt}};

pub fn set_gsk_renderer_from_config() {
    println!("Trying to load GTK4 config...");
    let gtk4_config = common::config::GTKConfig::load_config();
    match gtk4_config {
        Ok(config) => {
            println!("GTK4 config loaded");
            println!("Applying GSK_RENDERER variable to {} (without overwriting)", config.gsk_renderer);
            glib::setenv("GSK_RENDERER", config.gsk_renderer, false).expect("Failed to set GSK_RENDERER");
        }
        Err(err) => {
            println!("Error loading GTK4 config: {err}");
            println!("That's OK, continuing without GTK4 config");
        }
    }
}

fn show_admin_warning_common<W: glib::prelude::IsA<gtk::Widget>>(window: &W) {
    let alert = adw::AlertDialog::new(
        Some("Run as admin detected"),
        Some("It seems that you have launched the program as admin.

Generally you shouldn't do that.")
    );

    alert.add_response("ok", "OK, I got it!");
    alert.set_response_appearance("ok", adw::ResponseAppearance::Suggested);
    alert.set_close_response("ok");
    alert.present(Some(window));
}

pub fn show_admin_warning<W: glib::prelude::IsA<gtk::Widget>>(window: &W) {
    let running_as = is_sudo::check();
    match running_as {
        is_sudo::RunningAs::Root => show_admin_warning_common(window),
        is_sudo::RunningAs::User => {},
    }
}
