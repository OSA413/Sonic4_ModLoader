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

#[cfg(test)]
mod tests {
    use super::*;

    static SIMPLE_SOURCE: [u8; 6] = [0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC];

    #[test]
    fn test_read_0() {
        assert_eq!(read(&SIMPLE_SOURCE, 0, &None).unwrap(), 0x78563412);
    }

    #[test]
    fn test_read_1() {
        assert_eq!(read(&SIMPLE_SOURCE, 1, &None).unwrap(), 0x9A785634);
    }

    #[test]
    fn test_read_2() {
        assert_eq!(read(&SIMPLE_SOURCE, 2, &None).unwrap(), 0xBC9A7856);
    }

    #[test]
    fn test_read_3() {
        let result = read(&SIMPLE_SOURCE, 3, &None).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u32 for 6 at 3"
        );
    }

    #[test]
    fn test_read_4() {
        let result = read(&SIMPLE_SOURCE, 99, &None).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u32 for 6 at 99"
        );
    }

    #[test]
    fn test_read_le_0() {
        assert_eq!(read(&SIMPLE_SOURCE, 0, &Some(Endianness::Little)).unwrap(), 0x78563412);
    }

    #[test]
    fn test_read_le_1() {
        assert_eq!(read(&SIMPLE_SOURCE, 1, &Some(Endianness::Little)).unwrap(), 0x9A785634);
    }

    #[test]
    fn test_read_le_2() {
        assert_eq!(read(&SIMPLE_SOURCE, 2, &Some(Endianness::Little)).unwrap(), 0xBC9A7856);
    }

    #[test]
    fn test_read_le_3() {
        let result = read(&SIMPLE_SOURCE, 3, &Some(Endianness::Little)).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u32 for 6 at 3"
        );
    }

    #[test]
    fn test_read_le_4() {
        let result = read(&SIMPLE_SOURCE, 99, &Some(Endianness::Little)).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u32 for 6 at 99"
        );
    }

    #[test]
    fn test_read_be_0() {
        assert_eq!(read(&SIMPLE_SOURCE, 0, &Some(Endianness::Big)).unwrap(), 0x12345678);
    }

    #[test]
    fn test_read_be_1() {
        assert_eq!(read(&SIMPLE_SOURCE, 1, &Some(Endianness::Big)).unwrap(), 0x3456789A);
    }

    #[test]
    fn test_read_be_2() {
        assert_eq!(read(&SIMPLE_SOURCE, 2, &Some(Endianness::Big)).unwrap(), 0x56789ABC);
    }

    #[test]
    fn test_read_be_3() {
        let result = read(&SIMPLE_SOURCE, 3, &Some(Endianness::Big)).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u32 for 6 at 3"
        );
    }
    
    #[test]
    fn test_read_be_4() {
        let result = read(&SIMPLE_SOURCE, 99, &Some(Endianness::Big)).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u32 for 6 at 99"
        );
    }
}