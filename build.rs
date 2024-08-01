fn main() {
    glib_build_tools::compile_resources(
        &["src/example/resources"],
        "src/example/resources/resources.gresource.xml",
        "actions_6.gresource",
    );
    glib_build_tools::compile_resources(
        &["src/ManagerLauncher/resources"],
        "src/ManagerLauncher/resources/resources.gresource.xml",
        "ManagerLauncher.gresource",
    );
}