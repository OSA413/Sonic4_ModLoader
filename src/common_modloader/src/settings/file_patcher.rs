use std::fs;

use serde::{Deserialize, Serialize};

#[derive(Serialize)]
pub struct FilePatcherConfig {
    pub progress_bar: bool,
    pub sha_check: bool
}

#[derive(Deserialize)]
struct FilePatcherConfigPartial {
    pub progress_bar: Option<bool>,
    pub sha_check: Option<bool>,
}

pub fn load() -> FilePatcherConfig {
    let default_settings = FilePatcherConfig {
        progress_bar: true,
        sha_check: true
    };

    let json_config = std::fs::read_to_string("Sonic4FilePatcher.json").unwrap();
    let json_config = serde_json::from_str::<FilePatcherConfigPartial>(&json_config);
    
    match json_config {
        Ok(json) => {
            FilePatcherConfig {
                progress_bar: json.progress_bar.unwrap_or(default_settings.progress_bar),
                sha_check: json.sha_check.unwrap_or(default_settings.sha_check)
            }
        },
        Err(e) => {
            println!("Error loading Sonic4FilePatcher.ini: {e}");
            default_settings
        }
    }
}

pub fn save(config: &FilePatcherConfig) -> Result<(), Box<dyn std::error::Error>> {
    let json_config = serde_json::to_string(config)?;
    fs::write("Sonic4FilePatcher.json", json_config)?;
    Ok(())
}
