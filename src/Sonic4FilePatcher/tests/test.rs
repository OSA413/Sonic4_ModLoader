#[cfg(test)]
mod tests {
    use std::{fs, path::Path};

    use assert_cmd::Command;
    use common::walk_dir;
    use predicates::prelude::*;
    use sha2::{Digest, Sha256};

    pub fn get_sha256(data: impl AsRef<[u8]>) -> String {
        Sha256::digest(data).iter().map(|x| format!("{x:02x}")).collect()
    }

    fn ml_prepare(path: &str) {
        let temp_dir = std::env::temp_dir().join("mod-loader-tests").join(path);

        // BEFORE, it may fail because the dir may not exist
        let _ = fs::remove_dir_all(&temp_dir);
        fs::create_dir_all(&temp_dir).unwrap();

        let mods_dir = temp_dir.join("mods");
        fs::create_dir_all(&mods_dir).unwrap();
        fs::write(&mods_dir.join("mods.ini"), "").unwrap();
        fs::write(&temp_dir.join("Sonic4FilePatcher.ini"), "use_amb_rs_instead=1").unwrap();

        // Original files
        let files = [
            ("../amb-rs-tests/tests/reference_files/le/nested/actions.amb", "textures/actions.amb"),
            ("../amb-rs-tests/tests/reference_files/le/nested/devices.amb", "textures/devices.amb"),
            ("../amb-rs-tests/tests/reference_files/le/nested/notifications.amb", "textures/notifications.amb"),
        ];

        for (src, dest) in files {
            let dest = &temp_dir.join(dest);
            fs::create_dir_all(dest.parent().unwrap()).unwrap();
            fs::copy(
                Path::new(src),
                dest
            )
            .unwrap();
        }

        // 1
        fs::create_dir_all(&mods_dir.join("1/textures/actions.amb/edit.amb")).unwrap();
        let files = [
            "format-indent-less.png",
            "format-indent-more.png",
            "format-justify-center.png",
            "format-justify-fill.png",
            "format-justify-left.png",
            "format-justify-right.png",
            "format-text-bold.png",
            "format-text-italic.png",
            "format-text-strikethrough.png",
            "format-text-underline.png",
        ];

        for file in files {
            fs::copy(
                Path::new("../amb-rs-tests/test_files/tango-icon-theme").join(file),
                &mods_dir
                    .join("1/textures/actions.amb/edit.amb")
                    .join(file),
            )
            .unwrap();
        }

        // 2
        fs::create_dir_all(&mods_dir.join("2/textures/devices.amb")).unwrap();
        fs::create_dir_all(&mods_dir.join("2/textures/notifications.amb/warnings.amb")).unwrap();

        let files = [
            ("2/textures/devices.amb/computer.png", "accessories-calculator.png"),
            ("2/textures/devices.amb/network-wired.png", "preferences-desktop-remote-desktop.png"),
            ("2/textures/devices.amb/printer.png", "preferences-desktop-wallpaper.png"),
            ("2/textures/devices.amb/video-display.png", "utilities-terminal.png"),
            ("2/textures/notifications.amb/warnings.amb/dialog-error.png", "dialog-information.png"),
            ("2/textures/notifications.amb/warnings.amb/dialog-warning.png", "document-print-preview.png"),
            ("2/textures/notifications.amb/warnings.amb/emblem-unreadable.png", "emblem-unreadable.png"),
            ("2/textures/notifications.amb/warnings.amb/process-stop.png", "mail-message-new.png"),
            ("2/textures/notifications.amb/warnings.amb/software-update-available.png", "software-update-available.png"),
            ("2/textures/notifications.amb/warnings.amb/software-update-urgent.png", "software-update-urgent.png"),
            ("2/textures/notifications.amb/warnings.amb/system-lock-screen.png", "system-lock-screen.png"),
        ];

        for (dest, src) in files {
            fs::copy(
                Path::new("../amb-rs-tests/test_files/tango-icon-theme").join(src),
                &mods_dir.join(dest),
            )
            .unwrap();
        }
        
        // 3
        fs::create_dir_all(&mods_dir.join("3/textures/actions.amb/new.amb")).unwrap();
        
        let files = [
            ("3/textures/actions.amb/new.amb/bookmark-new.png", "accessories-text-editor.png"),
            ("3/textures/actions.amb/new.amb/contact-new.png", "preferences-desktop-theme.png"),
            ("3/textures/actions.amb/new.amb/document-new.png", "internet-news-reader.png"),
            ("3/textures/actions.amb/new.amb/folder-new.png", "folder-saved-search.png"),
            ("3/textures/actions.amb/new.amb/window-new.png", "preferences-desktop-screensaver.png"),
        ];

        for (dest, src) in files {
            fs::copy(
                Path::new("../amb-rs-tests/test_files/tango-icon-theme").join(src),
                &mods_dir.join(dest),
            )
            .unwrap();
        }

        fs::copy(
            "../amb-rs-tests/tests/reference_files/le/nested/new_media_for_ml_3.amb",
            &mods_dir.join("3/textures/actions.amb/media.amb"),
        ).unwrap();

        // 4
        fs::create_dir_all(&mods_dir.join("4/textures/actions.amb/edit.amb")).unwrap();
        fs::create_dir_all(&mods_dir.join("4/textures/devices.amb")).unwrap();
        
        let files = [
            ("4/textures/actions.amb/edit.amb/edit-copy.png", "media-optical.png"),
            ("4/textures/actions.amb/edit.amb/edit-cut.png", "applications-accessories.png"),
            ("4/textures/actions.amb/edit.amb/edit-delete.png", "user-trash.png"),
            ("4/textures/actions.amb/edit.amb/edit-find.png", "system-search.png"),
            ("4/textures/actions.amb/edit.amb/edit-redo.png", "go-next.png"),
            ("4/textures/actions.amb/edit.amb/edit-undo.png", "go-previous.png"),
            ("4/textures/actions.amb/edit.amb/format-text-bold.png", "start-here-alt.png"),
            ("4/textures/actions.amb/edit.amb/format-text-italic.png", "start-here.png"),
            ("4/textures/devices.amb/computer.png", "preferences-desktop-peripherals.png"),            
            ("4/textures/devices.amb/network-wired.png", "applications-internet.png"),
            ("4/textures/devices.amb/printer.png", "applications-graphics.png"),
        ];

        for (dest, src) in files {
            fs::copy(
                Path::new("../amb-rs-tests/test_files/tango-icon-theme").join(src),
                &mods_dir.join(dest),
            )
            .unwrap();
        }

        fs::copy(
            "../amb-rs-tests/tests/reference_files/le/nested/new_weather_for_ml_4.amb",
            &mods_dir.join("4/textures/weather.amb"),
        ).unwrap();
    }

