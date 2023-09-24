using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Diagnostics;

using Common.MyIO;
using Common.URL;
using Common.Launcher;

namespace OneClickModInstaller
{
    public partial class UltimateWinForm:Form
    {
        public static HandlerInstallerWrapper hiWrapper = Program.hiWrapper;
        public ModInstallationInstance mod;
        public Downloader Downloader = new();

        public UltimateWinForm()
        {
            InitializeComponent();
            mod = new (ArgsHandler.ModArgs);
            UpdateUI.AttachForm(this);
            UpdateUI.Initial();
            UpdateUI.Settings();
            UpdateUI.CurrentGame();
            UpdateUI.GlobalGameStatus();
        }

        private void PrepareInstallation()
        {
            if (File.Exists(tbModURL.Text) || Directory.Exists(tbModURL.Text))
            {
                lDownloadTrying.Text = lDownloadTrying.Text = "You are trying to install a mod from hard drive." + Environment.NewLine + "Aren't you?";
                lDownloadLink.Text = "Path to the mod:";
                mod.Link   = tbModURL.Text;
            }
            else if (tbModURL.Text.StartsWith("https://") || tbModURL.Text.StartsWith("http://"))
            {
                lDownloadTrying.Text = lDownloadTrying.Text = "You are trying to download a mod from {1}." + Environment.NewLine + "Aren't you?";
                lDownloadLink.Text  = "Download link:";
                mod.Link   = tbModURL.Text;

                if (mod.Link.Contains("gamebanana.com"))
                    lDownloadTrying.Text = lDownloadTrying.Text.Replace("{1}", "GameBanana");
                else if (mod.Link.Contains("github.com"))
                    lDownloadTrying.Text = lDownloadTrying.Text.Replace("{1}", "GitHub");
                else
                    lDownloadTrying.Text = lDownloadTrying.Text.Replace("{1}", "the Internet");
            }
        }

        private void UnInstallAndUpdateStatus(Action<GAME?> d, GAME? game = null)
        {
            d(game);
            UpdateUI.CurrentGame();
            UpdateUI.GlobalGameStatus();
        }
        private void bInstall_Click(object sender, EventArgs e) => UnInstallAndUpdateStatus(hiWrapper.Install);
        private void bUninstall_Click(object sender, EventArgs e) => UnInstallAndUpdateStatus(hiWrapper.Uninstall);

        private void bModInstall_Click(object sender, EventArgs e)
        {
            if (mod.Status == ModInstallationStatus.Beginning
                || mod.Cancelled == true
                || mod.Status == ModInstallationStatus.Installed
                || mod.Status == ModInstallationStatus.ServerError)
            {
                tbModURL.ReadOnly   = true;
                bModPath.Enabled    =
                bModInstall.Enabled = false;
                mod.Status = ModInstallationStatus.Beginning;

                while (DoNextStep())
                {
                    UpdateUI.Status(mod.Status.ToString());
                }
            }
        }

        public void ContinueAFterDownload(object o, AsyncCompletedEventArgs e)
        {
            mod.Status = ModInstallationStatus.Downloaded;
            while (DoNextStep())
            {
                UpdateUI.Status(mod.Status.ToString());
            }
        }

        public bool Download()
        {
            Downloader.Download(mod.Link, ContinueAFterDownload, wc_DownloadProgressChanged);
            return false;
        }

        public bool DoNextStep() => mod.Status switch
        {
            ModInstallationStatus.Beginning => mod.Prepare(),
            ModInstallationStatus.Downloading => Download(),
            ModInstallationStatus.Downloaded => mod.ExtractMod(),
            ModInstallationStatus.Extracted => mod.FindRoots(),
            ModInstallationStatus.Scanned => mod.InstallFromModRoots(),
            ModInstallationStatus.ServerError => false,
            ModInstallationStatus.Extracting => false,
            ModInstallationStatus.Scanning => false,
            ModInstallationStatus.Installing => false,
            ModInstallationStatus.Installed => false,
            ModInstallationStatus.ModIsComplicated => false,
            _ => false,
        };

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            string unit;
            int divider;
            long total;

            mod.Downloader.Recieved   = e.BytesReceived;
            mod.Downloader.Total      = e.TotalBytesToReceive;
            
