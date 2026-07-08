use std::{fs, path::Path};
use amb_rs_lib::amb::Amb;
use amb_rs_tests::{AmbPrint, BinaryObjectPrint};
use common_binary::path::make_safe;

macro_rules! add_tests {
    ($($name:ident: $value:expr,)*) => {
        $(
            #[test]
            fn $name() {
                let amb_path = "amb-rs-tests/test_files/files";
                let test_case: Vec<(&str, &str, Option<String>, u32, u32)> = $value;
                let mut amb = Amb::new_empty();
                amb.amb_path = amb_path.to_string();
                for (file_name, _, internal_name,  _, _) in &test_case {
                    amb.add_file(
                        Path::new(file_name),
                        // It's None or the name, so that should be ok
                        internal_name.clone(),
                    ).unwrap();
                }

                let resulting_binary_objects = test_case.iter().map(|(_, object_name, internal_name, pointer, length)| BinaryObjectPrint {
                    name: make_safe(internal_name.clone().unwrap_or(object_name.to_string()).as_str()),
                    real_name: internal_name.clone().unwrap_or(object_name.to_string()),
                    flag1: 0,
                    flag2: 0,
                    pointer: *pointer,
                    length: *length,
                }).collect();

                amb.prepare_for_print();
                assert_eq!(
                    format!("{amb}"),
                    serde_json::to_string(&AmbPrint {
                        name: amb_path.to_string(),
                        endianness: "little".to_string(),
                        objects: resulting_binary_objects,
                        version: "PC".to_string(),
                    }).unwrap()
                );

                for (object, (file_name, _, _, _, _)) in amb.objects.iter().zip(&test_case) {
                    assert_eq!(object.data, fs::read(file_name).unwrap());
                }

                assert_eq!(amb.objects.len(), test_case.len());

                let reference_file_name = format!("../amb-rs-tests/tests/reference_files/le/{}.amb", stringify!($name));
                let content = amb.write().unwrap();
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
    add_1: vec![("../amb-rs-tests/test_files/files/1", "1", None, 0x30, 73)],
    add_2: vec![("../amb-rs-tests/test_files/files/2", "2", None, 0x30, 45)],
    add_3: vec![("../amb-rs-tests/test_files/files/3", "3", None, 0x30, 23)],
    add_1_2: vec![
        ("../amb-rs-tests/test_files/files/1", "1", None, 0x40, 73),
        ("../amb-rs-tests/test_files/files/2", "2", None, 0x90, 45),
    ],
    add_2_1: vec![
        ("../amb-rs-tests/test_files/files/2", "2", None, 0x40, 45),
        ("../amb-rs-tests/test_files/files/1", "1", None, 0x70, 73),
    ],
    add_3_1: vec![
        ("../amb-rs-tests/test_files/files/3", "3", None, 0x40, 23),
        ("../amb-rs-tests/test_files/files/1", "1", None, 0x60, 73),
    ],
    add_1_3: vec![
        ("../amb-rs-tests/test_files/files/1", "1", None, 0x40, 73),
        ("../amb-rs-tests/test_files/files/3", "3", None, 0x90, 23),
    ],
    add_2_3: vec![
        ("../amb-rs-tests/test_files/files/2", "2", None, 0x40, 45),
        ("../amb-rs-tests/test_files/files/3", "3", None, 0x70, 23),
    ],
    add_3_2: vec![
        ("../amb-rs-tests/test_files/files/3", "3", None, 0x40, 23),
        ("../amb-rs-tests/test_files/files/2", "2", None, 0x60, 45),
    ],
    add_1_2_3: vec![
        ("../amb-rs-tests/test_files/files/1", "1", None, 0x50, 73),
        ("../amb-rs-tests/test_files/files/2", "2", None, 0xA0, 45),
        ("../amb-rs-tests/test_files/files/3", "3", None, 0xD0, 23),
    ],
    add_1_3_2: vec![
        ("../amb-rs-tests/test_files/files/1", "1", None, 0x50, 73),
        ("../amb-rs-tests/test_files/files/3", "3", None, 0xA0, 23),
        ("../amb-rs-tests/test_files/files/2", "2", None, 0xC0, 45),
    ],
    add_2_1_3: vec![
        ("../amb-rs-tests/test_files/files/2", "2", None, 0x50, 45),
        ("../amb-rs-tests/test_files/files/1", "1", None, 0x80, 73),
        ("../amb-rs-tests/test_files/files/3", "3", None, 0xD0, 23),
    ],
    add_2_3_1: vec![
        ("../amb-rs-tests/test_files/files/2", "2", None, 0x50, 45),
        ("../amb-rs-tests/test_files/files/3", "3", None, 0x80, 23),
        ("../amb-rs-tests/test_files/files/1", "1", None, 0xA0, 73),
    ],
    add_3_1_2: vec![
        ("../amb-rs-tests/test_files/files/3", "3", None, 0x50, 23),
        ("../amb-rs-tests/test_files/files/1", "1", None, 0x70, 73),
        ("../amb-rs-tests/test_files/files/2", "2", None, 0xC0, 45),
    ],
    add_3_2_1: vec![
        ("../amb-rs-tests/test_files/files/3", "3", None, 0x50, 23),
        ("../amb-rs-tests/test_files/files/2", "2", None, 0x70, 45),
        ("../amb-rs-tests/test_files/files/1", "1", None, 0xA0, 73),
    ],

    add_as_0: vec![
        ("../amb-rs-tests/test_files/files/1", "1", Some("..\\.\\folder\\file_1".to_string()), 0x40, 73),
        ("../amb-rs-tests/test_files/files/2", "2", Some("..\\.\\folder\\file_2".to_string()), 0x90, 45),
    ],
}