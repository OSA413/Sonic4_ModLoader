use std::env;
use winresource::WindowsResource;

fn main() {
    glib_build_tools::compile_resources(
        &["src/ManagerLauncher/resources"],
        "src/ManagerLauncher/resources/resources.gresource.xml",
        "ManagerLauncher.gresource",
    );

    // TODO: make sure to include the icon only to specified bins
    if env::var_os("CARGO_CFG_WINDOWS").is_some() {
        WindowsResource::new()
            .set_icon("icon.ico")
            .compile()
            .unwrap();
    }
}