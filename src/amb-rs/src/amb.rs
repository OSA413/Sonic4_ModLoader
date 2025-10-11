use std::path::Path;
use crate::binary_reader::read_u32;

pub enum Version {
    PC = 0x20,
    Mobile = 0x28,
}

pub enum Endianness {
    Little,
    Big,
}

pub struct Amb {
    pub amb_path: String,
    pub endianness: Endianness,
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

    // pub fn is_source_amb(&self, ptr: Option<usize>) -> bool {
    //     let ptr = ptr.unwrap_or(0);
    //     self.source.len() - ptr >= 0x20
    //         && self.source[ptr + 0] == '#' as u8
    //         && self.source[ptr + 1] == 'A' as u8
    //         && self.source[ptr + 2] == 'M' as u8
    //         && self.source[ptr + 3] == 'B' as u8
    // }

    // pub fn is_little_endian(&self, binary: Option<&Vec<u8>>) -> bool {
    //     let binary = binary.unwrap_or(&self.source);
    //     read_u32(binary, 0).unwrap() > 0xFFFF
    // }

    // pub fn get_version(&self, binary: Option<&Vec<u8>>, ptr: Option<usize>) -> Version {
    //     let binary = binary.unwrap_or(&self.source);
    //     let ptr = ptr.unwrap_or(0);
    //     match read_u32(binary, ptr + 0x4).unwrap() {
    //         0x20 => Version::PC,
    //         0x28 => Version::Mobile,
    //         value => {
    //             match value.swap_bytes() {
    //                 0x20 => Version::PC,
    //                 0x28 => Version::Mobile,
    //                 _ => {
    //                     println!("Could not detect version of AMB file");
    //                     Version::PC
    //                 },
    //             }
    //         }
    //     }
    // }

    pub fn swap_endianness(&self, binary: Option<&Vec<u8>>) -> Vec<u8> {
        todo!()
    }

    pub fn new_empty() -> Self {
        Self {
            amb_path: String::new(),
            endianness: Endianness::Little,
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
            amb_path: name.unwrap_or(String::new()),
            endianness: Endianness::Little,
            objects: Vec::new(),
            has_names: true,
            version: Version::PC,
        }
    }

    pub fn new_from_binary_object(bo: &BinaryObject) -> Self {
        todo!();
    }

    pub fn save(&self) {todo!()}
    pub fn write(&self) -> Vec<u8> {todo!()}

    pub fn get_relative_name(&self, main_file_name: String, object_name: String) -> String {
        //Turning "C:\1\2\3" into {"C:","1","2","3"}
        let mod_path_parts = object_name.replace('/', "\\");
        let mod_path_parts = mod_path_parts.split('\\').collect::<Vec<_>>();
        let orig_file_name = Path::new(&main_file_name).file_name().expect("That one bad thing happened that you thought would not happen #1");

        //Trying to find where the original file name starts in the mod file name.
        let index = mod_path_parts.iter().position(|&r| r == orig_file_name);

        let internal_name = match index {
            //If it's inside, return the part after original file ends
            Some(index) => mod_path_parts.iter().skip(index + 1).map(|x| x.to_owned()).collect::<Vec<_>>().join("\\"),
            //Else use file name
            None => mod_path_parts.last().expect("That one bad thing happened that you thought would not happen #2").to_string(),
        };

        //This may occur when main file and added file have the same name
        if internal_name == "" {
            return mod_path_parts.last().expect("That one bad thing happened that you thought would not happen #3").to_string();
        }

        return internal_name;
    }

