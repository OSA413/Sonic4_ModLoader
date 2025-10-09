use std::fs;

use ini::Ini;


pub fn load() -> String {
    let default = "".to_string();

    let ini_result = Ini::load_from_file("AML/AliceML.ini");
    match ini_result {
        Ok(ini) => {
            let config_section = ini.section(Some("Config"));
            match config_section {
                Some(config) => {
                    return config.get("PatcherDir").unwrap_or("").to_string();
                },
                None => (),
            }
        },
        Err(e) => println!("Error reading AML/AliceML.ini: {}", e),
    }
    return default;
}

pub fn save(patcher_dir: &str) {
    let ini_aml_result = fs::read_to_string("AML/AliceML.ini");
    match ini_aml_result {
        Ok(ini_aml) => {
            let mut result_lines = Vec::<String>::new();
            for line in ini_aml.lines() {
                if line.starts_with("PatcherDir=") {
                    result_lines.push(format!("PatcherDir={}", patcher_dir));
                } else {
                    result_lines.push(line.to_string());
                }
            }
            
            match fs::write("AML/AliceML.ini", result_lines.join("\n")) {
                Ok(_) => (),
                Err(e) => println!("Couldn't write AML/AliceML.ini: {}", e),
            }
        },
        Err(e) => println!("Error reading AML config: {}", e),
    }
}