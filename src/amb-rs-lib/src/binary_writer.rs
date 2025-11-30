use crate::{binary_reader::Endianness, error::{AmbLibRsError, PointerOutOfBoundsDetails}};

pub fn write_u32(target: &mut [u8], pointer: usize, data: u32, endianness: &Option<Endianness>) -> Result<(), AmbLibRsError> {
    let bytes = match endianness {
        Some(Endianness::Little) => data.to_le_bytes(),
        Some(Endianness::Big) => data.to_be_bytes(),
        None => data.to_le_bytes(),
    };

    if target.len() >= pointer + 4 {
        return Err(AmbLibRsError::PointerOutOfBounds(PointerOutOfBoundsDetails { 
            pointer,
            source_len: target.len(),
            when: "Writing an u32".to_string(),
        }))
    }

    target[pointer..pointer+4].copy_from_slice(&bytes);
    Ok(())
}
