use std::{ffi::OsStr, fs, path::Path};
use amb_rs_lib::amb::Amb;
use common::walk_dir;
use common_binary::error::CommonBinaryError;

fn continue_extraction(amb: Amb, base_dir: &Path) {
    for binary_object in amb.objects {
        // We do this because some of the files inside Episode 1 have no names and are AMB
        let probably_amb = Amb::new_from_binary_object(&binary_object);
        match probably_amb {
            Ok(inner_amb) => {
                println!("Extracting {base_dir:?}");
                continue_extraction(inner_amb, &base_dir.join(&binary_object.name));
            }
            Err(_) => {
                let file_path = base_dir.join(&binary_object.name);
                fs::create_dir_all(file_path.parent().unwrap()).unwrap();
                match fs::write(file_path, &binary_object.data) {
                    Ok(_) => println!("Extracted {}", &binary_object.name),
                    Err(e) => eprintln!("Error: {e}"),
                }
            },
        }
    }
}

pub fn extract_amb(file_or_dir: String, destination: Option<String>) -> Result<(), CommonBinaryError> {
    let path = Path::new(&file_or_dir);
    let probably_amb_files = if path.is_file() {
        vec![path.to_path_buf()]
    } else {
        walk_dir::walk_dir(path, Some(OsStr::new("amb")))
    };

    for entry in probably_amb_files {
        let path = entry.to_str().unwrap().to_string();
        println!("Extracting {path}");
        let amb = Amb::new_from_file_name(&path)?;
        let base_dir = match destination {
            Some(ref destination) => Path::new(destination).join(Path::new(&path).parent().unwrap()),
            None => Path::new(&format!("{path}_extracted")).to_path_buf(),
        };
        continue_extraction(amb, &base_dir);
    }
    println!("Done!");
    Ok(())
}

#[cfg(test)]
mod tests {
    use std::fs;
    use common::walk_dir;
    use super::*;

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
                    extract_amb(file_path, Some(temp_dir_str.clone())).unwrap();

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
}