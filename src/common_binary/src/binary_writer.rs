use crate::{endianness::Endianness, error::{CommonBinaryError, PointerOutOfBoundsDetails}};

pub fn write_u32(target: &mut [u8], pointer: usize, data: u32, endianness: &Option<Endianness>) -> Result<(), CommonBinaryError> {
    let bytes = match endianness {
        Some(Endianness::Little) => data.to_le_bytes(),
        Some(Endianness::Big) => data.to_be_bytes(),
        None => data.to_le_bytes(),
    };

    if target.len() >= pointer + 4 {
        return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails { 
            pointer,
            source_len: target.len(),
            when: "Writing an u32".to_string(),
        }))
    }

    target[pointer..pointer+4].copy_from_slice(&bytes);
    Ok(())
}
