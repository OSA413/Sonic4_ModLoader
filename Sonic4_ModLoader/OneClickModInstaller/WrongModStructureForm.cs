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
    public partial class WrongModStructureForm : Form
    {
        public WrongModStructureForm()
        {
            InitializeComponent();
        }
        
        private void bExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bOpen_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "explorer",
                Arguments = "extracted_mod"
            };
            Process.Start(startInfo);
            Application.Exit();
        }
    }
}
