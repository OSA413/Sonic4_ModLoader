using System.Diagnostics;
using System.Windows.Forms;

namespace Sonic4ModManager
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void linkMain_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/OSA413/Sonic4_ModLoader");
        }

        private void linkSAT_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/blueskythlikesclouds/SonicAudioTools");
        }
    }
}
