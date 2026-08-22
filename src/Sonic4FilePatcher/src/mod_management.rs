use std::{collections::HashMap, ffi::OsStr, fs, path::{Path, PathBuf}};

use amb_rs_lib::amb::Amb;
use common_binary::error::CommonBinaryError::{self, Description};
use common_modloader::Launcher;
use crate::{help, sha_checker};
use indicatif::ProgressBar;

pub fn full_recover_of_files() -> Result<(), CommonBinaryError> {
    let files_to_recover = common_utils::walk_dir::walk_dir(Path::new("."), Some(OsStr::new("bkp")));

    for file in files_to_recover {
        let file_string_path = &file.to_string_lossy().to_string();
        recover_from_bkp(&file.to_string_lossy().to_string())?;
        if file.ends_with(".CSB.bkp") || file.ends_with(".csb.bkp") {
            let path_without_extension = file_string_path.chars().take(file_string_path.chars().count() - 4 - 4).collect::<String>();
            recover_from_bkp( &format!("{path_without_extension}.CPK.bkp"))?;
            sha_checker::remove(&path_without_extension);
        }
        sha_checker::remove(file_string_path);
    }
    Ok(())
}

pub fn recover(file_name: &String) -> Result<u64, std::io::Error>{
    let backup_path = format!("{file_name}.bkp");
    fs::copy(backup_path, file_name)
}

pub fn recover_from_bkp(backup_file_name: &String) -> Result<u64, std::io::Error> {
    let original_path = backup_file_name.chars().take(backup_file_name.chars().count() - 4).collect::<String>();
    fs::copy(backup_file_name, original_path)
}

pub struct ModFile {
    pub file_path: String,
    pub mod_folder: String,
}

fn get_mod_files() -> HashMap<String, Vec<ModFile>> {
    if !Path::new("mods/mods.ini").is_file() {
        return HashMap::new();
    }
    
    let mods_ini = common_modloader::mod_logic::existing_mod::ExistingMod::load("mods");
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

        let paths = {
            let mut paths = common_utils::walk_dir::walk_dir(&Path::new("mods").join(mmod.path.clone()), None);
            paths.sort_by_key(|a| a.display().to_string());
            // Needs confirmation
            // // We have to replicate .Net default sort to not break some mods that rely on file addition order
            // // For example, SSON_DRAG.ZNM must come after SSON_DRAG_L.ZNM keeping overal file order the same
            // // TODO: optimize
            // // paths.sort_by(
            // //     |a, b| 
            // //         a.display().to_string().replace("_", ".")
            // //             .cmp(&b.display().to_string().replace("_", "."))
            // // );
            paths
        };

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
                else if Path::new(&format!("{}.CSB", possible_orig_file.display())).is_file()
                {
                    original_file = format!("{}.CSB", possible_orig_file.display());
                    break;
                }
                k += 1;
            }

            if original_file.is_empty() {
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
        let list = grouped.entry(original_file).or_default();
        if let Some(index) = list.iter().position(|existing_file| mod_file.file_path.contains(&existing_file.file_path)) {
            list[index] = mod_file;
        } else {
            list.push(mod_file);
        }
    }
    
    grouped
}

pub fn backup(file_name: &String) -> Result<(), CommonBinaryError> {
    let backup_path = format!("{}.bkp", &file_name);
    if !Path::new(&backup_path).exists() && Path::new(&file_name).is_file() { 
        fs::copy(file_name, backup_path)?;
    }
    Ok(())
}

