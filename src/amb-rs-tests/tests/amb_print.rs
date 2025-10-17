// TODO: move to tests/test_helpers/json

use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize)]
pub struct BinaryObjectPrint {
    pub name: String,
    pub real_name: String,
    pub flag1: u32,
    pub flag2: u32,
    pub pointer: u32,
    pub length: u32,
}

#[derive(Serialize, Deserialize)]
pub struct AmbPrint {
    pub name: String,
    pub version: String,
    pub endianness: String,
    pub objects: Vec<BinaryObjectPrint>
}