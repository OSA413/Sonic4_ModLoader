using System;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    public partial class UltimateWinForm:Form
    {
        public UltimateWinForm(string[] args)
        {
            InitializeComponent();

            //Dealing with arguments
            if (args.Length == 1)
            {
                //If this is not a 1-click installation call
                if (!(args[0].StartsWith("sonic4mmep1:") ||
                    args[0].StartsWith("sonic4mmep2:")))
                {
                    switch (args[0])
                    {
                        case "--install":   Reg.Install();   tabControl1.SelectTab(tabInstallation); break;
                        case "--uninstall": Reg.Uninstall(); tabControl1.SelectTab(tabInstallation); break;
                        case "--fix":       Reg.FixPath();   tabControl1.SelectTab(tabInstallation); break;
                    }
                }
                else
                {
                    lURL.Text = args[0].Substring(12).Split(',')[0];
                }
            }

            ////////////////////
            //Installation tab//
            ////////////////////

            lGameName.Text = "Sonic 4: " + GetGame.Full();
            if (Admin.AmI()) { label3.Text = ""; }
            
            ////////////////
            //Download tab//
            ////////////////

            UpdateWindow();
        }
        
        private void UpdateWindow()
        {
            int status = Reg.InstallationStatus();

            bInstall.Enabled =
            bUninstall.Enabled = true;

            switch(status)
            {
                case 0:
                    lInstallationStatus.Text = "Not installed";
                    bInstall.Text = "Install";
                    bUninstall.Enabled = false;
                    break;
                case 1:
                    lInstallationStatus.Text = "Installed";
                    bInstall.Enabled = false;
                    bInstall.Text = "Install";
                    break;
                case 2:
                    lInstallationStatus.Text = "Another installation present";
                    bInstall.Text = "Fix registry path";
                    break;
                case -1:
                    lInstallationStatus.Text = "Requires reinstallation";
                    bInstall.Text = "Install";
                    break;
            }
        }
        
        private void bInstall_Click(object sender, EventArgs e)
        {
            switch (Reg.InstallationStatus())
            {
                case 2:  Reg.FixPath(); break;
                default: Reg.Install(); break;
            }

            UpdateWindow();
        }

        private void bUninstall_Click(object sender, EventArgs e)
        {
            Reg.Uninstall();
            UpdateWindow();
        }
    }
}
