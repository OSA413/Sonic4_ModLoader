use std::fs;
use crate::{amb::Amb};
use common_binary::error::CommonBinaryError;

pub fn create_amb(file_name: String) -> Result<(), CommonBinaryError>  {
    let amb = Amb::new_empty();
    fs::write(file_name, amb.write()?)?;
    Ok(())
}
