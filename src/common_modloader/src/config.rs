use std::fs;
use std::error::Error;
use serde::{Deserialize, Serialize};
use serde_json;

#[derive(Debug, Deserialize, Serialize)]
pub struct GTKConfig {
    pub gsk_renderer: String
}

impl GTKConfig {
    pub fn load_config() -> Result<GTKConfig, Box<dyn Error>> {
        let config_content = fs::read_to_string("gtk4_config.json")?;
        let config: GTKConfig = serde_json::from_str(&config_content)?;
        Ok(config)
    }
}

#[derive(Debug, Deserialize, Serialize)]
pub struct OneClickModInstallerConfig {
    pub exit_on_install: bool,
    pub launch_mod_manager_on_exit_on_install: bool,
}

impl OneClickModInstallerConfig {
    pub fn load_config() -> Result<OneClickModInstallerConfig, Box<dyn Error>> {
        let config_content = fs::read_to_string("OneClickModInstaller.json")?;
        let config: OneClickModInstallerConfig = serde_json::from_str(&config_content)?;
        Ok(config)
    }

    pub fn save_config(&self) -> Result<(), Box<dyn Error>> {
        let config_content = serde_json::to_string(self)?;
        fs::write("OneClickModInstaller.json", config_content)?;
        Ok(())
    }
}