use std::fs;


pub fn save() {
    match fs::write("ModManager.cfg", "") {
        Ok(_) => (),
        Err(e) => eprintln!("Couldn't write ModManager.cfg: {e}"),
    }
}
