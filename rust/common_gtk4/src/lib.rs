use gtk::glib;

pub fn set_gsk_renderer_from_config() -> () {
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
}