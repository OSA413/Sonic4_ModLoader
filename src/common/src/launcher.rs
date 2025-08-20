use std::io;
use std::path::Path;
use std::process::{Child, Command};

pub enum Game {
    Unknown,
    Episode1,
    Episode2,
}

pub struct Launcher {}
impl Launcher {
    pub fn get_full_game<'a>(game: &Game) -> Option<&'a str> {
        match game {
            Game::Episode1 => Some("Episode 1"),
            Game::Episode2 => Some("Episode 2"),
            _ => None
        }
    }

    pub fn get_short_game<'a>(game: &Game) -> Option<&'a str> {
        match game {
            Game::Episode1 => Some("ep1"),
            Game::Episode2 => Some("ep2"),
            _ => None
        }
    }

    pub fn get_game_from_short(short: &str) -> Game {
        match short {
            "ep1" => Game::Episode1,
            "ep2" => Game::Episode2,
            _ => Game::Unknown
        }
    }

    pub fn get_game(path: &str) -> Game {
        let base_path = Path::new(path);
        if base_path.join("SonicLauncher.exe").exists() {
            return Game::Episode1;
        }
        else if base_path.join("Launcher.exe").exists() {
            return Game::Episode2;
        }

        return Game::Unknown;
    }

    pub fn get_current_game() -> Game {
        Launcher::get_game("")
    }

    pub fn launch_game() -> Result<Child, io::Error> {
        let game = Launcher::get_current_game();
        match game {
            Game::Unknown => Err(io::Error::new(io::ErrorKind::Other, "Game not found")),
            Game::Episode1 => {
                if Path::new("main.conf").exists() {
                    return Command::new("Sonic_vis.exe").spawn();
                } else {
                    return Command::new("SonicLauncher.orig.exe").spawn();
                }
            },
            Game::Episode2 => Command::new("Sonic.exe").spawn()
        }
    }

    pub fn launch_config() -> Result<Child, io::Error> {
        let game = Launcher::get_current_game();
        match game {
            Game::Unknown => Err(io::Error::new(io::ErrorKind::Other, "Game not found")),
            Game::Episode1 => Command::new("SonicLauncher.orig.exe").spawn(),
            Game::Episode2 => Command::new("Launcher.orig.exe").spawn()
        }
    }

    pub fn launch_mod_manager(args: Vec<String>) -> Result<Child, io::Error> {
        Command::new("Sonic4ModManager.exe").args(args).spawn()
    }
    
    pub fn launch_csb_editor(args: Vec<String>) -> Result<Child, io::Error> {
        Command::new("CsbEditor.exe").args(args).spawn()
    }

    pub fn launch_amb_patcher(args: Vec<String>) -> Result<Child, io::Error> {
        Command::new("AMBPatcher.exe").args(args).spawn()
    }

    pub fn launch_ocmi(args: Vec<String>) -> Result<Child, io::Error> {
        Command::new("OneClickModInstaller.exe").args(args).spawn()
    }

    pub fn open_mods_folder() -> Result<Child, io::Error> {
        Command::new("explorer.exe").arg("mods").spawn()
    }
}