    fn ml_change_mod_ini(path: &str, mods: &str) {
        let temp_dir = std::env::temp_dir().join("mod-loader-tests").join(path);
        let mods_ini = temp_dir.join("mods/mods.ini");
        fs::write(mods_ini, mods.split("").collect::<Vec<_>>().join("\n")).unwrap();
    }

    fn ml_run(path: &str) {
        let temp_dir = std::env::temp_dir().join("mod-loader-tests").join(path);
        
        let mut cmd = Command::cargo_bin("Sonic4FilePatcher").unwrap();
        
        let assert = cmd
            .current_dir(temp_dir)
            .assert();

        assert
            .success()
            .stdout(predicate::function(
                |x: &[u8]| x.starts_with(b"Using amb-rs...\n")
                    && x.ends_with(b"Patching complete!\n")
            ));
    }

    
    fn ml_recover(path: &str) {
        let temp_dir = std::env::temp_dir().join("mod-loader-tests").join(path);
        
        let mut cmd = Command::cargo_bin("Sonic4FilePatcher").unwrap();
        
        let assert = cmd
            .arg("recover")
            .current_dir(temp_dir)
            .assert();

        assert
            .success();
    }

    fn ml_check(path: &str) {
        let temp_dir = std::env::temp_dir().join("mod-loader-tests").join(path);
        let dirs_to_not_check = [
            temp_dir.join("mods"),
            temp_dir.join("Sonic4FilePatcher.ini"),
            temp_dir.join("mods_sha"),
        ];

        let files = {
            let files = walk_dir::walk_dir(&temp_dir, None);
            let mut files = files
                .iter().filter(|path| !dirs_to_not_check.iter().any(|d| path.starts_with(d)))
                .map(|x| x.to_owned())
                .collect::<Vec<_>>();
            files.sort();
            files
        };
        let temp_dir_str = temp_dir.display().to_string();

        let result = files.iter()
            .map(|file| format!(
                "{} {}",
                get_sha256(fs::read(file).unwrap()),
                file.display().to_string().replace("\\", "/").chars().skip(temp_dir_str.len()).collect::<String>()
            ))
            .collect::<Vec<_>>();

        let reference_file = format!("../Sonic4FilePatcher/tests/hashes/{path}");
        // if !Path::new(&reference_file).exists() {
        //     fs::write(&reference_file, &result.join("\n")).unwrap();
        // }

        assert_eq!(
            result,
            fs::read_to_string(reference_file).unwrap().lines().collect::<Vec<_>>(),
        );
    }
    
    #[test]
    fn empty_dir() {
        let temp_dir = std::env::temp_dir().join("mod-loader-tests").join("empty_dir");

        // BEFORE, it may fail because the dir may not exist
        let _ = fs::remove_dir_all(&temp_dir);
        fs::create_dir_all(&temp_dir).unwrap();

        let mut cmd = Command::cargo_bin("Sonic4FilePatcher").unwrap();
        
        let assert = cmd
            .current_dir(temp_dir)
            .assert();

        assert
            .success()
            .stdout(predicate::function(|x: &[u8]| x.starts_with(b"Usage:\n")));
    }

