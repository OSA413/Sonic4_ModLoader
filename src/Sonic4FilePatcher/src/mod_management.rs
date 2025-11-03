use std::{collections::HashMap, fs, path::{Path, PathBuf}};

use common::Launcher;
use glob::glob;
use crate::{help, sha_checker};
use indicatif::ProgressBar;

pub fn full_recover_of_files() {
    if Path::new("mods/mods_prev").is_file() {
        let mods_prev = fs::read_to_string("mods/mods_prev").expect("Rolling around at error messages");
        let mods_prev = mods_prev.lines();

        for file in mods_prev {
            recover(&file.to_string());
            if file.ends_with(".CSB") || file.ends_with(".csb") {
                let path_without_extension = file.chars().take(file.chars().count() - 4).collect::<String>();
                recover( &format!("{}.CPK", path_without_extension));
                sha_checker::remove(&path_without_extension);
            }
            else
            {
                sha_checker::remove(&file.to_string());
            }
        }
        fs::remove_file("mods/mods_prev").unwrap();
    }

}

pub fn recover(file_name: &String) {
    let backup_path = format!("{}.bkp", file_name);
    if Path::new(&backup_path).is_file() {
        fs::copy(backup_path, file_name).unwrap();
    }
}

pub struct ModFile {
    pub file_path: String,
    pub mod_folder: String,
}

fn get_mod_files() -> HashMap<String, Vec<ModFile>> {
    if !Path::new("mods/mods.ini").is_file() {
        return HashMap::new();
    }
    
    let mods_ini = common::mod_logic::existing_mod::ExistingMod::load("mods");
    let mods_ini = mods_ini
        .iter()
        .filter(|m| m.enabled)
        .rev()
        .collect::<Vec<_>>();

    let mut result: Vec<(String, ModFile)> = Vec::new();

    for mmod in mods_ini {
        if !Path::new("mods").join(mmod.path.clone()).is_dir() {
            continue;
        }

        let filenames = glob(&format!("mods/{}/**/*.*", mmod.path));
        let mut paths = Vec::new();
        match filenames {
            Ok(filenames) => {
                for entry in filenames {
                    match entry {
                        Ok(path) => {
                            // glob returns all entries, even if it's a folder that contains files
                            if path.is_dir() {
                                continue;
                            }
                            paths.push(path);
                        },
                        Err(e) => println!("Glob error: {}", e),
                    }
                }
            },
            Err(e) => println!("Error: {e}"),
        }

        paths.sort_by(|a, b| a.display().to_string().cmp(&b.display().to_string()));
        // Needs confirmation
        // // We have to replicate .Net default sort to not break some mods that rely on file addition order
        // // For example, SSON_DRAG.ZNM must come after SSON_DRAG_L.ZNM keeping overal file order the same
        // // TODO: optimize
        // // paths.sort_by(
        // //     |a, b| 
        // //         a.display().to_string().replace("_", ".")
        // //             .cmp(&b.display().to_string().replace("_", "."))
        // // );

        for path in paths {
            //Getting "folder/file" from "mods/mod/folder/file/mod_file"
            let ppath = path.display().to_string();
            let filename_parts = ppath.split(std::path::MAIN_SEPARATOR_STR).collect::<Vec<_>>();
            let mut original_file = "".to_string();
            let mut k = 0;

            while k < filename_parts.len() - 2 {
                let possible_orig_file = filename_parts.iter().skip(2).take(k + 1).collect::<PathBuf>();

                if possible_orig_file.is_file() {
                    original_file = possible_orig_file.display().to_string();
                    break;
                }
                else if Path::new(&format!("{}.CSB", possible_orig_file.display().to_string())).is_file()
                {
                    original_file = format!("{}.CSB", possible_orig_file.display().to_string());
                    break;
                }
                k += 1;
            }

            if original_file == "" {
                continue;
            }

            //Getting "folder/file/mod_file" from "mods/mod/folder/file/mod_file"
            let mod_file = filename_parts.iter().skip(2).collect::<PathBuf>();

            //Getting "mod" from "mods/mod/folder/file/mod_file"
            let mod_path = filename_parts[1];

            result
                .push((
                    original_file,
                    ModFile {
                        file_path: mod_file.display().to_string(),
                        mod_folder: mod_path.to_string(),
                    }
                ));
        }
    }

    let mut grouped: HashMap<String, Vec<ModFile>> = HashMap::new();

    for (original_file, mod_file) in result {
        let list = grouped.entry(original_file).or_insert(Vec::new());
        match list.last() {
            Some(last) => {
                if mod_file.file_path.contains(&last.file_path) {
                    continue;
                }
            },
            None => (),
        }
        list.push(mod_file);
    }
    
    grouped
}

pub fn backup(file_name: &String) {
    let backup_path = format!("{}.bkp", &file_name);
    if !Path::new(&backup_path).exists() && Path::new(&file_name).is_file() { 
        fs::copy(&file_name, backup_path).unwrap();
    }
}

