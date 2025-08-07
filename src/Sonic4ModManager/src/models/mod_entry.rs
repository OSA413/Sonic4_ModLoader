use adw::subclass::prelude::*;
use common::mod_logic::mod_entry::ModEntry;
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
        pub enabled: RefCell<bool>,
        #[property(get, set)]
        pub title: RefCell<Option<String>>,
        #[property(get, set)]
        pub authors: RefCell<Option<String>>,
        #[property(get, set)]
        pub version: RefCell<Option<String>>,
        #[property(get, set)]
        pub description: RefCell<Option<String>>,
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
        enabled: bool,
        title: Option<&str>,
        authors: Option<&str>,
        version: Option<&str>,
        description: Option<&str>,
    ) -> Self {
        glib::Object::builder()
            .property("path", path)
            .property("enabled", enabled)
            .property("title", title)
            .property("authors", authors)
            .property("version", version)
            .property("description", description)
            .build()
    }

    pub fn from_mod_entry(mod_entry: &ModEntry) -> Self {
        Self::new(
            mod_entry.path.as_str(),
            mod_entry.enabled,
            mod_entry.title.as_ref().map(|s| s.as_str()),
            mod_entry.authors.as_ref().map(|s| s.as_str()),
            mod_entry.version.as_ref().map(|s| s.as_str()),
            mod_entry.description.as_ref().map(|s| s.as_str()),
        )
    }
}
