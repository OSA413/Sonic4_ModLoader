use std::{ffi::OsStr, path::{Path, PathBuf}};

pub fn walk_dir(dir: &Path, extension: Option<&OsStr>) -> Vec<PathBuf> {
    let mut files: Vec<PathBuf> = vec![];
    match dir.read_dir() {
        Ok(entries) => {
            for entry in entries {
                match entry {
                    Ok(entry) => {
                        let path = entry.path();
                        if path.is_dir() {
                            files.extend(walk_dir(&path, extension));
                        } else {
                            match extension {
                                Some(desired_extension) => {
                                    if let Some(file_extension) = path.extension() {
                                        if file_extension == desired_extension {
                                            files.push(path);
                                        }
                                    }
                                },
                                None => files.push(path),
                            }
                        }
                    },
                    Err(e) => println!("Error: {e}"),
                }
            }
        },
        Err(e) => println!("Error: {e}"),
    }
    files
}