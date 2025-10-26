use std::{collections::HashMap, fs, path::{Path, PathBuf}};

use common::Launcher;
use glob::glob;
use amb_rs_lib::{amb::Amb, amb_management};
use crate::{help, sha_checker};

pub fn recover() {
    if Path::new("mods/mods_prev").is_file() {
        let mods_prev = fs::read_to_string("mods/mods_prev").expect("Rolling around at error messages");
        let mods_prev = mods_prev.lines();

        for file in mods_prev {           
            // ProgressBar.PrintProgress(i, mods_prev.Length, "Recovering \""+ file +"\" file...");

            reecover(&file.to_string());
            if file.ends_with(".CSB") || file.ends_with(".csb") {
                let path_without_extension = file.chars().take(file.chars().count() - 4).collect::<String>();
                reecover( &format!("{}.CPK", path_without_extension));
                sha_checker::remove(path_without_extension);
            }
            else
            {
                sha_checker::remove(file.to_string());
            }
        }
        fs::remove_file("mods/mods_prev").unwrap();
        // ProgressBar.PrintProgress(1, 1, "");
    }

}

pub fn reecover(file_name: &String) {
    let backup_path = format!("{}.bkp", file_name);
    if Path::new(&backup_path).is_file() {
        fs::copy(backup_path, file_name).unwrap();
    }
}

fn get_mod_files() -> HashMap<String, Vec<(String, String)>> {
    /* returns a list of:
        * list[0].OrigFile = Path to original file
        * list[0].ModFiles = List of mod files
        * list[0].ModName = List of mod names of ModFiles
        */

    //Reading the mods.ini file
    if !Path::new("mods/mods.ini").is_file() {
        return HashMap::new();
    }

    //The mods.ini contains directory names of the enabled mods in reversed priority
    /*e.g.
        * Mod 3
        * Mod 2
        * Mod 1
        */

    
    //TODO: Make it work with forward order, remove Reverse()
    let mods_ini = common::mod_logic::existing_mod::ExistingMod::load("mods");
    let mods_ini = mods_ini
        .iter()
        .filter(|m| m.enabled)
        .rev()
        .collect::<Vec<_>>();

    let mut result: Vec<(String, String, String)> = Vec::new();

    for mmod in mods_ini {
        if !Path::new("mods").join(mmod.path.clone()).is_dir() {
            continue;
        }

        let filenames = glob(&format!("mods/{}/**/*.*", mmod.path));
        match filenames {
            Ok(filenames) => {
                for entry in filenames {
                    match entry {
                        Ok(path) => {
                            // glob returns all entries, even if it's a folder that contains files
                            if path.is_dir() {
                                continue;
                            }
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
                            // BUT WHY??????
                            let mod_path = filename_parts[1];

                            result
                                .push((
                                    original_file,
                                    mod_file.display().to_string(),
                                    mod_path.to_string(),
                                ));
                        },
                        Err(e) => println!("Glob error: {}", e),
                    }
                }
            },
            Err(e) => println!("Error: {e}"),
        }
    }

    let mut grouped: HashMap<String, Vec<(String, String)>> = HashMap::new();

    for (original_file, mod_file, mod_path) in result {
        let list = grouped.entry(original_file).or_insert(Vec::new());
        match list.last() {
            Some(last) => {
                if mod_file.contains(&last.0) {
                    continue;
                }
            },
            None => (),
        }
        list.push((mod_file, mod_path));
    }

    println!("{:?}", grouped);
    
    grouped
}

pub fn backup(file_name: &String) {
    let backup_path = format!("{}.bkp", &file_name);
    if !Path::new(&backup_path).exists() && Path::new(&file_name).is_file() { 
        fs::copy(&file_name, backup_path).unwrap();
    }
}

