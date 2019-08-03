using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Sonic4ModManager
{
    public partial class SettingsForm:Form
    {
        public static class Settings
        {
            public static class AMBPatcher
            {
                public static bool ProgressBar  { set; get; }
                public static bool GenerateLog  { set; get; }
                public static bool SHACheck     { set; get; }
                public static int  SHAType      { set; get; }
            }

            public static class CsbEditor
            {
                public static bool  EnableThreading { set; get; }
                public static int   MaxThreads      { set; get; }
                public static int   BufferSize      { set; get; }
            }
            
            public static void Load()
            {
                //////////////
                //AMBPatcher//
                //////////////

                //Defaults
                Settings.AMBPatcher.ProgressBar = true;
                Settings.AMBPatcher.GenerateLog = false;
                Settings.AMBPatcher.SHACheck    = true;
                Settings.AMBPatcher.SHAType     = 1;

                if (File.Exists("AMBPatcher.cfg"))
                {
                    string[] cfg_file = File.ReadAllLines("AMBPatcher.cfg");

                    foreach (string line in cfg_file)
                    {
                        if (!line.Contains("=")) continue;
                        string key   = line.Substring(0, line.IndexOf("="));
                        string value = line.Substring(line.IndexOf("=") + 1);
                        
                        switch (key)
                        {
                            case "ProgressBar": Settings.AMBPatcher.ProgressBar = Convert.ToBoolean(Convert.ToInt32(value)); break;
                            case "GenerateLog": Settings.AMBPatcher.GenerateLog = Convert.ToBoolean(Convert.ToInt32(value)); break;
                            case "SHACheck":    Settings.AMBPatcher.SHACheck    = Convert.ToBoolean(Convert.ToInt32(value)); break;
                            case "SHAType":     Settings.AMBPatcher.SHAType     = Convert.ToInt32(value); break;
                        }
                    }
                }

                /////////////
                //CsbEditor//
                /////////////

                //Defaults
                Settings.CsbEditor.EnableThreading  = true;
                Settings.CsbEditor.MaxThreads       = 4;
                Settings.CsbEditor.BufferSize       = 4096;

                if (File.Exists("CsbEditor.exe.config"))
                {
                    XmlDocument xmlDoc= new XmlDocument();
                    xmlDoc.Load("CsbEditor.exe.config");
                    
                    XmlNodeList settings = xmlDoc.GetElementsByTagName("setting");
                    for (int i = 0; i < settings.Count; i++)
                    {
                        string value = settings[i].InnerText;
                        switch (settings[i].Attributes["name"].InnerText)
                        {
                            case "EnableThreading": Settings.CsbEditor.EnableThreading  = Boolean.Parse(value); break;
                            case "MaxThreads":      Settings.CsbEditor.MaxThreads       = Convert.ToInt32(value); break;
                            case "BufferSize":      Settings.CsbEditor.BufferSize       = Convert.ToInt32(value); break;
                        }
                    }
                }
            }

            public static void Save()
            {
                //AMBPatcher
                var text = new List<string> { };

                text.Add("ProgressBar="            + Convert.ToInt32(Settings.AMBPatcher.ProgressBar));
                text.Add("GenerateLog="            + Convert.ToInt32(Settings.AMBPatcher.GenerateLog));
                text.Add("SHACheck="               + Convert.ToInt32(Settings.AMBPatcher.SHACheck));
                text.Add("SHAType="                +                 Settings.AMBPatcher.SHAType);
                
                File.WriteAllLines("AMBPatcher.cfg", text);

                //CsbEditor
                XDocument xmlDoc = new XDocument(new XElement("configuration",
                                                    new XElement("configSections",
                                                        new XElement("sectionGroup",
                                                            new XAttribute("name", "userSettings"),
                                                            new XElement("section",
                                                                new XAttribute("name", "CsbEditor.Properties.Settings"),
                                                                new XAttribute("type", "System.Configuration.ClientSettingsSection")))),
                                                    new XElement("userSettings",
                                                        new XElement("CsbEditor.Properties.Settings",
                                                            new XElement("setting",
                                                                new XAttribute("name", "EnableThreading"),
                                                                new XAttribute("serializeAs", "String"),
                                                                new XElement("value", Settings.CsbEditor.EnableThreading)),
                                                            new XElement("setting",
                                                                new XAttribute("name", "MaxThreads"),
                                                                new XAttribute("serializeAs", "String"),
                                                                new XElement("value", Settings.CsbEditor.MaxThreads)),
                                                            new XElement("setting",
                                                                new XAttribute("name", "BufferSize"),
                                                                new XAttribute("serializeAs", "String"),
                                                                new XElement("value", Settings.CsbEditor.BufferSize))))));

                xmlDoc.Save("CsbEditor.exe.config");
            }
        }
        
        public SettingsForm()
        {
            InitializeComponent();
            Settings_Load();
            UpdateInstallationStatus();

            Settings.Load();
            Settings.Save();
        }

        private void UpdateInstallationStatus()
        {
            int status = MainForm.GetInstallationStatus();

            bInstall.Enabled = true;
            
            label5.Enabled =
            rb_rename.Enabled =
            rb_delete.Enabled =
            cb_recover_orig.Enabled = false;
            bInstall.Text = "Install";

            if (status == 1)
            {
                label_Installation_status.Text = "Installed";
                bInstall.Text = "Uninstall";

                label5.Enabled =
                rb_rename.Enabled =
                rb_delete.Enabled =
                cb_recover_orig.Enabled = true;
            }
            else if (status == 0 || status == -1)
            {
                label_Installation_status.Text = "Not installed";
            }
            else
            {
                if (status == -2)
                {
                    label_Installation_status.Text = "Current directory is not the game directory";
                    bInstall.Enabled = false;
                }
                else
                {
                    label_Installation_status.Text = "You have changed the .cfg file?";
                }
            }

            rb_rename.Checked = true;
            rb_delete.Checked = !rb_rename.Checked;
            cb_recover_orig.Checked = false;
        }
        
        private void ReadLicense(string program)
        {
            string files = "Mod Loader - licenses/";
            string license = "Mod Loader - licenses/";

            switch (program)
            {
                case "S4ML":    license += "LICENSE-Sonic4_ModLoader";
                                files   += "LICENSE-Sonic4_ModLoader_files"; break;
                
                case "SAT":     license += "LICENSE-SonicAudioTools";
                                files   += "LICENSE-SonicAudioTools_files"; break;

                case "7z":      license += "LICENSE-7-Zip";
                                files   += "LICENSE-7-Zip_files"; break;
            }

            if (files != "" && license != "")
                new LicenseReader(new string[] { files, license }).ShowDialog();
        }

        private void Settings_Save()
        {
            //AMBPatcher
            string AMBPatcher_progress_bar = Convert.ToInt32(cb_AMBPatcher_progress_bar.Checked).ToString();
            string AMBPatcher_generate_log = Convert.ToInt32(cb_AMBPatcher_generate_log.Checked).ToString();
            string AMBPatcher_sha_check    = Convert.ToInt32(cb_AMBPatcher_sha_check.Checked).ToString();
            string SHAType                 = list_SHAType.SelectedItem.ToString();

            string[] text = new string[]
            {
                "ProgressBar=" + AMBPatcher_progress_bar,
                "GenerateLog=" + AMBPatcher_generate_log,
                "SHACheck="    + AMBPatcher_sha_check,
                "SHAType="     + SHAType
            };

            File.WriteAllLines("AMBPatcher.cfg", text);
        }

        private void Settings_Load()
        {
            //AMBPatcher
            cb_AMBPatcher_progress_bar.Checked = true;
            cb_AMBPatcher_generate_log.Checked = false;
            cb_AMBPatcher_sha_check.Checked    = true;
            list_SHAType.SelectedIndex = 0;

            if (File.Exists("AMBPatcher.cfg"))
            {
                string[] cfg_file = File.ReadAllLines("AMBPatcher.cfg");
                    
                foreach (string line in cfg_file)
                {
                    if (!line.Contains("=")) {continue;}
                    string formatted_line = line.Substring(line.IndexOf("=") + 1);

                    if (line.StartsWith("ProgressBar="))
                    { cb_AMBPatcher_progress_bar.Checked = Convert.ToBoolean(Convert.ToInt32(formatted_line)); }
                        
                    else if (line.StartsWith("GenerateLog="))
                    { cb_AMBPatcher_generate_log.Checked = Convert.ToBoolean(Convert.ToInt32(formatted_line)); }
                        
                    else if (line.StartsWith("SHACheck="))
                    { cb_AMBPatcher_sha_check.Checked = Convert.ToBoolean(Convert.ToInt32(formatted_line)); }
                        
                    else if (line.StartsWith("SHAType="))
                    { 
                        if (new string[] {"256", "384", "512"}.Contains(formatted_line))
                        { list_SHAType.SelectedItem = formatted_line; }
                    }
                }
            }
        }

        ////////////////
        //Installation//
        ////////////////

        private void bInstall_Click(object sender, EventArgs e)
        {
            //Pro tip: imagine the binary representation of the integer
            int options = 0;

            options += Convert.ToInt32(cb_recover_orig.Checked);
            options += Convert.ToInt32(rb_delete.Checked)*2;

            if (bInstall.Text == "Install")
            {
                MainForm.Install(1);
            }
            else if (bInstall.Text == "Uninstall")
            {
                MainForm.Install(0, options);
            }
            UpdateInstallationStatus();
        }

        //////////////
        //AMBPatcher//
        //////////////

        /////////
        //About//
        /////////

        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(((Control)sender).Text);
        }
        
        private void bRL_S4ML_Click(object sender, EventArgs e)
        {
            ReadLicense("S4ML");
        }

        private void bRL_SAT_Click(object sender, EventArgs e)
        {
            ReadLicense("SAT");
        }

        private void bRL_7z_Click(object sender, EventArgs e)
        {
            ReadLicense("7z");
        }

        ////////
        //Main//
        ////////

        private void bOK_Click(object sender, System.EventArgs e)
        {
            Settings_Save();
            Close();
        }

        private void bCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void cb_AMBPatcher_sha_check_CheckedChanged(object sender, EventArgs e)
        {
            list_SHAType.Enabled = cb_AMBPatcher_sha_check.Checked;
        }
    }
}
