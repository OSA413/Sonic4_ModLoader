use std::{fs, path::Path};
use glob::glob;
use crate::{amb::{Amb, Version}, binary_reader::Endianness};

pub fn add_dir_to_amb(target_file: &Path, dir_to_add: &Path) {
    let amb_result = Amb::new_from_file_name(&target_file.display().to_string());
    match amb_result {
        Ok(mut amb) => {
            let files_to_add = glob(&format!("{}/**/*.*", dir_to_add.display().to_string()));
            match files_to_add {
                Ok(files) => {
                    for entry in files {
                        match entry {
                            Ok(path) => {
                                let file_path = path.display().to_string();
                                amb.add(file_path, None);
                            },
                            Err(e) => println!("Glob error: {}", e),
                        }
                    }
                    match fs::write(target_file, amb.write()) {
                        Ok(_) => (),
                        Err(e) => println!("Error: {}", e),
                    }
                },
                Err(e) => println!("Error reading directory: {}", e),
            }
        },
        Err(e) => println!("Error reading AMB file: {}", e),
    }
}

pub fn add_file_to_amb(target_file: &Path, file_to_add: &Path, internal_file_name: Option<String>) {
    let amb_result = Amb::new_from_file_name(&target_file.display().to_string());
    match amb_result {
        Ok(mut amb) => {
            amb.add(file_to_add.display().to_string(), internal_file_name);
            match fs::write(target_file, amb.write()) {
                Ok(_) => (),
                Err(e) => println!("Error: {}", e),
            }
        },
        Err(e) => println!("Error: {}", e),
    };
}

pub fn extract_amb(target_file: String, dir_to_extract: Option<String>) {
    let amb = Amb::new_from_file_name(&target_file);
    match amb {
        Ok(amb) => {
            let base_dir = dir_to_extract.unwrap_or(format!("{}_extracted", &target_file));
            let base_dir = Path::new(&base_dir);
            for binary_object in amb.objects {
                match fs::write(base_dir.join(&binary_object.name), &binary_object.data) {
                    Ok(_) => println!("Extracted {}", &binary_object.name),
                    Err(e) => println!("Error: {}", e),
                }
            }
        },
        Err(e) => println!("Error: {}", e),
    };
}

fn add_json_entry_str(field: &'static str, value: &String) -> String {
    add_json_entry(field, &format!("\"{}\"", value))
}

fn add_json_entry(field: &'static str, value: &String) -> String {
    format!("\"{}\":{}", field, value)
}

pub fn print_amb_table_of_content_from_file(target_file: String) {
    let source = std::fs::read(&target_file);
    match source {
        Ok(source) => print!("{}", get_json_string_amb_table_of_content(source, target_file)),
        Err(e) => print!("Error reading file: {}", e),
    };
}

pub fn get_json_string_amb_table_of_content(source: Vec<u8>, name: String) -> String {
    let amb = Amb::new_from_src_ptr_name(&source, Some(0), name);
    let mut amb_toc = Vec::<String>::new();

    amb_toc.push(add_json_entry_str("name", &amb.amb_path.replace("\\", "\\\\")));
    let amb_version = match &amb.version { Version::PC => &"PC".to_string(), Version::Mobile => &"Mobile".to_string() };
    amb_toc.push(add_json_entry_str("version", amb_version));
    let amb_endianness = match &amb.endianness { 
        Some(Endianness::Little) => &"little".to_string(),
        Some(Endianness::Big) => &"big".to_string(),
        None => &"unknown".to_string(),
    };
    amb_toc.push(add_json_entry_str("endianness", amb_endianness));
    let mut objects_toc = Vec::<String>::new();
    for binary_object in amb.objects {
        let mut object_toc = Vec::<String>::new();
        object_toc.push(add_json_entry_str("name", &binary_object.name.replace("\\", "\\\\")));
        object_toc.push(add_json_entry_str("real_name", &binary_object.real_name.replace("\\", "\\\\")));
        object_toc.push(add_json_entry("flag1", &binary_object.flag1.to_string()));
        object_toc.push(add_json_entry("flag2", &binary_object.flag2.to_string()));
        object_toc.push(add_json_entry("pointer", &binary_object.pointer.to_string()));
        object_toc.push(add_json_entry("length", &binary_object.length().to_string()));
        objects_toc.push(format!("{{{}}}", object_toc.join(",")))
    }
    amb_toc.push(add_json_entry("objects", &format!("[{}]", objects_toc.join(","))));

    format!("{{{}}}", amb_toc.join(","))
}

