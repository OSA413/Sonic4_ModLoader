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
                public static bool ProgressBar;
                public static bool GenerateLog;
                public static bool SHACheck;
                public static int  SHAType;
            }

            public static class CsbEditor
            {
                public static bool  EnableThreading;
                public static int   MaxThreads;
                public static int   BufferSize;
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

        private void Settings_Load()
        {
            Settings.Load();

            //AMBPatcher
            cb_AMBPatcher_progress_bar.Checked = Settings.AMBPatcher.ProgressBar;
            cb_AMBPatcher_generate_log.Checked = Settings.AMBPatcher.GenerateLog;
            cb_AMBPatcher_sha_check.Checked    = Settings.AMBPatcher.SHACheck;
            list_SHAType.SelectedIndex = 0;

            if (new string[] { "1", "256", "384", "512" }.Contains(Settings.AMBPatcher.SHAType.ToString()))
                list_SHAType.SelectedItem = Settings.AMBPatcher.SHAType;

            //CsbEditor
            num_CsbEditor_BufferSize.Value          = Settings.CsbEditor.BufferSize;
            cb_CsbEditor_EnableThreading.Checked    = Settings.CsbEditor.EnableThreading;
            num_CsbEditor_MaxThreads.Value          = Settings.CsbEditor.MaxThreads;
        }

        private void Settings_Save()
        {
            //AMBPatcher
            Settings.AMBPatcher.ProgressBar = cb_AMBPatcher_progress_bar.Checked;
            Settings.AMBPatcher.GenerateLog = cb_AMBPatcher_generate_log.Checked;
            Settings.AMBPatcher.SHACheck    = cb_AMBPatcher_sha_check.Checked;
            Settings.AMBPatcher.SHAType     = Convert.ToInt32(list_SHAType.SelectedItem);

            //CsbEditor
            Settings.CsbEditor.BufferSize       = (int)num_CsbEditor_BufferSize.Value;
            Settings.CsbEditor.EnableThreading  = cb_CsbEditor_EnableThreading.Checked;
            Settings.CsbEditor.MaxThreads       = (int)num_CsbEditor_MaxThreads.Value;

            Settings.Save();
        }
        
        public SettingsForm()
        {
            InitializeComponent();
            UpdateInstallationStatus();
            Settings_Load();
        }

        private void UpdateInstallationStatus()
        {
            int status = Installation.GetInstallationStatus();
            bool force_uninstall = cb_ForceUninstall.Checked;
            
            label5.Enabled =
            rb_rename.Enabled =
            rb_delete.Enabled =
            cb_Uninstall_OCMI.Enabled =
            cb_KeepSettings.Enabled =
            cb_recover_orig.Enabled = false;
            bInstall.Text = "Install";

            rb_rename.Checked = true;
            rb_delete.Checked = false;
            cb_recover_orig.Checked     =
            cb_KeepSettings.Checked     =
            cb_Uninstall_OCMI.Checked   = false;
            bInstall.Enabled = false;
            
            if (status == 1 || force_uninstall)
            {
                bInstall.Text = "Uninstall";
                rb_rename.Enabled =
                rb_delete.Enabled =
                label5.Enabled =
                cb_recover_orig.Enabled = true;
                bInstall.Enabled        = true;
                if (force_uninstall)
                {
                    rb_rename.Enabled =
                    rb_rename.Checked = false;
                    rb_delete.Checked = true;
                }
            }
            else if (status == 0 || status == -1)
            {
                bInstall.Enabled        = true;
            }

            string the_text = "Current directory is not the game directory";
            switch (status)
            {
                case  1: the_text = "Installed"; break;
                case  0:
                case -1: the_text = "Not installed"; break;
            }

            label_Installation_status.Text = the_text;
        }
        
        private void ReadLicense(string program)
        {
            string license = "Mod Loader - licenses/";

            switch (program)
            {
                case "S4ML":    license += "LICENSE-Sonic4_ModLoader";  break;
                case "SAT":     license += "LICENSE-SonicAudioTools";   break;
                case "7z":      license += "LICENSE-7-Zip";             break;
            }

            if (license != "Mod Loader - licenses/")
                new LicenseReader(license).ShowDialog();
        }
        
        ////////////////
        //Installation//
        ////////////////

        private void bInstall_Click(object sender, EventArgs e)
        {
            //Pro tip: imagine the binary representation of this integer
            int options = 0;

            //Recover original files
            options += Convert.ToInt32(cb_recover_orig.Checked);
            //Delete all Mod Loader files
            options += Convert.ToInt32(rb_delete.Checked)*2;
            //Uninstall and delete OCMI
            options += Convert.ToInt32(cb_Uninstall_OCMI.Checked)*4 * Convert.ToInt32(rb_delete.Checked);
            //Keep settings (affects OCMI removal)
            options += Convert.ToInt32(cb_KeepSettings.Checked)*8 * Convert.ToInt32(rb_delete.Checked);
            //Delete Mod Manager
            options += Convert.ToInt32(rb_delete.Checked)*16;
            
            if (bInstall.Text == "Install")
                Installation.Install(1);
            else if (bInstall.Text == "Uninstall")
                Installation.Install(0, options);

            UpdateInstallationStatus();
        }

        //////////////
        //AMBPatcher//
        //////////////

        private void cb_AMBPatcher_sha_check_CheckedChanged(object sender, EventArgs e)
        {
            list_SHAType.Enabled = cb_AMBPatcher_sha_check.Checked;
        }

        /////////////
        //CsbEditor//
        /////////////

        private void cb_CsbEditor_EnableThreading_CheckedChanged(object sender, EventArgs e)
        {
            num_CsbEditor_MaxThreads.Enabled = cb_CsbEditor_EnableThreading.Checked;
        }

        /////////
        //About//
        /////////
        
        private void ReadLicense_Click(object sender, EventArgs e)
        {
            ReadLicense(((Control)sender).Name.Substring(4));
        }

        //////////
        //Common//
        //////////

        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(((Control)sender).Text);
        }

        private void bOK_Click(object sender, System.EventArgs e)
        {
            Settings_Save();
        }

        private void bRecoverOriginalFiles_Click(object sender, EventArgs e)
        {
            if (File.Exists("AMBPatcher.exe"))
            {
                Process.Start("AMBPatcher.exe", "recover").WaitForExit();

                if (File.Exists("mods/mods_prev"))
                    File.Delete("mods/mods_prev");

                if (Directory.Exists("mods_sha"))
                    Directory.Delete("mods_sha", true);
            }
        }
        
        private void rb_delete_CheckedChanged(object sender, EventArgs e)
        {
            cb_Uninstall_OCMI.Enabled =
            cb_KeepSettings.Enabled = rb_delete.Checked;
        }

        private void cb_ForceUninstall_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInstallationStatus();
        }
    }
}
