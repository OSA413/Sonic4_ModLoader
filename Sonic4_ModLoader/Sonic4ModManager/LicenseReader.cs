using System;
using System.IO;
using System.Windows.Forms;

namespace Sonic4ModManager
{
    public partial class LicenseReader:Form
    {
        public LicenseReader(string[] args)
        {
            InitializeComponent();

            string files = "File \"" + args[0] + "\" not found.";
            string license = "File \"" + args[1] + "\" not found.";

            if (File.Exists(args[0]))
            {
                files = "This license applies to the following files:";
                foreach (string f in File.ReadLines(args[0]))
                {
                    files += "\n • " + f;
                }
            }
            if (File.Exists(args[1]))
            {
                license = File.ReadAllText(args[1]);
            }

            string text = "====================\n" + args[0] + "\n====================\n\n" + files +
                            "\n\n====================\n" + args[1] + "\n====================\n\n" + license;

            richTextBox1.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
