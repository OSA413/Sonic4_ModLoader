// Handle with care!
use std::{env::Args, ops, path::Path, sync::{LazyLock, Mutex, MutexGuard}};
use crate::handler_installer;

static MOD_ARG: LazyLock<Mutex<InitialArgs>> = LazyLock::new(|| Mutex::new(InitialArgs::None));

pub enum InitialArgs {
    FromDir(String),
    FromArchive(String),
    FromGameBanana { url: String, type_: String, id: u32 },
    FromInternet(String),
    None,
}

fn parse_game_banana_args(arg: String) -> Result<InitialArgs, String> {
    // sonic4mmepx:url,mod_type,mod_id
    let arg = arg.chars().skip(12).collect::<String>();
    let mut args = arg.split(',');

    let url = args.next()
        .ok_or_else(|| "Couldn't get url from the handler request, skipping with default option".to_owned())?
        .to_owned();
    let type_ = args.next()
        .ok_or_else(|| "Couldn't get type from the handler request, skipping with default option".to_owned())?
        .to_owned();
    let id_arg = args.next()
        .ok_or_else(|| "Couldn't get id from the handler request, skipping with default option".to_owned())?;
    let id = id_arg.parse::<u32>()
        .map_err(|e| format!("Couldn't convert id to u32: {e}"))?;

    Ok(InitialArgs::FromGameBanana { url, type_, id })
}

pub struct ArgHandler {}

impl ArgHandler {
    pub fn convert_url_to_args(arg: String) -> InitialArgs {
        let path = Path::new(&arg);
        if arg.starts_with("https://") {
           return InitialArgs::FromInternet(arg);
        } else if arg.starts_with("sonic4mmep1:") || arg.starts_with("sonic4mmep2:") {
            match parse_game_banana_args(arg) {
                Ok(args) => return args,
                Err(e) => eprintln!("Error parsing GameBanana args: {e}"),
            }
        } else if path.is_dir() {
            return InitialArgs::FromDir(path.display().to_string());
        } else if path.is_file() && match path.extension() {
            Some(extension) => extension == "zip"
                || extension == "7z"
                || extension == "rar",
            None => false
        } {
            return InitialArgs::FromArchive(path.display().to_string());
        }

        InitialArgs::None
    }

    pub fn init(args: Args) {
        let skip_shift = handler_installer::get_path_to_exe().0;
        let mut args = args.skip(1 + skip_shift);

        if let Some(arg) = args.next() {
            match &arg[ops::RangeFull] {
                "--install" => {
                    handler_installer::install(None).expect("Couldn't install One-Click Mod Installer. Exiting...");
                    std::process::exit(0);
                }
                "--uninstall" => {
                    handler_installer::uninstall(None).expect("Couldn't uninstall One-Click Mod Installer. Exiting...");
                    std::process::exit(0);
                }
                "--fix" => {
                    handler_installer::fix(None).expect("Couldn't fix One-Click Mod Installer. Exiting...");
                    std::process::exit(0);
                }
                _ => ()
            }

            *MOD_ARG.lock()
            .expect("Couldn't accuire lock on MOD_ARG to write to it. Exiting...")
            = ArgHandler::convert_url_to_args(arg);
        }
    }

    pub fn get() -> MutexGuard<'static, InitialArgs> {
        MOD_ARG.lock().expect("Couldn't accuire lock on MOD_ARG to get it. Exiting...")
    }
}
