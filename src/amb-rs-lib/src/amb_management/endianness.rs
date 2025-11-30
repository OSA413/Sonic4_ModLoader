use std::fs;
use crate::{amb::Amb, binary_reader::Endianness, error::AmbLibRsError};

pub fn swap_endianness_of_amb(amb: &mut Amb) {
    amb.endianness = match amb.endianness {
        Some(Endianness::Little) => Some(Endianness::Big),
        Some(Endianness::Big) => Some(Endianness::Little),
        None => None,
    };
}

pub fn swap_endianness_and_save(target_file: String, save_as_file_name: Option<String>) -> Result<(), AmbLibRsError> {
    let mut amb = Amb::new_from_file_name(&target_file)?;
    swap_endianness_of_amb(&mut amb);
        
    if amb.endianness.is_none() {
        println!("Couldn't detect endianness of the AMB file, doing nothing.");
        return Ok(());
    }

    fs::write(save_as_file_name.unwrap_or(target_file), amb.write()?)?;
    Ok(())
}
