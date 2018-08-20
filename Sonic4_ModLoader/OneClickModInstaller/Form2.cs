using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
