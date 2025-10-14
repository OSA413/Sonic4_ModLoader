use std::{fs::{self, File}, path::{Path, PathBuf}};

use glob::glob;
use sha1::{Digest, Sha1};

use crate::help;

pub fn recover() {
    todo!();
}

pub fn get_sha1(data: impl AsRef<[u8]>) -> String {
    Sha1::digest(data).iter().map(|x| format!("{:02x}", x)).collect()
}

fn GetModFiles() -> Vec<(String, Vec<String>, Vec<String>)> {
    /* returns a list of:
        * list[0].OrigFile = Path to original file
        * list[0].ModFiles = List of mod files
        * list[0].ModName = List of mod names of ModFiles
        */

    let mut result = Vec::new();

    //Reading the mods.ini file
    if !Path::new("mods/mods.ini").is_file() {
        return result;
    }

    //The mods.ini contains directory names of the enabled mods in reversed priority
    /*e.g.
        * Mod 3
        * Mod 2
        * Mod 1
        */

    
    //TODO: Make it work with forward order, remove Reverse()
    let modsIni = common::mod_logic::existing_mod::ExistingMod::load("mods/mods.ini");
    let modsIni = modsIni
        .iter()
        .filter(|m| m.enabled)
        .rev()
        .collect::<Vec<_>>();

    for mmod in modsIni {
        if !Path::new("mods").join(mmod.path.clone()).is_dir() {
            continue;
        }

        let filenames = glob(&format!("mods/{}/**/*.*", mmod.path));
        match filenames {
            Ok(filenames) => {
                for entry in filenames {
                    match entry {
                        Ok(path) => {
                            let mut mod_files = Vec::<String>::new();
                            let mut mod_dirs = Vec::<String>::new();

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

                            if mod_files.iter().find(|x | x.to_string() == mod_file.display().to_string()).is_some()
                            {
                                /* Updating queue of the files that will be modified
                                * to correspond to the given mod priority.
                                * This is needed because the old single-depth no replacing
                                * method doesn't work correctly (theoretically) after some new features.
                                * Before it was not a queue at all.
                                * Thank you for your attention.
                                * 
                                * ~OSA413
                                */
                                let mod_index = mod_files.iter().position(|mods| mods.to_string() == mod_file.display().to_string()).unwrap();
                                mod_files.remove(mod_index);
                                mod_dirs.remove(mod_index);
                            }

                            mod_files.push(mod_file.display().to_string());
                            mod_dirs.push(mod_path.to_string());

                            if result.iter().find(|mods| mods.0 == original_file).is_none()
                            {
                                result
                                    .push((
                                        original_file,
                                        mod_files,
                                        mod_dirs,
                                    ));
                            }
                        },
                        Err(e) => println!("Glob error: {}", e),
                    }
                }
            },
            Err(e) => println!("Error: {e}"),
        }
    }
    
    result
}

pub fn backup(file_name: &String) {
    let backup_path = format!("{}.bkp", &file_name);
    if !Path::new(&backup_path).exists() && Path::new(&file_name).is_file() { 
        fs::copy(&file_name, backup_path);
    }
}

