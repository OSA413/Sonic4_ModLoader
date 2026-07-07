use std::fs;
use amb_rs_lib::amb::Amb;
use common_binary::error::CommonBinaryError;

pub fn remove_object_from_file_and_write_to_file(target_file: String, object_name: String) -> Result<(), CommonBinaryError> {
    let mut amb = Amb::new_from_file_name(&target_file)?;
    amb.remove(object_name);
    fs::write(target_file, amb.write()?)?;
    Ok(())
}