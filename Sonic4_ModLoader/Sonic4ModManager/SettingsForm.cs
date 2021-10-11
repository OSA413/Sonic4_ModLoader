using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

using Common.Ini;
using Common.ValueUpdater;

namespace Sonic4ModManager
{
    public partial class SettingsForm:Form
    {
        public static class Settings
        {
            public static class AMBPatcher
            {
                public static bool ProgressBar;
                public static bool SHACheck;
            }

            public static class CsbEditor
            {
                public static bool  EnableThreading;
                public static int   MaxThreads;
                public static int   BufferSize;
            }
            
            public static void Load()
            {
                //AMBPatcher
                AMBPatcher.ProgressBar = true;
                AMBPatcher.SHACheck    = true;

                var cfg = IniReader.Read("AMBPatcher.cfg");
                if (cfg.ContainsKey(IniReader.DEFAULT_SECTION))
                {
                    ValueUpdater.UpdateIfKeyPresent(cfg, "ProgressBar", ref AMBPatcher.ProgressBar);
                    ValueUpdater.UpdateIfKeyPresent(cfg, "SHACheck", ref AMBPatcher.SHACheck);
                }

                //CsbEditor//
                CsbEditor.EnableThreading  = true;
                CsbEditor.MaxThreads       = 4;
                CsbEditor.BufferSize       = 4096;

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
                            case "EnableThreading": CsbEditor.EnableThreading  = Boolean.Parse(value); break;
                            case "MaxThreads":      CsbEditor.MaxThreads       = Convert.ToInt32(value); break;
                            case "BufferSize":      CsbEditor.BufferSize       = Convert.ToInt32(value); break;
                        }
                    }
                }
            }

            public static void Save()
            {
                //AMBPatcher
                var text = new List<string> { };

                text.Add("ProgressBar=" + Convert.ToInt32(AMBPatcher.ProgressBar));
                text.Add("SHACheck="    + Convert.ToInt32(AMBPatcher.SHACheck));
                
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

            cb_AMBPatcher_progress_bar.Checked = Settings.AMBPatcher.ProgressBar;
            cb_AMBPatcher_sha_check.Checked    = Settings.AMBPatcher.SHACheck;

            num_CsbEditor_BufferSize.Value          = Settings.CsbEditor.BufferSize;
            cb_CsbEditor_EnableThreading.Checked    = Settings.CsbEditor.EnableThreading;
            num_CsbEditor_MaxThreads.Value          = Settings.CsbEditor.MaxThreads;
        }

        private void Settings_Save()
        {
            Settings.AMBPatcher.ProgressBar = cb_AMBPatcher_progress_bar.Checked;
            Settings.AMBPatcher.SHACheck    = cb_AMBPatcher_sha_check.Checked;

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
            var status = Installation.GetInstallationStatus();
            bool force_uninstall = cb_ForceUninstall.Checked;
            
            label5.Enabled =
            rb_rename.Enabled =
            rb_delete.Enabled =
            cbKeepConfigs.Enabled =
            cb_Uninstall_OCMI.Enabled =
            cb_recover_orig.Enabled = false;
            bInstall.Text = "Install";

            rb_rename.Checked = true;
            rb_delete.Checked = false;
            cb_recover_orig.Checked     =
            cb_Uninstall_OCMI.Checked   = false;
            bInstall.Enabled = false;
            
            if (status == Installation.Status.Installed || force_uninstall)
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
            else if (status == Installation.Status.NotInstalled || status == Installation.Status.FirstLaunch)
                bInstall.Enabled = true;

            var statusText = "Current directory is not the game directory";
            switch (status)
            {
                case Installation.Status.Installed: statusText = "Installed"; break;
                case Installation.Status.NotInstalled:
                case Installation.Status.FirstLaunch: statusText = "Not installed"; break;
            }

            label_Installation_status.Text = statusText;
        }
        
        private void ReadLicense(string program)
        {
            var license = "";

            switch (program)
            {
                case "S4ML": license = "LICENSE-Sonic4_ModLoader"; break;
                case "AML": license = "LICENSE-AliceModLoader"; break;
                case "SAT": license = "LICENSE-SonicAudioTools"; break;
                case "7z": license = "LICENSE-7-Zip"; break;
            }

            if (license != "")
                new LicenseReader(license).ShowDialog();
        }
        
        private void bInstall_Click(object sender, EventArgs e)
        {
            var options = new Installation.UninstallationOptions();

            options.RecoverOriginalFiles = cb_recover_orig.Checked;
            options.DeleteAllModLoaderFiles = rb_delete.Checked;
            options.KeepSettings = cbKeepConfigs.Checked;
            options.UninstallAndDeleteOCMI = cb_Uninstall_OCMI.Checked && options.DeleteAllModLoaderFiles;
            
            if (bInstall.Text == "Install")
                Installation.Install();
            else if (bInstall.Text == "Uninstall")
                Installation.Uninstall(options);

            UpdateInstallationStatus();
        }

        private void cb_CsbEditor_EnableThreading_CheckedChanged(object sender, EventArgs e) =>
            num_CsbEditor_MaxThreads.Enabled = label6.Enabled = cb_CsbEditor_EnableThreading.Checked;

        private void ReadLicense_Click(object sender, EventArgs e) =>
            ReadLicense(((Control)sender).Name.Substring(4));

        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) =>
            Process.Start(((Control)sender).Text);
        private void bOK_Click(object sender, EventArgs e) => Settings_Save();
        private void bRecoverOriginalFiles_Click(object sender, EventArgs e)
        {
            if (File.Exists("AMBPatcher.exe"))
            {
                Process.Start("AMBPatcher.exe", "recover").WaitForExit();
                if (File.Exists("mods/mods_prev")) File.Delete("mods/mods_prev");
                if (Directory.Exists("mods_sha")) Directory.Delete("mods_sha", true);
            }
        }
        
        private void rb_delete_CheckedChanged(object sender, EventArgs e) =>
            cb_Uninstall_OCMI.Enabled = cbKeepConfigs.Enabled = rb_delete.Checked;

        private void cb_ForceUninstall_CheckedChanged(object sender, EventArgs e) =>
            UpdateInstallationStatus();
    }
}
