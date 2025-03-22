use std::env;
use winresource::WindowsResource;

fn main() {
    glib_build_tools::compile_resources(
        &["src/resources"],
        "src/resources/resources.gresource.xml",
        "ManagerLauncher.gresource",
    );

    if env::var_os("CARGO_CFG_WINDOWS").is_some() {
        WindowsResource::new()
            .set_icon("../../icon.ico")
            .compile()
            .unwrap();
    }
}