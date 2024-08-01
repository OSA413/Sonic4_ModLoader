mod imp;

use gio::{ActionEntry, PropertyAction};
use glib::Object;
use gtk::prelude::*;
use gtk::subclass::prelude::*;
use gtk::{gio, glib, Orientation};

glib::wrapper! {
    pub struct Window(ObjectSubclass<imp::Window>)
        @extends gtk::ApplicationWindow, gtk::Window, gtk::Widget,
        @implements gio::ActionGroup, gio::ActionMap, gtk::Accessible, gtk::Buildable,
                    gtk::ConstraintTarget, gtk::Native, gtk::Root, gtk::ShortcutManager;
}

impl Window {
    pub fn new(app: &adw::Application) -> Self {
        Object::builder().property("application", app).build()
    }

    fn setup_actions(&self) {
    }
}
