using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Sonic4ModManager
{
    public partial class LicenseReader:Form
    {
        public LicenseReader(string license_file)
        {
            InitializeComponent();

            string license = "File \"" + license_file + "\" not found.";

            if (File.Exists(license_file))
                license = File.ReadAllText(license_file);

            string source_link = "";
            if (File.Exists(license_file + "_link"))
                source_link = File.ReadAllText(license_file + "_link");

            richTextBox1.Text = "====================\n"
                                + license_file
                                + "\n====================\n\n"
                                + license;

            if (source_link != "")
                richTextBox1.Text = "====================\n"
                                    + "You can get the source code and binary files of this program at\n"
                                    + source_link
                                    + "====================\n\n"
                                    + richTextBox1.Text;
        }
        
        private void LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }
    }
}
