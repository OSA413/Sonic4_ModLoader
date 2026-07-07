use std::fs;
use amb_rs_lib::amb::Amb;
use common_binary::error::CommonBinaryError;

pub fn create_amb(file_name: String) -> Result<(), CommonBinaryError>  {
    fs::write(file_name, Amb::new_empty().write()?)?;
    Ok(())
}