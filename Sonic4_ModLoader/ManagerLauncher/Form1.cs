using System;
using System.Windows.Forms;

using Common.Launcher;

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
            if (Launcher.LaunchGame())
                Application.Exit();
        }

        private void bConf_Click(object sender, EventArgs e)
        {
            if (Launcher.LaunchConfig())
                Application.Exit();
        }

        private void bManager_Click(object sender, EventArgs e)
        {
            if (Launcher.LaunchModManager())
                Application.Exit();
        }
    }
}
