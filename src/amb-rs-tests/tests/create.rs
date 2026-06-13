use amb_rs_lib::amb;

#[test]
fn create_empty_pc_le() {
    let amb = amb::Amb::new_empty();
    let content = amb.write().unwrap();

    let reference = std::fs::read("../amb-rs-tests/tests/reference_files/le/add_empty.amb").unwrap();
    assert_eq!(reference, content);
}