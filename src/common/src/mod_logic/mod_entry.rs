use std::path::PathBuf;

use crate::mod_logic::existing_mod::ExistingMod;
use ini::Ini;

pub struct ModEntry {
    pub path: String,
    pub enabled: bool,
    pub title: Option<String>,
    pub authors: Option<String>,
    pub version: Option<String>,
    pub description: Option<String>,
}

impl ModEntry {
    pub fn new(
        path: String,
        enabled: bool,
        title: Option<String>,
        authors: Option<String>,
        version: Option<String>,
        description: Option<String>
    ) -> Self {
        ModEntry {
            path: path,
            enabled: enabled,
            title: title,
            authors: authors,
            version: version,
            description: description
        }
    }

    pub fn from_existing_mod(existing_mod: &ExistingMod) -> Self {
        let mod_path = existing_mod.path.to_owned();
        let mod_ini_path = ["mods".to_string(), mod_path, "mod.ini".to_string()].iter().collect::<PathBuf>();
        let mod_ini = Ini::load_from_file(mod_ini_path);

        match mod_ini {
            Ok(mod_ini) => {
                let section = mod_ini.section::<String>(None);
                match section {
                    Some(section) => {
                        let title = section.get("Name");
                        let authors = section.get("Authors");
                        let version = section.get("Version");
                        let description = section.get("Description");
        
                        ModEntry::new(
                            existing_mod.path.to_owned(),
                            existing_mod.enabled,
                            title.map(|x| x.to_string()),
                            authors.map(|x| x.to_string()),
                            version.map(|x| x.to_string()),
                            description.map(|x| x.to_string())
                        )
                    },
                    None => ModEntry::new(
                        existing_mod.path.to_owned(),
                        existing_mod.enabled,
                        None,
                        None,
                        None,
                        None
                    )
                }
            },
            Err(_) => ModEntry::new(
                existing_mod.path.to_owned(),
                existing_mod.enabled,
                None,
                None,
                None,
                None
            )
        }
    }

    pub fn load(path: &str) -> Vec<Self> {
        let existing_mods = ExistingMod::load(path);
        existing_mods.iter().map(|x| ModEntry::from_existing_mod(x)).collect()
    }
}