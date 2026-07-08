use common_binary::error::CommonBinaryError;
use crate::amb_management::common_hander::do_a_thing_over_an_amb_and_save;

pub fn swap_endianness_and_save(target_file: String, save_as_file_name: Option<String>) -> Result<(), CommonBinaryError> {
    do_a_thing_over_an_amb_and_save(
        &target_file,
        &|amb| amb.swap_endianness(),
        &save_as_file_name.unwrap_or(target_file.clone()),
    )
}
