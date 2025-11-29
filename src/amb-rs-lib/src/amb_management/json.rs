use std;
use crate::amb::{Amb, Version};
use crate::binary_reader::Endianness;

fn add_json_entry_str(field: &'static str, value: &String) -> String {
    add_json_entry(field, &format!("\"{value}\""))
}

fn add_json_entry(field: &'static str, value: &String) -> String {
    format!("\"{field}\":{value}")
}

pub fn print_from_file_to_stdout(target_file: String) {
    print!("{}", print_from_file(&target_file))
}

pub fn print_from_file(target_file: &String) -> String {
    let source = std::fs::read(target_file);
    match source {
        Ok(source) => print_from_vec_u8(source, target_file).to_string(),
        Err(e) => format!("Error reading file: {e}"),
    }
}

pub fn print_from_vec_u8(source: Vec<u8>, name: &String) -> String {
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
