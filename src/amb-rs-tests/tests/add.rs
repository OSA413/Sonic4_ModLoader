use std::{fs, path::Path, vec};

use amb_rs_lib::{amb, amb_management};

mod amb_print;
use crate::amb_print::{AmbPrint, BinaryObjectPrint};

macro_rules! add_tests {
    ($($name:ident: $value:expr,)*) => {
        $(
            #[test]
            fn $name() {
                let amb_path = "amb-rs-tests/test_files/files";
                let test_case: Vec<(&str, &str, u32, u32)> = $value;
                let mut amb = amb::Amb::new_empty();
                amb.amb_path = amb_path.to_string();
                for (file_name, _, _, _) in &test_case {   
                    amb_management::add::file::add_file_to_amb(
                        &mut amb,
                        Path::new(file_name),
                        None
                    ).unwrap();
                }

                let content = amb.write().unwrap();

                let resulting_binary_objects = test_case.iter().map(|(_, object_name, pointer, length)| BinaryObjectPrint {
                    name: object_name.to_string(),
                    real_name: object_name.to_string(),
                    flag1: 0,
                    flag2: 0,
                    pointer: *pointer,
                    length: *length,
                }).collect();

                assert_eq!(
                    amb_management::json::print_from_vec_u8(&content, &amb_path.to_string()).unwrap(),
                    serde_json::to_string(&AmbPrint {
                        name: amb_path.to_string(),
                        endianness: "little".to_string(),
                        objects: resulting_binary_objects,
                        version: "PC".to_string(),
                    }).unwrap()
                );

                for (object, (file_name, _, _, _)) in amb.objects.iter().zip(&test_case) {   
                    assert_eq!(object.data, fs::read(file_name).unwrap());
                }

                assert_eq!(amb.objects.len(), test_case.len());

                let reference_file_name = format!("../amb-rs-tests/tests/reference_files/{}.amb", stringify!($name));
                // println!("{reference_file_name}");
                // if !fs::exists(&reference_file_name).unwrap() {
                //     fs::write(&reference_file_name, &content).unwrap();
                // }
                assert_eq!(
                    fs::read(&reference_file_name).unwrap(),
                    content,
                );
            }
        )*
    }
}

add_tests! {
    add_empty: vec![],
    add_1: vec![("../amb-rs-tests/test_files/files/1", "1", 0x30, 73)],
    add_2: vec![("../amb-rs-tests/test_files/files/2", "2", 0x30, 45)],
    add_3: vec![("../amb-rs-tests/test_files/files/3", "3", 0x30, 24)],
    add_1_2: vec![
        ("../amb-rs-tests/test_files/files/1", "1", 0x40, 73),
        ("../amb-rs-tests/test_files/files/2", "2", 0x90, 45),
    ],
    add_2_1: vec![
        ("../amb-rs-tests/test_files/files/2", "2", 0x40, 45),
        ("../amb-rs-tests/test_files/files/1", "1", 0x70, 73),
    ],
    add_3_1: vec![
        ("../amb-rs-tests/test_files/files/3", "3", 0x40, 24),
        ("../amb-rs-tests/test_files/files/1", "1", 0x60, 73),
    ],
    add_1_3: vec![
        ("../amb-rs-tests/test_files/files/1", "1", 0x40, 73),
        ("../amb-rs-tests/test_files/files/3", "3", 0x90, 24),
    ],
    add_2_3: vec![
        ("../amb-rs-tests/test_files/files/2", "2", 0x40, 45),
        ("../amb-rs-tests/test_files/files/3", "3", 0x70, 24),
    ],
    add_3_2: vec![
        ("../amb-rs-tests/test_files/files/3", "3", 0x40, 24),
        ("../amb-rs-tests/test_files/files/2", "2", 0x60, 45),
    ],
    add_1_2_3: vec![
        ("../amb-rs-tests/test_files/files/1", "1", 0x50, 73),
        ("../amb-rs-tests/test_files/files/2", "2", 0xA0, 45),
        ("../amb-rs-tests/test_files/files/3", "3", 0xD0, 24),
    ],
    add_1_3_2: vec![
        ("../amb-rs-tests/test_files/files/1", "1", 0x50, 73),
        ("../amb-rs-tests/test_files/files/3", "3", 0xA0, 24),
        ("../amb-rs-tests/test_files/files/2", "2", 0xC0, 45),
    ],
    add_2_1_3: vec![
        ("../amb-rs-tests/test_files/files/2", "2", 0x50, 45),
        ("../amb-rs-tests/test_files/files/1", "1", 0x80, 73),
        ("../amb-rs-tests/test_files/files/3", "3", 0xD0, 24),
    ],
    add_2_3_1: vec![
        ("../amb-rs-tests/test_files/files/2", "2", 0x50, 45),
        ("../amb-rs-tests/test_files/files/3", "3", 0x80, 24),
        ("../amb-rs-tests/test_files/files/1", "1", 0xA0, 73),
    ],
    add_3_1_2: vec![
        ("../amb-rs-tests/test_files/files/3", "3", 0x50, 24),
        ("../amb-rs-tests/test_files/files/1", "1", 0x70, 73),
        ("../amb-rs-tests/test_files/files/2", "2", 0xC0, 45),
    ],
    add_3_2_1: vec![
        ("../amb-rs-tests/test_files/files/3", "3", 0x50, 24),
        ("../amb-rs-tests/test_files/files/2", "2", 0x70, 45),
        ("../amb-rs-tests/test_files/files/1", "1", 0xA0, 73),
    ],
}
