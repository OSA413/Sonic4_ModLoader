use std::{io::Read, path::Path};
use crate::{binary_reader::{self, Endianness}, binary_writer};

pub enum Version {
    PC = 0x20,
    Mobile = 0x28,
}

pub struct Amb {
    pub amb_path: String,
    pub endianness: Option<Endianness>,
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

    pub fn is_source_amb(source: &Vec<u8>, ptr: Option<usize>) -> bool {
        let ptr = ptr.unwrap_or(0);
        source.len() - ptr >= 0x20
            && source[ptr + 0] == '#' as u8
            && source[ptr + 1] == 'A' as u8
            && source[ptr + 2] == 'M' as u8
            && source[ptr + 3] == 'B' as u8
    }

    pub fn get_version(source: &Vec<u8>, ptr: Option<usize>) -> (Version, Option<Endianness>) {
        let ptr = ptr.unwrap_or(0);
        match binary_reader::read_u32(source, ptr + 0x4, &None).unwrap() {
            0x20 => (Version::PC, Some(Endianness::Little)),
            0x28 => (Version::Mobile, Some(Endianness::Little)),
            value => {
                match value.swap_bytes() {
                    0x20 => (Version::PC, Some(Endianness::Big)),
                    0x28 => (Version::Mobile, Some(Endianness::Big)),
                    _ => {
                        println!("Could not detect version of AMB file");
                        (Version::PC, None)
                    },
                }
            }
        }
    }

    pub fn new_empty() -> Self {
        Self {
            amb_path: String::new(),
            endianness: Some(Endianness::Little),
            objects: Vec::new(),
            has_names: true,
            version: Version::PC,
        }
    }

    pub fn new_from_file_name(file_path: &String) -> Result<Self, std::io::Error> {
        Ok(Self::new_from_src_ptr_name(&std::fs::read(&file_path)?, Some(0), file_path.to_string()))
    }

    pub fn new_from_src_ptr_name(
        source: &Vec<u8>,
        ptr: Option<usize>,
        name: String
    ) -> Self {
        if !Amb::is_source_amb(source, ptr) {
            panic!("Provided source is not an AMB file");
        }
        let ptr = ptr.unwrap_or(0);
        let (version, endianness) = Amb::get_version(source, Some(ptr));
        let shift: usize = match version {
            Version::Mobile => 0x4,
            _ => 0,
        };

        let object_number = binary_reader::read_u32(source, ptr + 0x10, &endianness).expect("Another bad thing happened that you didn't account for #10");
        let list_pointer = binary_reader::read_u32(source, ptr + 0x14, &endianness).expect("Another bad thing happened that you didn't account for #11") + ptr as u32;
        //var dataPtr = binary_reader::read_u32(source, sourcePtr + 0x18 + shift) + sourcePtr; //this may be not dataPtr for mobile
        let names_pointer = binary_reader::read_u32(source, ptr + 0x1C + shift, &endianness).expect("Another bad thing happened that you didn't account for #12") + ptr as u32;
        let has_names = names_pointer != 0;

        let mut objects = Vec::<BinaryObject>::new();
        let mut i: usize = 0;
        while i < object_number as usize {
            let object_pointer = binary_reader::read_u32(source, list_pointer as usize + (0x10 + shift) * i, &endianness).expect("Who's bad?") + ptr as u32;
            if object_pointer == 0 {
                i += 1;
                continue;
            }
            let object_length = binary_reader::read_u32(source, list_pointer as usize + (0x10 + shift) * i + 4 + shift, &endianness).expect("Who's bad? (2)");
            let mut new_object = BinaryObject::new_from_src_ptr_len(source, object_pointer as usize, object_length as usize);
            new_object.real_name = match has_names {
                true => binary_reader::read_string32(source, names_pointer as usize + 0x20 * i),
                false =>  i.to_string(),
            };
            new_object.name = Amb::make_name_safe(&new_object.real_name);
            new_object.flag1 = binary_reader::read_u32(source, list_pointer as usize + (0x10 + shift) * i + 8 + shift, &endianness).expect("Who's bad? (3)");
            new_object.flag2 = binary_reader::read_u32(source, list_pointer as usize + (0x10 + shift) * i + 12 + shift, &endianness).expect("Who's bad? (4)");
            if Amb::is_source_amb(source, Some(object_pointer as usize)) {
                new_object.amb = Some(Amb::new_from_src_ptr_name(source, Some(object_pointer as usize), name.clone() + "\\" + &new_object.name));
            }
            objects.push(new_object);
            i += 1;
        }

        Amb {
            amb_path: name,
            endianness,
            objects,
            has_names,
            version,
        }
    }

    pub fn new_from_binary_object(bo: &BinaryObject) -> Self {
        Amb::new_from_src_ptr_name(&bo.data, Some(bo.pointer), bo.name.clone())
    }

    pub fn write(&self) -> Vec<u8> {
        let mut result = Vec::<u8>::with_capacity(self.length());
        let mut pointers = self.predict_pointers();

        "#AMB".as_bytes().read_exact(&mut result[0x0..0x4]);
        binary_writer::write_u32(&mut result, 0x4, match self.version {
            Version::PC => 0x20,
            Version::Mobile => 0x28,
        }, &self.endianness);
        binary_writer::write_u32(&mut result, 0x10, self.objects.iter().count() as u32, &self.endianness);
        binary_writer::write_u32(&mut result, 0x14, pointers.list as u32, &self.endianness);
        binary_writer::write_u32(&mut result, 0x18, pointers.data as u32, &self.endianness);
        binary_writer::write_u32(&mut result, 0x1C, pointers.name as u32, &self.endianness);

        for o in self.objects.iter() {
            binary_writer::write_u32(&mut result, pointers.list, pointers.data as u32, &self.endianness);
            binary_writer::write_u32(&mut result, pointers.list + 4, o.length_nice() as u32, &self.endianness);
            binary_writer::write_u32(&mut result, pointers.list + 8, o.flag1 as u32, &self.endianness);
            binary_writer::write_u32(&mut result, pointers.list + 12, o.flag2 as u32, &self.endianness);

            let object_data = &o.data;
            result[pointers.data..pointers.data + o.length()].copy_from_slice(&object_data);
            if self.has_names {
                o.real_name.as_bytes().read_exact(&mut result[pointers.name..pointers.name + o.real_name.len()]);
                pointers.name += 0x20;
            }

            pointers.list += 0x10;
            pointers.data += o.length_nice();
        }

        result
    }

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

    pub fn make_name_safe(raw_name: &String) -> String {
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

    // Make recursive as `add`?
    pub fn remove(&mut self, object_name: String) {
        let target = self.objects.iter().position(|x| x.name == object_name);
        if let Some(target) = target {
            self.objects.remove(target);
        }
    }
}

pub struct BinaryObject {
    pub name: String,
    pub real_name: String,
    pub amb: Option<Amb>,

    pub flag1: u32,
    pub flag2: u32,

    pub pointer: usize, //This is used just for the output and debugging
    pub data: Vec<u8>,
}

impl BinaryObject {
    pub fn length(&self) -> usize {
        return self.data.len();
    }

    pub fn length_nice(&self) -> usize {
        self.length() + (16 - self.length() % 16) % 16
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
            amb: None,
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
            amb: None,
            name: String::new(),
            real_name: String::new(),
        })
    }
}