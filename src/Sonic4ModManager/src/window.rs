use std::cmp;

use adw::{prelude::{ActionRowExt}, subclass::prelude::*, ActionRow};
use common::mod_logic::mod_entry::ModEntry;
use gtk::{gio::{self, prelude::ListModelExt}, glib::{self, clone, gobject_ffi::GObject, object::Cast, property::{PropertyGet, PropertySet}}, prelude::{ActionMapExtManual, ListBoxRowExt, TextTagExt, WidgetExt}, Align, CheckButton};
use crate::models::mod_entry::GModEntry;

enum Offset {
    Top,
    Up,
    Down,
    Bottom,
}

mod imp {
    use std::cell::RefCell;

    use super::*;

    #[derive(Debug, Default, gtk::CompositeTemplate)]
    #[template(resource = "/Sonic4ModLoader/Sonic4ModManager/window.ui")]
    pub struct Sonic4ModManagerWindow {
        #[template_child]
        pub mod_list: TemplateChild<gtk::ListBox>,
        #[template_child]
        pub description: TemplateChild<gtk::TextView>,
        #[template_child]
        pub refresh_button: TemplateChild<gtk::Button>,
        
        #[template_child]
        pub mod_store: TemplateChild<gio::ListStore>,
        pub selected_mod_index: RefCell<Option<String>>,
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

    fn save_mods(&self) {
        // let mod_entries = self.imp().mod_store.get().iter().map(|x| x.downcast_ref::<GModEntry>().unwrap().to_mod_entry()).collect::<Vec<_>>();
        // ModEntry::save("./mods", &mod_entries);
    }

    fn get_selected_mod_index(&self) -> Option<u32> {
        match self.imp().mod_list.selected_row() {
            Some(row) => Some(row.index() as u32),
            None => None,
        }
    }

    fn move_selected_mod(&self, offset: Offset) {
        match self.get_selected_mod_index() {
            Some(index) => {
                let mod_to_move = self.imp().mod_store.get().item(index);
                match mod_to_move {
                    Some(mod_to_move) => {
                        self.imp().mod_store.remove(index);
                        let final_index = match offset {
                            Offset::Top => 0,
                            Offset::Up => match index {0 => 0, _ => index - 1},
                            Offset::Down => cmp::min(index + 1, self.imp().mod_store.n_items()),
                            Offset::Bottom => self.imp().mod_store.n_items(),
                        };

                        let mod_folder = mod_to_move.downcast_ref::<GModEntry>().unwrap().path();
                        self.imp().mod_store.insert(final_index , &mod_to_move);
                        self.imp().selected_mod_index.replace(Some(mod_folder));
                    }
                    None => (),
                }
            }
            None => (),
        }
    }

    fn setup_actions(&self) {
        let mod_check_action = gio::ActionEntry::builder("check_mod")
            .activate(move |app: &Self, _, _| app.imp().mod_store.remove(0))
            .build();

        let move_mod_to_top_action = gio::ActionEntry::builder("move_mod_to_top")
            .activate(move |app: &Self, _, _| app.move_selected_mod(Offset::Top))
            .build();

        let move_mod_up_action = gio::ActionEntry::builder("move_mod_up")
            .activate(move |app: &Self, _, _| app.move_selected_mod(Offset::Up))
            .build();

        let move_mod_down_action = gio::ActionEntry::builder("move_mod_down")
            .activate(move |app: &Self, _, _| app.move_selected_mod(Offset::Down))
            .build();
        
        let move_mod_to_bottom_action = gio::ActionEntry::builder("move_mod_to_bottom")
            .activate(move |app: &Self, _, _| app.move_selected_mod(Offset::Bottom))
            .build();

        self.add_action_entries([
            mod_check_action,
            move_mod_to_top_action,
            move_mod_up_action,
            move_mod_down_action,
            move_mod_to_bottom_action,
        ]);
    }

    fn create_mod_list_closure(&self, obj: &gtk::glib::Object) -> gtk::Widget {
        let g_mod_entry = obj
            .downcast_ref::<GModEntry>()
            .expect("The object should be of type `GModEntry`.");

        let check_button = CheckButton::builder()
            .valign(Align::Center)
            .active(g_mod_entry.enabled())
            .can_focus(false)
            .build();

        let version_string = match g_mod_entry.version() {
            Some(version) => {
                match g_mod_entry.authors() {
                    Some(authors) => format!("Version {} by {}", version, authors),
                    None => format!("Version {}", version)
                }
            },
            None => match g_mod_entry.authors() {
                Some(authors) => format!("by {}", authors),
                None => "".to_string()
            },
        };

        let row = ActionRow::builder()
            .title(g_mod_entry.title().unwrap_or(g_mod_entry.path()))
            .subtitle(version_string)
            .build();
        row.add_prefix(&check_button);

        let row = row.upcast::<gtk::Widget>();
        
        match self.imp().mod_list.first_child() {
            Some(first_child) => first_child.activate(),
            None => false,
        };

        row
    }

    fn startup(&self) {
        let mod_entries = ModEntry::load("./mods");
        let g_mod_entries = mod_entries.iter().map(|x| GModEntry::from_mod_entry(x)).collect::<Vec<_>>();
        self.imp().mod_store.extend_from_slice(&g_mod_entries);

        // TODO remove clone
        let closure = {
            clone!(
                #[strong (rename_to = this)] self,
                move |obj: &glib::Object| this.create_mod_list_closure(obj)
            )
        };

        self.imp().mod_list.bind_model(Some(&self.imp().mod_store.get()), move |obj: &glib::Object| closure(obj));

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
