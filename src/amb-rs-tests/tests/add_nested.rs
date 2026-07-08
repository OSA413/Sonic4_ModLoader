use std::{fs, path::Path, vec};

use amb_rs_lib::amb::Amb;

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

            root_amb.add_u8_vec(
                &amb.write().unwrap(),
                None,
                None,
                &name.clone()
            ).unwrap();
        }
        Object::BinaryObject(name, path) => {
            root_amb.add_file(
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

            root_amb.add_u8_vec(
                &amb.write().unwrap(),
                None,
                None,
                &name.clone()
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
            let parent_file = match parent_file.is_empty() {
                true => name,
                false => &format!("{parent_file}\\{name}")
            };
            root_amb.add_file(
                Path::new(path),
                Some(parent_file.clone())
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
                    format!("{amb_way1}"),
                    format!("{amb_way2}"),
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

// TODO add header/flag values to AMB and binary objects
add_nested_tests! {
    actions: (
        "actions.amb".to_string(),
        vec![
            Object::Amb(
                "new.amb".to_string(),
                vec![
                    Object::BinaryObject("address-book-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/address-book-new.png".to_string()),
                    Object::BinaryObject("bookmark-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/bookmark-new.png".to_string()),
                    Object::BinaryObject("tab-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/tab-new.png".to_string()),
                    Object::BinaryObject("contact-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/contact-new.png".to_string()),
                    Object::BinaryObject("appointment-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/appointment-new.png".to_string()),
                    Object::BinaryObject("window-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/window-new.png".to_string()),
                    Object::BinaryObject("folder-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/folder-new.png".to_string()),
                    Object::BinaryObject("document-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/document-new.png".to_string()),
                ]
            ),
            Object::Amb(
                "edit.amb".to_string(),
                vec![
                    Object::BinaryObject("edit-find-replace.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-find-replace.png".to_string()),
                    Object::BinaryObject("edit-select-all.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-select-all.png".to_string()),
                    Object::BinaryObject("edit-cut.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-cut.png".to_string()),
                    Object::BinaryObject("edit-undo.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-undo.png".to_string()),
                    Object::BinaryObject("list-remove.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/list-remove.png".to_string()),
                    Object::BinaryObject("edit-copy.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-copy.png".to_string()),
                    Object::BinaryObject("edit-find.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-find.png".to_string()),
                    Object::BinaryObject("edit-redo.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-redo.png".to_string()),
                    Object::BinaryObject("list-add.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/list-add.png".to_string()),
                    Object::BinaryObject("edit-delete.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-delete.png".to_string()),
                    Object::BinaryObject("edit-paste.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-paste.png".to_string()),
                    Object::BinaryObject("edit-clear.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/edit-clear.png".to_string()),
                ]
            )
        ]
    ),
    devices: (
        "devices.amb".to_string(),
        vec![
            Object::BinaryObject("input-mouse.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/input-mouse.png".to_string()),
            Object::BinaryObject("drive-optical.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/drive-optical.png".to_string()),
            Object::BinaryObject("drive-removable-media.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/drive-removable-media.png".to_string()),
            Object::BinaryObject("input-keyboard.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/input-keyboard.png".to_string()),
            Object::BinaryObject("network-wireless.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/network-wireless.png".to_string()),
            Object::BinaryObject("battery.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/battery.png".to_string()),
            Object::BinaryObject("multimedia-player.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/multimedia-player.png".to_string()),
            Object::BinaryObject("audio-card.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/audio-card.png".to_string()),
            Object::BinaryObject("media-optical.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/media-optical.png".to_string()),
            Object::BinaryObject("audio-input-microphone.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/audio-input-microphone.png".to_string()),
            Object::BinaryObject("camera-photo.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/camera-photo.png".to_string()),
            Object::BinaryObject("video-display.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/video-display.png".to_string()),
            Object::BinaryObject("media-floppy.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/media-floppy.png".to_string()),
            Object::BinaryObject("input-gaming.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/input-gaming.png".to_string()),
            Object::BinaryObject("printer.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/printer.png".to_string()),
            Object::BinaryObject("network-wired.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/network-wired.png".to_string()),
            Object::BinaryObject("drive-harddisk.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/drive-harddisk.png".to_string()),
            Object::BinaryObject("media-flash.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/media-flash.png".to_string()),
            Object::BinaryObject("computer.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/computer.png".to_string()),
            Object::BinaryObject("camera-video.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/camera-video.png".to_string()),
        ]
    ),
    notifications: (
        "notifications.amb".to_string(),
        vec![
            Object::Amb(
                "info.amb".to_string(),
                vec![
                    Object::BinaryObject("system-lock-screen.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/system-lock-screen.png".to_string()),
                    Object::BinaryObject("software-update-available.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/software-update-available.png".to_string()),
                    Object::BinaryObject("emblem-important.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/emblem-important.png".to_string()),
                    Object::BinaryObject("mail-message-new.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/mail-message-new.png".to_string()),
                    Object::BinaryObject("dialog-information.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/dialog-information.png".to_string()),
                    Object::BinaryObject("document-print-preview.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/document-print-preview.png".to_string()),
                ]
            ),
            Object::Amb(
                "warnings.amb".to_string(),
                vec![
                    Object::BinaryObject("dialog-warning.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/dialog-warning.png".to_string()),
                    Object::BinaryObject("emblem-unreadable.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/emblem-unreadable.png".to_string()),
                    Object::BinaryObject("process-stop.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/process-stop.png".to_string()),
                    Object::BinaryObject("dialog-error.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/dialog-error.png".to_string()),
                    Object::BinaryObject("software-update-urgent.png".to_string(), "../amb-rs-tests/test_files/tango-icon-theme/software-update-urgent.png".to_string()),
                ]
            )
        ]
    ),
}