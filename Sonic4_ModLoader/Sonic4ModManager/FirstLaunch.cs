using System;
using System.IO;
using System.Windows.Forms;

namespace Sonic4ModManager
{
    public partial class FirstLaunch : Form
    {
        public FirstLaunch()
        {
            InitializeComponent();
        }

        private void bYes_Click(object sender, EventArgs e)
        {
            MainForm.Install(1);
        }

        private void bNo_Click(object sender, EventArgs e)
        {
            File.WriteAllText("mod_manager.cfg", "0");
        }
    }
}
