use gtk::{pango, prelude::{TextBufferExt, TextTagExt}, TextSearchFlags};

pub fn format_buffer(text: String) -> gtk::TextBuffer {
    let formatting_markers = vec!["b", "i", "u", "strike"];
    let alignment_markers = vec!["l", "c", "r", "f"];
    let special_chars = vec![("\\n", "\n"), ("\\t", "\t"), ("\n* ", "\n • ")];

    let mut text = text.clone();

    if text.starts_with("* ") {
        text = text.replacen("* ", " • ", 1);
    };

    for (marker, replacement) in special_chars.iter() {
        text = text.replace(marker, replacement);
    };

    let buffer_builder = gtk::TextBuffer::builder();
    let table = gtk::TextTagTable::new();
    
    let bold_tag = gtk::TextTag::new(Some("b"));
    bold_tag.set_weight(700);
    table.add(&bold_tag);
    
    let italic_tag = gtk::TextTag::new(Some("i"));
    italic_tag.set_style(pango::Style::Italic);
    table.add(&italic_tag);

    let underline_tag = gtk::TextTag::new(Some("u"));
    underline_tag.set_style(pango::Style::Oblique);
    table.add(&underline_tag);

    let strike_tag = gtk::TextTag::new(Some("strike"));
    strike_tag.set_strikethrough(true);
    table.add(&strike_tag);

    let alignment_left_tag = gtk::TextTag::new(Some("l"));
    alignment_left_tag.set_justification(gtk::Justification::Left);
    table.add(&alignment_left_tag);

    let alignment_center_tag = gtk::TextTag::new(Some("c"));
    alignment_center_tag.set_justification(gtk::Justification::Center);
    table.add(&alignment_center_tag);

    let alignment_right_tag = gtk::TextTag::new(Some("r"));
    alignment_right_tag.set_justification(gtk::Justification::Right);
    table.add(&alignment_right_tag);

    let alignment_fill_tag = gtk::TextTag::new(Some("f"));
    alignment_fill_tag.set_justification(gtk::Justification::Fill);
    table.add(&alignment_fill_tag);

    let buffer_builder = buffer_builder.tag_table(&table);
    let buffer_builder = buffer_builder.text(text);
    let buffer = buffer_builder.build();

    let mut search_position = buffer.start_iter();
    for marker in formatting_markers.iter() {
        while !search_position.is_end() {
            match search_position.forward_search(format!("[{}]", marker).as_str(), TextSearchFlags::empty(), None) {
                Some(mut tag_start) => {
                    buffer.delete(&mut tag_start.0, &mut tag_start.1);
                    match tag_start.1.forward_search(format!("[\\{}]", marker).as_str(), TextSearchFlags::empty(), None) {
                        Some(mut tag_end) => {
                            buffer.apply_tag_by_name(marker, &tag_start.0, &tag_end.0);
                            buffer.delete(&mut tag_end.0, &mut tag_end.1);
                        }
                        None => buffer.apply_tag_by_name(marker, &tag_start.0, &buffer.end_iter()),
                    };
                    search_position = buffer.start_iter();
                }
                None => search_position.forward_to_end(),
            };
        }
        search_position = buffer.start_iter();
    }

    for marker in alignment_markers.iter() {
        while !search_position.is_end() {
            let tag_start = search_position.forward_search(format!("[{}]", marker).as_str(), TextSearchFlags::empty(), None);
            match tag_start {
                Some(mut tag_start) => {
                    buffer.delete(&mut tag_start.0, &mut tag_start.1);
                    tag_start.1.forward_to_line_end();
                    buffer.apply_tag_by_name(marker, &tag_start.0, &tag_start.1);            
                    search_position = buffer.start_iter();
                }
                None => search_position.forward_to_end(),
            };
        }
        search_position = buffer.start_iter();
    }

    buffer
}