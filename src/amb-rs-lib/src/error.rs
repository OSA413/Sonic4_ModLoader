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

pub enum AmbLibRsError {
    Io(std::io::Error),
    PointerOutOfBounds(PointerOutOfBoundsDetails),
    ProvidedSourceIsNotAnAmb(String),
    StringTooLong(StringTooLongDetails),
}

impl From<std::io::Error> for AmbLibRsError {
    fn from(e: std::io::Error) -> Self {
        Self::Io(e)
    }
}

impl std::fmt::Debug for AmbLibRsError {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            AmbLibRsError::Io(e) => write!(f, "IO error: {e}"),
            AmbLibRsError::PointerOutOfBounds(e) => write!(f, "PointerOutOfBounds when {} for {} at {}", e.when, e.source_len, e.pointer),
            AmbLibRsError::ProvidedSourceIsNotAnAmb(e) => write!(f, "{e}"),
            AmbLibRsError::StringTooLong(e) => write!(f, "StringTooLong when {} at {} with value {}", e.when, e.pointer, e.target_string),
        }
    }
}