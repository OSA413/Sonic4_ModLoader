use std::{fs::File, io::Write, ops::Deref};
use async_channel::Sender;
use futures_util::StreamExt;
use adw::subclass::prelude::*;
use gtk::{ProgressBar, gio::{self, ActionEntry, prelude::ActionMapExtManual}, glib::{self, clone}, prelude::{ButtonExt, EditableExt, WidgetExt}};

use crate::{arg_handler::{ArgHandler, InitialArgs}, tokio_runtime};

async fn download_mod(url: String, to: String, progress_bar: Sender<f64>, progress_bar_text: Sender<String>) {
    progress_bar_text.send_blocking("Connecting to the server...".to_string()).unwrap();

    let response = reqwest::Client::new()
        .get(url)
        .send()
        .await
        .unwrap();
    let total_size = response.content_length().unwrap();

    progress_bar_text.send_blocking("Downloading...".to_string()).unwrap();
    progress_bar.send_blocking(0.0).unwrap();

    let mut file = File::create(to).unwrap();
    let mut downloaded = 0;
    let mut stream = response.bytes_stream();

    while let Some(item) = stream.next().await {
        let chunk = item.unwrap();
        file.write_all(&chunk).unwrap();
        downloaded += chunk.len();
        println!("{}", downloaded as f64 / total_size as f64);
        progress_bar.send_blocking(downloaded as f64 / total_size as f64).unwrap();
    }

    progress_bar_text.send_blocking("Done!".to_string()).unwrap();
    progress_bar.send_blocking(1.0).unwrap();
}

mod imp {
    use super::*;

    #[derive(Debug, Default, gtk::CompositeTemplate)]
    #[template(resource = "/Sonic4ModLoader/OneClickModInstaller/window.ui")]
    pub struct OneClickModInstallerWindow {
        #[template_child]
        pub logo: TemplateChild<gtk::Picture>,
        #[template_child]
        pub stack: TemplateChild<adw::ViewStack>,

        // Mod Installation
        #[template_child]
        pub mod_path_entry: TemplateChild<gtk::Entry>,
        #[template_child]
        pub mod_path_button: TemplateChild<gtk::Button>,
        #[template_child]
        pub exit_after_install_checkbutton: TemplateChild<gtk::CheckButton>,
        #[template_child]
        pub install_button: TemplateChild<gtk::Button>,
        #[template_child]
        pub progress_bar: TemplateChild<gtk::ProgressBar>,
    }

    #[glib::object_subclass]
    impl ObjectSubclass for OneClickModInstallerWindow {
        const NAME: &'static str = "OneClickModInstaller";
        type Type = super::OneClickModInstallerWindow;
        type ParentType = adw::ApplicationWindow;

        fn class_init(klass: &mut Self::Class) {
            klass.bind_template();
        }

        fn instance_init(obj: &glib::subclass::InitializingObject<Self>) {
            obj.init_template();
        }
    }

    impl ObjectImpl for OneClickModInstallerWindow {
        fn constructed(&self) {
            self.parent_constructed();
            self.obj().setup_actions();
            self.obj().startup();
        }
    }

    impl WidgetImpl for OneClickModInstallerWindow {}
    impl WindowImpl for OneClickModInstallerWindow {}
    impl ApplicationWindowImpl for OneClickModInstallerWindow {}
    impl AdwApplicationWindowImpl for OneClickModInstallerWindow {}
}

glib::wrapper! {
    pub struct OneClickModInstallerWindow(ObjectSubclass<imp::OneClickModInstallerWindow>)
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

impl OneClickModInstallerWindow {
    pub fn new<P: glib::prelude::IsA<gtk::Application>>(application: &P) -> Self {
        glib::Object::builder()
            .property("application", application)
            .build()
    }

    fn setup_actions(&self) {
        let install_mod_action = gio::ActionEntry::builder("install_mod")
            .activate(move |app: &Self, _, _| app.download_mod())
            .build();

        self.add_action_entries([install_mod_action]);
    }

    fn download_mod(&self) {
        let (progress_bar_sender, progress_bar_receiver) = async_channel::bounded(1);
        let (progress_bar_text_sender, progress_bar_text_receiver) = async_channel::bounded(1);
        glib::spawn_future_local(clone!(
            #[weak (rename_to = this)]
            self,
            async move {
                this.imp().install_button.set_sensitive(false);
                tokio_runtime::tokio_runtime().spawn(
                    download_mod(
                        this.imp().mod_path_entry.text().to_string(),
                        "./mod".to_string(),
                        progress_bar_sender,
                        progress_bar_text_sender
                    )
                ).await;
                this.imp().install_button.set_sensitive(true);
            }
        ));

        glib::spawn_future_local(clone!(
            #[weak (rename_to = this)]
            self,
            async move {
                while let Ok(fraction) = progress_bar_receiver.recv().await {
                    this.imp().progress_bar.set_fraction(fraction);
                }
            }
        ));
        
        glib::spawn_future_local(clone!(
            #[weak (rename_to = this)]
            self,
            async move {
                while let Ok(text) = progress_bar_text_receiver.recv().await {
                    this.imp().progress_bar.set_text(Some(&text));
                }
            }
        ));
    }

    fn handle_initial_args(&self) {
        let initial_args = ArgHandler::get();
        let initial_args = initial_args.deref();
        self.imp().mod_path_entry.set_sensitive(false);
        self.imp().mod_path_button.set_sensitive(false);
        self.imp().stack.set_visible_child_name("mod_installation");
        match initial_args {
            InitialArgs::FromDir(dir) => {
                println!("Provied args for mod from directory: {dir}");        
                self.imp().mod_path_entry.set_text(dir);
            },
            InitialArgs::FromArchive(archive) => {
                println!("Provied args for mod from archive: {archive}");
                self.imp().mod_path_entry.set_text(archive);
            },
            InitialArgs::FromGameBanana { url, type_, id } => {
                println!("Provied args for mod from GameBanana: {url}, {type_}, {id}");
                self.imp().mod_path_entry.set_text(url);
            },
            InitialArgs::FromInternet(url) => {
                println!("Provied args for mod from the internet: {url}");
                self.imp().mod_path_entry.set_text(url);
            },
            InitialArgs::None => {
                println!("No initial args provided");
                self.imp().mod_path_entry.set_sensitive(true);
                self.imp().mod_path_button.set_sensitive(true);
                self.imp().stack.set_visible_child_name("current_installation");
            },
        };
    }

    fn startup(&self) {
        self.imp().logo.set_resource(Some("/Sonic4ModLoader/OneClickModInstaller/logo.svg"));
        common::Launcher::where_in_the_world_am_i();
        common_gtk4::show_admin_warning(self);
        self.handle_initial_args();
    }
}
