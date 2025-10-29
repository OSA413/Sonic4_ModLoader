fn main() -> () {
    let program = "AMBPatcher.exe";

    println!("Launching {}...", program);
    let process = std::process::Command::new(program).args(std::env::args().skip(1)).spawn();
    match process {
        Ok(_) => (),
        Err(e) => {
            println!("Error launching the program: {}", e);
            std::thread::sleep(std::time::Duration::from_millis(10_000));
        },
    }
}