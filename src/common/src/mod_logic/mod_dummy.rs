use std::{fs, path::PathBuf};

pub struct ModDummy {
    pub path: String,
}

impl PartialEq for ModDummy {
    fn eq(&self, other: &Self) -> bool {
        self.path == other.path
    }
}

impl ModDummy {
    pub fn new(
        path: String,
    ) -> Self {
        ModDummy {
            path,
        }
    }

    pub fn load_from_mods_ini(path: &str) -> Vec<Self> {
        let mut result = Vec::new();
        let path_to_mod_ini = [path, "mods.ini"].iter().collect::<PathBuf>();
        let file = fs::read_to_string(path_to_mod_ini);
        match file {
            Ok(file) => {
                for line in file.lines() {
                    result.push(ModDummy::new(line.to_string()));
                }
            }
            Err(e) => println!("Error reading file: {e}"),
        }
        result
    }

    pub fn load_from_mods_directory(path: &str) -> Vec<Self> {
        let mut result = Vec::new();
        let files = fs::read_dir(path);
        match files {
            Ok(files) => {
                for dir_entry in files {
                    match dir_entry {
                        Ok(dir_entry) => {
                            let path = dir_entry.path();
                            if path.is_dir() {
                                let dir_name = path.file_name();
                                match dir_name {
                                    Some(dir_name) => {
                                        let dir_string = dir_name.to_str();
                                        match dir_string {
                                            Some(dir_string) => {
                                                result.push(ModDummy::new(dir_string.to_string()));
                                            }
                                            None => println!("Something went wrong, idk what, see source code")
                                        }
                                    }
                                    None => println!("Something went wrong2, idk what, see source code")
                                }
                            }
                        }
                        Err(e) => println!("Error reading directory enrty: {e}"),
                    }
                }
            }
            Err(e) => println!("Error reading folder: {e}"),
        }
        result
    }
}