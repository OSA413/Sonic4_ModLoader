static PROGRAM: &str = "bin/ManagerLauncher";

fn main() -> () {
    println!("Launching {}...", PROGRAM);
    let process = std::process::Command::new(PROGRAM).spawn();
    match process {
        Ok(_) => hide_console::hide_console(),
        Err(e) => {
            println!("Error launching the program: {}", e);
            std::thread::sleep(std::time::Duration::from_millis(10_000));
        },
    }
}