use common_binary::error::CommonBinaryError;
use crate::amb_management::common_hander::do_a_thing_over_an_amb_and_save;

pub fn remove_object_from_file_and_write_to_file(target_file: String, object_name: String) -> Result<(), CommonBinaryError> {
    do_a_thing_over_an_amb_and_save(
        &target_file,
        &|amb| amb.remove(object_name.clone()),
        &target_file,
    )
}