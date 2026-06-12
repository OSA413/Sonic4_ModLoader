use std::ops::RangeInclusive;

use crate::error::{CommonBinaryError, PointerOutOfBoundsDetails, StringBadCharacterDetails, StringTooLongDetails};

static ALLOWED_CHARACTER_RANGES: [RangeInclusive<u8>; 7] = [
    0x20..=0x20, // Space
    0x28..=0x29, // ()
    0x2C..=0x39, // ,-./0123456789
    0x41..=0x5A, // A-Z
    0x5C..=0x5C, // \
    0x5F..=0x5F, // _
    0x61..=0x7A, // a-z
];

pub fn read(source: &[u8], pointer: usize) -> Result<String, CommonBinaryError> {
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

        let character = source[pointer];

        if character == 0x00 {
            break;
        }

        if !(ALLOWED_CHARACTER_RANGES).iter().any(|range| range.contains(&character)) {
            return Err(CommonBinaryError::StringBadCharacter(StringBadCharacterDetails {
                pointer,
                target_string: result,
                bad_character: character,
                when: "Reading a string".to_string(),
            }));
        }

        result.push(character as char);
        pointer += 1;
    }

    Ok(result)
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
        assert_eq!(read(&HELLO_WORLD, 0).unwrap(), "Hello World".to_string());
    }

    #[test]
    fn read_string32_1() {
        assert_eq!(read(&HELLO_WORLD, 1).unwrap(), "ello World".to_string());
    }
    
    #[test]
    fn read_string32_2() {
        assert_eq!(read(&HELLO_WORLD, 2).unwrap(), "llo World".to_string());
    }

    #[test]
    fn read_string32_3() {
        assert_eq!(read(&HELLO_WORLD, 3).unwrap(), "lo World".to_string());
    }

    #[test]
    fn read_string32_4() {
        assert_eq!(read(&HELLO_WORLD, 4).unwrap(), "o World".to_string());
    }

    #[test]
    fn read_string32_5() {
        assert_eq!(read(&HELLO_WORLD, 5).unwrap(), " World".to_string());
    }

    #[test]
    fn read_string32_6() {
        assert_eq!(read(&HELLO_WORLD, 6).unwrap(), "World".to_string());
    }

    #[test]
    fn read_string32_7() {
        assert_eq!(read(&HELLO_WORLD, 7).unwrap(), "orld".to_string());
    }

    #[test]
    fn read_string32_8() {
        assert_eq!(read(&HELLO_WORLD, 8).unwrap(), "rld".to_string());
    }

    #[test]
    fn read_string32_9() {
        assert_eq!(read(&HELLO_WORLD, 9).unwrap(), "ld".to_string());
    }

    #[test]
    fn read_string32_10() {
        assert_eq!(read(&HELLO_WORLD, 10).unwrap(), "d".to_string());
    }

    #[test]
    fn read_string32_11() {
        assert_eq!(read(&HELLO_WORLD, 11).unwrap(), "".to_string());
    }

    #[test]
    fn read_string32_out_of_bounds_0() {
        let result = read(&HELLO_WORLD, 12).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 12 at 12"
        );
    }

    #[test]
    fn read_string32_out_of_bounds_1() {
        let result = read(&HELLO_WORLD, 13).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 12 at 13"
        );
    }

    #[test]
    fn read_string32_out_of_bounds_2() {
        let result = read(&HELLO_WORLD, 99).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 12 at 99"
        );
    }

    #[test]
    fn read_exactly_32_string_0() {
        assert_eq!(read(&EXACTLY_32_BYTE_LONG_STRING, 0).unwrap(), "abcdefghijklmnopqrstuvwxyzABCDE".to_string());
    }

    #[test]
    fn read_exactly_32_string_1() {
        assert_eq!(read(&EXACTLY_32_BYTE_LONG_STRING, 16).unwrap(), "qrstuvwxyzABCDE".to_string());
    }

    #[test]
    fn read_exactly_33_string_0() {
        let result = read(&EXACTLY_33_BYTE_LONG_STRING, 0).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "StringTooLong when Reading a string at 32 with value abcdefghijklmnopqrstuvwxyzABCDEF"
        );
    }

    #[test]
    fn read_exactly_33_string_1() {
        assert_eq!(read(&EXACTLY_33_BYTE_LONG_STRING, 1).unwrap(), "bcdefghijklmnopqrstuvwxyzABCDEF".to_string());
    }
    
    #[test]
    fn read_exactly_33_string_2() {
        assert_eq!(read(&EXACTLY_33_BYTE_LONG_STRING, 16).unwrap(), "qrstuvwxyzABCDEF".to_string());
    }

    #[test]
    fn read_very_long_string_0() {
        let result = read(&VERY_LONG_STRING, 0).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "StringTooLong when Reading a string at 32 with value abcdefghijklmnopqrstuvwxyzABCDEF"
        );
    }

    #[test]
    fn read_very_long_string_1() {
        let result = read(&VERY_LONG_STRING, 8).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "StringTooLong when Reading a string at 40 with value ijklmnopqrstuvwxyzABCDEFGHIJKLMN"
        );
    }

    #[test]
    fn read_very_long_string_2() {
        let result = read(&VERY_LONG_STRING, 20).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "StringTooLong when Reading a string at 52 with value uvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        );
    }

    #[test]
    fn read_very_long_string_3() {
        assert_eq!(
            read(&VERY_LONG_STRING, 21).unwrap(),
            "vwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".to_string()
        );
    }

    #[test]
    fn read_very_long_string_4() {
        assert_eq!(
            read(&VERY_LONG_STRING, 22).unwrap(),
            "wxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".to_string()
        );
    }

    // We only care about null terminated strings
    #[test]
    fn read_string32_wo_null_0() {
        let result = read(&HELLO_WORLD_WO_NULL, 0).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 11 at 11"
        );
    }

    #[test]
    fn read_string32_wo_null_1() {
        let result = read(&HELLO_WORLD_WO_NULL, 6).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 11 at 11"
        );
    }
    
    #[test]
    fn read_string32_wo_null_2() {
        let result = read(&HELLO_WORLD_WO_NULL, 11).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 11 at 11"
        );
    }

    #[test]
    fn read_string32_wo_null_out_of_bounds() {
        let result = read(&HELLO_WORLD_WO_NULL, 12).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 11 at 12"
        );
    }
}

