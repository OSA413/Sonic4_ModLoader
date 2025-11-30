use amb_rs_lib::amb_management;

mod amb_print;
use crate::amb_print::AmbPrint;

#[test]
fn empty() {
    let file_name = "../amb-rs-tests/amb_files/pc/EMPTY.AMB".to_string();
    assert_eq!(
        amb_management::json::print_from_file(&file_name).unwrap(),
        serde_json::to_string(&AmbPrint {
            name: file_name,
            endianness: "little".to_string(),
            objects: vec![],
            version: "PC".to_string(),
        }).unwrap()
    )
}
