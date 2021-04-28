using System;
using System.Windows.Forms;

namespace Sonic4ModManager
{
    public partial class FirstLaunch : Form
    {
        public FirstLaunch() => InitializeComponent();
        private void bYes_Click(object sender, EventArgs e) => Installation.Install(1);
        private void bNo_Click(object sender, EventArgs e) => Settings.Save();
    }
}
