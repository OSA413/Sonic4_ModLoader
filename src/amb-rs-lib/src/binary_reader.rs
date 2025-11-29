pub enum Endianness {
    Little,
    Big,
}

pub fn read_string32(source: &[u8], pointer: usize) -> String {
    let mut result = String::new();
    // Isn't it cool that you can do that in Rust?
    let mut pointer = pointer;

    while source[pointer] != 0x00 || result.len() >= 31 {
        result.push(source[pointer] as char);
        pointer += 1;
    }

    result
}

pub fn read_u32(source: &[u8], pointer: usize, endianness: &Option<Endianness>) -> Result<u32, &'static str> {
    // This approach won't eat up the RAM and should be safe and fast
    // And is using Rust's built in conversion to type from binary
    if source.len() < pointer + size_of::<u32>() {
        return Err("Test");
    }

    let slice = [
        source[pointer],
        source[pointer + 1],
        source[pointer + 2],
        source[pointer + 3]
    ];

    match endianness {
        Some(Endianness::Little) => Ok(u32::from_le_bytes(slice)),
        Some(Endianness::Big) => Ok(u32::from_be_bytes(slice)),
        None => Ok(u32::from_le_bytes(slice))
    }
}