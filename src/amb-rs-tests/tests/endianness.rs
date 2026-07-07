use std::fs;

use amb_rs_lib::amb::Amb;

#[test]
fn swap_endianness_le_to_be() {
    let file_path = "../amb-rs-tests/tests/reference_files/le/add_1_2_3.amb";
    let mut amb = Amb::new_from_file_name(&file_path.to_string()).unwrap();

    amb.swap_endianness();

    let result = amb.write().unwrap();

    assert_eq!(
        fs::read("../amb-rs-tests/tests/reference_files/be/add_1_2_3.amb").unwrap(),
        result
    );
}

#[test]
fn swap_endianness_be_to_le() {
    // Currently we don't have an example of big endian AMB files
    // I'll need to check Wii and PS3 versions of the games
}

#[test]
fn swap_endianness_twice_le() {
    let file_path = "../amb-rs-tests/tests/reference_files/le/add_1_2_3.amb".to_string();
    let original = fs::read(&file_path).unwrap();

    let mut amb = Amb::new_from_file_name(&file_path).unwrap();
    amb.swap_endianness();
    amb.swap_endianness();

    let result = amb.write().unwrap();
    assert_eq!(original, result);
}

#[test]
fn swap_endianness_twice_be() {
    // Read swap_endianness_be_to_le comment
}
