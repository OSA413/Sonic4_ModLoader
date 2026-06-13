use crate::{endianness::Endianness, error::{CommonBinaryError, PointerOutOfBoundsDetails}};

pub fn write(target: &mut [u8], pointer: usize, data: u32, endianness: &Option<Endianness>, when: String) -> Result<(), CommonBinaryError> {
    if target.len() < pointer + size_of::<u32>() {
        return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails { 
            pointer,
            source_len: target.len(),
            when: format!("Writing {}", when).to_string(),
        }))
    }

    let bytes = match endianness {
        Some(Endianness::Little) => data.to_le_bytes(),
        Some(Endianness::Big) => data.to_be_bytes(),
        None => data.to_le_bytes(),
    };

    target[pointer..pointer+size_of::<u32>()].copy_from_slice(&bytes);
    Ok(())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_write_0() {
        let mut target = [0; 6];
        write(&mut target, 0, 0x12345678, &None, "test_write_0".to_string()).unwrap();
        assert_eq!(target, [0x78, 0x56, 0x34, 0x12, 0, 0]);
    }

    #[test]
    fn test_write_1() {
        let mut target = [0; 6];
        write(&mut target, 1, 0x12345678, &None, "test_write_1".to_string()).unwrap();
        assert_eq!(target, [0, 0x78, 0x56, 0x34, 0x12, 0]);
    }

    #[test]
    fn test_write_2() {
        let mut target = [0; 6];
        write(&mut target, 2, 0x12345678, &None, "test_write_2".to_string()).unwrap();
        assert_eq!(target, [0, 0, 0x78, 0x56, 0x34, 0x12]);
    }

    #[test]
    fn test_write_3() {
        let mut target = [0; 6];
        let result = write(&mut target, 3, 0x12345678, &None, "test_write_3".to_string()).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_3 for 6 at 3"
        );
    }

    #[test]
    fn test_write_4() {
        let mut target = [0; 6];
        let result = write(&mut target, 99, 0x12345678, &None, "test_write_4".to_string()).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_4 for 6 at 99"
        );
    }

    #[test]
    fn test_write_le_0() {
        let mut target = [0; 6];
        write(&mut target, 0, 0x12345678, &Some(Endianness::Little), "test_write_le_0".to_string()).unwrap();
        assert_eq!(target, [0x78, 0x56, 0x34, 0x12, 0, 0]);
    }

    #[test]
    fn test_write_le_1() {
        let mut target = [0; 6];
        write(&mut target, 1, 0x12345678, &Some(Endianness::Little), "test_write_le_1".to_string()).unwrap();
        assert_eq!(target, [0, 0x78, 0x56, 0x34, 0x12, 0]);
    }

    #[test]
    fn test_write_le_2() {
        let mut target = [0; 6];
        write(&mut target, 2, 0x12345678, &Some(Endianness::Little), "test_write_le_2".to_string()).unwrap();
        assert_eq!(target, [0, 0, 0x78, 0x56, 0x34, 0x12]);
    }

    #[test]
    fn test_write_le_3() {
        let mut target = [0; 6];
        let result = write(&mut target, 3, 0x12345678, &Some(Endianness::Little), "test_write_le_3".to_string()).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_le_3 for 6 at 3"
        );
    }

    #[test]
    fn test_write_le_4() {
        let mut target = [0; 6];
        let result = write(&mut target, 99, 0x12345678, &Some(Endianness::Little), "test_write_le_4".to_string()).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_le_4 for 6 at 99"
        );
    }

    #[test]
    fn test_write_be_0() {
        let mut target = [0; 6];
        write(&mut target, 0, 0x12345678, &Some(Endianness::Big), "test_write_be_0".to_string()).unwrap();
        assert_eq!(target, [0x12, 0x34, 0x56, 0x78, 0, 0]);
    }

    #[test]
    fn test_write_be_1() {
        let mut target = [0; 6];
        write(&mut target, 1, 0x12345678, &Some(Endianness::Big), "test_write_be_1".to_string()).unwrap();
        assert_eq!(target, [0, 0x12, 0x34, 0x56, 0x78, 0]);
    }

    #[test]
    fn test_write_be_2() {
        let mut target = [0; 6];
        write(&mut target, 2, 0x12345678, &Some(Endianness::Big), "test_write_be_2".to_string()).unwrap();
        assert_eq!(target, [0, 0, 0x12, 0x34, 0x56, 0x78]);
    }

    #[test]
    fn test_write_be_3() {
        let mut target = [0; 6];
        let result = write(&mut target, 3, 0x12345678, &Some(Endianness::Big), "test_write_be_3".to_string()).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_be_3 for 6 at 3"
        );
    }

    #[test]
    fn test_write_be_4() {
        let mut target = [0; 6];
        let result = write(&mut target, 99, 0x12345678, &Some(Endianness::Big), "test_write_be_4".to_string()).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_be_4 for 6 at 99"
        );
    }
}
