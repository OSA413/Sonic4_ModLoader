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

pub struct ArgHandler {}

impl ArgHandler {
    pub fn convert_url_to_args(arg: String) -> InitialArgs {
        let path = Path::new(&arg);
        if arg.starts_with("https://") {
           return InitialArgs::FromInternet(arg);
        } else if arg.starts_with("sonic4mmep1:") || arg.starts_with("sonic4mmep2:") {
            // sonic4mmepx:url,mod_type,mod_id
            let arg = arg.chars().skip(12).collect::<String>();
            let mut args = arg.split(',');
            return InitialArgs::FromGameBanana{
                url: args.next().unwrap().to_owned(),
                type_: args.next().unwrap().to_owned(),
                id: args.next().unwrap().parse::<u32>().unwrap()
            };
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

        return InitialArgs::None;
    }

    pub fn init(args: Args) {
        let mut args = args.skip(1);
        match args.next() {
            Some(arg) => {
                match &arg[ops::RangeFull] {
                    "--install" => {
                        handler_installer::install();
                        std::process::exit(0);
                    }
                    "--uninstall" => {
                        handler_installer::uninstall();
                        std::process::exit(0);
                    }
                    "--fix" => {
                        handler_installer::fix();
                        std::process::exit(0);
                    }
                    _ => ()
                }

                *MOD_ARG.lock().unwrap() = ArgHandler::convert_url_to_args(arg);
            }
            None => {}
        }
    }

    pub fn get() -> MutexGuard<'static, InitialArgs> {
        MOD_ARG.lock().unwrap()
    }
}
