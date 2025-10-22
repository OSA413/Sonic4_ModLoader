use std::{path::Path};
use crate::{amb::Amb, binary_object::BinaryObject};

pub fn add_vec_u8_to_amb(amb: &mut Amb, file_name: &String, file_data: &Vec<u8>, new_name: Option<String>) {
    let new_obj = BinaryObject::new_from_src_ptr_len(file_data, 0, file_data.len());
    let internal_name = new_name.unwrap_or(Amb::get_relative_name(amb.amb_path.clone(), file_name.replace("_extracted", ""))).replace('/', "\\");
    amb.add_binary_object(new_obj, internal_name);
}

pub fn add_file_to_amb(amb: &mut Amb, file_to_add: &Path, internal_file_name: Option<String>) -> Result<(), Box<dyn std::error::Error>> {
    let file_path = file_to_add.display().to_string();
    let new_object = BinaryObject::new_from_file_path(file_to_add.display().to_string())?;
    Ok(add_vec_u8_to_amb(amb, &file_path, &new_object.data, internal_file_name))
}

pub fn add_file_to_file(target_amb_file: &Path, file_to_add: &Path, internal_file_name: Option<String>) {
    match Amb::new_from_file_name(&target_amb_file.display().to_string()) {
        Ok(mut amb) => {
            match add_file_to_amb(&mut amb, file_to_add, internal_file_name) {
                Ok(_) => (),
                Err(e) => println!("Error: {e}",)
            }
        },
        Err(e) => println!("Error: {e}"),
    };
}