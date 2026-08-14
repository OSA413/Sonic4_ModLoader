use std::path::Path;

static PROGRAM: &str = "bin/OneClickModInstaller";

fn main() {
    println!("Launching {PROGRAM}...");
    let args = std::env::args().collect::<Vec<_>>();
    println!("With the following arguments: {:?}", args);

    // This thing is needed when launching from URI handler on Windows
    if let Ok(current_directory) = std::env::current_dir()
        && current_directory == Path::new("C:\\WINDOWS\\system32") {
            let current_exe = std::env::current_exe()
                .expect("Couldn't get current exe path to change current path from system's one, exiting...");
            let actual_directory = current_exe.parent()
                .expect("Couldn't get directory of current exe to change current path from system's one, exiting...");
            std::env::set_current_dir(actual_directory)
                .expect("Couldn't change current working directory from the system's one, exiting...");
        }

    let process = std::process::Command::new(PROGRAM).args(args).spawn();
    match process {
        Ok(_) => hide_console::hide_console(),
        Err(e) => {
            println!("Error launching the program: {e}");
            std::thread::sleep(std::time::Duration::from_millis(10_000));
        },
    }
}