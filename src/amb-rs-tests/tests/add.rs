use std::{fs, path::Path, vec};

use amb_rs_lib::{amb, amb_management};

mod amb_print;
use crate::amb_print::{AmbPrint, BinaryObjectPrint};

#[test]
fn simple_add() {
    let amb_path = "amb-rs-tests/test_files/files";
    let test_cases  = vec![
        vec![],
        vec![("../amb-rs-tests/test_files/files/1", "1", 0x30, 73)],
        vec![("../amb-rs-tests/test_files/files/2", "2", 0x30, 45)],
        vec![("../amb-rs-tests/test_files/files/3", "3", 0x30, 24)],
        vec![
            ("../amb-rs-tests/test_files/files/1", "1", 0x40, 73),
            ("../amb-rs-tests/test_files/files/2", "2", 0x90, 45),
        ],
        vec![
            ("../amb-rs-tests/test_files/files/2", "2", 0x40, 45),
            ("../amb-rs-tests/test_files/files/1", "1", 0x70, 73),
        ],
        vec![
            ("../amb-rs-tests/test_files/files/3", "3", 0x40, 24),
            ("../amb-rs-tests/test_files/files/1", "1", 0x60, 73),
        ],
        vec![
            ("../amb-rs-tests/test_files/files/1", "1", 0x40, 73),
            ("../amb-rs-tests/test_files/files/3", "3", 0x90, 24),
        ],
        vec![
            ("../amb-rs-tests/test_files/files/2", "2", 0x40, 45),
            ("../amb-rs-tests/test_files/files/3", "3", 0x70, 24),
        ],
        vec![
            ("../amb-rs-tests/test_files/files/3", "3", 0x40, 24),
            ("../amb-rs-tests/test_files/files/2", "2", 0x60, 45),
        ],
        vec![
            ("../amb-rs-tests/test_files/files/1", "1", 0x50, 73),
            ("../amb-rs-tests/test_files/files/2", "2", 0xA0, 45),
            ("../amb-rs-tests/test_files/files/3", "3", 0xD0, 24),
        ],
        vec![
            ("../amb-rs-tests/test_files/files/1", "1", 0x50, 73),
            ("../amb-rs-tests/test_files/files/3", "3", 0xA0, 24),
            ("../amb-rs-tests/test_files/files/2", "2", 0xC0, 45),
        ],
        vec![
            ("../amb-rs-tests/test_files/files/2", "2", 0x50, 45),
            ("../amb-rs-tests/test_files/files/1", "1", 0x80, 73),
            ("../amb-rs-tests/test_files/files/3", "3", 0xD0, 24),
        ],
        vec![
            ("../amb-rs-tests/test_files/files/2", "2", 0x50, 45),
            ("../amb-rs-tests/test_files/files/3", "3", 0x80, 24),
            ("../amb-rs-tests/test_files/files/1", "1", 0xA0, 73),
        ],
        vec![
            ("../amb-rs-tests/test_files/files/3", "3", 0x50, 24),
            ("../amb-rs-tests/test_files/files/1", "1", 0x70, 73),
            ("../amb-rs-tests/test_files/files/2", "2", 0xC0, 45),
        ],
        vec![
            ("../amb-rs-tests/test_files/files/3", "3", 0x50, 24),
            ("../amb-rs-tests/test_files/files/2", "2", 0x70, 45),
            ("../amb-rs-tests/test_files/files/1", "1", 0xA0, 73),
        ]
    ];

    for test_case in test_cases {
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

        // fs::write("../amb-rs-tests/test_files/references-123", &content).unwrap();

        let resulting_binary_objects = test_case.iter().map(|(_, object_name, pointer, length)| BinaryObjectPrint {
            name: object_name.to_string(),
            real_name: object_name.to_string(),
            flag1: 0,
            flag2: 0,
            pointer: *pointer,
            length: *length,
        }).collect();

        assert_eq!(
            amb_management::json::print_from_vec_u8(content, &amb_path.to_string()).unwrap(),
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
    }
}
