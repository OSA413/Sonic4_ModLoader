pub fn make_safe(raw_name: &str) -> String {
    //removing ".\" in the names (Windows can't create "." folders)
    //sometimes they can have several ".\" in the names
    //Turns out there's a double dot directory in file names
    //And double backslash in file names
    let mut safe_index = 0;
    let mut chars = raw_name.chars();
    while let Some(ch) = chars.nth(0) {
        if ch == '.' || ch == '\\' || ch == '/' {
            safe_index += 1;
        } else {
            break;
        }
    }

    raw_name.chars().skip(safe_index).collect()
}