    pub fn add_binary_object(&mut self, mut new_obj: BinaryObject, internal_name: String) {
        match self.objects.iter().position(|x| x.name == internal_name) {
            Some(internal_index) => {
                let existing_object = &self.objects[internal_index];
                new_obj.name = existing_object.name.clone();
                new_obj.real_name = existing_object.real_name.clone();
                
                self.replace(new_obj, internal_index);
                return;
            },
            None => (),
        }

        match self.objects.iter().position(|x| x.name == internal_name.split('\\').take(x.name.chars().filter(|&c| c == '\\').count() + 1).collect::<Vec<_>>().join("\\")) {
            Some(parent_index) => {
                let parent_object = &self.objects[parent_index];
                // Why do we consider that this object is an AMB?
                let mut parent_amb = Amb::new_from_binary_object(parent_object);
                // ????????
                return parent_amb.add_binary_object(new_obj, internal_name);
            }
            None => todo!(),
        }
    }

    pub fn add(&mut self, file_path: String, new_name: Option<String>) {
        let mut new_obj = BinaryObject::new_from_file_path(file_path.clone()).expect("This happend #6");

        let internal_name = new_name.unwrap_or(self.get_relative_name(self.amb_path.clone(), file_path.replace("_extracted", ""))).replace('/', "\\");

        match self.objects.iter().position(|x| x.name == internal_name) {
            Some(internal_index) => {
                let existing_object = &self.objects[internal_index];
                new_obj.name = existing_object.name.clone();
                new_obj.real_name = existing_object.real_name.clone();
                
                self.objects[internal_index] = new_obj;
                return;
            },
            None => (),
        }

        match self.objects.iter().position(|x| x.name == internal_name.split('\\').take(x.name.chars().filter(|&c| c == '\\').count() + 1).collect::<Vec<_>>().join("\\")) {
            Some(parent_index) => {
                let parent_object = &self.objects[parent_index];
                // Why do we consider that this object is an AMB?
                let mut parent_amb = Amb::new_from_binary_object(parent_object);
                return parent_amb.add_binary_object(new_obj, internal_name);
            }
            None => todo!(),
        }
    }

    pub fn replace(&mut self, mut bo: BinaryObject, target_index: usize) {
        let prev = &self.objects[target_index];
        bo.real_name = prev.real_name.clone();
        bo.flag1 = prev.flag1;
        bo.flag2 = prev.flag2;
        self.objects.remove(target_index);
        self.objects.insert(target_index, bo.try_into().expect("That one bad thing happened that you thought would not happen #5"));
    }

    pub fn extract_all(&self) {todo!()}

    pub fn extract(&self) {todo!()}

    pub fn make_name_safe(&self, raw_name: String) -> String {
        //removing ".\" in the names (Windows can't create "." folders)
        //sometimes they can have several ".\" in the names
        //Turns out there's a double dot directory in file names
        //And double backslash in file names
        let mut safe_index = 0;
        // Why mut?
        let mut chars = raw_name.chars();
        while let Some(ch) = chars.nth(safe_index) {
            if ch == '.' || ch == '\\' || ch == '/' {
                safe_index += 1;
            } else {
                break;
            }
        }

        return raw_name.chars().skip(safe_index).collect();
    }

    pub fn remove_at(&mut self, index: usize) {
        self.objects.remove(index);
    }

    // pub fn remove(&mut self, object_name: String) {
    //     let target = self.find_object(object_name);
    //     target.0.remove_at(target.1.expect("That one bad thing happened that you thought would not happen #5"));
    // }
}

pub struct BinaryObject {
    pub name: String,
    pub real_name: String,

    pub flag1: usize,
    pub flag2: usize,

    pub source: Vec<u8>,

    // use usize?
    pub pointer: usize,
    length: usize,
}

impl BinaryObject {
    pub fn length(&self) -> usize {
        return self.length;
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
            flag1: 0,
            flag2: 0,
            name: String::new(),
            real_name: String::new(),
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
            flag1: 0,
            flag2: 0,
            name: String::new(),
            real_name: String::new(),
        })
    }

    pub fn write(&self) -> Vec<u8> {
        return self.source.iter().skip(self.pointer).take(self.length).map(|x| x.to_owned()).collect();
    }
}