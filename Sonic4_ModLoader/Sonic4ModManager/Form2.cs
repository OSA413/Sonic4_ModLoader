using System;
using System.IO;
using System.Windows.Forms;

namespace Sonic4ModManager
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            bIDUNNO.Select();
        }

        private void bYes_Click(object sender, EventArgs e)
        {
            //Episode 1
            if (File.Exists("Sonic_vis.exe"))
            {
                if (File.Exists("PatchLauncher.exe"))
                {
                    File.Move("Sonic_vis.exe", "Sonic_vis.orig.exe");
                    File.Move("PatchLauncher.exe", "Sonic_vis.exe");
                }
                if (File.Exists("ManagerLauncher.exe") && File.Exists("SonicLauncher.exe"))
                {
                    File.Move("SonicLauncher.exe", "SonicLauncher.orig.exe");
                    File.Move("ManagerLauncher.exe", "SonicLauncher.exe");
                }
            }
            //Episode 2
            else if (File.Exists("Sonic.exe"))
            {
                if (File.Exists("PatchLauncher.exe"))
                {
                    File.Move("Sonic.exe", "Sonic.orig.exe");
                    File.Move("PatchLauncher.exe", "Sonic.exe");
                }
                if (File.Exists("ManagerLauncher.exe") && File.Exists("Launcher.exe"))
                {
                    File.Move("Launcher.exe", "Launcher.orig.exe");
                    File.Move("ManagerLauncher.exe", "Launcher.exe");
                }
            }
            File.Create("mod_manager.cfg");
            Close();
        }

        private void bNo_Click(object sender, EventArgs e)
        {
            File.Create("mod_manager.cfg");
            Close();
        }

        private void bIDUNNO_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
