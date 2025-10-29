use std::{fs, path::Path, process};

use common::{settings, Game, Launcher};

pub enum InstallationStatus {
    Installed,
    NotInstalled,
    FirstLaunch,
    NotGameDirectory,
}

pub fn get_installation_status() -> InstallationStatus {
    let game = Launcher::get_current_game();
    let aml_config = settings::alice_mod_loader::load();
    let check_launcher = cfg!(windows);
    
    match game {
        Game::Unknown => return InstallationStatus::NotGameDirectory,
        Game::Episode1 => {
            if (!check_launcher || Path::new("SonicLauncher.orig.exe").exists())
                && aml_config == "Sonic4FilePatcher.exe" {
                return InstallationStatus::Installed;
            }
        },
        Game::Episode2 => {
            if (!check_launcher || Path::new("Launcher.orig.exe").exists())
                && aml_config == "Sonic4FilePatcher.exe" {
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
    pub only_if_this_file_exists: Option<String>,
    pub modloader_file: bool,
}

impl InstallationInstruction {
    pub fn new(orig_name: String, new_name: Option<String>, only_if_this_file_exists: Option<String>, modloader_file: bool) -> InstallationInstruction {
        InstallationInstruction { orig_name, new_name, only_if_this_file_exists, modloader_file }
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
    installation_order.push(InstallationInstruction::new(
        original_launcher.clone(),
        Some(original_launcher_bkp.clone()),
        Some(manager_launcher.to_string()),
        false )
    );
    installation_order.push(InstallationInstruction::new(
        manager_launcher.to_string(),
        Some(original_launcher),
        Some(original_launcher_bkp.clone()),
        true)
    );
    
    //Mod Loader files
    installation_order.push(InstallationInstruction::new("7z.exe".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("7z.dll".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("AMBPatcher.exe".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("amb-rs.exe".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("Sonic4FilePatcher.exe".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("CsbEditor.exe".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("Mod Loader - Whats new.txt".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("README.md".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("SonicAudioLib.dll".to_string(), None, None, true));

    installation_order.push(InstallationInstruction::new("AMBPatcher.cfg".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("CsbEditor.exe.config".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("ModManager.cfg".to_string(), None, None, true));

    installation_order.push(InstallationInstruction::new("Mod Loader - licenses".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("AML".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("d3d9.dll".to_string(), None, None, true));

    installation_order.push(InstallationInstruction::new("lib".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("share".to_string(), None, None, true));
    installation_order.push(InstallationInstruction::new("bin".to_string(), None, None, true));

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
        if Path::new(&i.orig_name).exists()
            && i.new_name.is_some()
            && !Path::new(&i.new_name.clone().unwrap()).exists()
            && match i.only_if_this_file_exists { Some(path) => Path::new(&path).exists(), None => true, } {
            match fs::rename(&i.orig_name, &i.new_name.unwrap()) {
                Ok(_) => (),
                Err(e) => println!("Failed to move some files: {}", e)
            }
        }
    }

    match fs::write("ModManager.cfg", "") {
        Ok(_) => (),
        Err(e) => println!("Couldn't write ModManager.cfg: {}", e),
    }
    settings::alice_mod_loader::save("Sonic4FilePatcher.exe");
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
        if i.new_name.is_some()
            && Path::new(&i.new_name.clone().unwrap()).exists()
            && !Path::new(&i.orig_name).exists()
            && match &i.only_if_this_file_exists { Some(path) => Path::new(&path).exists(), None => true } {
            match fs::rename(i.new_name.clone().unwrap(), &i.orig_name) {
                Ok(_) => (),
                Err(e) => println!("Error: {}", e),
            }
        }
    }

    settings::alice_mod_loader::save("");

    if options.recover_original_files {
        match process::Command::new("AMBPatcher.exe")
            .arg("recover")
            .output() {
            Ok(_) => println!("Original files should be recovered"),
            Err(e) => println!("Error: {}", e),
        }

        if options.delete_all_mod_loader_files {
            match fs::remove_dir_all("mods_sha") {
                Ok(_) => println!("Removed directory [mods_sha]"),
                Err(e) => println!("Error removing directory [mods_sha]: {}", e)
            }
        }
    }

    if options.uninstall_and_delete_ocmi {
        if Path::new("OneClickModInstaller.exe").exists() {
            match process::Command::new("OneClickModInstaller.exe")
                .arg("--uninstall")
                .output() {
                Ok(_) => println!("OneClickModInstaller should be uninstalled"),
                Err(e) => println!("Error: {}", e),
            }

            match fs::remove_file("OneClickModInstaller.exe") {
                Ok(_) => println!("OneClickModInstaller.exe deleted"),
                Err(e) => println!("Error removing file [OneClickModInstaller.exe]: {}", e)
            }
        }

        if !options.keep_configs {
            match fs::remove_file("OneClickModInstaller.cfg") {
                Ok(_) => (),
                Err(e) => println!("Error removing file [OneClickModInstaller.cfg]: {}", e)
            };
        }
    }

    if options.delete_all_mod_loader_files {
        for file in installation_order {
            if file.modloader_file && Path::new(&file.orig_name).exists() {
                if !options.keep_configs || !(options.keep_configs && (file.orig_name.ends_with(".cfg") || file.orig_name.ends_with(".config"))) {
                    if Path::new(&file.orig_name).is_file() {
                        match fs::remove_file(&file.orig_name) {
                            Ok(_) => println!("Removed file [{}]", file.orig_name),
                            Err(e) => println!("Error removing file [{}]: {}", file.orig_name, e),
                        }
                    } else if Path::new(&file.orig_name).is_dir() {
                        match fs::remove_dir_all(&file.orig_name) {
                            Ok(_) => println!("Removed directory [{}]", file.orig_name),
                            Err(e) => println!("Error removing directory [{}]: {}", file.orig_name, e),
                        }
                    }
                }
            }
        }

        let bat = format!("{}\n{}\n{}",
            "taskkill /IM Sonic4ModManager.exe /F",
            "DEL Sonic4ModManager.exe",
            "DEL FinishInstallation.bat");
        
        match fs::write("FinishInstallation.bat", bat) {
            Ok(_) => {
                match process::Command::new("FinishInstallation.bat").spawn() {
                    Ok(_) => process::exit(0),
                    Err(e) => println!("Error launching FinishInstallation.bat: {}", e),
                }
            },
            Err(e) => println!("Error writing FinishInstallation.bat: {}", e),
        }
    }
}