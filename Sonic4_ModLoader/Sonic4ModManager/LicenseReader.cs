using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Sonic4ModManager
{
    public partial class LicenseReader:Form
    {
        public LicenseReader(string licenseFile)
        {
            InitializeComponent();

            licenseFile = Path.Combine("Mod Loader - licenses", licenseFile);
            var license = "File \"" + licenseFile + "\" not found.";
            if (File.Exists(licenseFile))
                license = File.ReadAllText(licenseFile);

            var sourceLink = "";
            if (File.Exists(licenseFile + "_link"))
                sourceLink = File.ReadAllText(licenseFile + "_link");

            richTextBox1.Text = "====================\n"
                                + licenseFile
                                + "\n====================\n\n"
                                + license;

            if (sourceLink != "") richTextBox1.Text = "====================\n"
                                    + "You can get source code and/or binary files of this program at\n"
                                    + sourceLink
                                    + "====================\n\n"
                                    + richTextBox1.Text;
        }
        
        private void LinkClicked(object sender, LinkClickedEventArgs e) => Process.Start(e.LinkText);
    }
}
