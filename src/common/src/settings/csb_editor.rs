use std::fs;
use serde::{Deserialize, Serialize};

#[derive(Debug, Serialize, Deserialize)]
#[serde(rename = "configuration")]
struct XmlConfiguration {
    #[serde(rename = "configSections")]
    pub config_sections: XmlConfigSections,
    #[serde(rename = "userSettings")]
    pub user_settings: XmlUserSettingsGroup,
}


#[derive(Debug, Serialize, Deserialize)]
struct XmlConfigSections {
    #[serde(rename = "sectionGroup")]
    pub section_group: XmlSectionGroup,
}


#[derive(Debug, Serialize, Deserialize)]
struct XmlSectionGroup {
    #[serde(rename = "@name")]
    name: String,
    section: XmlSection,
}

#[derive(Debug, Serialize, Deserialize)]
struct XmlSection {
    #[serde(rename = "@name")]
    name: String,
    #[serde(rename = "@type")]
    type_: String,
}

#[derive(Debug, Serialize, Deserialize)]
struct XmlUserSettingsGroup {
    #[serde(rename = "CsbEditor.Properties.Settings")]
    csb_editor_properties_settings: Vec<XmlSettingElement>,
}

#[derive(Debug, Serialize, Deserialize)]
struct XmlSettingElement {
    #[serde(rename = "@name")]
    name: String,
    #[serde(rename = "@serializeAs")]
    serialize_as: String,
    value: XmlValue,
}

#[derive(Debug, Serialize, Deserialize)]
struct XmlValue {
    #[serde(rename = "#text")]
    value: String,
}

pub struct CSBEditorConfig {
    pub enable_threading: bool,
    pub max_threads: u32,
    pub buffer_size: u32
}

pub fn load() -> Result<CSBEditorConfig, Box<dyn std::error::Error>> {
    let default_config = CSBEditorConfig { enable_threading: true, max_threads: 4, buffer_size: 4096 };
    let config_result = fs::read_to_string("CsbEditor.exe.config");
    match config_result {
        Ok(config) => {
            let xml_config_result: Result<XmlConfiguration, serde_xml_rs::Error> = serde_xml_rs::from_str(config.as_str());

            match xml_config_result {
                Ok(xml_config) => {
                    let enable_threading = match xml_config.user_settings.csb_editor_properties_settings.iter().find(|x| x.name == "EnableThreading") {
                        Some(x) => x.value.value == "true",
                        None => default_config.enable_threading
                    };
                    let max_threads = match xml_config.user_settings.csb_editor_properties_settings.iter().find(|x| x.name == "MaxThreads") {
                        Some(x) => x.value.value.clone().parse::<u32>().unwrap_or(default_config.max_threads),
                        None => default_config.max_threads
                    };
                    let buffer_size = match xml_config.user_settings.csb_editor_properties_settings.iter().find(|x| x.name == "BufferSize") {
                        Some(x) => x.value.value.clone().parse::<u32>().unwrap_or(default_config.buffer_size),
                        None => default_config.buffer_size
                    };

                    Ok(CSBEditorConfig { enable_threading, max_threads, buffer_size })
                }
                Err(e) => {
                    println!("Error parsing CsbEditor.exe.config: {}", e);
                    return Ok(default_config);
                }
            }
        },
        Err(e) => {
            println!("Error reading CsbEditor.exe.config: {}", e);
            return Ok(default_config);
        }
    }
}

pub fn save(config: &CSBEditorConfig) -> Result<(), Box<dyn std::error::Error>> {
    let xml_config = XmlConfiguration {
        config_sections: XmlConfigSections {
            section_group: XmlSectionGroup {
                name: "userSettings".to_string(),
                section: XmlSection {
                    name: "CsbEditor.Properties.Settings".to_string(),
                    type_: "System.Configuration.ClientSettingsSection".to_string()
                }
            }
        },
        user_settings: XmlUserSettingsGroup {
            csb_editor_properties_settings: vec![
                XmlSettingElement {
                    name: "EnableThreading".to_string(),
                    serialize_as: "String".to_string(),
                    value: XmlValue {
                        value: config.enable_threading.to_string()
                    }
                },
                XmlSettingElement {
                    name: "MaxThreads".to_string(),
                    serialize_as: "String".to_string(),
                    value: XmlValue {
                        value: config.max_threads.to_string()
                    }
                },
                XmlSettingElement {
                    name: "BufferSize".to_string(),
                    serialize_as: "String".to_string(),
                    value: XmlValue {
                        value: config.buffer_size.to_string()
                    }
                }
            ]
        }
    };

    let config_str = serde_xml_rs::to_string(&xml_config)?;
    fs::write("CsbEditor.exe.config", config_str)?;

    Ok(())
}
