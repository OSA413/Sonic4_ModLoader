use std::path::{Path, PathBuf};
use std::fmt::Debug;

use common_modloader::{Launcher, Game};

pub enum InstallationInfo {
    Installed(String),
    AnotherInstallationPresent(String),
    NotInstalled,
}

pub enum HadnlerInstallationError {
    Io(std::io::Error),
    UnknownGame,
}

impl From<std::io::Error> for HadnlerInstallationError {
    fn from(e: std::io::Error) -> Self {
        HadnlerInstallationError::Io(e)
    }
}

impl Debug for HadnlerInstallationError {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            HadnlerInstallationError::Io(e) => write!(f, "I/O error: {e}"),
            HadnlerInstallationError::UnknownGame => write!(f, "You can not install One-Click Mod Installer into an unknown game!"),
        }
    }
}

pub fn get_path_to_exe() -> (usize, Result<PathBuf, std::io::Error>) {
    match std::env::args().nth(1) {
        Some(arg) => {
            if arg.ends_with("_link.exe") {
                return (1, Ok(Path::new(&arg).to_path_buf()));
            }
            (0, std::env::current_exe())
        },
        None => (0, std::env::current_exe())
    }
}

#[cfg(not(target_os = "windows"))]
pub fn get_info(game: Option<Game>) -> (Game, InstallationInfo) {
    todo!()
}

#[cfg(not(target_os = "windows"))]
pub fn install(game: Option<Game>) -> Result<(), HadnlerInstallationError> {
    todo!()
}
#[cfg(not(target_os = "windows"))]
pub fn uninstall(game: Option<Game>) -> Result<(), HadnlerInstallationError> {
    todo!()
}

#[cfg(not(target_os = "windows"))]
pub fn fix(game: Option<Game>) -> Result<(), HadnlerInstallationError> {
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
                    let current_path = get_path_to_exe().1
                        .expect("Couldn't get path to current exe to check installation of One-Click Mod Installer handler")
                        .display().to_string();
                    match shell_key.get_value::<String, _>("") {
                        Ok(value) => {
                            let installed_path = value.chars().skip(1).take(value.len() - "\" \"%1\"".len() - 1).collect::<String>();
                            if installed_path == current_path || !check_another_installation {
                                return (game_to_check, InstallationInfo::Installed(installed_path))
                            }
                            (game_to_check, InstallationInfo::AnotherInstallationPresent(installed_path))
                        }
                        Err(_) => (game_to_check, InstallationInfo::NotInstalled)
                    }
                }
                Err(_) => (game_to_check, InstallationInfo::NotInstalled)
            }
        }
        Err(_) => (game_to_check, InstallationInfo::NotInstalled)
    }
}

#[cfg(target_os = "windows")]
pub fn install(game: Option<Game>) -> Result<(), HadnlerInstallationError> {
    use std::path::Path;
    use winreg::HKCU;

    let game = match game.unwrap_or(Launcher::get_current_game()) {
        Game::Episode1 => "ep1",
        Game::Episode2 => "ep2",
        Game::Unknown => {
            eprintln!("You can not install One-Click Mod Installer into an unknown game!");
            return Err(HadnlerInstallationError::UnknownGame);
        }
    };
    let formatted_game = format!("sonic4mm{game}");

    let root_path = Path::new("Software").join("Classes").join(&formatted_game);
    
    let (root_key, _) = HKCU.create_subkey(&root_path)?;
    root_key.set_value("", &format!("URL:Sonic 4 {game}'s One-Click Mod Installer protocol"))?;
    root_key.set_value("URL Protocol", &"")?;

    let icon_path = Path::new(&root_path).join("DefaultIcon");
    let (icon_key, _) = HKCU.create_subkey(&icon_path)?;
    icon_key.set_value("", &"OneClickModInstaller.exe")?;
    
    let shell_path = Path::new(&root_path).join("Shell").join("Open").join("Command");
    let (shell_key, _) = HKCU.create_subkey(&shell_path)?;
    let current_path = get_path_to_exe().1?;
    let current_path = current_path.display();
    shell_key.set_value("", &format!("\"{current_path}\" \"%1\""))?;
    Ok(())
}

#[cfg(target_os = "windows")]
pub fn uninstall(game: Option<Game>) -> Result<(), HadnlerInstallationError> {
    use std::path::Path;
    use winreg::HKCU;

    let game = match game.unwrap_or(Launcher::get_current_game()) {
        Game::Episode1 => "ep1",
        Game::Episode2 => "ep2",
        Game::Unknown => {
            eprintln!("You can not install One-Click Mod Installer into an unknown game!");
            return Err(HadnlerInstallationError::UnknownGame);
        }
    };
    let formatted_game = format!("sonic4mm{game}");

    let root_path = Path::new("Software").join("Classes").join(&formatted_game);

    HKCU.delete_subkey_all(&root_path)?;
    Ok(())
}

#[cfg(target_os = "windows")]
pub fn fix(game: Option<Game>) -> Result<(), HadnlerInstallationError> {
    use std::path::Path;
    use winreg::HKCU;

    let game = match game.unwrap_or(Launcher::get_current_game()) {
        Game::Episode1 => "ep1",
        Game::Episode2 => "ep2",
        Game::Unknown => {
            eprintln!("You can not install One-Click Mod Installer into an unknown game!");
            return Err(HadnlerInstallationError::UnknownGame);
        }
    };
    let formatted_game = format!("sonic4mm{game}");

    let root_path = Path::new("Software").join("Classes").join(&formatted_game);

    let shell_path = Path::new(&root_path).join("Shell").join("Open").join("Command");
    let (shell_key, _) = HKCU.create_subkey(&shell_path)?;
    let current_path = get_path_to_exe().1?;
    let current_path = current_path.display();
    shell_key.set_value("", &format!("\"{current_path}\" \"%1\""))?;
    Ok(())
}
