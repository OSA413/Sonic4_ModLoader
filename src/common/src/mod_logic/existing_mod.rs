use crate::mod_logic::mod_dummy::ModDummy;

pub struct ExistingMod {
    pub path: String,
    pub enabled: bool,
}

impl ExistingMod {
    pub fn new(
        path: String,
        enabled: bool,
    ) -> Self {
        ExistingMod {
            path,
            enabled,
        }
    }

    pub fn load(path: &str) -> Vec<Self> {
        let mut folder_content = ModDummy::load_from_mods_directory(path);
        let mut mod_ini_content = ModDummy::load_from_mods_ini(path);
        mod_ini_content.reverse();
        let mut result = Vec::new();

        for mod_dummy in mod_ini_content {
            let index_of_enabled_mod = folder_content.iter().position(|x| x == &mod_dummy);
            match index_of_enabled_mod {
                Some(index) => {
                    let enabled_mod = folder_content.remove(index);
                    result.push(ExistingMod::new(enabled_mod.path, true));
                },
                None => println!("Mod [{}] is in mods.ini but doesn't exists in the mods folder", mod_dummy.path),
            }
        }
        for mod_dummy in folder_content {
            result.push(ExistingMod::new(mod_dummy.path, false));
        }

        result
    }
}