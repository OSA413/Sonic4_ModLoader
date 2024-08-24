fn main() {
    glib_build_tools::compile_resources(
        &["src/ManagerLauncher/resources"],
        "src/ManagerLauncher/resources/resources.gresource.xml",
        "ManagerLauncher.gresource",
    );
}