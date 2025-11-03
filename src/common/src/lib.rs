mod launcher;
pub use launcher::Launcher;
pub use launcher::Game;
pub mod global;
pub mod config {
    pub mod config;
}
pub mod settings {
    pub mod amb_patcher;
    pub mod alice_mod_loader;
    pub mod csb_editor;
    pub mod file_pathcer;
    pub mod mod_manager;
}
pub mod mod_logic {
    pub mod mod_dummy;
    pub mod existing_mod;
    pub mod mod_entry;
}