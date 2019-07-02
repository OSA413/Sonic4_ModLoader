using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    public partial class DFtEM : Form
    {
        public DFtEM()
        {
            InitializeComponent();
        }

        private void bOpen_Click(object sender, EventArgs e)
        {
            if (File.Exists("Sonic4ModManager.exe"))
            {
                Process.Start("Sonic4ModManager.exe", "\"" + UltimateWinForm.last_mod + "\"");
                Close();
            }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
