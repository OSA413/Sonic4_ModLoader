using System;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    public partial class WrongWorkingDirectory : Form
    {
        public WrongWorkingDirectory()
        {
            InitializeComponent();
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
