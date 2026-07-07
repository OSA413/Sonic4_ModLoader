use std::fs;
use common_binary::error::CommonBinaryError;

use amb_rs_lib::amb::Amb;

pub fn swap_endianness_and_save(target_file: String, save_as_file_name: Option<String>) -> Result<(), CommonBinaryError> {
    let mut amb = Amb::new_from_file_name(&target_file)?;
    amb.swap_endianness();

    fs::write(save_as_file_name.unwrap_or(target_file), amb.write()?)?;
    Ok(())
}
