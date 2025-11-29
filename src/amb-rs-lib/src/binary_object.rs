
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
        source: &Vec<u8>,
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
        file_path: String
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