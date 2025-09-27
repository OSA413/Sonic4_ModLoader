use std::cmp;

use adw::{prelude::{ActionRowExt, AdwDialogExt, AlertDialogExt}, subclass::prelude::*, ActionRow};
use common::{mod_logic::mod_entry::ModEntry, Launcher};
use gtk::{gio::{self, prelude::{ApplicationExt, ListModelExt, ListModelExtManual}}, glib::{self, clone, object::Cast, Object}, prelude::{ActionMapExtManual, CheckButtonExt, GtkWindowExt, ListBoxRowExt, TextBufferExt, TextTagExt, TextViewExt}, Align, CheckButton};
use crate::{buffer_formatter, installation, models::g_mod_entry::GModEntry, settings_dialog::SettingsWindow};
use std::cell::RefCell;
use std::fs;
use rand::rng;
use rand::seq::SliceRandom;

enum Offset {
    Top,
    Up,
    Down,
    Bottom,
}

mod imp {
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
        let mut result = Vec::<String>::new();
        let mod_entries = self.imp().mod_store.iter::<Object>();
        for mod_entry in mod_entries {
            let g_mod_entry = mod_entry.unwrap().downcast::<GModEntry>().unwrap();
            if g_mod_entry.enabled() {
                result.push(g_mod_entry.path());
            }
        }
        result.reverse();
        self.save_mods_ini(result);
    }

    fn save_mods_ini(&self, mods: Vec<String>) {
        // It will probably crash, fix later
        fs::create_dir_all("mods");
        fs::write("mods/mods.ini", mods.join("\n"));
    }

    fn save_mods_and_play(&self) {
        self.save_mods();
        let game_lauched = common::Launcher::launch_game();
        match game_lauched {
            Ok(_) => self.application().unwrap().quit(),
            Err(e) => println!("{}", e),
        }
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

        let save_mods_action = gio::ActionEntry::builder("save_mods")
            .activate(move |app: &Self, _, _| app.save_mods())
            .build();

        let save_mods_and_play_action = gio::ActionEntry::builder("save_mods_and_play")
            .activate(move |app: &Self, _, _| app.save_mods_and_play())
            .build();

        let open_mods_folder_action = gio::ActionEntry::builder("open_mods_folder")
            .activate(move |app: &Self, _, _| app.open_mods_folder())
            .build();

        let refresh_mod_list_action = gio::ActionEntry::builder("refresh_mod_list")
            .activate(move |app: &Self, _, _| app.refresh_mod_list())
            .build();

        let randomize_mod_list_action = gio::ActionEntry::builder("randomize_mod_list")
            .activate(move |app: &Self, _, _| app.randomize_mod_list())
            .build();

        let show_settings_action = gio::ActionEntry::builder("show_settings")
            .activate(move |app: &Self, _, _| app.show_settings())
            .build();

        self.add_action_entries([
            mod_check_action,
            move_mod_to_top_action,
            move_mod_up_action,
            move_mod_down_action,
            move_mod_to_bottom_action,
            save_mods_action,
            save_mods_and_play_action,
            open_mods_folder_action,
            refresh_mod_list_action,
            randomize_mod_list_action,
            show_settings_action,
        ]);
    }

    fn show_settings(&self) {
        SettingsWindow::new().present();
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

        let g_mod_entry_ = g_mod_entry.clone();
        check_button.connect_toggled(move |check_box| g_mod_entry_.set_enabled(check_box.is_active()));

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

        row
    }

    fn refresh_mod_list(&self) {
        let mod_entries = ModEntry::load("./mods");
        let g_mod_entries = mod_entries.iter().map(|x| GModEntry::from_mod_entry(x)).collect::<Vec<_>>();
        self.imp().mod_store.remove_all();
        self.imp().mod_store.extend_from_slice(&g_mod_entries);
    }

    fn open_mods_folder(&self) {
        Launcher::open_mods_folder();
    }

    fn randomize_mod_list(&self) {
        let range = 0..self.imp().mod_store.n_items();
        let mut result: Vec<glib::Object> = range.map(|position| self.imp().mod_store.item(position).unwrap()).collect();
        result.shuffle(&mut rng());
        result.iter().for_each(|obj| {
            let new_mod = obj
                .downcast_ref::<GModEntry>()
                .expect("The object should be of type `GModEntry`.");

            new_mod.set_enabled(rand::random_bool(0.5));
        });
        self.imp().mod_store.remove_all();
        self.imp().mod_store.extend_from_slice(&result);
    }

    fn show_first_time_dialog(&self) {
        let alert = adw::AlertDialog::new(Some("First Launch Dialog"), Some("Hello!
It seems that you have launched the Mod Manager for the first time. Do you want to install the Mod Loader to be able to launch it from other shortcuts (e.g. Steam)?

You can install/uninstall and configure it through the settings menu at any time."));

        alert.add_response("no", "No");
        alert.set_response_appearance("no", adw::ResponseAppearance::Destructive);
        alert.add_response("ask_later", "Ask Later");
        alert.set_response_appearance("ask_later", adw::ResponseAppearance::Default);
        alert.add_response("yes", "Yes");
        alert.set_response_appearance("yes", adw::ResponseAppearance::Suggested);
        alert.set_close_response("ask_later");
        alert.present(Some(self));

        alert.connect_response(None, move |_, response| {
            match response {
                "no" => installation::write_empty_config(),
                "yes" => installation::install(),
                _ => {}
            }
        });
    }

    fn startup(&self) {
        // TODO remove clone
        let closure = {
            clone!(
                #[strong (rename_to = this)] self,
                move |obj: &glib::Object| this.create_mod_list_closure(obj)
            )
        };
        self.imp().mod_list.bind_model(Some(&self.imp().mod_store.get()), move |obj: &glib::Object| closure(obj));

        self.refresh_mod_list();

        let description = fs::read_to_string("Mod Loader - Whats new.txt").unwrap_or("File \"Mod Loader - Whats new.txt\" not found.".to_string());
        let description = format!("Select a mod to read its description\n\n[c][b][i]What's new:[\\i][\\b]\n{}\n\nHome page: https://github.com/OSA413/Sonic4_ModLoader", description);
        let description = buffer_formatter::format_buffer(description);
        self.imp().description.set_buffer(Some(&description));
        
        match installation::get_installation_status() {
            installation::InstallationStatus::FirstLaunch => self.show_first_time_dialog(),
            _ => ()
        };
    }
}
