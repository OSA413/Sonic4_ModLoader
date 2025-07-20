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