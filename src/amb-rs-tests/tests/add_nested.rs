use std::{fs, path::Path, vec};

use amb_rs_lib::{amb::Amb, amb_management};

enum Object {
    Amb(String, Vec<Object>),
    BinaryObject(String, String),
}

// We can do that two different ways:
// 1. Create each AMB separately and then combine it into the single AMB
// 2. Create empty AMBs inside a parent AMB and then add files with proper names as file patcher does
// Both ways are worth testing

fn handle_recursive_add_way1(root_amb: &mut Amb, object: &Object) {
    match object {
        Object::Amb(name, objects) => {
            let mut amb = Amb::new_empty();
            amb.amb_path = name.clone();

            for object in objects {
                handle_recursive_add_way1(&mut amb, object);
            }

            amb_management::add::file::add_vec_u8_to_amb(
                root_amb,
                name,
                &amb.write().unwrap(),
                Some(name.clone())
            ).unwrap();
        }
        Object::BinaryObject(name, path) => {
            amb_management::add::file::add_file_to_amb(
                root_amb,
                Path::new(path),
                Some(name.clone())
            ).unwrap();
        }
    }
}

fn handle_recursive_add_way2_amb(root_amb: &mut Amb, object: &Object) {
    match object {
        Object::Amb(name, objects) => {
            let mut amb = Amb::new_empty();
            amb.amb_path = name.clone();

            for object in objects {
                handle_recursive_add_way2_amb(&mut amb, object);
            }

            amb_management::add::file::add_vec_u8_to_amb(
                root_amb,
                name,
                &amb.write().unwrap(),
                Some(name.clone())
            ).unwrap();
            
        }
        Object::BinaryObject(_name, _path) => {}
    }
}

fn handle_recursive_add_way2_bo(root_amb: &mut Amb, object: &Object, parent_file: &String) {
    match object {
        Object::Amb(name, objects) => {
            for object in objects {
                let parent_file = match parent_file.is_empty() {
                    true => name,
                    false => &format!("{parent_file}\\{name}")
                };
                handle_recursive_add_way2_bo(root_amb, object, parent_file);
            }
        }
        Object::BinaryObject(name, path) => {
            amb_management::add::file::add_file_to_amb(
                root_amb,
                Path::new(path),
                Some(format!("{parent_file}\\{name}"))
            ).unwrap();
        }
    }
}


macro_rules! add_nested_tests {
    ($($name:ident: $value:expr,)*) => {
        $(
            #[test]
            fn $name() {
                let (file_name, objects): (String, Vec<Object>) = $value;

                let mut amb_way1 = Amb::new_empty();
                amb_way1.amb_path = file_name.clone();

                for object in &objects {
                    handle_recursive_add_way1(&mut amb_way1, object);
                }

                let mut amb_way2 = Amb::new_empty();
                amb_way2.amb_path = file_name.clone();

                for object in &objects {
                    handle_recursive_add_way2_amb(&mut amb_way2, object);
                }
                for object in &objects {
                    handle_recursive_add_way2_bo(&mut amb_way2, object, &"".to_string());
                }

                // Firstly check JSON representations are equal
                assert_eq!(
                    amb_management::json::print_from_amb(&amb_way1),
                    amb_management::json::print_from_amb(&amb_way2),
                );

                let reference = amb_way1.write().unwrap();
                assert_eq!(
                    reference,
                    amb_way2.write().unwrap(),
                );

                
                let reference_file_name = format!("../amb-rs-tests/tests/reference_files/le/nested/{}.amb", stringify!($name));
                // println!("{reference_file_name}");
                // if !fs::exists(&reference_file_name).unwrap() {
                //     fs::write(&reference_file_name, &reference).unwrap();
                // }
                assert_eq!(
                    fs::read(&reference_file_name).unwrap(),
                    reference,
                );
            }
        )*
    }
}

add_nested_tests! {
    actions: (
        "actions.amb".to_string(),
        vec![
            Object::Amb(
                "new.amb".to_string(),
                vec![
                    Object::BinaryObject("address-book-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/address-book-new.png".to_string()),
                    Object::BinaryObject("appointment-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/appointment-new.png".to_string()),
                    Object::BinaryObject("bookmark-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/bookmark-new.png".to_string()),
                    Object::BinaryObject("contact-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/contact-new.png".to_string()),
                    Object::BinaryObject("document-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/document-new.png".to_string()),
                    Object::BinaryObject("folder-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/folder-new.png".to_string()),
                    Object::BinaryObject("tab-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/tab-new.png".to_string()),
                    Object::BinaryObject("window-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/window-new.png".to_string()),
                ]
            ),
            Object::Amb(
                "edit.amb".to_string(),
                vec![
                    Object::BinaryObject("edit-clear.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-clear.png".to_string()),
                    Object::BinaryObject("edit-copy.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-copy.png".to_string()),
                    Object::BinaryObject("edit-cut.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-cut.png".to_string()),
                    Object::BinaryObject("edit-delete.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-delete.png".to_string()),
                    Object::BinaryObject("edit-find-replace.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-find-replace.png".to_string()),
                    Object::BinaryObject("edit-find.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-find.png".to_string()),
                    Object::BinaryObject("edit-paste.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-paste.png".to_string()),
                    Object::BinaryObject("edit-redo.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-redo.png".to_string()),
                    Object::BinaryObject("edit-select-all.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-select-all.png".to_string()),
                    Object::BinaryObject("edit-undo.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-undo.png".to_string()),
                    Object::BinaryObject("list-add.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/list-add.png".to_string()),
                    Object::BinaryObject("list-remove.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/list-remove.png".to_string()),
                ]
            )
        ]
    ),
}
