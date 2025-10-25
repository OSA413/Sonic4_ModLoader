use std::{fs, path::{Path, PathBuf}};
use glob::glob;
use sha1::{Digest, Sha1};

pub fn get(data: impl AsRef<[u8]>) -> String {
    Sha1::digest(data).iter().map(|x| format!("{:02x}", x)).collect()
}

pub fn remove(file_name: String)
{
    let orig_file_sha_root = Path::new("mods_sha").join(file_name);

    if orig_file_sha_root.is_dir() {
        fs::remove_dir_all(orig_file_sha_root).expect("Bad thing happened #123");
    }
}

pub fn is_changed(do_sha_check: bool, file_name: &String, mod_files: &Vec<String>, mod_paths: &Vec<String>) -> bool {
    if !do_sha_check {
        return true;
    }

    let mut files_changed = false;

    let mut sha_list = Vec::<String>::new();

    let orig_file_sha_root = Path::new("mods_sha").join(file_name);

    if orig_file_sha_root.is_dir() {
        let glob_result = glob(&(orig_file_sha_root.display().to_string() + "/**/*.txt"));
        match glob_result {
            Ok(glob_result) => {
                for entry in glob_result {
                    match entry {
                        Ok(path) => sha_list.push(path.display().to_string()),
                        Err(e) => println!("Glob error: {e}"),
                    }
                }
            },
            Err(e) => println!("Error: {e}"),
        }
    }

    //Checking SHA1s
    let mut i = 0;
    while i < mod_files.len() {
        if files_changed { 
            break;
        }

        let mod_file_full = Path::new("mods").join(mod_paths[i].clone()).join(mod_files[i].clone());
        let mod_file_sha = Path::new("mods_sha").join(mod_files[i].clone() + ".txt");

        match sha_list.iter().position(|x| x.to_string() == mod_file_sha.display().to_string()) {
            Some(index) => sha_list.remove(index),
            None => {
                files_changed = true;
                break;
            }
        };

        if mod_file_sha.is_file() {
            let sha_tmp = get(fs::read(mod_file_full).expect("Bad, bad thing happened"));
            if sha_tmp != fs::read_to_string(mod_file_sha).expect("Badder thing happened") {
                files_changed = true;
            }
        } else {
            files_changed = true;
        }
        i += 1;
    }

    //Checking if there're removed files
    //And removing those SHAs
    if sha_list.len() > 0
    {
        files_changed = true;

        for file in sha_list {
            fs::remove_file(file).expect("Worse thing happened");
        }
    }

    return files_changed;
}

pub fn write(relative_mod_file_path: String, full_mod_file_path: PathBuf) {
    let sha_file = Path::new("mods_sha").join(relative_mod_file_path + ".txt");
    let sha_dir = sha_file.parent().unwrap();

    fs::create_dir_all(sha_dir.parent().unwrap()).expect("Failed to create directory for SHA-1 file");
    fs::write(sha_file, get(fs::read(full_mod_file_path).expect("Failed to read file for SHA-1 file "))).expect("Couldn't write SHA-1 of file");
}