use ini::Ini;

pub struct AMBPatcherConfig {
    pub progress_bar: bool,
    pub sha_check: bool
}

pub fn load() -> AMBPatcherConfig {
    let default_settings = AMBPatcherConfig { progress_bar: true, sha_check: true };
    let ini_result = Ini::load_from_file("AMBPatcher.cfg");

    match ini_result {
        Ok(ini) => {
            let default_section = ini.section(None::<String>);

            match default_section {
                Some(section) => {
                    let progress_bar = section
                        .get("progress_bar")
                        .unwrap_or("1");
                    let sha_check = section
                        .get("sha_check")
                        .unwrap_or("1");
                    AMBPatcherConfig { progress_bar: progress_bar == "1", sha_check: sha_check == "1" }
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

pub fn save(config: &AMBPatcherConfig) -> Result<(), Box<dyn std::error::Error>> {
    let mut ini = Ini::new();

    ini.with_section(None::<String>).set("progress_bar", match config.progress_bar {
        true => "1",
        false => "0"
    });
    ini.with_section(None::<String>).set("sha_check", match config.sha_check {
        true => "1",
        false => "0"
    });

    ini.write_to_file("AMBPatcher.cfg")?;

    Ok(())
}
