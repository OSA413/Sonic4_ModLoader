use crate::error::{CommonBinaryError, PointerOutOfBoundsDetails, StringTooLongDetails};
use crate::endianness::Endianness;

pub fn read_string32(source: &[u8], pointer: usize) -> Result<String, CommonBinaryError> {
    let mut result = String::new();
    // Isn't it cool that you can do that in Rust?
    let mut pointer = pointer;

    loop {
        if result.len() >= 31 {
            return Err(CommonBinaryError::StringTooLong(StringTooLongDetails {
                pointer,
                target_string: result,
                when: "Reading a string".to_string(),
            }))
        }

        if pointer >= source.len() {
            return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails { 
                pointer,
                source_len: source.len(),
                when: "Reading a string".to_string(),
            }));
        }

        if source[pointer] == 0x00 {
            break;
        }

        result.push(source[pointer] as char);
        pointer += 1;
    }

    Ok(result)
}

pub fn read_u32(source: &[u8], pointer: usize, endianness: &Option<Endianness>) -> Result<u32, CommonBinaryError> {
    // This approach won't eat up the RAM and should be safe and fast
    // And is using Rust's built in conversion to type from binary
    if source.len() < pointer + size_of::<u32>() {
        return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails {
            when: "Reading an u32".to_string(),
            pointer,
            source_len: source.len(),
        }));
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

#[cfg(test)]
mod tests {
    use super::*;

    static HELLO_WORLD: [u8; 12] = [
        0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64, 0x00,
    ];

    static HELLO_WORLD_WO_NULL: [u8; 11] = [
        0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64,
    ];

    #[test]
    fn read_string32_0() {
        assert_eq!(read_string32(&HELLO_WORLD, 0).unwrap(), "Hello World".to_string());
    }

    #[test]
    fn read_string32_1() {
        assert_eq!(read_string32(&HELLO_WORLD, 1).unwrap(), "ello World".to_string());
    }
    
    #[test]
    fn read_string32_2() {
        assert_eq!(read_string32(&HELLO_WORLD, 2).unwrap(), "llo World".to_string());
    }

    #[test]
    fn read_string32_3() {
        assert_eq!(read_string32(&HELLO_WORLD, 3).unwrap(), "lo World".to_string());
    }

    #[test]
    fn read_string32_4() {
        assert_eq!(read_string32(&HELLO_WORLD, 4).unwrap(), "o World".to_string());
    }

    #[test]
    fn read_string32_5() {
        assert_eq!(read_string32(&HELLO_WORLD, 5).unwrap(), " World".to_string());
    }

    #[test]
    fn read_string32_6() {
        assert_eq!(read_string32(&HELLO_WORLD, 6).unwrap(), "World".to_string());
    }

    #[test]
    fn read_string32_7() {
        assert_eq!(read_string32(&HELLO_WORLD, 7).unwrap(), "orld".to_string());
    }

    #[test]
    fn read_string32_8() {
        assert_eq!(read_string32(&HELLO_WORLD, 8).unwrap(), "rld".to_string());
    }

    #[test]
    fn read_string32_9() {
        assert_eq!(read_string32(&HELLO_WORLD, 9).unwrap(), "ld".to_string());
    }

    #[test]
    fn read_string32_10() {
        assert_eq!(read_string32(&HELLO_WORLD, 10).unwrap(), "d".to_string());
    }

    #[test]
    fn read_string32_11() {
        assert_eq!(read_string32(&HELLO_WORLD, 11).unwrap(), "".to_string());
    }

    #[test]
    fn read_string32_out_of_bounds_0() {
        let result = read_string32(&HELLO_WORLD, 12).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 12 at 12"
        );
    }

    #[test]
    fn read_string32_out_of_bounds_1() {
        let result = read_string32(&HELLO_WORLD, 13).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 12 at 13"
        );
    }

    #[test]
    fn read_string32_out_of_bounds_2() {
        let result = read_string32(&HELLO_WORLD, 99).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 12 at 99"
        );
    }

    // We only care about null terminated strings
    #[test]
    fn read_string32_wo_null_0() {
        let result = read_string32(&HELLO_WORLD_WO_NULL, 0).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 11 at 11"
        );
    }

    #[test]
    fn read_string32_wo_null_1() {
        let result = read_string32(&HELLO_WORLD_WO_NULL, 6).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 11 at 11"
        );
    }
    
    #[test]
    fn read_string32_wo_null_2() {
        let result = read_string32(&HELLO_WORLD_WO_NULL, 11).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 11 at 11"
        );
    }

    
    #[test]
    fn read_string32_wo_null_out_of_bounds() {
        let result = read_string32(&HELLO_WORLD_WO_NULL, 12).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 11 at 12"
        );
    }
}