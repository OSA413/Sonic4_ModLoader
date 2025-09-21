use ini::Ini;

pub struct AMBPatcherConfig {
    pub progress_bar: bool,
    pub sha_check: bool
}

pub fn load() -> Result<AMBPatcherConfig, Box<dyn std::error::Error>> {
    let ini = Ini::load_from_file("AMBPatcher.cfg")?;

    let default_section = ini.section(None::<String>);

    match default_section {
        Some(section) => {
            let progress_bar = section
                .get("progress_bar")
                .unwrap_or("1");
            let sha_check = section
                .get("sha_check")
                .unwrap_or("1");
            return Ok(AMBPatcherConfig { progress_bar: progress_bar == "1", sha_check: sha_check == "1" });
        },
        None => return Ok(AMBPatcherConfig { progress_bar: true, sha_check: true })
    }
}

pub fn save(config: &AMBPatcherConfig) -> Result<(), Box<dyn std::error::Error>> {
    let mut ini = Ini::new();

    ini.with_section(None::<String>).set("progress_bar", config.progress_bar.to_string());
    ini.with_section(None::<String>).set("sha_check", config.sha_check.to_string());

    ini.write_to_file("AMBPatcher.cfg")?;

    Ok(())
}
