use common::{Launcher, Game};

#[cfg(not(target_os = "windows"))]
pub fn install() {

}
#[cfg(not(target_os = "windows"))]
pub fn uninstall() {

}

#[cfg(not(target_os = "windows"))]
pub fn fix() {

}

#[cfg(target_os = "windows")]
pub fn install() {
    use std::path::Path;
    use winreg::HKCU;

    let game = match Launcher::get_current_game() {
        Game::Episode1 => "ep1",
        Game::Episode2 => "ep2",
        Game::Unknown => panic!("You can not install One-Click Mod Installer into an unknown game!")
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
pub fn uninstall() {
    use std::path::Path;
    use winreg::HKCU;

    let game = match Launcher::get_current_game() {
        Game::Episode1 => "ep1",
        Game::Episode2 => "ep2",
        Game::Unknown => panic!("You can not install One-Click Mod Installer into an unknown game!")
    };
    let formatted_game = format!("sonic4mm{game}");

    let root_path = Path::new("Software").join("Classes").join(&formatted_game);

    HKCU.delete_subkey_all(&root_path).unwrap();
}

#[cfg(target_os = "windows")]
pub fn fix() {
    use std::path::Path;
    use winreg::HKCU;

    let game = match Launcher::get_current_game() {
        Game::Episode1 => "ep1",
        Game::Episode2 => "ep2",
        Game::Unknown => panic!("You can not install One-Click Mod Installer into an unknown game!")
    };
    let formatted_game = format!("sonic4mm{game}");

    let root_path = Path::new("Software").join("Classes").join(&formatted_game);

    let shell_path = Path::new(&root_path).join("Shell").join("Open").join("Command");
    let (shell_key, _) = HKCU.create_subkey(&shell_path).unwrap();
    let current_path = std::env::current_exe().unwrap();
    let current_path = current_path.display();
    shell_key.set_value("", &format!("\"{current_path}\" \"%1\"")).unwrap();
}
