use std::fs;
use crate::{amb::Amb, error::AmbLibRsError};

pub fn create_amb(file_name: String) -> Result<(), AmbLibRsError>  {
    let amb = Amb::new_empty();
    fs::write(file_name, amb.write()?)?;
    Ok(())
}
