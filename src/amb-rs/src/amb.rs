use crate::binary_reader::read_u32;

pub enum Version {
    PC = 0x20,
    Mobile = 0x28,
}

pub struct Amb {
    source: Vec<u8>,
    pub amb_path: String,
    pub endianness: bool,
    pub objects: Vec<BinaryObject>,
    pub has_names: bool,
    pub version: Version,
}

struct AmbPointersPrediction {
    pub list: usize,
    pub data: usize,
    pub name: usize,
}

impl Amb {
    pub fn length(&self) -> usize {
        self.predict_pointers().name + self.objects.len() * match self.has_names {
            true => 0x20,
            false => 0
        }
    }

    fn predict_pointers(&self) -> AmbPointersPrediction {
        let data = 0x20 + 0x10 * self.objects.len();
        let ptr = data + self.objects.iter().map(|object| object.length_nice()).sum::<usize>();
        AmbPointersPrediction {
            list: 0x20, 
            data: data,
            name: match self.has_names { 
                true => ptr,
                false => 0
            }
        }
    }

    pub fn is_source_amb(&self, ptr: Option<usize>) -> bool {
        let ptr = ptr.unwrap_or(0);
        self.source.len() - ptr >= 0x20
            && self.source[ptr + 0] == '#' as u8
            && self.source[ptr + 1] == 'A' as u8
            && self.source[ptr + 2] == 'M' as u8
            && self.source[ptr + 3] == 'B' as u8
    }

    pub fn is_little_endian(&self, binary: Option<&Vec<u8>>) -> bool {
        let binary = binary.unwrap_or(&self.source);
        read_u32(binary, 0).unwrap() > 0xFFFF
    }

    pub fn get_version(&self, binary: Option<&Vec<u8>>, ptr: Option<usize>) -> Version {
        let binary = binary.unwrap_or(&self.source);
        let ptr = ptr.unwrap_or(0);
        match read_u32(binary, ptr + 0x4).unwrap() {
            0x20 => Version::PC,
            0x28 => Version::Mobile,
            value => {
                match value.swap_bytes() {
                    0x20 => Version::PC,
                    0x28 => Version::Mobile,
                    _ => {
                        println!("Could not detect version of AMB file");
                        Version::PC
                    },
                }
            }
        }
    }

    pub fn swap_endianness(&self, binary: Option<&Vec<u8>>) -> Vec<u8> {
        todo!()
    }

    pub fn new_empty() -> Self {
        Self {
            source: Vec::new(),
            amb_path: String::new(),
            endianness: true,
            objects: Vec::new(),
            has_names: true,
            version: Version::PC,
        }
    }

    pub fn from_file_name(file_path: &String) -> Result<Self, std::io::Error> {
        Ok(Self::new_from_src_ptr_name(&std::fs::read(&file_path)?, Some(0), Some(file_path.to_string())))
    }

    pub fn new_from_src_ptr_name(
        source: &Vec<u8>,
        ptr: Option<usize>,
        name: Option<String>
    ) -> Self {
        Amb {
            source: source.to_vec(),
            amb_path: name.unwrap_or(String::new()),
            endianness: true,
            objects: Vec::new(),
            has_names: true,
            version: Version::PC,
        }
    }

    pub fn save(&self) {todo!()}
    pub fn write(&self) -> Vec<u8> {todo!()}
    pub fn get_relative_name(&self) {todo!()}
    pub fn add(&self) {todo!()}
    pub fn find_object(&self) {todo!()}
    pub fn replace(&self) {todo!()}
    pub fn extract_all(&self) {todo!()}
    pub fn extract(&self) {todo!()}
    pub fn make_name_safe(&self) {todo!()}
    pub fn remove(&self) {todo!()}
}

pub struct BinaryObject {
    pub name: String,
    pub real_name: String,
    pub amb: Option<Amb>,
    pub parent_amb: Option<Amb>,

    pub flag1: usize,
    pub flag2: usize,

    pub source: Vec<u8>,

    // use usize?
    pub pointer: usize,
    length: usize,
}

impl BinaryObject {
    pub fn is_amb(&self) -> bool {
        match self.amb {
            Some(_) => true,
            None => false
        }
    }

    pub fn length(&self) -> usize {
        match &self.amb {
            Some(amb) => amb.length(),
            None => self.length
        }
    }

    pub fn length_nice(&self) -> usize {
        self.length() + (16 - self.length() % 16) % 16
    }

    pub fn new_from_src_ptr_len(
        source: Vec<u8>,
        pointer: usize,
        length: usize
    ) -> Self {
        BinaryObject {
            source: source,
            pointer: pointer,
            length: length,
            amb: None,
            flag1: 0,
            flag2: 0,
            name: String::new(),
            real_name: String::new(),
            parent_amb: None,
        }
    }

    pub fn new_from_file_path(
        file_path: String
    ) -> Result<Self, std::io::Error> {
        let file_content = std::fs::read(file_path)?;
        let file_length = file_content.len();
        Ok(BinaryObject {
            source: file_content,
            pointer: 0,
            length: file_length,
            amb: None,
            flag1: 0,
            flag2: 0,
            name: String::new(),
            real_name: String::new(),
            parent_amb: None,
        })
    }

    pub fn write(&self) -> Vec<u8> {
        match &self.amb {
            Some(amb) => amb.write(),
            None => self.source.iter().skip(self.pointer).take(self.length).map(|x| x.to_owned()).collect()
        }
    }
}