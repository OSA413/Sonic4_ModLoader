use std::{fs, path::{Path, PathBuf}};

// TODO: fix panics
pub fn copy_dir(from: &PathBuf, to: &PathBuf) {
    fs::create_dir_all(to).unwrap();
    match fs::read_dir(from) {
        Ok(entries) => {
            for entry in entries {
                match entry {
                    Ok(entry) => {
                        let file_type = entry.file_type().unwrap();
                        if file_type.is_dir() {
                            copy_dir(&entry.path(), &Path::new(to).join(entry.file_name()));
                        } else {
                            fs::copy(&entry.path(), &Path::new(to).join(entry.file_name())).unwrap();
                        }
                    },
                    Err(e) => eprintln!("Error: {e}"),
                }
            }
        }
        Err(e) => eprintln!("Error: {e}"),
    }
}