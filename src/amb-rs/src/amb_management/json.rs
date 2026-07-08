use amb_rs_lib::amb::Amb;
use common_binary::error::CommonBinaryError;

pub fn print_from_file_to_stdout(target_file: &String) -> Result<(), CommonBinaryError> {
    println!("{}", Amb::new_from_file_name(target_file)?);
    Ok(())
}