pub fn patch_all(file_name: &String, mod_files: Vec<ModFile>, bar: Option<&ProgressBar>) {
    if mod_files.is_empty() {
        return;
    }

    if Path::new(&file_name).is_file() {
        if file_name.ends_with(".AMB") || file_name.ends_with(".amb")
        {
            if sha_checker::is_changed(true , file_name, &mod_files)
            {
                if file_name == &mod_files.first().unwrap().file_path {
                    let mod_full = Path::new("mods").join(mod_files[0].mod_folder.clone()).join(mod_files[0].file_path.clone());
                    fs::copy(&mod_full, file_name).unwrap();
                    sha_checker::write(mod_files[0].file_path.clone(), mod_full);
                    match bar {
                        Some(bar) => bar.inc(1),
                        None => (),
                    }
                    return;
                }

                for mod_file in mod_files {
                    let mod_file_full = Path::new("mods").join(mod_file.mod_folder.clone()).join(mod_file.file_path.clone());
                    match bar {
                        Some(bar) => bar.inc(1),
                        None => (),
                    }
                    match common::Launcher::launch_amb_rs(vec!["add".to_string(), file_name.to_string(), mod_file_full.display().to_string()]) {
                        Ok(mut child) => {
                            match child.wait() {
                                Ok(_) => sha_checker::write(mod_file.file_path.clone(), mod_file_full),
                                Err(e) => println!("Error: {}", e),
                            }
                        },
                        Err(e) => println!("Error: {}", e),
                    }

                }
            }
        }
        else if file_name.ends_with(".csb") || file_name.ends_with(".CSB")
        {
            if sha_checker::is_changed(true, &file_name.chars().take(file_name.chars().count() - 4).collect::<String>(), &mod_files)
            {
                recover(file_name);
                recover(&format!("{}.CPK", &file_name.chars().take(file_name.chars().count() - 4).collect::<String>()));

                match Launcher::launch_csb_editor(vec![file_name.to_string()]) {
                    Ok(mut child) => {
                        match child.wait() {
                            Ok(_) => {
                                for mod_file in mod_files {
                                    let mod_file_path = Path::new("mods").join(mod_file.mod_folder.clone()).join( mod_file.file_path.clone());

                                    fs::copy(mod_file_path.clone(), mod_file.file_path.clone()).unwrap();

                                    match bar {
                                        Some(bar) => bar.inc(1),
                                        None => (),
                                    }

                                    sha_checker::write(mod_file.file_path.clone(), mod_file_path);
                                }
                                
                                match Launcher::launch_csb_editor(vec![file_name.chars().take(file_name.chars().count() - 4).collect::<String>()]) {
                                    Ok(mut child) => {
                                        match child.wait() {
                                            Ok(_) => (),
                                            Err(e) => println!("Error waiting for CsbEditor: {e}"),
                                        }
                                    },
                                    Err(e) => println!("Error launching CsbEditor: {e}"),
                                }
                            },
                            Err(e) => println!("Error waiting for CsbEditor: {e}"),
                        }
                    },
                    Err(e) => println!("Error launching CsbEditor: {e}"),
                }
            }
        }
    }
}

pub fn load_file_mods() {
    if !Path::new("mods/mods.ini").is_file() {
        help::print();
        return;
    }

    let file_patcher_config = common::settings::file_pathcer::load();
    if file_patcher_config.use_amb_rs_instead {
        println!("Using amb-rs instead of AMBPathcer");
    } else {
        println!("Using AMBPathcer...");
        match common::Launcher::launch_amb_patcher(vec![]) {
            Ok(mut child) => {
                match child.wait() {
                    Ok(_) => println!("AMBPatcher finished"),
                    Err(e) => println!("Error waiting for AMBPatcher: {e}"),
                }
            },
            Err(e) => println!("Error launching AMBPatcher: {e}"),
        }
        return;
    }

    println!("Preparing list of files to patch...");
    let files_that_i_have_to_patch = get_mod_files();
    println!("There are {} files to patch...", files_that_i_have_to_patch.len());
    let total_files_to_read: usize = files_that_i_have_to_patch.iter().map(|x| x.1.len()).sum();
    println!("And approximately {} files to read...", total_files_to_read);
    println!("Starting patching files of the game");
    let mut mods_prev = Vec::<String>::new();
    let mut modified_files = Vec::<String>::new();

    let bar = ProgressBar::new(total_files_to_read as u64);

    if Path::new("mods/mods_prev").is_file() {
        mods_prev = fs::read_to_string("mods/mods_prev").unwrap().lines().map(|x| x.to_string()).collect::<Vec<String>>();
    }

    for (key, value) in files_that_i_have_to_patch {
        modified_files.push(key.clone());

        backup(&key);
        //Some CSB files may have CPK archive
        if Path::new(&format!("{}.CPK", key.chars().take(key.len() - 4).collect::<String>())).is_file() {
            backup(&(key.chars().take(key.len() - 4).collect::<String>() + ".CPK"));
        }

        patch_all(&key, value, Some(&bar));
        mods_prev.retain(|x| x != &key);
    }

    for mod_file in mods_prev {
        println!("Recovering {}...", mod_file);
        recover(&mod_file);
        //Some CSB files may have CPK archive
        if mod_file.ends_with(".csb") || mod_file.ends_with(".CSB") {
            let mods_prev_path = mod_file.chars().take(mod_file.chars().count() - 4).collect::<String>();
            recover(&format!("{}.CPK", mods_prev_path));
            sha_checker::remove(&mods_prev_path);
        } else {
            sha_checker::remove(&mod_file);
        }
    }
    
    fs::write("mods/mods_prev", modified_files.join("\n")).unwrap();
    println!("\nPatching complete!");
}