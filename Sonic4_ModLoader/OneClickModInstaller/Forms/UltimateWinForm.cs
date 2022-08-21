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
                mod.Local  = true;
                mod.FromDir= false;
                if (Directory.Exists(tbModURL.Text))
                    mod.FromDir  = true;
            }
            else if (tbModURL.Text.StartsWith("https://") || tbModURL.Text.StartsWith("http://"))
            {
                lDownloadTrying.Text = lDownloadTrying.Text = "You are trying to download a mod from {1}." + Environment.NewLine + "Aren't you?";
                lDownloadLink.Text  = "Download link:";
                mod.Link   = tbModURL.Text;
                mod.Local  = false;
                mod.FromDir= false;

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
                || mod.Status == ModInstallationStatus.Cancelled
                || mod.Status == ModInstallationStatus.Installed
                || mod.Status == ModInstallationStatus.ServerError)
            {
                tbModURL.ReadOnly   = true;
                bModPath.Enabled    =
                bModInstall.Enabled = false;
                StartInstallation();
            }
        }

        public void fake_DoTheRest(object sender, AsyncCompletedEventArgs e)
        {
            if (mod.Downloader.Total == -1 || mod.Downloader.Recieved == mod.Downloader.Total)
                DoTheRest();
            else
            {
                mod.Status = ModInstallationStatus.ServerError;
                statusBar.Text      = "Couldn't download full file (server error)";
                File.Delete(mod.ArchiveName);
                UpdateUI.ModInstallation();
            }
        }

        async public void StartInstallation()
        {
            await Task.Run((() =>
            {
                string archive_url = mod.Link;

                if (File.Exists(archive_url) || Directory.Exists(archive_url))
                {
                    if (mod.FromDir)
                        mod.ArchiveDir = archive_url;
                    else
                        mod.ArchiveName = archive_url;
                    DoTheRest();
                }
                else
                {
                    statusBar.Text = "Connecting to the server...";
                    mod.ArchiveName = mod.Downloader.Download(archive_url, fake_DoTheRest, wc_DownloadProgressChanged);
                }
            }));
        }

        async public void DoTheRest()
        {
            tcMain.Invoke(new MethodInvoker(delegate
            {
                progressBar.Style = ProgressBarStyle.Marquee;
                progressBar.Value = 0;
            }));

            await Task.Run(() =>
            {
                if (!mod.FromDir)
                {
                    statusBar.Text = "Extracting downloaded archive...";

                    mod.ArchiveDir = mod.ArchiveName + "_extracted";

                    if (File.Exists(mod.ArchiveName))
                    {
                        if (Directory.Exists(mod.ArchiveName + "_extracted"))
                            MyDirectory.DeleteRecursively(mod.ArchiveName + "_extracted");

                        if (Settings.UseLocal7zip)
                            ModArchive.Extract(mod.ArchiveName, Settings.Paths["7-Zip"]);
                        else
                            ModArchive.Extract(mod.ArchiveName);
                    }
                }

                var cont = -1;

                if (!Directory.Exists(mod.ArchiveDir))
                {
                    statusBar.Text = "Couldn't extract archive";
                    mod.Status = ModInstallationStatus.Downloaded;
                    if (!(File.Exists("7z.exe") || (File.Exists(Settings.Paths["7-Zip"]) && Settings.UseLocal7zip)))
                        statusBar.Text += " (7-Zip not found)";
                }
                else
                {
                    statusBar.Text = "Checking extracted files...";
                    cont = ModArchive.CheckFiles(mod.ArchiveDir);
                }
                
                if (cont == 0)
                {
                    MyDirectory.DeleteRecursively(mod.ArchiveDir);
                    statusBar.Text = "Installation was cancelled";
                    mod.Status = ModInstallationStatus.Cancelled;
                }
                else if (cont == 1)
                {
                    statusBar.Text = "Finding root directories...";

                    var FoundRootDirs = ModArchive.FindRoot(mod.ArchiveDir);
                    
                    if (FoundRootDirs.Item2 != ModType.Unknown)
                    {
                        mod.ModRoots = FoundRootDirs.Item1;
                        mod.Platform = FoundRootDirs.Item2;
                    }

                    mod.Status = ModInstallationStatus.Scanned;

                    tcMain.Invoke(new MethodInvoker(delegate
                    {
                        ContinueInstallation();
                    }));
                }
                UpdateUI.ModInstallation();
            });
        }

        public void ContinueInstallation()
        {
            progressBar.Style = ProgressBarStyle.Marquee;
            if (mod.Platform == ModType.ModLoader)
            {
                File.Delete(mod.ArchiveName);
                var result = MessageBox.Show("One-Click Mod Installer detected a Mod Loader distributive. Do you want to replace the current version of Mod Loader with the downloaded one?"
                                  , "Mod Loader update"
                                  , MessageBoxButtons.YesNo
                                  , MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Process.Start("Sonic4ModManager", "--upgrade \"" + mod.ArchiveDir + "\"");
                    Application.Exit();
                }
                else
                {
                    mod.Status = ModInstallationStatus.Cancelled;
                    statusBar.Text = "Installation was cancelled";
                }
            }
            else if (mod.Platform == ModType.Unknown)
            {
                //Status description
                /*  1 - start (open SelectRoots window)
                 *  2 - continue (choose destination directory)
                 *  0 - ok (user selected destination directory)
                 *  -1 - break (user cancelled SelectRoots window)
                 */
                var status = 1;
                var sr = new SelectRoots(mod.ArchiveDir);

                while (status > 0)
                {
                    if (status == 1)
                    {
                        status = -1;
                        if (sr.ShowDialog() == DialogResult.Yes)
                        {
                            status = 2;
                            mod.ModRoots = sr.output.ToArray();
                            if (mod.ModRoots.Length == 0)
                                status = -1;
                        }
                    }
                    else if (status == 2)
                    {
                        status = 1;
                        var path = MyDirectory.Select("test", "dir");
                        if (path != null)
                        {
                            mod.CustomPath = path;
                            status = 0;
                        }
                    }
                }

                if (status == -1)
                {
                    mod.Status = ModInstallationStatus.Cancelled;
                    statusBar.Text = "Installation was cancelled";
                    return;
                }
            }
            else if (mod.Platform == ModType.Mixed)
            {
                //TODO: think up a better explanation
                MessageBox.Show("Looks like this thing is complicated, try to install it manually. bye"
                              , "Couldn't install the mod"
                              , MessageBoxButtons.OK
                              , MessageBoxIcon.Exclamation);
                mod.Status = ModInstallationStatus.ModIsComplicated;
                return;
            }
            else
            {
                if (mod.ModRoots.Length > 1)
                {
                    var tmm_form = new TooManyMods(mod.ModRoots, mod.Platform);
                    tmm_form.ShowDialog();

                    mod.ModRoots = tmm_form.mods;
                }

                if (Settings.Paths.ContainsKey(mod.Platform.ToString()))
                {
                    if (Settings.Paths[mod.Platform.ToString()] == "")
                    {
                        /*tcMain.SelectTab(tabSettings);

                        MessageBox.Show("Looks like the mod you installed requires a special path to be installed to.\nPlease, specify "
                                      + mod.Platform + "\nand then press the \"Continue Installation\" button in the \"Download\" tab."
                                      , "Please specify a path"
                                      , MessageBoxButtons.OK
                                      , MessageBoxIcon.Information);
                        mod.Status = "Waiting for path";
                        UpdateUI.ModInstallation();*/
                        MessageBox.Show("That thing REALLY happened");
                    }
                }
            }
            
            if (mod.ModRoots.Length > 0)
                mod.LastMod = Path.GetFileName(mod.ModRoots[0]);

            if (mod.Status == ModInstallationStatus.Scanned)
                FinishInstallation();
        }

        async public void FinishInstallation()
        {
            tcMain.Invoke(new MethodInvoker(delegate
            {
                progressBar.Style = ProgressBarStyle.Marquee;
            }));
            await Task.Run(() =>
            {
                tcMain.Invoke(new MethodInvoker(delegate
                {
                    statusBar.Text = "Installing downloaded mod...";
                }));

                foreach (string mod in mod.ModRoots)
                {
                    string dest;
                    
                    switch (this.mod.Platform)
                    {
                        case ModType.PC:      dest = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "mods", Path.GetFileName(mod)); break;
                        case ModType.Dolphin: dest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Dolphin Emulator", "Load", "Textures"); break;
                        case ModType.Unknown: dest = this.mod.CustomPath; break;
                        default:              dest = Settings.Paths[this.mod.Platform.ToString()]; break;
                    }
                    
                    if (Directory.Exists(dest) && this.mod.Platform == ModType.PC)
                        MyDirectory.DeleteRecursively(dest);
                    
                    if (this.mod.Platform != ModType.Unknown)
                        MyDirectory.CopyAll(mod, dest);
                    else
                    {
                        if (File.Exists(Path.Combine(this.mod.ArchiveDir, mod)))
                            File.Copy(Path.Combine(this.mod.ArchiveDir, mod), Path.Combine(dest, Path.GetFileName(mod)));
                        else if (Directory.Exists(Path.Combine(this.mod.ArchiveDir, mod)))
                            MyDirectory.CopyAll(Path.Combine(this.mod.ArchiveDir, mod), Path.Combine(dest, Path.GetFileName(mod)));
                    }
                }

                if (!mod.FromDir)
                    MyDirectory.DeleteRecursively(mod.ArchiveDir);

                if (File.Exists(mod.ArchiveName) && !mod.Local)
                {
                    if (Settings.SaveDownloadedArchives)
                    {
                        Directory.CreateDirectory(Settings.Paths["DownloadedArhives"]);
                        File.Move(mod.ArchiveName, Path.Combine(Settings.Paths["DownloadedArhives"], mod.ArchiveName));
                    }
                    else
                        File.Delete(mod.ArchiveName);
                }

                if (Settings.ExitLaunchManager)
                {
                    if (mod.Platform == ModType.PC && File.Exists("Sonic4ModManager.exe"))
                        Process.Start("Sonic4ModManager.exe", "\"" + mod.LastMod + "\"");
                    Environment.Exit(0);
                }

                tcMain.Invoke(new MethodInvoker(delegate
                {
                    statusBar.Text = "Mod installation complete!";
                    mod.Status = ModInstallationStatus.Installed;
                }));
                
                tcMain.Invoke(new MethodInvoker(delegate {
                    UpdateUI.ModInstallation();
                }));
            });
        }

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