#[cfg(test)]
mod string_tests {
    use super::*;

    static HELLO_WORLD: [u8; 12] = *b"Hello World\0";

    static HELLO_WORLD_WO_NULL: [u8; 11] = *b"Hello World";

    static EXACTLY_32_BYTE_LONG_STRING: [u8; 32] = *b"abcdefghijklmnopqrstuvwxyzABCDE\0";

    static EXACTLY_33_BYTE_LONG_STRING: [u8; 33] = *b"abcdefghijklmnopqrstuvwxyzABCDEF\0";

    static VERY_LONG_STRING: [u8; 53] = *b"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ\0";

    #[test]
    fn read_string32_0() {
        assert_eq!(read(&HELLO_WORLD, 0).unwrap(), "Hello World".to_string());
    }

    #[test]
    fn read_string32_1() {
        assert_eq!(read(&HELLO_WORLD, 1).unwrap(), "ello World".to_string());
    }
    
    #[test]
    fn read_string32_2() {
        assert_eq!(read(&HELLO_WORLD, 2).unwrap(), "llo World".to_string());
    }

    #[test]
    fn read_string32_3() {
        assert_eq!(read(&HELLO_WORLD, 3).unwrap(), "lo World".to_string());
    }

    #[test]
    fn read_string32_4() {
        assert_eq!(read(&HELLO_WORLD, 4).unwrap(), "o World".to_string());
    }

    #[test]
    fn read_string32_5() {
        assert_eq!(read(&HELLO_WORLD, 5).unwrap(), " World".to_string());
    }

    #[test]
    fn read_string32_6() {
        assert_eq!(read(&HELLO_WORLD, 6).unwrap(), "World".to_string());
    }

    #[test]
    fn read_string32_7() {
        assert_eq!(read(&HELLO_WORLD, 7).unwrap(), "orld".to_string());
    }

    #[test]
    fn read_string32_8() {
        assert_eq!(read(&HELLO_WORLD, 8).unwrap(), "rld".to_string());
    }

    #[test]
    fn read_string32_9() {
        assert_eq!(read(&HELLO_WORLD, 9).unwrap(), "ld".to_string());
    }

    #[test]
    fn read_string32_10() {
        assert_eq!(read(&HELLO_WORLD, 10).unwrap(), "d".to_string());
    }

    #[test]
    fn read_string32_11() {
        assert_eq!(read(&HELLO_WORLD, 11).unwrap(), "".to_string());
    }

