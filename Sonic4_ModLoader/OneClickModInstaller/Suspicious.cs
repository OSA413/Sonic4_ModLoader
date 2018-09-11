using System;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    public partial class Suspicious : Form
    {
        public Suspicious()
        {
            InitializeComponent();
        }

        private void bContinue_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bDelContinue_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
