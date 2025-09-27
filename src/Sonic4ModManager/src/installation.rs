use std::{fs, path::Path, process};

use common::{Game, Launcher};

pub enum InstallationStatus {
    Installed,
    NotInstalled,
    FirstLaunch,
    NotGameDirectory,
}

pub fn get_installation_status() -> InstallationStatus {
    let game = Launcher::get_current_game();
    
    match game {
        Game::Unknown => return InstallationStatus::NotGameDirectory,
        Game::Episode1 => {
            if Path::new("SonicLauncher.orig.exe").exists() {
                return InstallationStatus::Installed;
            }
        },
        Game::Episode2 => {
            if Path::new("Launcher.orig.exe").exists() {
                return InstallationStatus::Installed;
            }
        }
    }

    if !Path::new("ModManager.cfg").exists() {
        return InstallationStatus::FirstLaunch;
    }

    return InstallationStatus::NotInstalled;
}

pub struct InstallationInstruction {
    pub orig_name: String,
    pub new_name: Option<String>,
    pub modloader_file: bool,
}

impl InstallationInstruction {
    pub fn new(orig_name: String, new_name: Option<String>, modloader_file: bool) -> InstallationInstruction {
        InstallationInstruction { orig_name, new_name, modloader_file }
    }
}

fn get_installation_order() -> Vec<InstallationInstruction> {
    let game = Launcher::get_current_game();
    let mut installation_order = Vec::<InstallationInstruction>::new();

    let manager_launcher = "ManagerLauncher_link.exe";

    let original_launcher = match game {
        Game::Episode1 => "SonicLauncher",
        Game::Episode2 => "Launcher",
        _ => return vec![],
    };

    let original_launcher_bkp = format!("{}.orig.exe", original_launcher);
    let original_launcher = format!("{}.exe", original_launcher);

    //Original files
    installation_order.push(InstallationInstruction::new(original_launcher.clone(), Some(original_launcher_bkp), false ));
    installation_order.push(InstallationInstruction::new(manager_launcher.to_string(), Some(original_launcher), true));
    
    //Mod Loader files
    installation_order.push(InstallationInstruction::new("7z.exe".to_string(), None, true));
    installation_order.push(InstallationInstruction::new("7z.dll".to_string(), None, true));
    installation_order.push(InstallationInstruction::new("AMBPatcher.exe".to_string(), None, true));
    installation_order.push(InstallationInstruction::new("CsbEditor.exe".to_string(), None, true));
    installation_order.push(InstallationInstruction::new("Mod Loader - Whats new.txt".to_string(), None, true));
    installation_order.push(InstallationInstruction::new("README.md".to_string(), None, true));
    installation_order.push(InstallationInstruction::new("SonicAudioLib.dll".to_string(), None, true));

    installation_order.push(InstallationInstruction::new("AMBPatcher.cfg".to_string(), None, true));
    installation_order.push(InstallationInstruction::new("CsbEditor.exe.config".to_string(), None, true));
    installation_order.push(InstallationInstruction::new("ModManager.cfg".to_string(), None, true));

    installation_order.push(InstallationInstruction::new("Mod Loader - licenses".to_string(), None, true));
    installation_order.push(InstallationInstruction::new("AML".to_string(), None, true));
    installation_order.push(InstallationInstruction::new("d3d9.dll".to_string(), None, true));

    installation_order.push(InstallationInstruction::new("lib".to_string(), None, true));
    installation_order.push(InstallationInstruction::new("share".to_string(), None, true));
    installation_order.push(InstallationInstruction::new("bin".to_string(), None, true));

    installation_order
}

pub fn install() {
    let status = get_installation_status();

    match status {
        InstallationStatus::NotInstalled => (),
        InstallationStatus::FirstLaunch => (),
        _ => return,
    }

    let installation_order = get_installation_order();
    for i in installation_order {
        if Path::new(&i.orig_name).exists() && i.new_name.is_some() && !Path::new(&i.new_name.clone().unwrap()).exists() {
            fs::rename(&i.orig_name, &i.new_name.unwrap());
        }
    }

    fs::write("ModManager.cfg", "");
    write_aml_config("AMBPatcher.exe");
}

pub fn write_empty_config() {
    fs::write("ModManager.cfg", "");
}

pub fn write_aml_config(patcher_dir: &str) {
    let ini_aml = fs::read_to_string("AML/AliceML.ini").unwrap();
    let mut result_lines = Vec::<String>::new();
    for line in ini_aml.lines() {
        if line.starts_with("PatcherDir=") {
            result_lines.push(format!("PatcherDir={}", patcher_dir));
        } else {
            result_lines.push(line.to_string());
        }
    }
            
    fs::write("AML/AliceML.ini", result_lines.join("\n"));
}

pub struct UninstallationOptions {
    pub recover_original_files: bool,
    pub delete_all_mod_loader_files: bool,
    pub uninstall_and_delete_ocmi: bool,
    pub keep_configs: bool,
}

pub fn uninstall(options: UninstallationOptions) {
    let installation_order = {
        let mut order = get_installation_order();
        order.reverse();
        order
    };

    for i in &installation_order {
        if i.new_name.is_some() && Path::new(&i.new_name.clone().unwrap()).exists() && !Path::new(&i.orig_name).exists() {
            fs::rename(i.new_name.clone().unwrap(), &i.orig_name);
        }
    }

    write_aml_config("");

    if options.recover_original_files {
        let result = process::Command::new("AMBPatcher.exe")
            .arg("recover")
            .output();
        if result.is_err() {
            println!("Error: {}", result.err().unwrap());
        }

        if options.delete_all_mod_loader_files && Path::new("mods_sha").exists() {
            fs::remove_dir_all("mods_sha");
        }
    }

    if options.uninstall_and_delete_ocmi {
        if Path::new("OneClickModInstaller.exe").exists() {
            let result = process::Command::new("OneClickModInstaller.exe")
                .arg("--uninstall")
                .output();
            if result.is_err() {
                println!("Error: {}", result.err().unwrap());
            }

            fs::remove_file("OneClickModInstaller.exe");
        }

        if !options.keep_configs && Path::new("OneClickModInstaller.cfg").exists() {
            fs::remove_file("OneClickModInstaller.cfg");
        }
    }

    if options.delete_all_mod_loader_files {
        for file in installation_order {
            if file.modloader_file && Path::new(&file.orig_name).exists() {
                if (!options.keep_configs || !(options.keep_configs && (file.orig_name.ends_with(".cfg") || file.orig_name.ends_with(".config")))) {
                    if Path::new(&file.orig_name).is_file() {
                        fs::remove_file(file.orig_name);
                    } else if Path::new(&file.orig_name).is_dir() {
                        fs::remove_dir_all(file.orig_name);
                    }
                }
            }
        }

        let bat = format!("{}\n{}\n{}",
            "taskkill /IM Sonic4ModManager.exe /F",
            "DEL Sonic4ModManager.exe",
            "DEL FinishInstallation.bat");
        fs::write("FinishInstallation.bat", bat);

        process::Command::new("FinishInstallation.bat");
        process::exit(0);
    }
}