    #[test]
    fn empty_mods_ini_without_config() {
        let temp_dir = std::env::temp_dir().join("mod-loader-tests").join("empty_mods_ini_without_config");

        // BEFORE, it may fail because the dir may not exist
        let _ = fs::remove_dir_all(&temp_dir);
        fs::create_dir_all(&temp_dir).unwrap();

        fs::create_dir_all(&temp_dir.join("mods")).unwrap();
        fs::write(&temp_dir.join("mods").join("mods.ini"), "").unwrap();

        let mut cmd = Command::cargo_bin("Sonic4FilePatcher").unwrap();
        
        let assert = cmd
            .current_dir(temp_dir)
            .assert();

        assert
            .success()
            .stdout(predicate::function(
                |x: &[u8]| x.starts_with(b"Error loading Sonic4FilePatcher.ini")
                    && x.ends_with(b"Using AMBPatcher...\n")
            ));
    }

    #[test]
    fn empty_mods_ini_with_config() {
        let temp_dir = std::env::temp_dir().join("mod-loader-tests").join("empty_mods_ini_with_config");

        // BEFORE, it may fail because the dir may not exist
        let _ = fs::remove_dir_all(&temp_dir);
        fs::create_dir_all(&temp_dir).unwrap();

        fs::create_dir_all(&temp_dir.join("mods")).unwrap();
        fs::write(&temp_dir.join("mods").join("mods.ini"), "").unwrap();
        fs::write(&temp_dir.join("Sonic4FilePatcher.ini"), "use_amb_rs_instead=1").unwrap();

        let mut cmd = Command::cargo_bin("Sonic4FilePatcher").unwrap();
        
        let assert = cmd
            .current_dir(temp_dir)
            .assert();

        assert
            .success()
            .stdout(predicate::function(
                |x: &[u8]| x.starts_with(b"Using amb-rs...\n")
                    && x.ends_with(b"Patching complete!\n")
            ));
    }
    
    #[test]
    fn ml_empty() {
        let path = "ml_empty";
        ml_prepare(path);
        ml_change_mod_ini(path, "");
        ml_run(path);
        ml_check(path);
    }

    #[test]
    fn ml_single() {
        let path = "ml_single";
        ml_prepare(path);
        ml_change_mod_ini(path, "2");
        ml_run(path);
        ml_check(path);
    }

    #[test]
    fn ml_multiple() {
        let path = "ml_multiple";
        ml_prepare(path);
        ml_change_mod_ini(path, "1243");
        ml_run(path);
        ml_check(path);
    }

    #[test]
    fn ml_inversed() {
        let path = "ml_inversed";
        ml_prepare(path);
        ml_change_mod_ini(path, "3421");
        ml_run(path);
        ml_check(path);
    }

    #[test]
    fn ml_recover_test() {
        let path = "ml_recover";
        ml_prepare(path);
        ml_change_mod_ini(path, "1243");
        ml_run(path);
        ml_recover(path);
        ml_check(path);
    }

    #[test]
    fn ml_changed_0() {
        let path = "ml_changed_0";
        ml_prepare(path);
        ml_change_mod_ini(path, "1");
        ml_run(path);
        ml_change_mod_ini(path, "3");
        ml_run(path);
        ml_check(path);
    }

    #[test]
    fn ml_changed_1() {
        let path = "ml_changed_1";
        ml_prepare(path);
        ml_change_mod_ini(path, "2");
        ml_run(path);
        ml_change_mod_ini(path, "134");
        ml_run(path);
        ml_check(path);
    }

    #[test]
    fn ml_changed_2() {
        let path = "ml_changed_2";
        ml_prepare(path);
        ml_change_mod_ini(path, "4");
        ml_run(path);
        ml_change_mod_ini(path, "4123");
        ml_run(path);
        ml_check(path);
    }

    #[test]
    fn ml_changed_3() {
        let path = "ml_changed_3";
        ml_prepare(path);
        ml_change_mod_ini(path, "31");
        ml_run(path);
        ml_change_mod_ini(path, "42");
        ml_run(path);
        ml_check(path);
    }

    #[test]
    fn ml_changed_4() {
        let path = "ml_changed_4";
        ml_prepare(path);
        ml_change_mod_ini(path, "24");
        ml_run(path);
        ml_change_mod_ini(path, "4321");
        ml_run(path);
        ml_check(path);
    }

    #[test]
    fn ml_changed_5() {
        let path = "ml_changed_5";
        ml_prepare(path);
        ml_change_mod_ini(path, "324");
        ml_run(path);
        ml_change_mod_ini(path, "1");
        ml_run(path);
        ml_check(path);
    }

    #[test]
    fn ml_changed_6() {
        let path = "ml_changed_6";
        ml_prepare(path);
        ml_change_mod_ini(path, "123");
        ml_run(path);
        ml_change_mod_ini(path, "2314");
        ml_run(path);
        ml_check(path);
    }
}