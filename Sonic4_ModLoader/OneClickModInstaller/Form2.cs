using System;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    public partial class Form2 : Form
    {
        public Form2()
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
