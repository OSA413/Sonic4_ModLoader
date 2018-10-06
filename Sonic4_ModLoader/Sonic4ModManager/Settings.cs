using System;
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
                label5.Enabled = radioButton1.Enabled = radioButton2.Enabled = checkBox1.Enabled = true;
            }
            else if (status == 0 || status == -1)
            {
                label_Installation_status.Text = "Not installed";
                bInstall.Text = "Install";
                label5.Enabled = radioButton1.Enabled = radioButton2.Enabled = checkBox1.Enabled = false;
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
                bInstall.Enabled = label5.Enabled = radioButton1.Enabled = radioButton2.Enabled = checkBox1.Enabled = false;
            }

            radioButton1.Checked = true;
            radioButton2.Checked = !radioButton1.Checked;
            checkBox1.Checked = false;
        }
        
        private void ReadLicense(string program)
        {
            string files = "";
            string license = "";

            if (program == "S4ML")
            {
                files   = "LICENSE-Sonic4_ModLoader_files";
                license = "LICENSE-Sonic4_ModLoader";
            }
            else if (program == "SAT")
            {
                files   = "LICENSE-SonicAudioTools_files";
                license = "LICENSE-SonicAudioTools";
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

            string text = String.Join("\n", new string[]
            {
                "ProgressBar=" + AMBPatcher_progress_bar,
                "GenerateLog=" + AMBPatcher_generate_log
            });

            File.WriteAllText("AMBPatcher.cfg", text);
        }

        private void Settings_Load()
        {
            //AMBPatcher
            cb_AMBPatcher_progress_bar.Checked = true;
            cb_AMBPatcher_generate_log.Checked = false;

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
                }
            }
        }

        ////////////////
        //Installation//
        ////////////////

        private void bInstall_Click(object sender, EventArgs e)
        {
            if (bInstall.Text == "Install")
            {
                MainForm.Install(1);
            }
            else if (bInstall.Text == "Uninstall")
            {
                MainForm.Install(0);
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
        
        private void bRL_S4ML_Click(object sender, System.EventArgs e)
        {
            ReadLicense("S4ML");
        }

        private void bRL_SAT_Click(object sender, System.EventArgs e)
        {
            ReadLicense("SAT");
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
    }
}