pub fn PatchAll(file_name: &String, mod_files: &Vec<String>, mod_paths: &Vec<String>) {
    if Path::new(&file_name).is_file() {
        todo!();
    //     if file_name.ends_with(".AMB") || file_name.ends_with(".amb")
    //     {
    //         if (ShaChecker.ShaChanged(file_name, mod_files, mod_paths))
    //         {
    //             if (file_name == mod_files.FirstOrDefault())
    //             {
    //                 var modFull = Path.Combine("mods", mod_paths[0], mod_files[0]);
    //                 File.Copy(modFull, file_name, true);
    //                 ShaChecker.ShaWrite(mod_files[0], modFull);
    //                 return;
    //             }

    //             AMB amb = new(File.Exists(file_name + ".bkp") ? file_name + ".bkp" : file_name);
    //             amb.AmbPath = file_name;

    //             for (int i = 0; i < mod_files.Count; i++)
    //             {
    //                 var mod_file_full = Path.Combine("mods", mod_paths[i], mod_files[i]);
    //                 ProgressBar.PrintProgress(i, mod_files.Count, mod_file_full);
    //                 amb.Add(mod_file_full);
    //                 ShaChecker.ShaWrite(mod_files[i], mod_file_full);
    //             }

    //             amb.Save(file_name);
    //         }
    //     }
    //     else if (file_name.ToUpper().EndsWith(".CSB"))
    //     {
    //         if (ShaChecker.ShaChanged(file_name.Substring(0, file_name.Length - 4), mod_files, mod_paths))
    //         {
    //             Recover(file_name);
    //             if (file_name.EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
    //                 Recover(file_name.Substring(0, file_name.Length - 4) + ".CPK");

    //             ProgressBar.PrintProgress(0, 100, "Asking CsbEditor to unpack " + file_name);

    //             //Needs CSB Editor (from SonicAudioTools) to work
    //             //FIXME
    //             if (!Launcher.LaunchCsbEditor(file_name))
    //                 throw new Exception("CsbEditor not found (PatchAll)");

    //             for (int i = 0; i < mod_files.Count; i++)
    //             {
    //                 string mod_file = Path.Combine("mods", mod_paths[i], mod_files[i]);

    //                 ProgressBar.PrintProgress(i, mod_files.Count, mod_file);
    //                 File.Copy(mod_file, mod_files[i], true);

    //                 ShaChecker.ShaWrite(mod_files[i], mod_file);
    //             }

    //             ProgressBar.PrintProgress(99, 100, "Asking CsbEditor to repack " + file_name);
    //             Launcher.LaunchCsbEditor(file_name.Substring(0, file_name.Length - 4));
    //         }
    //     }
    }
}

pub fn load_file_mods() {
    if !Path::new("mods/mods.ini").is_file() {
        help::print();
        return;
    }

    let test = GetModFiles();
    let mut mods_prev = Vec::<String>::new();
    let mut modified_files = Vec::<String>::new();

    if Path::new("mods/mods_prev").is_file() {
        mods_prev = fs::read_to_string("mods/mods_prev").unwrap().lines().map(|x| x.to_string()).collect::<Vec<String>>();
    }

    // ProgressBar.PrintFiller();
    // ProgressBar.MoveCursorUp();

    let mut i = 0;
    while i < test.len() {
        modified_files.push(test[i].0.clone());
        
        // ProgressBar.PrintProgress(i, test.Count, "Modifying \"" + test[i].OrigFile + "\"...");
        // ProgressBar.MoveCursorDown();

        backup(&test[i].0);
        //Some CSB files may have CPK archive
        if Path::new(&format!("{}.CPK", test[i].0.chars().take(test[i].0.len() - 4).collect::<String>())).is_file() {
            backup(&(test[i].0.chars().take(test[i].0.len() - 4).collect::<String>() + ".CPK"));
        }

        PatchAll(&test[i].0, &test[i].1, &test[i].2);
        // fnaionfioanfoiwandioawdioaw
        mods_prev = mods_prev.iter().filter(|x | x.to_string() != test[i].0).map(|x| x.to_string()).collect();

        // ProgressBar.MoveCursorUp();
        i += 1;
    }

    // ProgressBar.PrintProgress(1, 1, "");
    // ProgressBar.MoveCursorDown();
    // ProgressBar.PrintProgress(1, 1, "");

    let mut i = 0;

    todo!();
    // while i < mods_prev.len() {
    //     Recover(mods_prev[i]);
    //     //Some CSB files may have CPK archive
    //     if (mods_prev[i].EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
    //         Recover(mods_prev[i].Substring(0, mods_prev[i].Length - 4) + ".CPK");
        
    //     if (mods_prev[i].EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
    //         ShaChecker.ShaRemove(mods_prev[i].Substring(0, mods_prev[i].Length - 4));
    //     else
    //         ShaChecker.ShaRemove(mods_prev[i]);
    // }
    // 
    // if Directory.Exists("mods") {
    //     File.WriteAllText("mods/mods_prev", string.Join("\n", modified_files.ToArray()));
    // }
}