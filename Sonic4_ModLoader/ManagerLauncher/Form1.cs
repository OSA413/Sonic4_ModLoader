using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ManagerLauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            bConf.Select();
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            //Episode 1
            if (File.Exists("Sonic_vis.exe"))
            {
                Process.Start("Sonic_vis.exe");
            }
            //Episode 2
            else if (File.Exists("Sonic.exe"))
            {
                Process.Start("Sonic.exe");
            }
            Application.Exit();
        }

        private void bConf_Click(object sender, EventArgs e)
        {
            //Episode 1
            if (File.Exists("SonicLauncher.orig.exe"))
            {
                Process.Start("SonicLauncher.orig.exe");
            }
            //Episode 2
            else if (File.Exists("Launcher.orig.exe"))
            {
                Process.Start("Launcher.orig.exe");
            }
            Application.Exit();
        }

        private void bManager_Click(object sender, EventArgs e)
        {
            Process.Start("Sonic4ModManager.exe");
            Application.Exit();
        }
    }
}