pub fn patch_all(file_name: &String, mod_files: Vec<ModFile>, bar: Option<&ProgressBar>) -> Result<(), CommonBinaryError> {
    if mod_files.is_empty() {
        return Ok(());
    }

    if Path::new(&file_name).is_file() {
        if file_name.ends_with(".AMB") || file_name.ends_with(".amb")
        {
            if sha_checker::is_changed(true , file_name, &mod_files)
            {
                if file_name == &mod_files.first().ok_or_else(|| Description("A error at `&mod_files.first()`".to_string()))?.file_path {
                    let mod_full = Path::new("mods").join(mod_files[0].mod_folder.clone()).join(mod_files[0].file_path.clone());
                    fs::copy(&mod_full, file_name)?;
                    sha_checker::write(mod_files[0].file_path.clone(), mod_full);
                    if let Some(bar) = bar { bar.inc(1) }
                    return Ok(());
                }

                let mut amb = Amb::new_from_file_name(&match Path::new(&format!("{file_name}.bkp")).is_file() {
                    true => format!("{file_name}.bkp"),
                    false => file_name.to_string(),
                }).expect("I'm runnning out of error messages");
                amb.amb_path = file_name.to_string();

                for mod_file in mod_files {
                    let mod_file_full = Path::new("mods").join(mod_file.mod_folder.clone()).join(mod_file.file_path.clone());
                    if let Some(bar) = bar { bar.inc(1) }
                    amb.add_file(&mod_file_full, None)?;
                    sha_checker::write(mod_file.file_path.clone(), mod_file_full);
                }

                match amb.write() {
                    Ok(content) => {
                        match fs::write(file_name, content) {
                            Ok(_) => {},
                            Err(e) => eprintln!("Error writing AMB file: {e}"),
                        }
                    }
                    Err(e) => eprintln!("Error creating AMB file: {e:?}"),
                }
            }
        }
        else if (file_name.ends_with(".csb") || file_name.ends_with(".CSB"))
            && sha_checker::is_changed(true, &file_name.chars().take(file_name.chars().count() - 4).collect::<String>(), &mod_files)
            {
                recover(file_name)?;
                recover(&format!("{}.CPK", &file_name.chars().take(file_name.chars().count() - 4).collect::<String>()))?;

                match Launcher::launch_csb_editor(vec![file_name.to_string()]) {
                    Ok(mut child) => {
                        match child.wait() {
                            Ok(_) => {
                                for mod_file in mod_files {
                                    let mod_file_path = Path::new("mods").join(mod_file.mod_folder.clone()).join( mod_file.file_path.clone());

                                    fs::copy(mod_file_path.clone(), mod_file.file_path.clone())?;

                                    if let Some(bar) = bar { bar.inc(1) }

                                    sha_checker::write(mod_file.file_path.clone(), mod_file_path);
                                }
                                
                                match Launcher::launch_csb_editor(vec![file_name.chars().take(file_name.chars().count() - 4).collect::<String>()]) {
                                    Ok(mut child) => {
                                        match child.wait() {
                                            Ok(_) => (),
                                            Err(e) => eprintln!("Error waiting for CsbEditor: {e}"),
                                        }
                                    },
                                    Err(e) => eprintln!("Error launching CsbEditor: {e}"),
                                }
                            },
                            Err(e) => eprintln!("Error waiting for CsbEditor: {e}"),
                        }
                    },
                    Err(e) => eprintln!("Error launching CsbEditor: {e}"),
                }
            }
    }

    Ok(())
}

pub fn load_file_mods() -> Result<(), CommonBinaryError> {
    if !Path::new("mods/mods.ini").is_file() {
        help::print();
        return Ok(());
    }

    println!("Preparing list of files to patch...");
    let files_that_i_have_to_patch = get_mod_files();
    println!("There are {} files to patch...", files_that_i_have_to_patch.len());
    let total_files_to_read: usize = files_that_i_have_to_patch.iter().map(|x| x.1.len()).sum();
    println!("And approximately {total_files_to_read} files to read...");
    println!("Starting patching files of the game");
    let mut modified_files = Vec::<String>::new();

    let bar = ProgressBar::new(total_files_to_read as u64);

    let mut mods_prev = match fs::read_to_string("mods/mods_prev") {
        Ok(data) => data.lines().map(|x| x.to_string()).collect::<Vec<String>>(),
        Err(_) => {
            full_recover_of_files()?;
            Vec::new()
        },
    };

    for (key, value) in files_that_i_have_to_patch {
        modified_files.push(key.clone());

        backup(&key)?;
        //Some CSB files may have CPK archive
        if Path::new(&format!("{}.CPK", key.chars().take(key.len() - 4).collect::<String>())).is_file() {
            backup(&(key.chars().take(key.len() - 4).collect::<String>() + ".CPK"))?;
        }

        patch_all(&key, value, Some(&bar))?;
        mods_prev.retain(|x| x != &key);
    }

    for mod_file in mods_prev {
        recover(&mod_file)?;
        //Some CSB files may have CPK archive
        if mod_file.ends_with(".csb") || mod_file.ends_with(".CSB") {
            let mods_prev_path = mod_file.chars().take(mod_file.chars().count() - 4).collect::<String>();
            recover(&format!("{mods_prev_path}.CPK"))?;
            sha_checker::remove(&mods_prev_path);
        }
        sha_checker::remove(&mod_file);
    }
    
    match fs::write("mods/mods_prev", modified_files.join("\n")) {
        Ok(_) => (),
        Err(e) => eprintln!("Couldn't write contents to mods/mods_prev, this means that the next launch will re-patch files again: {e}")
    }
    println!("\nPatching complete!");
    Ok(())
}