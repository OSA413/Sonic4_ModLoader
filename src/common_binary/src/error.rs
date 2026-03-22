pub struct PointerOutOfBoundsDetails {
    pub pointer: usize,
    pub source_len: usize,
    pub when: String,
}

pub struct StringTooLongDetails {
    pub pointer: usize,
    pub target_string: String,
    pub when: String,
}

pub enum CommonBinaryError {
    Io(std::io::Error),
    PointerOutOfBounds(PointerOutOfBoundsDetails),
    ProvidedSourceIsNotAnAmb(String),
    StringTooLong(StringTooLongDetails),
}

impl From<std::io::Error> for CommonBinaryError {
    fn from(e: std::io::Error) -> Self {
        Self::Io(e)
    }
}

impl std::fmt::Debug for CommonBinaryError {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            CommonBinaryError::Io(e) => write!(f, "IO error: {e}"),
            CommonBinaryError::PointerOutOfBounds(e) => write!(f, "PointerOutOfBounds when {} for {} at {}", e.when, e.source_len, e.pointer),
            CommonBinaryError::ProvidedSourceIsNotAnAmb(e) => write!(f, "{e}"),
            CommonBinaryError::StringTooLong(e) => write!(f, "StringTooLong when {} at {} with value {}", e.when, e.pointer, e.target_string),
        }
    }
}