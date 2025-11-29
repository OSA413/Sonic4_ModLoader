use ini::Ini;

pub struct FilePatcherConfig {
    pub use_amb_rs_instead: bool,
}

pub fn load() -> FilePatcherConfig {
    let default_settings = FilePatcherConfig { use_amb_rs_instead: false };
    let ini_result = Ini::load_from_file("Sonic4FilePatcher.ini");

    match ini_result {
        Ok(ini) => {
            let default_section = ini.section(None::<String>);

            match default_section {
                Some(section) => {
                    let use_amb_rs_instead = section
                        .get("use_amb_rs_instead")
                        .unwrap_or("1");
                    FilePatcherConfig { use_amb_rs_instead: use_amb_rs_instead == "1" }
                },
                None => default_settings
            }
        },
        Err(e) => {
            println!("Error loading AMBPatcher.cfg: {e}");
            default_settings
        }
    }
}

pub fn save(config: &FilePatcherConfig) -> Result<(), Box<dyn std::error::Error>> {
    let mut ini = Ini::new();

    ini.with_section(None::<String>).set("use_amb_rs_instead", match config.use_amb_rs_instead {
        true => "1",
        false => "0"
    });

    ini.write_to_file("Sonic4FilePatcher.ini")?;

    Ok(())
}