pub fn patch_all(file_name: &String, mod_files: &Vec<String>, mod_paths: &Vec<String>) {
    if Path::new(&file_name).is_file() {
        if file_name.ends_with(".AMB") || file_name.ends_with(".amb")
        {
            if sha_checker::is_changed(true , file_name, mod_files, mod_paths)
            {
                if file_name == mod_files.first().unwrap_or(&"".to_string()) {
                    let mod_full = Path::new("mods").join(mod_paths[0].clone()).join(mod_files[0].clone());
                    println!("+++++++++++++++++");
                    println!("{}", mod_files[0]);
                    println!("{:?}", mod_full);
                    println!("+++++++++++++++++");
                    fs::copy(&mod_full, file_name).unwrap();
                    sha_checker::write(mod_files[0].clone(), mod_full);
                    return;
                }

                let mut amb = Amb::new_from_file_name(&match Path::new(&format!("{}.bkp", file_name)).is_file() {
                    true => format!("{}.bkp", file_name),
                    false => file_name.to_string(),
                }).expect("I'm runnning out of error messages");
                amb.amb_path = file_name.to_string();

                let mut i = 0;
                while i < mod_files.len() {
                    let mod_file_full = Path::new("mods").join(mod_paths[i].clone()).join(mod_files[i].clone());
                    // ProgressBar.PrintProgress(i, mod_files.Count, mod_file_full);
                    amb_management::add::file::add_file_to_amb(&mut amb, &mod_file_full, None).unwrap();
                    sha_checker::write(mod_files[i].clone(), mod_file_full);
                    i += 1;
                }

                match fs::write(file_name, amb.write()) {
                    Ok(_) => {},
                    Err(e) => println!("Error writing AMB file: {e}"),
                }
            }
        }
        else if file_name.ends_with(".csb") || file_name.ends_with(".CSB")
        {
            if sha_checker::is_changed(true, &file_name.chars().take(file_name.chars().count() - 4).collect::<String>(), mod_files, mod_paths)
            {
                reecover(file_name);
                reecover(&format!("{}.CPK", &file_name.chars().take(file_name.chars().count() - 4).collect::<String>()));

                // ProgressBar.PrintProgress(0, 100, "Asking CsbEditor to unpack " + file_name);

                match Launcher::launch_csb_editor(vec![file_name.to_string()]) {
                    Ok(_) => {
                        let mut i = 0;
                        while i < mod_files.len() {
                            let mod_file = Path::new("mods").join(mod_paths[i].clone()).join( mod_files[i].clone());
                            
                            // ProgressBar.PrintProgress(i, mod_files.Count, mod_file);
                            fs::copy(mod_file.clone(), mod_files[i].clone()).unwrap();
                            
                            sha_checker::write(mod_files[i].clone(), mod_file);
                            i += 1;
                        }
                        
                        // ProgressBar.PrintProgress(99, 100, "Asking CsbEditor to repack " + file_name);
                        Launcher::launch_csb_editor(vec![file_name.chars().take(file_name.chars().count() - 4).collect::<String>()]).unwrap();
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

    let test = get_mod_files();
    let mut mods_prev = Vec::<String>::new();
    let mut modified_files = Vec::<String>::new();

    if Path::new("mods/mods_prev").is_file() {
        mods_prev = fs::read_to_string("mods/mods_prev").unwrap().lines().map(|x| x.to_string()).collect::<Vec<String>>();
    }

    // ProgressBar.PrintFiller();
    // ProgressBar.MoveCursorUp();

    for (key, value) in test {
        modified_files.push(key.clone());
        
        // ProgressBar.PrintProgress(i, test.Count, "Modifying \"" + test[i].OrigFile + "\"...");
        // ProgressBar.MoveCursorDown();

        backup(&key);
        //Some CSB files may have CPK archive
        if Path::new(&format!("{}.CPK", key.chars().take(key.len() - 4).collect::<String>())).is_file() {
            backup(&(key.chars().take(key.len() - 4).collect::<String>() + ".CPK"));
        }

        patch_all(&key, &value.iter().map(|x| x.0.clone()).collect(), &value.iter().map(|x| x.1.clone()).collect());
        // TODO: burn it with fire
        mods_prev = mods_prev.iter().filter(|x | x.to_string() != key).map(|x| x.to_string()).collect();

        // ProgressBar.MoveCursorUp();
    }

    // ProgressBar.PrintProgress(1, 1, "");
    // ProgressBar.MoveCursorDown();
    // ProgressBar.PrintProgress(1, 1, "");

    let mut i = 0;
    while i < mods_prev.len() {
        reecover(&mods_prev[i]);
        //Some CSB files may have CPK archive
        if mods_prev[i].ends_with(".csb") || mods_prev[i].ends_with(".CSB") {
            let mods_prev_path = mods_prev[i].chars().take(mods_prev[i].chars().count() - 4).collect::<String>();
            reecover(&format!("{}.CPK", mods_prev_path));
            sha_checker::remove(mods_prev_path);
        } else {
            sha_checker::remove(mods_prev[i].clone());
        }
        i += 1;
    }
    
    fs::write("mods/mods_prev", modified_files.join("\n")).unwrap();
}