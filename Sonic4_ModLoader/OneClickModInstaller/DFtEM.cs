using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Process.Start("Sonic4ModManager.exe");
            Close();
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
