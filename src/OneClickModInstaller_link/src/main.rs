static PROGRAM: &str = "bin/OneClickModInstaller";

fn main() {
    println!("Launching {PROGRAM}...");
    let args = std::env::args().collect::<Vec<_>>();
    println!("With the following arguments: {:?}", args);

    // This thing is needed when launching from URI handler on Windows
    if let Ok(current_directory) = std::env::current_dir() {
        if current_directory.to_str().unwrap() == "C:\\WINDOWS\\system32" {
            let current_exe = std::env::current_exe().unwrap();
            let actual_directory = current_exe.parent().unwrap();
            std::env::set_current_dir(actual_directory).unwrap();
        }
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