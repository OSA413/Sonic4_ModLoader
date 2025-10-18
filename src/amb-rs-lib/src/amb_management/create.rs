use std::fs;
use crate::amb::Amb;

pub fn create_amb(file_name: String) {
    let amb = Amb::new_empty();
    match fs::write(file_name, amb.write()) {
        Ok(_) => (),
        Err(e) => println!("Error: {}", e),
    };
}
