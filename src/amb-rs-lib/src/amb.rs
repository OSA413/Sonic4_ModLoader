use std::{io::Read, path::Path};
use crate::binary_object::BinaryObject;
use common_binary::{
    binary_reader, binary_writer, endianness::Endianness, error::CommonBinaryError, json_formatter
};

pub enum Version {
    PC = 0x20,
    Mobile = 0x28,
}

pub struct Amb {
    pub amb_path: String,
    pub endianness: Endianness,
    pub flag1: u32,
    pub flag2: u32,
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
        let pointers_predition = self.predict_pointers();
        pointers_predition.name + self.objects.len() * match self.has_names {
            true => 0x20,
            false => 0
        }
    }

    fn predict_pointers(&self) -> AmbPointersPrediction {
        let data = 0x20 + 0x10 * self.objects.len();
        let name = data + self.objects.iter().map(|object| object.length_nice()).sum::<usize>();
        AmbPointersPrediction {
            list: 0x20,
            data,
            name
        }
    }

    pub fn is_source_amb(source: &[u8], ptr: Option<usize>) -> bool {
        let ptr = ptr.unwrap_or(0);
        source.len() - ptr >= 0x20
            && source[ptr] == b'#'
            && source[ptr + 1] == b'A'
            && source[ptr + 2] == b'M'
            && source[ptr + 3] == b'B'
    }

    pub fn get_version(source: &[u8], ptr: Option<usize>) -> (Version, Endianness) {
        let ptr = ptr.unwrap_or(0);
        match binary_reader::u32::read(source, ptr + 0x4, &Endianness::Little).unwrap() {
            0x20 => (Version::PC, Endianness::Little),
            0x28 => (Version::Mobile, Endianness::Little),
            value => {
                match value.swap_bytes() {
                    0x20 => (Version::PC, Endianness::Big),
                    0x28 => (Version::Mobile, Endianness::Big),
                    _ => {
                        eprintln!("Could not detect version of AMB file, assuming it's little endian");
                        (Version::PC, Endianness::Little)
                    },
                }
            }
        }
    }

    pub fn new_empty() -> Self {
        Self {
            amb_path: String::new(),
            endianness: Endianness::Little,
            flag1: 0,
            flag2: 0,
            objects: Vec::new(),
            has_names: true,
            version: Version::PC,
        }
    }

    pub fn new_from_file_name(file_path: &String) -> Result<Self, CommonBinaryError> {
        Self::new_from_src_ptr_name(&std::fs::read(file_path)?, Some(0), file_path)
    }

    pub fn new_from_src_ptr_name(
        source: &[u8],
        ptr: Option<usize>,
        name: &String
    ) -> Result<Self, CommonBinaryError> {
        if !Amb::is_source_amb(source, ptr) {
            return Err(CommonBinaryError::ProvidedSourceIsNotAnAmb(format!("Provided source is not an AMB file, ptr: {ptr:?}, name: {name}")));
        }
        let (version, endianness) = Amb::get_version(source, ptr);
        let shift: usize = match version {
            Version::Mobile => 0x4,
            _ => 0,
        };

        let ptr = ptr.unwrap_or(0);

        let flag1 = binary_reader::u32::read(source, ptr + 0x8, &endianness).expect("Another bad thing happened that you didn't account for #9");
        let flag2 = binary_reader::u32::read(source, ptr + 0xC, &endianness).expect("Another bad thing happened that you didn't account for #9 and a half");
        // Btw this also might be incorrect, this actually shows number of entries in the list, but any entry can be empty resulting in "skips" and a bit smaller number of actual objects
        let object_number = binary_reader::u32::read(source, ptr + 0x10, &endianness).expect("Another bad thing happened that you didn't account for #10");
        let list_pointer = binary_reader::u32::read(source, ptr + 0x14, &endianness).expect("Another bad thing happened that you didn't account for #11") + ptr as u32;
        //var dataPtr = binary_reader::u32::read(source, sourcePtr + 0x18 + shift) + sourcePtr; //this may be not dataPtr for mobile
        let names_pointer = binary_reader::u32::read(source, ptr + 0x1C + shift, &endianness).expect("Another bad thing happened that you didn't account for #12") + ptr as u32;
        let has_names = names_pointer != 0;

        let mut objects = Vec::<BinaryObject>::new();
        let mut i: usize = 0;
        while i < object_number as usize {
            let object_pointer = binary_reader::u32::read(source, list_pointer as usize + (0x10 + shift) * i, &endianness).expect("Who's bad?") + ptr as u32;
            if object_pointer == 0 {
                i += 1;
                continue;
            }

            let object_length = binary_reader::u32::read(source, list_pointer as usize + (0x10 + shift) * i + 4 + shift, &endianness).expect("Who's bad? (2)");
            let mut new_object = BinaryObject::new_from_src_ptr_len(source, object_pointer as usize, object_length as usize);
            new_object.real_name = match has_names {
                true => binary_reader::string32::read(source, names_pointer as usize + 0x20 * i)?.0,
                false =>  i.to_string(),
            };
            new_object.name = Amb::make_name_safe(&new_object.real_name);
            new_object.flag1 = binary_reader::u32::read(source, list_pointer as usize + (0x10 + shift) * i + 8 + shift, &endianness).expect("Who's bad? (3)");
            new_object.flag2 = binary_reader::u32::read(source, list_pointer as usize + (0x10 + shift) * i + 12 + shift, &endianness).expect("Who's bad? (4)");

            objects.push(new_object);

            i += 1;
        }

        Ok(Amb {
            amb_path: name.to_string(),
            endianness,
            flag1,
            flag2,
            objects,
            has_names,
            version,
        })
    }

    pub fn new_from_binary_object(bo: &BinaryObject) -> Result<Self, CommonBinaryError> {
        Amb::new_from_src_ptr_name(&bo.data, None, &bo.name)
    }

    pub fn write(&self) -> Result<Vec<u8>, CommonBinaryError> {
        let amb_length = self.length();
        let mut result = Vec::<u8>::with_capacity(amb_length);
        let mut pointers = self.predict_pointers();

        let mut i = 0;
        while i < amb_length {
            result.push(0);
            i += 1;
        }

        "#AMB".as_bytes().read_exact(&mut result[0x0..0x4]).unwrap();
        binary_writer::u32::write(&mut result, 0x4, match self.version {
            Version::PC => 0x20,
            Version::Mobile => 0x28,
        }, &self.endianness, "version".to_string())?;
        binary_writer::u32::write(&mut result, 0x8, self.flag1, &self.endianness, "flag1".to_string())?;
        binary_writer::u32::write(&mut result, 0xC, self.flag2, &self.endianness, "flag2".to_string())?;
        binary_writer::u32::write(&mut result, 0x10, self.objects.len() as u32, &self.endianness, "object length".to_string())?;
        binary_writer::u32::write(&mut result, 0x14, pointers.list as u32, &self.endianness, "list pointer".to_string())?;
        binary_writer::u32::write(&mut result, 0x18, pointers.data as u32, &self.endianness, "data pointer".to_string())?;
        if self.has_names {
            binary_writer::u32::write(&mut result, 0x1C, pointers.name as u32, &self.endianness, "name pointer".to_string())?;
        }

        for o in self.objects.iter() {
            binary_writer::u32::write(&mut result, pointers.list, pointers.data as u32, &self.endianness, "object data pointer".to_string())?;
            binary_writer::u32::write(&mut result, pointers.list + 4, o.length() as u32, &self.endianness, "object length".to_string())?;
            binary_writer::u32::write(&mut result, pointers.list + 8, o.flag1, &self.endianness, "object flag1".to_string())?;
            binary_writer::u32::write(&mut result, pointers.list + 12, o.flag2, &self.endianness, "object flag2".to_string())?;

            let object_data = &o.data;
            result[pointers.data..pointers.data + o.length()].copy_from_slice(object_data);
            if self.has_names {
                o.real_name.as_bytes().read_exact(&mut result[pointers.name..pointers.name + o.real_name.len()]).unwrap();
                pointers.name += 0x20;
            }

            pointers.list += 0x10;
            pointers.data += o.length_nice();
        }

        Ok(result)
    }

    pub fn get_relative_name(main_file_name: String, object_name: String) -> String {
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
        if internal_name.is_empty() {
            return mod_path_parts.last().expect("That one bad thing happened that you thought would not happen #3").to_string();
        }

        internal_name
    }

    pub fn add_file(&mut self, path: &Path, internal_file_name: Option<String>) -> Result<(), CommonBinaryError> {
        let file_name = path.display().to_string();
        let new_object = BinaryObject::new_from_file_path(path)?;
        let internal_name = internal_file_name.unwrap_or(Amb::get_relative_name(
            self.amb_path.clone(),
            file_name.replace("_extracted", ""),
        )).replace('/', "\\");
        self.add_binary_object(new_object, &internal_name)
    }

    pub fn add_u8_vec(
        &mut self,
        source: &[u8],
        pointer: Option<usize>,
        length: Option<usize>,
        internal_name: &String
    ) -> Result<(), CommonBinaryError> {
        let new_object = BinaryObject::new_from_src_ptr_len(
            source,
            pointer.unwrap_or(0),
            length.unwrap_or(source.len()),
        );
        self.add_binary_object(new_object, internal_name)
    }

    pub fn add_binary_object(&mut self, binary_object: BinaryObject, internal_name: &String) -> Result<(), CommonBinaryError> {
        if let Some(internal_index) = self.objects.iter().position(|x| &x.name == internal_name) {
            let existing_object = &self.objects[internal_index];                
            self.objects[internal_index] = BinaryObject {
                name: existing_object.name.clone(),
                real_name: existing_object.real_name.clone(),
                flag1: existing_object.flag1,
                flag2: existing_object.flag2,
                pointer: existing_object.pointer,
                data: binary_object.data,
            };
            return Ok(());
        }

        match self.objects.iter().position(|x|
            x.name == internal_name
                .split('\\')
                .take(x.name.chars().filter(|&c| c == '\\').count() + 1)
                .collect::<Vec<_>>()
                .join("\\")
        ) {
            Some(parent_index) => {
                let parent_object = &self.objects[parent_index];
                let mut parent_amb = Amb::new_from_binary_object(parent_object)?;
                parent_amb.add_binary_object(
                    binary_object,
                    &internal_name
                        .chars()
                        .skip(parent_object.real_name.chars().count() + 1)
                        .collect::<String>()
                )?;
                let parent_amb_content = parent_amb.write()?;
                self.objects[parent_index] = BinaryObject {
                    name: parent_object.name.clone(),
                    real_name: parent_object.real_name.clone(),
                    flag1: parent_object.flag1,
                    flag2: parent_object.flag2,
                    pointer: 0,
                    data: parent_amb_content,
                };
            }
            None => self.objects.push(BinaryObject {
                name: internal_name.clone(),
                real_name: internal_name.clone(),
                flag1: 0,
                flag2: 0,
                pointer: 0,
                data: binary_object.data,
            }),
        }
        Ok(())
    }

    pub fn make_name_safe(raw_name: &str) -> String {
        //removing ".\" in the names (Windows can't create "." folders)
        //sometimes they can have several ".\" in the names
        //Turns out there's a double dot directory in file names
        //And double backslash in file names
        let mut safe_index = 0;
        let mut chars = raw_name.chars();
        while let Some(ch) = chars.nth(0) {
            if ch == '.' || ch == '\\' || ch == '/' {
                safe_index += 1;
            } else {
                break;
            }
        }

        raw_name.chars().skip(safe_index).collect()
    }

    pub fn swap_endianness(&mut self) {
        self.endianness = match self.endianness {
            Endianness::Little => Endianness::Big,
            Endianness::Big => Endianness::Little,
        };
    }

    // Make recursive as `add`?
    pub fn remove(&mut self, object_name: String) {
        let target = self.objects.iter().position(|x| x.name == object_name);
        if let Some(target) = target {
            self.objects.remove(target);
        }
    }

    // This method is needed to recalculate pointers of newly added objects
    // so that they corespond to the binary form of the AMB (like you just read it and print)
    pub fn prepare_for_print(&mut self) {
        let pointer_predition = self.predict_pointers();
        let mut pointer = pointer_predition.data;
        for object in &mut self.objects {
            object.pointer = pointer;
            object.name = Amb::make_name_safe(&object.name);
            pointer += object.length_nice();
        }
    }
}

impl std::fmt::Display for Amb {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "{{{}}}", [
            json_formatter::add_str("name", &self.amb_path.replace("\\", "\\\\")),
            json_formatter::add_str("version", match &self.version {
                Version::PC => "PC",
                Version::Mobile => "Mobile"
            }),
            json_formatter::add_str("endianness", match self.endianness {
                Endianness::Little => "little",
                Endianness::Big => "big",
            }),
            json_formatter::add_value(
                "objects",
                &format!("[{}]", &self.objects.iter().map(|x| format!("{x}")).collect::<Vec<_>>().join(","))
            ),
        ].join(","))
    }
}