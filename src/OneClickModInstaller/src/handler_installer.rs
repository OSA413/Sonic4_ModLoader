use common::{Launcher, Game};

pub enum InstallationInfo {
    Installed(String),
    AnotherInstallationPresent(String),
    NotInstalled,
}

#[cfg(not(target_os = "windows"))]
pub fn install(game: Option<Game>) {
    todo!()
}
#[cfg(not(target_os = "windows"))]
pub fn uninstall(game: Option<Game>) {
    todo!()
}

#[cfg(not(target_os = "windows"))]
pub fn fix(game: Option<Game>) {
    todo!()
}

#[cfg(target_os = "windows")]
pub fn get_info(game: Option<Game>) -> (Game, InstallationInfo) {
    use std::path::Path;
    use winreg::HKCU;

    let check_another_installation = game.clone().is_none_or(|game| game == Launcher::get_current_game());
    let game_to_check = game.unwrap_or(Launcher::get_current_game());
    let game = match game_to_check {
        Game::Episode1 => "ep1",
        Game::Episode2 => "ep2",
        Game::Unknown => return (game_to_check, InstallationInfo::NotInstalled)
    };
    let formatted_game = format!("sonic4mm{game}");

    let root_path = Path::new("Software").join("Classes").join(&formatted_game);

    match HKCU.open_subkey(&root_path) {
        Ok(_) => {
            let shell_path = Path::new(&root_path).join("Shell").join("Open").join("Command");
            match HKCU.open_subkey(&shell_path) {
                Ok(shell_key) => {
                    let current_path = std::env::current_exe().unwrap();
                    let current_path = current_path.display().to_string();
                    match shell_key.get_value::<String, _>("") {
                        Ok(value) => {
                            let installed_path = value.chars().skip(1).take("\" \"%1\"".len()).collect::<String>();
                            if installed_path == current_path {
                                return (game_to_check, InstallationInfo::Installed(installed_path))
                            }
                            if check_another_installation {
                                return (game_to_check, InstallationInfo::AnotherInstallationPresent(installed_path))
                            }
                            return (game_to_check, InstallationInfo::NotInstalled)
                        }
                        Err(_) => (game_to_check, InstallationInfo::NotInstalled)
                    }
                }
                Err(_) => (game_to_check, InstallationInfo::NotInstalled)
            }
        }
        Err(e) => (game_to_check, InstallationInfo::NotInstalled)
    }
}

#[cfg(target_os = "windows")]
pub fn install(game: Option<Game>) {
    use std::path::Path;
    use winreg::HKCU;

    let game = match game.unwrap_or(Launcher::get_current_game()) {
        Game::Episode1 => "ep1",
        Game::Episode2 => "ep2",
        Game::Unknown => {
            eprintln!("You can not install One-Click Mod Installer into an unknown game!");
            return;
        }
    };
    let formatted_game = format!("sonic4mm{game}");

    let root_path = Path::new("Software").join("Classes").join(&formatted_game);
    
    let (root_key, _) = HKCU.create_subkey(&root_path).unwrap();
    root_key.set_value("", &format!("URL:Sonic 4 {game}'s One-Click Mod Installer protocol")).unwrap();
    root_key.set_value("URL Protocol", &"").unwrap();

    let icon_path = Path::new(&root_path).join("DefaultIcon");
    let (icon_key, _) = HKCU.create_subkey(&icon_path).unwrap();
    icon_key.set_value("", &"OneClickModInstaller.exe").unwrap();
    
    let shell_path = Path::new(&root_path).join("Shell").join("Open").join("Command");
    let (shell_key, _) = HKCU.create_subkey(&shell_path).unwrap();
    let current_path = std::env::current_exe().unwrap();
    let current_path = current_path.display();
    shell_key.set_value("", &format!("\"{current_path}\" \"%1\"")).unwrap();
}

#[cfg(target_os = "windows")]
pub fn uninstall(game: Option<Game>) {
    use std::path::Path;
    use winreg::HKCU;

    let game = match game.unwrap_or(Launcher::get_current_game()) {
        Game::Episode1 => "ep1",
        Game::Episode2 => "ep2",
        Game::Unknown => {
            eprintln!("You can not install One-Click Mod Installer into an unknown game!");
            return;
        }
    };
    let formatted_game = format!("sonic4mm{game}");

    let root_path = Path::new("Software").join("Classes").join(&formatted_game);

    HKCU.delete_subkey_all(&root_path).unwrap();
}

#[cfg(target_os = "windows")]
pub fn fix(game: Option<Game>) {
    use std::path::Path;
    use winreg::HKCU;

    let game = match game.unwrap_or(Launcher::get_current_game()) {
        Game::Episode1 => "ep1",
        Game::Episode2 => "ep2",
        Game::Unknown => {
            eprintln!("You can not install One-Click Mod Installer into an unknown game!");
            return;
        }
    };
    let formatted_game = format!("sonic4mm{game}");

    let root_path = Path::new("Software").join("Classes").join(&formatted_game);

    let shell_path = Path::new(&root_path).join("Shell").join("Open").join("Command");
    let (shell_key, _) = HKCU.create_subkey(&shell_path).unwrap();
    let current_path = std::env::current_exe().unwrap();
    let current_path = current_path.display();
    shell_key.set_value("", &format!("\"{current_path}\" \"%1\"")).unwrap();
}
