use std;
use amb_rs_lib::amb::Amb;
use common_binary::error::CommonBinaryError;

pub fn print_from_file_to_stdout(target_file: &String) -> Result<(), CommonBinaryError> {
    println!(
        "{}",
        Amb::new_from_src_ptr_name(
            &std::fs::read(target_file)?,
            Some(0),
            target_file
        )?
    );
    Ok(())
}
