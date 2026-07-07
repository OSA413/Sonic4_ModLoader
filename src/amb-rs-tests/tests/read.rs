use amb_rs_lib::amb::Amb;
use amb_rs_tests::AmbPrint;

#[test]
fn empty() {
    let file_name = "../amb-rs-tests/amb_files/pc/EMPTY.AMB".to_string();
    assert_eq!(
        format!("{}", Amb::new_from_file_name(&file_name).unwrap()),
        serde_json::to_string(&AmbPrint {
            name: file_name,
            endianness: "little".to_string(),
            objects: vec![],
            version: "PC".to_string(),
        }).unwrap()
    )
}
