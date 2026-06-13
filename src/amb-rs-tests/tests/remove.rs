use std::fs;

use amb_rs_lib::amb::Amb;

macro_rules! remove_tests {
    ($($name:ident: $value:expr,)*) => {
        $(
            #[test]
            fn $name() {
                let (source_ref, objects_to_remove, expected_ref): (&str, Vec<&str>, &str) = $value;

                let mut amb = Amb::new_from_file_name(
                    &format!("../amb-rs-tests/tests/reference_files/{source_ref}")
                ).unwrap();

                for object_name in objects_to_remove {
                    amb.remove(object_name.to_string());
                }

                let content = amb.write().unwrap();
                let reference = fs::read(
                    format!("../amb-rs-tests/tests/reference_files/{expected_ref}")
                ).unwrap();
                assert_eq!(reference, content);
            }
        )*
    }
}

remove_tests! {
    // Existing
    remove_1_from_1_2_3: ("add_1_2_3.amb", vec!["1"], "add_2_3.amb"),
    remove_2_from_1_2_3: ("add_1_2_3.amb", vec!["2"], "add_1_3.amb"),
    remove_3_from_1_2_3: ("add_1_2_3.amb", vec!["3"], "add_1_2.amb"),
    remove_1_from_1_3_2: ("add_1_3_2.amb", vec!["1"], "add_3_2.amb"),
    remove_2_from_1_3_2: ("add_1_3_2.amb", vec!["2"], "add_1_3.amb"),
    remove_3_from_1_3_2: ("add_1_3_2.amb", vec!["3"], "add_1_2.amb"),
    remove_1_from_2_3_1: ("add_2_3_1.amb", vec!["1"], "add_2_3.amb"),
    remove_2_from_2_3_1: ("add_2_3_1.amb", vec!["2"], "add_3_1.amb"),
    remove_3_from_2_3_1: ("add_2_3_1.amb", vec!["3"], "add_2_1.amb"),
    remove_1_from_2_1_3: ("add_2_1_3.amb", vec!["1"], "add_2_3.amb"),
    remove_2_from_2_1_3: ("add_2_1_3.amb", vec!["2"], "add_1_3.amb"),
    remove_3_from_2_1_3: ("add_2_1_3.amb", vec!["3"], "add_2_1.amb"),
    remove_1_from_3_2_1: ("add_3_2_1.amb", vec!["1"], "add_3_2.amb"),
    remove_2_from_3_2_1: ("add_3_2_1.amb", vec!["2"], "add_3_1.amb"),
    remove_3_from_3_2_1: ("add_3_2_1.amb", vec!["3"], "add_2_1.amb"),
    remove_1_from_3_1_2: ("add_3_1_2.amb", vec!["1"], "add_3_2.amb"),
    remove_2_from_3_1_2: ("add_3_1_2.amb", vec!["2"], "add_3_1.amb"),
    remove_3_from_3_1_2: ("add_3_1_2.amb", vec!["3"], "add_1_2.amb"),

    // Remove all
    remove_1_2_3_from_1_2_3: ("add_1_2_3.amb", vec!["1", "2", "3"], "add_empty.amb"),

    // Non-existing
    remove_4_from_1_2_3: ("add_1_2_3.amb", vec!["4"], "add_1_2_3.amb"),
    remove_4_from_1_3_2: ("add_1_2_3.amb", vec!["4"], "add_1_2_3.amb"),
    remove_4_from_2_1_3: ("add_1_2_3.amb", vec!["4"], "add_1_2_3.amb"),
    remove_4_from_2_3_1: ("add_1_2_3.amb", vec!["4"], "add_1_2_3.amb"),
    remove_4_from_3_1_2: ("add_1_2_3.amb", vec!["4"], "add_1_2_3.amb"),
    remove_4_from_3_2_1: ("add_1_2_3.amb", vec!["4"], "add_1_2_3.amb"),
    remove_3_from_1_2: ("add_1_2.amb", vec!["3"], "add_1_2.amb"),
    remove_3_from_2_1: ("add_2_1.amb", vec!["3"], "add_2_1.amb"),
    remove_2_from_1_3: ("add_1_3.amb", vec!["2"], "add_1_3.amb"),
    remove_2_from_3_1: ("add_3_1.amb", vec!["2"], "add_3_1.amb"),
    remove_1_from_2_3: ("add_2_3.amb", vec!["1"], "add_2_3.amb"),
    remove_1_from_3_2: ("add_3_2.amb", vec!["1"], "add_3_2.amb"),
    remove_1_2_from_3: ("add_3.amb", vec!["1", "2"], "add_3.amb"),
    remove_2_1_from_3: ("add_3.amb", vec!["2", "1"], "add_3.amb"),
    remove_1_3_from_2: ("add_2.amb", vec!["1", "3"], "add_2.amb"),
    remove_3_1_from_2: ("add_2.amb", vec!["3", "1"], "add_2.amb"),
    remove_2_3_from_1: ("add_1.amb", vec!["2", "3"], "add_1.amb"),
    remove_3_2_from_1: ("add_1.amb", vec!["3", "2"], "add_1.amb"),
    remove_1_2_3_from_empty: ("add_empty.amb", vec!["1", "2", "3"], "add_empty.amb"),
    remove_1_3_2_from_empty: ("add_empty.amb", vec!["1", "2", "3"], "add_empty.amb"),
    remove_2_1_3_from_empty: ("add_empty.amb", vec!["1", "2", "3"], "add_empty.amb"),
    remove_2_3_1_from_empty: ("add_empty.amb", vec!["1", "2", "3"], "add_empty.amb"),
    remove_3_1_2_from_empty: ("add_empty.amb", vec!["1", "2", "3"], "add_empty.amb"),
    remove_3_2_1_from_empty: ("add_empty.amb", vec!["1", "2", "3"], "add_empty.amb"),

    // Repeating
    remove_1_twice_from_1_2_3: ("add_1_2_3.amb", vec!["1", "1"], "add_2_3.amb"),
    remove_1_thrice_from_1_2_3: ("add_1_2_3.amb", vec!["1", "1", "1"], "add_2_3.amb"),

    // Nothing, just to check test doesn't delete anything
    remove_nothing_from_empty: ("add_empty.amb", vec![], "add_empty.amb"),
    remove_nothing_from_1: ("add_1.amb", vec![], "add_1.amb"),
    remove_nothing_from_2: ("add_2.amb", vec![], "add_2.amb"),
    remove_nothing_from_3: ("add_3.amb", vec![], "add_3.amb"),
    remove_nothing_from_1_2: ("add_1_2.amb", vec![], "add_1_2.amb"),
    remove_nothing_from_1_3: ("add_1_3.amb", vec![], "add_1_3.amb"),
    remove_nothing_from_2_1: ("add_2_1.amb", vec![], "add_2_1.amb"),
    remove_nothing_from_2_3: ("add_2_3.amb", vec![], "add_2_3.amb"),
    remove_nothing_from_1_2_3: ("add_1_2_3.amb", vec![], "add_1_2_3.amb"),
    remove_nothing_from_1_3_2: ("add_1_3_2.amb", vec![], "add_1_3_2.amb"),
    remove_nothing_from_2_1_3: ("add_2_1_3.amb", vec![], "add_2_1_3.amb"),
    remove_nothing_from_2_3_1: ("add_2_3_1.amb", vec![], "add_2_3_1.amb"),
    remove_nothing_from_3_1_2: ("add_3_1_2.amb", vec![], "add_3_1_2.amb"),
    remove_nothing_from_3_2_1: ("add_3_2_1.amb", vec![], "add_3_2_1.amb"),
}