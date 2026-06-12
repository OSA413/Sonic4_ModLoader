use crate::error::{CommonBinaryError, PointerOutOfBoundsDetails, StringTooLongDetails};
use crate::endianness::Endianness;

pub fn read_string32(source: &[u8], pointer: usize) -> Result<String, CommonBinaryError> {
    let mut result = String::new();
    // Isn't it cool that you can do that in Rust?
    let mut pointer = pointer;

    loop {
        if result.len() > 31 {
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

#[cfg(test)]
mod binary_tests {
    use super::*;

    static HELLO_WORLD: [u8; 12] = [
        0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64, 0x00,
    ];

    static HELLO_WORLD_WO_NULL: [u8; 11] = [
        0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64,
    ];

    static EXACTLY_32_BYTE_LONG_STRING: [u8; 32] = [
        0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f, 0x70,
        0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x41, 0x42, 0x43, 0x44, 0x45, 0x00,
    ];

    static EXACTLY_33_BYTE_LONG_STRING: [u8; 33] = [
        0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f, 0x70,
        0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46,
        0x00,
    ];

    static VERY_LONG_STRING: [u8; 53] = [
        0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f, 0x70,
        0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46,
        0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56,
        0x57, 0x58, 0x59, 0x5A, 0x00,
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

    #[test]
    fn read_exactly_32_string_0() {
        assert_eq!(read_string32(&EXACTLY_32_BYTE_LONG_STRING, 0).unwrap(), "abcdefghijklmnopqrstuvwxyzABCDE".to_string());
    }

    #[test]
    fn read_exactly_32_string_1() {
        assert_eq!(read_string32(&EXACTLY_32_BYTE_LONG_STRING, 16).unwrap(), "qrstuvwxyzABCDE".to_string());
    }

    #[test]
    fn read_exactly_33_string_0() {
        let result = read_string32(&EXACTLY_33_BYTE_LONG_STRING, 0).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "StringTooLong when Reading a string at 32 with value abcdefghijklmnopqrstuvwxyzABCDEF"
        );
    }

    #[test]
    fn read_exactly_33_string_1() {
        assert_eq!(read_string32(&EXACTLY_33_BYTE_LONG_STRING, 1).unwrap(), "bcdefghijklmnopqrstuvwxyzABCDEF".to_string());
    }
    
    #[test]
    fn read_exactly_33_string_2() {
        assert_eq!(read_string32(&EXACTLY_33_BYTE_LONG_STRING, 16).unwrap(), "qrstuvwxyzABCDEF".to_string());
    }

    #[test]
    fn read_very_long_string_0() {
        let result = read_string32(&VERY_LONG_STRING, 0).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "StringTooLong when Reading a string at 32 with value abcdefghijklmnopqrstuvwxyzABCDEF"
        );
    }

    #[test]
    fn read_very_long_string_1() {
        let result = read_string32(&VERY_LONG_STRING, 8).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "StringTooLong when Reading a string at 40 with value ijklmnopqrstuvwxyzABCDEFGHIJKLMN"
        );
    }

    #[test]
    fn read_very_long_string_2() {
        let result = read_string32(&VERY_LONG_STRING, 20).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "StringTooLong when Reading a string at 52 with value uvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        );
    }

    #[test]
    fn read_very_long_string_3() {
        assert_eq!(
            read_string32(&VERY_LONG_STRING, 21).unwrap(),
            "vwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".to_string()
        );
    }

    #[test]
    fn read_very_long_string_4() {
        assert_eq!(
            read_string32(&VERY_LONG_STRING, 22).unwrap(),
            "wxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".to_string()
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

// Maybe add string_tests to check Rust interoperability?

// Maybe add garbage_tests to check that bad characters are not returned when read?

// Looks like I'll need to add a multi-string test to check that it actually takes exaclty one string from that
// I mean, more like the real use-case (like in TXB)
