﻿using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;

namespace Sonic4ModManager
{
    public partial class Settings:Form
    {
        public Settings()
        {
            InitializeComponent();
            Settings_Load();
            UpdateInstallationStartus();
        }

        private void UpdateInstallationStartus()
        {
            int status = MainForm.GetInstallationStatus();
            if (status == 1)
            {
                label_Installation_status.Text = "Installed";
                bInstall.Text = "Uninstall";
                label5.Enabled = rb_rename.Enabled = rb_delete.Enabled = cb_recover_orig.Enabled = true;
            }
            else if (status == 0 || status == -1)
            {
                label_Installation_status.Text = "Not installed";
                bInstall.Text = "Install";
                label5.Enabled = rb_rename.Enabled = rb_delete.Enabled = cb_recover_orig.Enabled = false;
            }
            else
            {
                if (status == -2)
                {
                    label_Installation_status.Text = "Current directory is not the game directory";
                }
                else
                {
                    label_Installation_status.Text = "You have changed the .cfg file?";
                }
                bInstall.Text = "Install";
                bInstall.Enabled = label5.Enabled = rb_rename.Enabled = rb_delete.Enabled = cb_recover_orig.Enabled = false;
            }

            rb_rename.Checked = true;
            rb_delete.Checked = !rb_rename.Checked;
            cb_recover_orig.Checked = false;
        }
        
        private void ReadLicense(string program)
        {
            string files = "";
            string license = "";

            if (program == "S4ML")
            {
                license = "LICENSE-Sonic4_ModLoader";
                files   = "LICENSE-Sonic4_ModLoader_files";
            }
            else if (program == "SAT")
            {
                license = "LICENSE-SonicAudioTools";
                files   = "LICENSE-SonicAudioTools_files";
            }
            else if (program == "7z")
            {
                license = "LICENSE-7-Zip";
                files   = "LICENSE-7-Zip_files";
            }

            if (files != "" && license != "")
            {
                LicenseReader f = new LicenseReader(new string[] { files, license });
                f.ShowDialog();
            }
        }

        private void Settings_Save()
        {
            //AMBPatcher
            string AMBPatcher_progress_bar = Convert.ToInt32(cb_AMBPatcher_progress_bar.Checked).ToString();
            string AMBPatcher_generate_log = Convert.ToInt32(cb_AMBPatcher_generate_log.Checked).ToString();
            string AMBPatcher_sha_check    = Convert.ToInt32(cb_AMBPatcher_sha_check.Checked).ToString();
            string SHAType                 = list_SHAType.SelectedItem.ToString();

            string text = String.Join("\n", new string[]
            {
                "ProgressBar=" + AMBPatcher_progress_bar,
                "GenerateLog=" + AMBPatcher_generate_log,
                "SHACheck="    + AMBPatcher_sha_check,
                "SHAType="     + SHAType
            });

            File.WriteAllText("AMBPatcher.cfg", text);
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
                for (int j = 0; j < cfg_file.Length; j++)
                {
                    if (cfg_file[j].StartsWith("ProgressBar="))
                    {
                        cb_AMBPatcher_progress_bar.Checked = Convert.ToBoolean(Convert.ToInt32(String.Join("=", cfg_file[j].Split('=').Skip(1))));
                    }
                    else if (cfg_file[j].StartsWith("GenerateLog="))
                    {
                        cb_AMBPatcher_generate_log.Checked = Convert.ToBoolean(Convert.ToInt32(String.Join("=", cfg_file[j].Split('=').Skip(1))));
                    }
                    else if (cfg_file[j].StartsWith("SHACheck="))
                    {
                        cb_AMBPatcher_sha_check.Checked = Convert.ToBoolean(Convert.ToInt32(String.Join("=", cfg_file[j].Split('=').Skip(1))));
                    }
                    else if (cfg_file[j].StartsWith("SHAType="))
                    {
                        string tmp = String.Join("=", cfg_file[j].Split('=').Skip(1));
                        if (tmp == "256" || tmp == "384" || tmp == "512")
                        { list_SHAType.SelectedItem = tmp; }
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
            UpdateInstallationStartus();
        }

        //////////////
        //AMBPatcher//
        //////////////

        /////////
        //About//
        /////////

        private void linkMain_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/OSA413/Sonic4_ModLoader");
        }

        private void linkSAT_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/blueskythlikesclouds/SonicAudioTools");
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
