use std::fs;

use amb_rs_lib::amb_management;
use common::walk_dir;

macro_rules! extract_tests {
    ($($name:ident: $value:expr,)*) => {
        $(
            #[test]
            fn $name() {
                let (source_ref, expected_objects): (&str, Vec<(&str, &str)>) = $value;

                let file_path = format!("../amb-rs-tests/tests/reference_files/le/{source_ref}");
                let temp_dir = std::env::temp_dir().join("amb-rs-tests").join(format!("extract_test_{source_ref}"));
                let temp_dir_str = temp_dir.display().to_string();

                // BEFORE, it may fail because the dir may not exist
                let _ = fs::remove_dir_all(&temp_dir);

                // TEST
                amb_management::extract::extract_amb(file_path, Some(temp_dir_str.clone())).unwrap();

                for (name, data_path) in &expected_objects {
                    assert_eq!(
                        fs::read(format!("../amb-rs-tests/{data_path}")).unwrap(),
                        fs::read(format!("{temp_dir_str}/{name}")).unwrap()
                    );
                }

                let dir_files = walk_dir::walk_dir(&temp_dir, None);
                let dir_dirs = walk_dir::walk_dir_for_dirs(&temp_dir);

                assert_eq!(dir_files.len(), expected_objects.len());
                assert_eq!(dir_dirs.len(), 0);

                // AFTER, must not fail, or else something is wrong
                if !expected_objects.is_empty() {
                    fs::remove_dir_all(&temp_dir).unwrap();
                }
            }
        )*
    }
}

extract_tests! {
    extract_from_empty: (
        "add_empty.amb",
        vec![]
    ),
    extract_from_1: (
        "add_1.amb",
        vec![("1", "test_files/files/1")]
    ),
    extract_from_2: (
        "add_2.amb",
        vec![("2", "test_files/files/2")]
    ),
    extract_from_3: (
        "add_3.amb",
        vec![("3", "test_files/files/3")]
    ),
    extract_from_1_2: (
        "add_1_2.amb",
        vec![
            ("1", "test_files/files/1"),
            ("2", "test_files/files/2"),
        ]
    ),
    extract_from_1_3: (
        "add_1_3.amb",
        vec![
            ("1", "test_files/files/1"),
            ("3", "test_files/files/3"),
        ]
    ),
    extract_from_2_3: (
        "add_2_3.amb",
        vec![
            ("2", "test_files/files/2"),
            ("3", "test_files/files/3"),
        ]
    ),
    extract_from_1_2_3: (
        "add_1_2_3.amb",
        vec![
            ("1", "test_files/files/1"),
            ("2", "test_files/files/2"),
            ("3", "test_files/files/3"),
        ]
    ),

    // Shuffled content doesn't affect the extraction result
    extract_from_3_2_1: (
        "add_3_2_1.amb",
        vec![
            ("1", "test_files/files/1"),
            ("2", "test_files/files/2"),
            ("3", "test_files/files/3"),
        ]
    ),
}