pub fn swap_endianness_and_save(target_file: String, save_as_file_name: Option<String>) {
    let amb_result = Amb::new_from_file_name(&target_file);
    match amb_result {
        Ok(mut amb) => {
            amb.endianness = match amb.endianness {
                Some(Endianness::Little) => Some(Endianness::Big),
                Some(Endianness::Big) => Some(Endianness::Little),
                None => None,
            };
            
            if amb.endianness.is_none() {
                println!("Couldn't detect endianness of the AMB file, doing nothing.");
                return;
            }

            match fs::write(save_as_file_name.unwrap_or(target_file), amb.write()) {
                Ok(_) => (),
                Err(e) => println!("Error: {}", e),
            }
        },
        Err(e) => println!("Error: {}", e),
    }
}

pub fn create_amb(file_name: String) {
    let amb = Amb::new_empty();
    match fs::write(file_name, amb.write()) {
        Ok(_) => (),
        Err(e) => println!("Error: {}", e),
    };
}

pub fn recreate_amb(file: String, save_as_file_name: Option<String>) {
    let amb = Amb::new_from_file_name(&file);
    match amb {
        Ok(amb) => {
            match fs::write(save_as_file_name.unwrap_or(file), amb.write()) {
                Ok(_) => (),
                Err(e) => println!("Error: {}", e),
            }
        },
        Err(e) => println!("Error: {}", e),
    };
}

pub fn recreate_amb_from_dir(dir: String) {
    let dir_path = Path::new(&dir);
    if !dir_path.is_dir() {
        println!("Error: {:?} is not a directory", dir);
        return;
    }

    let extracted_prefix = "_extracted";

    let amb_file_path = if dir.ends_with(extracted_prefix) {
        let possible_file = &dir;
        let possible_file = possible_file.chars().take(possible_file.len() - extracted_prefix.len()).collect::<String>();
        if Path::new(&possible_file).is_file() {
            Some(possible_file)
        } else {
            None
        }
    } else {
        let mut result = None;
        let possible_file = dir_path.join(".amb");
        if possible_file.is_file() {
            result = possible_file.as_os_str().to_str().and_then(|x| Some(x.to_string()))
        }
        
        let possible_file = dir_path.join(".AMB");
        if possible_file.is_file() {
            result = possible_file.as_os_str().to_str().and_then(|x| Some(x.to_string()))
        }

        result
    };

    match amb_file_path {
        Some(amb_file_path) => {
            let amb_result = Amb::new_from_file_name(&amb_file_path);
            match amb_result {
                Ok(mut amb) => {
                    let files_to_add = glob(&format!("{}/**/*.*", dir));
                    match files_to_add {
                        Ok(files) => {
                            for entry in files {
                                match entry {
                                    Ok(path) => {
                                        let file_path = path.display().to_string();
                                        amb.add(file_path, None);
                                    },
                                    Err(e) => println!("Glob error: {}", e),
                                }
                            }
                            match fs::write(amb_file_path, amb.write()) {
                                Ok(_) => (),
                                Err(e) => println!("Error: {}", e),
                            }
                        },
                        Err(e) => println!("Error reading directory: {}", e),
                    }
                },
                Err(e) => println!("Error reading AMB file: {}", e),
            }
        },
        None => println!("No AMB file found for provided directory"),
    }
}

pub fn remove_object_from_amb(target_file: String, object_name: String) {
    let amb = Amb::new_from_file_name(&target_file);
    match amb {
        Ok(mut amb) => {
            amb.remove(object_name);
            match fs::write(target_file, amb.write()) {
                Ok(_) => (),
                Err(e) => println!("Error: {}", e),
            }
        },
        Err(e) => println!("Error: {}", e),
    }
}