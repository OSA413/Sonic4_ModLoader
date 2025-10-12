use crate::binary_reader::Endianness;

pub fn write_u32(target: &mut Vec<u8>, pointer: usize, data: u32, endianness: &Option<Endianness>) {
    // TODO find a solution that doesn't panic
    let bytes = match endianness {
        Some(Endianness::Little) => data.to_le_bytes(),
        Some(Endianness::Big) => data.to_be_bytes(),
        None => data.to_le_bytes(),
    };

    target[pointer..pointer+4].copy_from_slice(&bytes);
}
