use std::fs;
use crate::amb::Amb;

pub fn remove_object_from_amb(amb: &mut Amb, object_name: String) {
    amb.remove(object_name);
}

pub fn remove_object_from_vec_u8(source: Vec<u8>, amb_name: String, object_name: String) {
    let mut amb = Amb::new_from_src_ptr_name(&source, None, &amb_name);
    remove_object_from_amb(&mut amb, object_name);
}

pub fn remove_object_from_file_and_write_to_file(target_file: String, object_name: String) {
    let amb = Amb::new_from_file_name(&target_file);
    match amb {
        Ok(mut amb) => {
            remove_object_from_amb(&mut amb, object_name);
            match fs::write(target_file, amb.write()) {
                Ok(_) => (),
                Err(e) => println!("Error: {}", e),
            }
        },
        Err(e) => println!("Error: {}", e),
    }
}