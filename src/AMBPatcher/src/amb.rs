use databuffer::DataBuffer;

pub enum Version {
    PC = 0x20,
    Mobile = 0x28,
}

pub struct Amb {
    source: Vec<u8>,
    pub amb_path: String,
    pub same_endianness: Option<bool>,
    pub objects: Vec<BinaryObject>,
    pub has_names: bool,
    pub version: Version,
}

struct AmbPointersPrediction {
    pub list: u32,
    pub data: u32,
    pub name: u32,
}

impl Amb {
    pub fn length(&self) -> u32 {
        self.predict_pointers().name + u32::try_from(self.objects.len()).unwrap() * match self.has_names {
            true => 0x20,
            false => 0
        }
    }

    fn predict_pointers(&self) -> AmbPointersPrediction {
        let data = 0x20 + 0x10 * u32::try_from(self.objects.len()).unwrap();
        let ptr = data + self.objects.iter().map(|object| object.length_nice()).sum::<u32>();
        AmbPointersPrediction {
            list: 0x20, 
            data: data.try_into().unwrap(),
            name: match self.has_names { 
                true => ptr.try_into().unwrap(),
                false => 0
            }
        }
    }

    pub fn is_source_amb(&self, ptr: Option<u32>) -> bool {
        let ptr = ptr.unwrap_or(0);
        u32::try_from(self.source.len()).unwrap() - ptr >= 0x20
            && self.source[(ptr + 0) as usize] == '#' as u8
            && self.source[(ptr + 1) as usize] == 'A' as u8
            && self.source[(ptr + 2) as usize] == 'M' as u8
            && self.source[(ptr + 3) as usize] == 'B' as u8
    }

    pub fn is_little_endian(&self, binary: Option<&Vec<u8>>) -> bool {
        let binary = binary.unwrap_or(&self.source);
        let mut buffer = DataBuffer::from_bytes(&binary);
        buffer.read_u32() > 0xFFFF
    }

    pub fn get_version(&self, binary: Option<&Vec<u8>>, ptr: Option<u32>) -> Version {
        let binary = binary.unwrap_or(&self.source);
        let ptr = ptr.unwrap_or(0);
        let mut buffer = DataBuffer::from_bytes(&binary);
        buffer.read_bytes((ptr + 0x4) as usize);
        match buffer.read_u32() {
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

    fn read_string(source: &Vec<u8>, ptr: u32) -> String {
        let mut buffer = DataBuffer::from_bytes(source);
        buffer.read_bytes(ptr as usize);
        buffer.read_ntstr()
    }

    pub fn new_empty() -> Self {
        Self {
            source: Vec::new(),
            amb_path: String::new(),
            same_endianness: Some(true),
            objects: Vec::new(),
            has_names: true,
            version: Version::PC,
        }
    }

    pub fn from_file_name(file_path: &str) -> Result<Self, std::io::Error> {
        Ok(Self::new_from_src_ptr_name(&std::fs::read(file_path)?, Some(0), Some(file_path.to_string())))
    }

    pub fn new_from_src_ptr_name(
        source: &Vec<u8>,
        ptr: Option<u32>,
        name: Option<String>
    ) -> Self {
        todo!()
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

    pub flag1: u32,
    pub flag2: u32,

    pub source: Vec<u8>,

    // use usize?
    pub pointer: u32,
    length: u32,
}

impl BinaryObject {
    pub fn is_amb(&self) -> bool {
        match self.amb {
            Some(_) => true,
            None => false
        }
    }

    pub fn length(&self) -> u32 {
        match &self.amb {
            Some(amb) => amb.length(),
            None => self.length
        }
    }

    pub fn length_nice(&self) -> u32 {
        self.length() + (16 - self.length() % 16) % 16
    }

    pub fn new_from_src_ptr_len(
        source: Vec<u8>,
        pointer: usize,
        length: usize
    ) -> Self {
        BinaryObject {
            source: source,
            pointer: pointer.try_into().unwrap(),
            length: length.try_into().unwrap(),
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
            length: file_length.try_into().unwrap(),
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
            None => self.source.iter().skip(self.pointer.try_into().unwrap()).take(self.length.try_into().unwrap()).map(|x| x.to_owned()).collect()
        }
    }
}