    #[test]
    fn read_string32_out_of_bounds_0() {
        let result = read(&HELLO_WORLD, 12).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 12 at 12"
        );
    }

    #[test]
    fn read_string32_out_of_bounds_1() {
        let result = read(&HELLO_WORLD, 13).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 12 at 13"
        );
    }

    #[test]
    fn read_string32_out_of_bounds_2() {
        let result = read(&HELLO_WORLD, 99).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 12 at 99"
        );
    }

    #[test]
    fn read_exactly_32_string_0() {
        assert_eq!(read(&EXACTLY_32_BYTE_LONG_STRING, 0).unwrap(), "abcdefghijklmnopqrstuvwxyzABCDE".to_string());
    }

    #[test]
    fn read_exactly_32_string_1() {
        assert_eq!(read(&EXACTLY_32_BYTE_LONG_STRING, 16).unwrap(), "qrstuvwxyzABCDE".to_string());
    }

    #[test]
    fn read_exactly_33_string_0() {
        let result = read(&EXACTLY_33_BYTE_LONG_STRING, 0).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "StringTooLong when Reading a string at 32 with value abcdefghijklmnopqrstuvwxyzABCDEF"
        );
    }

    #[test]
    fn read_exactly_33_string_1() {
        assert_eq!(read(&EXACTLY_33_BYTE_LONG_STRING, 1).unwrap(), "bcdefghijklmnopqrstuvwxyzABCDEF".to_string());
    }
    
    #[test]
    fn read_exactly_33_string_2() {
        assert_eq!(read(&EXACTLY_33_BYTE_LONG_STRING, 16).unwrap(), "qrstuvwxyzABCDEF".to_string());
    }

    #[test]
    fn read_very_long_string_0() {
        let result = read(&VERY_LONG_STRING, 0).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "StringTooLong when Reading a string at 32 with value abcdefghijklmnopqrstuvwxyzABCDEF"
        );
    }

    #[test]
    fn read_very_long_string_1() {
        let result = read(&VERY_LONG_STRING, 8).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "StringTooLong when Reading a string at 40 with value ijklmnopqrstuvwxyzABCDEFGHIJKLMN"
        );
    }

    #[test]
    fn read_very_long_string_2() {
        let result = read(&VERY_LONG_STRING, 20).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "StringTooLong when Reading a string at 52 with value uvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        );
    }

    #[test]
    fn read_very_long_string_3() {
        assert_eq!(
            read(&VERY_LONG_STRING, 21).unwrap(),
            "vwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".to_string()
        );
    }

    #[test]
    fn read_very_long_string_4() {
        assert_eq!(
            read(&VERY_LONG_STRING, 22).unwrap(),
            "wxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".to_string()
        );
    }

    // We only care about null terminated strings
    #[test]
    fn read_string32_wo_null_0() {
        let result = read(&HELLO_WORLD_WO_NULL, 0).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 11 at 11"
        );
    }

    #[test]
    fn read_string32_wo_null_1() {
        let result = read(&HELLO_WORLD_WO_NULL, 6).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 11 at 11"
        );
    }
    
    #[test]
    fn read_string32_wo_null_2() {
        let result = read(&HELLO_WORLD_WO_NULL, 11).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 11 at 11"
        );
    }

    #[test]
    fn read_string32_wo_null_out_of_bounds() {
        let result = read(&HELLO_WORLD_WO_NULL, 12).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading a string for 11 at 12"
        );
    }
}

#[cfg(test)]
mod garbage_tests {
    use super::*;
    use std::time::SystemTime;

    static DISALLOWED_CHARACTERS: [u8; 56] = [
        0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
        0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F,
        0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x2A, 0x2B,
        0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
        0x40,
        0x5B, 0x5D, 0x5E,
        0x60,
        0x7B, 0x7C, 0x7D, 0x7E, 0x7F,
    ];

    static DISALLOWED_CHARACTER_IN_BEGINNING: [u8; 6] = *b"+test\0";
    static DISALLOWED_CHARACTER_IN_MIDDLE: [u8; 13] = *b"middle+after\0";
    static DISALLOWED_CHARACTER_IN_END: [u8; 12] = *b"end_before+\0";

    #[test]
    fn garbage_string() {
        let pointer = (SystemTime::now()
            .duration_since(SystemTime::UNIX_EPOCH).unwrap().as_nanos() as usize)
            % DISALLOWED_CHARACTERS.len();
        let result = read(&DISALLOWED_CHARACTERS, pointer).unwrap_err();
        let char = DISALLOWED_CHARACTERS[pointer];
        assert_eq!(
            format!("{result:?}"),
            format!("Detected non-ASCII character {char:#04X} when Reading a string at {pointer} with value ")
        );
    }

