use std::fs;
use amb_rs_lib::amb::Amb;
use common_binary::error::CommonBinaryError;

pub fn do_a_thing_over_an_amb_and_save_result(
    source_file_name: &String,
    action: &dyn Fn(&mut Amb) -> Result<(), CommonBinaryError>,
    destination_file: &String,
) -> Result<(), CommonBinaryError> {
    let mut amb = Amb::new_from_file_name(source_file_name)?;
    action(&mut amb)?;
    fs::write(&destination_file, amb.write()?)?;
    Ok(())
}

pub fn do_a_thing_over_an_amb_and_save(
    source_file_name: &String,
    action: &dyn Fn(&mut Amb) -> (),
    destination_file: &String,
) -> Result<(), CommonBinaryError> {
    let mut amb = Amb::new_from_file_name(source_file_name)?;
    action(&mut amb);
    fs::write(&destination_file, amb.write()?)?;
    Ok(())
}