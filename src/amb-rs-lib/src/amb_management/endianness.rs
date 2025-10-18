use std::fs;
use crate::{amb::Amb, binary_reader::Endianness};

pub fn swap_endianness_and_save(target_file: String, save_as_file_name: Option<String>) {
    let amb_result = Amb::new_from_file_name(&target_file);
    match amb_result {
        Ok(mut amb) => {
            amb.endianness = match amb.endianness {
                Some(Endianness::Little) => Some(Endianness::Big),
                Some(Endianness::Big) => Some(Endianness::Little),
                None => None,
            };
            
            if amb.endianness.is_none() {
                println!("Couldn't detect endianness of the AMB file, doing nothing.");
                return;
            }

            match fs::write(save_as_file_name.unwrap_or(target_file), amb.write()) {
                Ok(_) => (),
                Err(e) => println!("Error: {}", e),
            }
        },
        Err(e) => println!("Error: {}", e),
    }
}
