use std::{fs, path::Path};

use crate::{amb::{Amb, Version}, binary_reader::Endianness};

pub fn add_dir_to_amb(target_file: &Path, dir_to_add: &Path) {
    todo!();
}

pub fn add_file_to_amb(target_file: &Path, file_to_add: &Path, internal_file_name: Option<String>) {
    todo!();
}

pub fn extract_amb(target_file: String, dir_to_extract: Option<String>) {
    let amb = Amb::from_file_name(&target_file);
    match amb {
        Ok(amb) => {
            let base_dir = dir_to_extract.unwrap_or(format!("{}_extracted", &target_file));
            let base_dir = Path::new(&base_dir);
            for binary_object in amb.objects {
                // Fix path when out of bounds
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

pub fn swap_endianness_and_save(target_file: String) {
    todo!();
}

pub fn create_amb(file_name: String) {
    todo!();
}

pub fn recreate_amb(file: String) {
    todo!();
}

pub fn extract_amb_from_file(target_file: &Path, dir_to_extract: Option<String>) {
    todo!();
}

pub fn recreate_amb_from_dir(dir: &Path) {
    todo!();
}