            total = e.TotalBytesToReceive;
            if (e.TotalBytesToReceive == -1)
                total = e.BytesReceived;

            if (total > 1024 * 1024 * 16)
            { unit = "MBs"; divider = 1024 * 1024; }
            else if (total > 1024 * 16)
            { unit = "KBs"; divider = 1024; }
            else
            { unit = "Bytes"; divider = 1; }

            //Sometimes invoke is required, sometimes itn't.
            tcMain.Invoke(new MethodInvoker(delegate {
                //Yep, sometimes TotalBytesToReceive equals -1
                if (e.TotalBytesToReceive == -1)
                {
                    statusBar.Text = "Downloading... (" + e.BytesReceived / divider + unit + ")";
                    progressBar.Style = ProgressBarStyle.Marquee;
                }
                else
                {
                    statusBar.Text = "Downloading... (" + e.BytesReceived / divider + " / " + e.TotalBytesToReceive / divider + " " + unit + ")";
                    progressBar.Style = ProgressBarStyle.Blocks;
                    progressBar.Value = (int)(1000 * e.BytesReceived / e.TotalBytesToReceive);
                }
            }));
        }

        private void VisitIfInstalled(GAME game)
        {
            var status = hiWrapper.GetInstallationStatus(game);
            if (status.Status == InstallationStatus.NotInstalled) return;
            MyDirectory.OpenInFileManager(status.Location);
        }
        private void bIOEp1Visit_Click(object sender, EventArgs e) => VisitIfInstalled(GAME.Episode1);
        private void bIOEp2Visit_Click(object sender, EventArgs e) => VisitIfInstalled(GAME.Episode2);

        private void Uninstall(GAME game)
        {
            Program.hiWrapper.Uninstall(game);
            UpdateUI.GlobalGameStatus();
        }
        private void bIOEp1Uninstall_Click(object sender, EventArgs e) => Uninstall(GAME.Episode1);
        private void bIOEp2Uninstall_Click(object sender, EventArgs e) => Uninstall(GAME.Episode2);

        private void cbUseLocal7zip_CheckedChanged(object sender, EventArgs e)
        {
            tbPath7z.Enabled =
            bPath7z.Enabled  = cbUseLocal7zip.Checked;
            fake_SettingsSave(sender, e);
        }

        private void bPath7z_Click(object sender, EventArgs e)
        {
            var path = MyDirectory.Select("7z.exe", "7z");
            if (path != null)
                tbPath7z.Text = path ?? tbPath7z.Text;
        }

        private void bPathDownloadedArchives_Click(object sender, EventArgs e)
        {
            var path = MyDirectory.Select("directory where you want to save downloaded archives", "dir");
            if (path != null)
                tbDownloadedArchiveLocation.Text = path;
        }

        private void fake_SettingsSave(object sender, EventArgs e)
        {
            switch (((Control)sender).Name)
            {
                case "cbUseLocal7zip":              Settings.UseLocal7zip               = cbUseLocal7zip.Checked;           break;
                case "chSaveDownloadedArchives":    Settings.SaveDownloadedArchives     = chSaveDownloadedArchives.Checked; break;
                case "cbExitLaunchManager":         Settings.ExitLaunchManager          = cbExitLaunchManager.Checked;      break;
                case "tbPath7z":                    Settings.Paths["7-Zip"]             = tbPath7z.Text;                    break;
                case "tbDownloadedArchiveLocation": Settings.Paths["DownloadedArhives"] = tbDownloadedArchiveLocation.Text; break;
            }
            Settings.Save();
        }
        
        private void tbModURL_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(tbModURL.Text)
                || Directory.Exists(tbModURL.Text)
                || tbModURL.Text.StartsWith("https://")
                || tbModURL.Text.StartsWith("http://"))
            {
                bModInstall.Enabled = true;
                if (mod.FromArgs)
                {
                    tbModURL.ReadOnly   = true;
                    bModPath.Enabled    = false;
                }
            }
            else
                bModInstall.Enabled = false;
            PrepareInstallation();
        }

        private void bModPath_Click(object sender, EventArgs e)
        {
            var path = MyDirectory.Select("mod", "arc/dir");
            if (path != null)
                tbModURL.Text = path;
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(((Control)sender).Text);
    }
}