    #[test]
    fn garbage_string_in_beginning_0() {
        let result = read(&DISALLOWED_CHARACTER_IN_BEGINNING, 0).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            format!("Detected non-ASCII character 0x2B when Reading a string at 0 with value ")
        )
    }

    #[test]
    fn garbage_string_in_beginning_1() {
        assert_eq!(
            read(&DISALLOWED_CHARACTER_IN_BEGINNING, 1).unwrap(),
            "test".to_string()
        )
    }
    
    #[test]
    fn garbage_string_in_middle_0() {
        let result = read(&DISALLOWED_CHARACTER_IN_MIDDLE, 6).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            format!("Detected non-ASCII character 0x2B when Reading a string at 6 with value ")
        )
    }

    #[test]
    fn garbage_string_in_middle_2() {
        let result = read(&DISALLOWED_CHARACTER_IN_MIDDLE, 5).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            format!("Detected non-ASCII character 0x2B when Reading a string at 6 with value e")
        )
    }

    #[test]
    fn garbage_string_in_middle_3() {
        assert_eq!(
            read(&DISALLOWED_CHARACTER_IN_MIDDLE, 7).unwrap(),
            "after".to_string()
        )
    }

    #[test]
    fn garbage_string_in_end_0() {
        let result = read(&DISALLOWED_CHARACTER_IN_END, 0).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            format!("Detected non-ASCII character 0x2B when Reading a string at 10 with value end_before")
        )
    }

    #[test]
    fn garbage_string_in_end_1() {
        let result = read(&DISALLOWED_CHARACTER_IN_END, 4).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            format!("Detected non-ASCII character 0x2B when Reading a string at 10 with value before")
        )
    }
    
    #[test]
    fn garbage_string_in_end_2() {
        let result = read(&DISALLOWED_CHARACTER_IN_END, 10).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            format!("Detected non-ASCII character 0x2B when Reading a string at 10 with value ")
        )
    }

    #[test]
    fn garbage_string_in_end_3() {
        assert_eq!(
            read(&DISALLOWED_CHARACTER_IN_END, 11).unwrap(),
            "".to_string()
        )
    }
}

#[cfg(test)]
mod complex_tests {
    use super::*;

    static AMB_EXAMPLE: [u8; 128] = [
        0x23, 0x24, 0x2B, 0x3D, 0x23, 0x24, 0x2B, 0x3D, 0x23, 0x24, 0x2B, 0x3D, 0x23, 0x24, 0x2B, 0x3D,
        0x23, 0x24, 0x2B, 0x3D, 0x23, 0x24, 0x2B, 0x3D, 0x23, 0x24, 0x2B, 0x00, 0x00, 0x00, 0x00, 0x00,
        b'F', b'I', b'L', b'E', b'_', b'0', b'0', 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        b'F', b'I', b'L', b'E', b'_', b'0', b'1', 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        b'F', b'I', b'L', b'E', b'_', b'0', b'1', b'2', b'3', b'4', b'5', b'6', b'7', b'8', b'9', b'0',
        b'1', b'2', b'3', b'4', b'5', b'6', b'7', b'8', b'9', b'A', b'B', b'C', b'D', b'E', b'F', 0x00,
    ];

    static TXB_EXAMPLE: [u8; 64] = *b"#$+=#$+=#$+=#$+=FILE_00\0FILE_01\0FILE_01234567890123456789ABCDEF\0";

    
    #[test]
    fn amb_example_0() {
        let result = read(&AMB_EXAMPLE, 0x00).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "Detected non-ASCII character 0x23 when Reading a string at 0 with value "
        );
    }

    #[test]
    fn amb_example_1() {
        assert_eq!(read(&AMB_EXAMPLE, 0x20).unwrap(), "FILE_00".to_string());
    }

    #[test]
    fn amb_example_2() {
        assert_eq!(read(&AMB_EXAMPLE, 0x40).unwrap(), "FILE_01".to_string());
    }
    
    #[test]
    fn amb_example_3() {
        assert_eq!(read(&AMB_EXAMPLE, 0x60).unwrap(), "FILE_01234567890123456789ABCDEF".to_string());
    }

    #[test]
    fn txb_example_0() {
        let result = read(&TXB_EXAMPLE, 0x00).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "Detected non-ASCII character 0x23 when Reading a string at 0 with value "
        );
    }

    #[test]
    fn txb_example_1() {
        assert_eq!(read(&TXB_EXAMPLE, 0x10).unwrap(), "FILE_00".to_string());
        // That's a really good point, how to know when is the next string?
        assert_eq!(read(&TXB_EXAMPLE, 0x18).unwrap(), "FILE_01".to_string());
        assert_eq!(read(&TXB_EXAMPLE, 0x20).unwrap(), "FILE_01234567890123456789ABCDEF".to_string());
    }
}
