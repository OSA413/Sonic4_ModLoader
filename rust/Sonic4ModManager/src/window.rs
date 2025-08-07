use adw::{prelude::{ActionRowExt}, subclass::prelude::*, ActionRow};
use gtk::{gio::{self}, glib::{self, object::Cast}, prelude::{ActionMapExtManual, TextBufferExt, TextTagExt, TextViewExt}, Align, CheckButton, Widget};

use crate::models::mod_entry::GModEntry;

mod imp {
    use super::*;

    #[derive(Debug, Default, gtk::CompositeTemplate)]
    #[template(resource = "/Sonic4ModLoader/Sonic4ModManager/window.ui")]
    pub struct Sonic4ModManagerWindow {
        #[template_child]
        pub mod_list: TemplateChild<gtk::ListBox>,
        #[template_child]
        pub description: TemplateChild<gtk::TextView>,
    }

    #[glib::object_subclass]
    impl ObjectSubclass for Sonic4ModManagerWindow {
        const NAME: &'static str = "Sonic4ModManager";
        type Type = super::Sonic4ModManagerWindow;
        type ParentType = gtk::ApplicationWindow;

        fn class_init(klass: &mut Self::Class) {
            klass.bind_template();
        }

        fn instance_init(obj: &glib::subclass::InitializingObject<Self>) {
            obj.init_template();
        }
    }

    impl ObjectImpl for Sonic4ModManagerWindow {
        fn constructed(&self) {
            self.parent_constructed();
            self.obj().setup_actions();
            self.obj().startup();
        }
    }

    impl WidgetImpl for Sonic4ModManagerWindow {}
    impl WindowImpl for Sonic4ModManagerWindow {}
    impl ApplicationWindowImpl for Sonic4ModManagerWindow {}
    impl AdwApplicationWindowImpl for Sonic4ModManagerWindow {}
}

glib::wrapper! {
    pub struct Sonic4ModManagerWindow(ObjectSubclass<imp::Sonic4ModManagerWindow>)
        @extends gtk::Widget, gtk::Window, gtk::ApplicationWindow, adw::ApplicationWindow,
        @implements 
            gio::ActionGroup,
            gio::ActionMap,
            gtk::ConstraintTarget,
            gtk::Buildable,
            gtk::Accessible,
            gtk::ShortcutManager,
            gtk::Root,
            gtk::Native;
}

impl Sonic4ModManagerWindow {
    pub fn new<P: glib::prelude::IsA<gtk::Application>>(application: &P) -> Self {
        glib::Object::builder()
            .property("application", application)
            .build()
    }

    fn setup_actions(&self) {
        // let mod_check_action = gio::ActionEntry::builder("check_mod")
        //     .activate(move |app: &Self, _, _| app.create_mod_row("test", "Version 1.0.1 by OSA413"))
        //     .build();

        // self.add_action_entries([mod_check_action]);
    }

    fn startup(&self) {
        let list_store = gio::ListStore::new::<GModEntry>();
        list_store.extend_from_slice(&vec![
            GModEntry::new("my cool mod", None, None, None, None),
            GModEntry::new("another_mod", Some("Another mod 🍪"), None, None, None),
            GModEntry::new("another_mod2", None, Some("OSA413"), None, None),
        ]);

        self.imp().mod_list.bind_model(Some(&list_store),move |obj| {
            let mod_entry = obj
                .downcast_ref::<GModEntry>()
                .expect("The object should be of type `ModEntry`.");

            let check_button = CheckButton::builder()
                .valign(Align::Center)
                .can_focus(false)
                .build();

            let version_string = match mod_entry.version() {
                Some(version) => {
                    match mod_entry.authors() {
                        Some(authors) => format!("Version {} by {}", version, authors),
                        None => format!("Version {}", version)
                    }
                },
                None => match mod_entry.authors() {
                    Some(authors) => format!("by {}", authors),
                    None => "".to_string()
                },
            };

            let row = ActionRow::builder()
                .title(mod_entry.title().unwrap_or(mod_entry.path()))
                .subtitle(version_string)
                .build();
            row.add_prefix(&check_button);
            
            row.upcast()
        });

        let buffer_builder = gtk::TextBuffer::builder();
        let tag = gtk::TextTag::new(Some("test"));
        tag.set_weight(600);
        tag.set_size(16);
        let table = gtk::TextTagTable::new();
        table.add(&tag);
        let buffer_builder = buffer_builder.tag_table(&table);
        let buffer_builder = buffer_builder.text("This is a test description");
        // let buffer = buffer_builder.build();
        // let start = buffer.
        // buffer.apply_tag(&tag, gtk::TextIter::, -1);
        // self.imp().description.set_buffer(Some(&buffer));
    }
}
