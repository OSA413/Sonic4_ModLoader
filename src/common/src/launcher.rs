use std::{env, io};
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

        Game::Unknown
    }

    pub fn where_in_the_world_am_i() {
        let game = Launcher::get_current_game();
        match game {
            Game::Episode1 => println!("Detected: Episode 1"),
            Game::Episode2 => println!("Detected: Episode 2"),
            Game::Unknown => println!("
##############################
Game not found, you are probably running the app not from the game's directory.
The Mod Loader must be placed in the game's root directory.
##############################
"),
        };
    }

    pub fn get_current_game() -> Game {
        Launcher::get_game("")
    }

    #[cfg(not(target_os = "windows"))]
    pub fn launch_steam_game(id: u32) -> Result<Child, io::Error> {
        Command::new("xdg-open").arg(format!("steam://launch/{id}/dialog")).spawn()
    }

    #[cfg(target_os = "windows")]
    fn launch_ep1() -> Result<Child, io::Error> {
        let current_dir = env::current_dir().unwrap();
        if Path::new("main.conf").exists() {
            Command::new(current_dir.join("Sonic_vis.exe")).spawn()
        } else {
            Command::new(current_dir.join("SonicLauncher.orig.exe")).spawn()
        }
    }

    #[cfg(not(target_os = "windows"))]
    fn launch_ep1() -> Result<Child, io::Error> {
        Launcher::launch_steam_game(202530)
    }

    #[cfg(target_os = "windows")]
    fn launch_ep2() -> Result<Child, io::Error> {
        let current_dir = env::current_dir().unwrap();
        Command::new(current_dir.join("Sonic.exe")).spawn()
    }

    #[cfg(not(target_os = "windows"))]
    fn launch_ep2() -> Result<Child, io::Error> {
        Launcher::launch_steam_game(203650)
    }

    pub fn launch_current_game() -> Result<Child, io::Error> {
        let game = Launcher::get_current_game();
        match game {
            Game::Unknown => Err(io::Error::other("Game not found")),
            Game::Episode1 => Launcher::launch_ep1(),
            Game::Episode2 => Launcher::launch_ep2()
        }
    }

    pub fn launch_config() -> Result<Child, io::Error> {
        let game = Launcher::get_current_game();
        let current_dir = env::current_dir().unwrap();
        match game {
            Game::Unknown => Err(io::Error::other("Game not found")),
            Game::Episode1 => Command::new(current_dir.join("SonicLauncher.orig.exe")).spawn(),
            Game::Episode2 => Command::new(current_dir.join("Launcher.orig.exe")).spawn()
        }
    }

    pub fn launch_mod_manager(args: Vec<String>) -> Result<Child, io::Error> {
        Command::new("Sonic4ModManager.exe").args(args).spawn()
    }
    
    pub fn launch_csb_editor(args: Vec<String>) -> Result<Child, io::Error> {
        let current_dir = env::current_dir().unwrap();
        Command::new(current_dir.join("CsbEditor.exe")).args(args).spawn()
    }

    pub fn launch_amb_patcher(args: Vec<String>) -> Result<Child, io::Error> {
        let current_dir = env::current_dir().unwrap();
        Command::new(current_dir.join("AMBPatcher.exe")).args(args).spawn()
    }

    pub fn launch_7zip(args: Vec<String>) -> Result<Child, io::Error> {
        let current_dir = env::current_dir().unwrap();
        let local_7z = current_dir.join("7z.exe");
        let global_7z = Path::new("C:\\").join("Program Files").join("7-Zip").join("7z.exe");
        if local_7z.is_file() {
            Command::new(local_7z).args(args).spawn()
        } else if global_7z.is_file() {
            Command::new(global_7z).args(args).spawn()
        } else {
            Command::new("7z").args(args).spawn()
        }
    }

    pub fn launch_amb_rs(args: Vec<String>) -> Result<Child, io::Error> {
        Command::new("amb-rs").args(args).spawn()
    }
    
    pub fn launch_file_patcher(args: Vec<String>) -> Result<Child, io::Error> {
        Command::new("Sonic4FilePatcher").args(args).spawn()
    }

    pub fn launch_ocmi(args: Vec<String>) -> Result<Child, io::Error> {
        Command::new("OneClickModInstaller.exe").args(args).spawn()
    }

    #[cfg(target_os = "windows")]
    pub fn open_mods_folder() -> Result<Child, io::Error> {
        Command::new("explorer.exe").arg("mods").spawn()
    }

    #[cfg(not(target_os = "windows"))]
    pub fn open_mods_folder() -> Result<Child, io::Error> {
        Command::new("xdg-open").arg("mods").spawn()
    }
}