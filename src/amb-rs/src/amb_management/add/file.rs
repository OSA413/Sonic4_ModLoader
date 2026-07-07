use std::path::Path;
use amb_rs_lib::amb::Amb;
use common_binary::error::CommonBinaryError;

pub fn add_file_to_file(target_amb_file: &Path, file_to_add: &Path, internal_file_name: Option<String>) -> Result<(), CommonBinaryError> {
    let mut amb = Amb::new_from_file_name(&target_amb_file.display().to_string())?;
    amb.add_file(file_to_add, internal_file_name)
}

#[cfg(test)]
mod tests {
    use std::fs;
    use super::*;
    use amb_rs_tests::{AmbPrint, BinaryObjectPrint};

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
                        name: Amb::make_name_safe(internal_name.clone().unwrap_or(object_name.to_string()).as_str()),
                        real_name: internal_name.clone().unwrap_or(object_name.to_string()),
                        flag1: 0,
                        flag2: 0,
                        pointer: *pointer,
                        length: *length,
                    }).collect();

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
}

#[cfg(test)]
mod nested_tests {
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

}