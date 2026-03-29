use adw::subclass::prelude::*;
use gtk::prelude::ObjectExt;
use gtk::glib;
use gtk::glib::Properties;
use std::cell::RefCell;

// G means GNOME because I don't want to confuse pure Rust structs with GObject
mod imp {
    use super::*;

    #[derive(Debug, Default, Properties)]
    #[properties(wrapper_type = super::MyGString)]
    pub struct MyGString {
        #[property(get, set)]
        pub value: RefCell<String>,
    }

    #[glib::object_subclass]
    impl ObjectSubclass for MyGString {
        const NAME: &'static str = "MyGString";
        type Type = super::MyGString;
        type ParentType = glib::Object;
    }

    #[glib::derived_properties]
    impl ObjectImpl for MyGString {}
}

glib::wrapper! {
    pub struct MyGString(ObjectSubclass<imp::MyGString>);
}

impl MyGString {
    pub fn new(
        value: &str,
    ) -> Self {
        glib::Object::builder()
            .property("value", value)
            .build()
    }

    pub fn from_string(value: &String) -> Self {
        Self::new(value)
    }
}
