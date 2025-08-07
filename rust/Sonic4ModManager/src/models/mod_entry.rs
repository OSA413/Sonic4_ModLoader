use adw::subclass::prelude::*;
use gtk::prelude::ObjectExt;
use gtk::glib;
use gtk::glib::Properties;
use std::cell::RefCell;

// G means GNOME because I don't want to confuse pure Rust structs with GObject
mod imp {
    use super::*;

    #[derive(Debug, Default, Properties)]
    #[properties(wrapper_type = super::GModEntry)]
    pub struct GModEntry {
        #[property(get, set)]
        pub path: RefCell<String>,
        #[property(get, set)]
        pub title: RefCell<Option<String>>,
        #[property(get, set)]
        pub authors: RefCell<Option<String>>,
        #[property(get, set)]
        pub version: RefCell<Option<String>>,
        #[property(get, set)]
        pub description_file: RefCell<Option<String>>,
    }

    #[glib::object_subclass]
    impl ObjectSubclass for GModEntry {
        const NAME: &'static str = "GModEntry";
        type Type = super::GModEntry;
        type ParentType = glib::Object;
    }

    #[glib::derived_properties]
    impl ObjectImpl for GModEntry {}
}

glib::wrapper! {
    pub struct GModEntry(ObjectSubclass<imp::GModEntry>);
}

impl GModEntry {
    pub fn new(
        path: &str,
        title: Option<&str>,
        authors: Option<&str>,
        version: Option<&str>,
        description_file: Option<&str>,
    ) -> Self {
        glib::Object::builder()
            .property("path", path)
            .property("title", title)
            .property("authors", authors)
            .property("version", version)
            .property("description_file", description_file)
            .build()
    }
}
