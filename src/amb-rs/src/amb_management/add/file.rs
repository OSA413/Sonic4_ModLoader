use std::path::Path;
use common_binary::error::CommonBinaryError;
use crate::amb_management::common_hander::{do_a_thing_over_an_amb_and_save_result};

pub fn add_file_to_file(target_amb_file: &Path, file_to_add: &Path, internal_file_name: Option<String>) -> Result<(), CommonBinaryError> {
    let path = &target_amb_file.display().to_string();
    do_a_thing_over_an_amb_and_save_result(
        &path,
        &|amb| amb.add_file(file_to_add, internal_file_name.clone()),
        &path,
    )
}
