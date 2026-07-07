use std::path::Path;

use common_binary::json_formatter;

pub struct BinaryObject {
    pub name: String,
    pub real_name: String,

    pub flag1: u32,
    pub flag2: u32,

    pub pointer: usize, //This is used just for the json print and debugging
    pub data: Vec<u8>,
}

impl BinaryObject {
    pub fn length(&self) -> usize {
        self.data.len()
    }

    pub fn length_nice(&self) -> usize {
        self.length() + (16 - self.length() % 16) % 16_usize
    }

    pub fn new_from_src_ptr_len(
        source: &[u8],
        pointer: usize,
        length: usize
    ) -> Self {
        BinaryObject {
            data: source.iter().skip(pointer).take(length).map(|x| x.to_owned()).collect(),
            flag1: 0,
            flag2: 0,
            pointer,
            name: String::new(),
            real_name: String::new(),
        }
    }

    pub fn new_from_file_path(
        file_path: &Path
    ) -> Result<Self, std::io::Error> {
        let file_content = std::fs::read(file_path)?;
        Ok(BinaryObject {
            data: file_content,
            flag1: 0,
            flag2: 0,
            pointer: 0,
            name: String::new(),
            real_name: String::new(),
        })
    }
}

impl std::fmt::Display for BinaryObject {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "{{{}}}", [
            json_formatter::add_str("name", &self.name.replace("\\", "\\\\")),
            json_formatter::add_str("real_name", &self.real_name.replace("\\", "\\\\")),
            json_formatter::add_value("flag1", &self.flag1.to_string()),
            json_formatter::add_value("flag2", &self.flag2.to_string()),
            json_formatter::add_value("pointer", &self.pointer.to_string()),
            json_formatter::add_value("length", &self.length().to_string()),
        ].join(","))
    }
}