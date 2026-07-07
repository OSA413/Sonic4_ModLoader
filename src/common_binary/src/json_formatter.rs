pub fn add_str(field: &'static str, value: &str) -> String {
    add_value(field, &format!("\"{value}\""))
}

pub fn add_value(field: &'static str, value: &str) -> String {
    format!("\"{field}\":{value}")
}