use std::fs;


pub fn save() {
    match fs::write("ModManager.cfg", "") {
        Ok(_) => (),
        Err(e) => println!("Couldn't write ModManager.cfg: {}", e),
    }
}
