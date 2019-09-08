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

            richTextBox1.Text = "====================\n"
                                + license_file
                                + "\n====================\n\n"
                                + license;
        }
        
        private void LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }
    }
}
