use std::fs;

use amb_rs_lib::amb;

macro_rules! read_reference_tests {
    ($($name:ident: $value:expr,)*) => {
        $(
            #[test]
            fn $name() {
                let (file_name, expected_objects): (&str, Vec<(&str, &str)>) = $value;

                let file_path = format!("../amb-rs-tests/tests/reference_files/{file_name}");
                let amb = amb::Amb::new_from_file_name(&file_path).unwrap();

                assert_eq!(amb.objects.len(), expected_objects.len());

                for (object, (expected_name, expected_data_path)) in amb.objects.iter().zip(&expected_objects) {
                    assert_eq!(object.name, *expected_name);
                    let expected_data = fs::read(format!("../amb-rs-tests/{expected_data_path}")).unwrap();
                    assert_eq!(object.data, expected_data);
                }
            }
        )*
    }
}

// TODO: add header/metadata check of AMB and binary objects later
read_reference_tests! {
    read_empty: (
        "add_empty.amb",
        vec![]
    ),
    read_1: (
        "add_1.amb",
        vec![("1", "test_files/files/1")]
    ),
    read_2: (
        "add_2.amb",
        vec![("2", "test_files/files/2")]
    ),
    read_3: (
        "add_3.amb",
        vec![("3", "test_files/files/3")]
    ),
    read_1_2: (
        "add_1_2.amb",
        vec![
            ("1", "test_files/files/1"),
            ("2", "test_files/files/2")
        ]
    ),
    read_2_1: (
        "add_2_1.amb",
        vec![
            ("2", "test_files/files/2"),
            ("1", "test_files/files/1")
        ]
    ),
    read_3_1: (
        "add_3_1.amb",
        vec![
            ("3", "test_files/files/3"),
            ("1", "test_files/files/1")
        ]
    ),
    read_1_3: (
        "add_1_3.amb",
        vec![
            ("1", "test_files/files/1"),
            ("3", "test_files/files/3")
        ]
    ),
    read_2_3: (
        "add_2_3.amb",
        vec![
            ("2", "test_files/files/2"),
            ("3", "test_files/files/3")
        ]
    ),
    read_3_2: (
        "add_3_2.amb",
        vec![
            ("3", "test_files/files/3"),
            ("2", "test_files/files/2")
        ]
    ),
    read_1_2_3: (
        "add_1_2_3.amb",
        vec![
            ("1", "test_files/files/1"),
            ("2", "test_files/files/2"),
            ("3", "test_files/files/3")
        ]
    ),
    read_1_3_2: (
        "add_1_3_2.amb",
        vec![
            ("1", "test_files/files/1"),
            ("3", "test_files/files/3"),
            ("2", "test_files/files/2")
        ]
    ),
    read_2_1_3: (
        "add_2_1_3.amb",
        vec![
            ("2", "test_files/files/2"),
            ("1", "test_files/files/1"),
            ("3", "test_files/files/3")
        ]
    ),
    read_2_3_1: (
        "add_2_3_1.amb",
        vec![
            ("2", "test_files/files/2"),
            ("3", "test_files/files/3"),
            ("1", "test_files/files/1")
        ]
    ),
    read_3_1_2: (
        "add_3_1_2.amb",
        vec![
            ("3", "test_files/files/3"),
            ("1", "test_files/files/1"),
            ("2", "test_files/files/2")
        ]
    ),
    read_3_2_1: (
        "add_3_2_1.amb",
        vec![
            ("3", "test_files/files/3"),
            ("2", "test_files/files/2"),
            ("1", "test_files/files/1")
        ]
    ),
}
