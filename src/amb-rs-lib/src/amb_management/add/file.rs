use std::{path::Path};
use crate::{amb::Amb, binary_object::BinaryObject, error::AmbLibRsError};

pub fn add_vec_u8_to_amb(amb: &mut Amb, file_name: &str, file_data: &[u8], new_name: Option<String>) -> Result<(), AmbLibRsError> {
    let new_obj = BinaryObject::new_from_src_ptr_len(file_data, 0, file_data.len());
    let internal_name = new_name.unwrap_or(Amb::get_relative_name(amb.amb_path.clone(), file_name.replace("_extracted", ""))).replace('/', "\\");
    amb.add_binary_object(new_obj, internal_name)
}

pub fn add_file_to_amb(amb: &mut Amb, file_to_add: &Path, internal_file_name: Option<String>) -> Result<(), AmbLibRsError> {
    let file_path = file_to_add.display().to_string();
    let new_object = BinaryObject::new_from_file_path(file_to_add.display().to_string())?;
    add_vec_u8_to_amb(amb, &file_path, &new_object.data, internal_file_name)
}

pub fn add_file_to_file(target_amb_file: &Path, file_to_add: &Path, internal_file_name: Option<String>) -> Result<(), AmbLibRsError> {
    let mut amb = Amb::new_from_file_name(&target_amb_file.display().to_string())?;
    add_file_to_amb(&mut amb, file_to_add, internal_file_name)
}