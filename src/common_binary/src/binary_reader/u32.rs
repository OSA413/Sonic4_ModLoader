use crate::error::{CommonBinaryError, PointerOutOfBoundsDetails};
use crate::endianness::Endianness;

pub fn read(source: &[u8], pointer: usize, endianness: &Option<Endianness>) -> Result<u32, CommonBinaryError> {
    // This approach won't eat up the RAM and should be safe and fast
    // And is using Rust's built in conversion to type from binary
    if source.len() < pointer + size_of::<u32>() {
        return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails {
            when: "Reading an u32".to_string(),
            pointer,
            source_len: source.len(),
        }));
    }

    let bytes = [
        source[pointer],
        source[pointer + 1],
        source[pointer + 2],
        source[pointer + 3]
    ];

    match endianness {
        Some(Endianness::Little) => Ok(u32::from_le_bytes(bytes)),
        Some(Endianness::Big) => Ok(u32::from_be_bytes(bytes)),
        None => Ok(u32::from_le_bytes(bytes))
    }
}
