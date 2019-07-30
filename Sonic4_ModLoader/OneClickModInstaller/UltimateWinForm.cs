using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace OneClickModInstaller
{
    public partial class UltimateWinForm:Form
    {
        //This class stores variables related to mod installation
        public static class Installation
        {
            public static string    Link        { set; get; }
            public static string    ServerHost  { set; get; }
            public static string    ArchiveName { set; get; }
            public static string    ArchiveDir  { set; get; }
            public static bool      Local       { set; get; }
            public static string    LastMod     { set; get; }
            public static string[]  ModRoots    { set; get; }
            public static string    Platform    { set; get; }
            public static string    Status      { set; get; }
            public static bool      FromArgs    { set; get; }
            public static string    CustomPath  { set; get; }
            public static bool      FromDir     { set; get; }
            //Sometimes server may break connection when file is not fully downloaded
            //User will be offered to redownload it
            public static long      Recieved    { set; get; }
            public static long      Total       { set; get; }
        }

        public static class Settings
        {
            public static bool UseLocal7zip             { set; get; }
            public static bool SaveDownloadedArchives   { set; get; }
            public static bool ExitLaunchManager        { set; get; }

            public static Dictionary<string, string> Paths { set; get; }

            public static void Load()
            {
                //Defaults
                Settings.UseLocal7zip           = false;
                Settings.SaveDownloadedArchives = false;
                Settings.ExitLaunchManager      = true;

                Settings.Paths = new Dictionary<string, string>();
                Settings.Paths.Add("CheatTables", "");
                Settings.Paths.Add("7-Zip", "");
                Settings.Paths.Add("DownloadedArhives", "mods_downloaded");

                if (File.Exists("OneClickModInstaller.cfg"))
                {
                    string[] cfg_file = File.ReadAllLines("OneClickModInstaller.cfg");

                    foreach (string line in cfg_file)
                    {
                        if (!line.Contains("=")) continue;
                        string key   = line.Substring(0, line.IndexOf("="));
                        string value = line.Substring(line.IndexOf("=") + 1);

                        //Paths
                        if (Settings.Paths.ContainsKey(key))
                            Settings.Paths[key] = value.Replace("\\", "/");

                        //Booleans
                        else if (key == "UseLocal7zip")
                            Settings.UseLocal7zip = Convert.ToBoolean(Convert.ToInt32(value));

                        else if (key == "SaveDownloadedArchives")
                            Settings.SaveDownloadedArchives = Convert.ToBoolean(Convert.ToInt32(value));

                        else if (key == "ExitLaunchManager")
                            Settings.ExitLaunchManager = Convert.ToBoolean(Convert.ToInt32(value));
                    }
                }
            }

            public static void Save()
            {
                var text = new List<string> { };

                text.Add("UseLocal7zip="           + Convert.ToInt32(Settings.UseLocal7zip));
                text.Add("SaveDownloadedArchives=" + Convert.ToInt32(Settings.SaveDownloadedArchives));
                text.Add("ExitLaunchManager="      + Convert.ToInt32(Settings.ExitLaunchManager));

                text.Add("[Paths]");
                foreach (string path in Settings.Paths.Keys)
                {
                    text.Add(path + "=" + Settings.Paths[path]);
                }

                File.WriteAllLines("OneClickModInstaller.cfg", text);
            }
        }

        public UltimateWinForm(string[] args)
        {
            InitializeComponent();

            Settings.Load();
            fake_SettingsLoad();
            Installation.Status = "Idle";
            Installation.Local      =
            Installation.FromDir    =
            Installation.FromArgs   = false;

            lType.Text = lModID.Text = lDownloadType.Text = lDownloadID.Text = null;

            //Dealing with arguments
            if (args.Length > 0)
            {
                //If this is not a 1-click installation call
                if (!(args[0].StartsWith("sonic4mmep1:") ||
                    args[0].StartsWith("sonic4mmep2:")))
                {
                    //Drag&Drop mod installation
                    if (File.Exists(args[0])
                        || Directory.Exists(args[0])
                        || args[0].StartsWith("https://")
                        || args[0].StartsWith("http://"))
                    {
                        Installation.FromArgs = true;
                        tbModURL.Text     = args[0];
                        tcMain.SelectTab(tabModInst);

                        PrepareInstallation();
                    }
                }
                else
                {
                    Installation.FromArgs = true;
                    //a 1-lick installation call
                    //sonic4mmepx:url,mod_type,mod_id
                    tcMain.SelectTab(tabModInst);
                    var tmp_args = args[0].Substring(12).Split(',');
                    tbModURL.Text       = tmp_args[0];
                    if (tmp_args.Length > 1) { lDownloadType.Text = "Mod type:"; lType.Text  = tmp_args[1]; }
                    if (tmp_args.Length > 2) { lDownloadID.Text   = "Mod ID:";   lModID.Text = tmp_args[2]; }

                    PrepareInstallation();
                }
            }
            UpdateWindow();
        }

        private void PrepareInstallation()
        {
            if (File.Exists(tbModURL.Text) || Directory.Exists(tbModURL.Text))
            {
                lDownloadTrying.Text = lDownloadTrying.Text = "You are trying to install a mod from hard drive." + Environment.NewLine + "Aren't you?";
                lDownloadLink.Text = "Path to the mod:";
                Installation.Link   = tbModURL.Text;
                Installation.Local  = true;
                Installation.FromDir= false;
                if (Directory.Exists(tbModURL.Text))
                    Installation.FromDir  = true;
            }
            else if (tbModURL.Text.StartsWith("https://") || tbModURL.Text.StartsWith("http://"))
            {
                lDownloadTrying.Text = lDownloadTrying.Text = "You are trying to download a mod from {1}." + Environment.NewLine + "Aren't you?";
                lDownloadLink.Text  = "Download link:";
                Installation.Link   = tbModURL.Text;
                Installation.Local  = false;
                Installation.FromDir= false;

                if (Installation.Link.Contains("gamebanana.com"))
                {
                    lDownloadTrying.Text = lDownloadTrying.Text.Replace("{1}", "GameBanana");
                    Installation.ServerHost = "gamebanana";
                }
                else if (Installation.Link.Contains("github.com"))
                {
                    lDownloadTrying.Text = lDownloadTrying.Text.Replace("{1}", "GitHub");
                    Installation.ServerHost = "github";
                }
                else
                {
                    lDownloadTrying.Text = lDownloadTrying.Text.Replace("{1}", "the Internet");
                    Installation.ServerHost = "else";
                }
            }
        }

        private void UpdateWindow(bool begin = true)
        {
            if (begin)
                if (tcMain.InvokeRequired)
                    tcMain.Invoke(new MethodInvoker(delegate { UpdateWindow(false); ; }));
                else UpdateWindow(false);

            if (begin) return;

            ////////////////
            //Installaller//
            ////////////////

            ///////////
            //Overall//
            ///////////

            var statuses  = Reg.InstallationStatus();
            var locations = Reg.InstallationLocation();

            foreach (string key in statuses.Keys)
            {
                Label  lIOStatus;
                Label  lIOPath;
                Button bIOUninstall;
                Button bIOVisit;

                switch (key)
                {
                    case "ep1":
                        lIOStatus    = lIOEp1Stat;
                        lIOPath      = lIOEp1Path;
                        bIOUninstall = bIOEp1Uninstall;
                        bIOVisit     = bIOEp1Visit;
                        break;
                    case "ep2":
                        lIOStatus    = lIOEp2Stat;
                        lIOPath      = lIOEp2Path;
                        bIOUninstall = bIOEp2Uninstall;
                        bIOVisit     = bIOEp2Visit;
                        break;
                    default: continue;
                }

                if (statuses[key] > 0)
                {
                    lIOStatus.Text = "Installed";
                    lIOPath.Text   = ("Path: " + locations[key]).Replace(' ', '\u2007');
                    bIOUninstall.Enabled =
                    bIOVisit.Enabled     = true;

                    if (Admin.AmI()) bUninstall.Image = null;
                }
                else
                {
                    lIOStatus.Text = "Not installed";
                    lIOPath.Text   = "";
                    bIOUninstall.Enabled =
                    bIOVisit.Enabled     = false;
                }
            }

            ///////////
            //Current//
            ///////////

            string current_game = GetGame.Short();

            if (Admin.AmI())
            {
                lInstallAdmin.Text = "";
                bInstall.Image   =
                bUninstall.Image = null;
            }

            if (current_game == "dunno")
            {
                lGameName.Text           = "Not found";
                lInstallationStatus.Text = "None";
                bInstall.Text            = "Install as fake Episode 1";
                bUninstall.Text          = "Uninstall fake Episode 1";
                bInstall.Enabled   =
                bUninstall.Enabled = false;

                switch (statuses["ep1"])
                {
                    case 0:
                        bInstall.Enabled = true;
                        break;
                    case 1:
                        lInstallationStatus.Text = "Installed as fake Episode 1 here";
                        bUninstall.Enabled = true;
                        break;
                }
            }
            else
            {
                lGameName.Text = "Sonic 4: " + GetGame.Full();
                int current_status = statuses[current_game];

                bInstall.Enabled =
                bUninstall.Enabled = true;

                switch (current_status)
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

            ////////////////////
            //Mod Installation//
            ////////////////////
            
            bModPath.Enabled    =
            bModInstall.Enabled = false;
            bModInstall.Text    = "Install";
            tbModURL.ReadOnly   = Installation.FromArgs;

            switch (Installation.Status)
            {
                case "Idle":
                case "Cancelled":
                    bModPath.Enabled    = !Installation.FromArgs;
                    bModInstall.Enabled = true;
                    break;
                case "Waiting for path":
                    tbModURL.ReadOnly   =
                    bModInstall.Enabled = true;
                    bModInstall.Text    = "Continue installation";
                    break;
                case "Mod is complicated":
                    bModPath.Enabled    = !Installation.FromArgs;
                    break;
                case "Installed":
                    if (!Installation.FromArgs)
                    {
                        bModPath.Enabled    =
                        bModInstall.Enabled = true;
                    }
                    else if (Installation.Local)
                        bModInstall.Enabled = true;
                    break;
                case "Server error":
                    bModInstall.Text    = "Retry";
                    bModInstall.Enabled = true;
                    tbModURL.ReadOnly   = Installation.FromArgs;
                    bModPath.Enabled    = !Installation.FromArgs;
                    break;
            }

            if (!Installation.FromArgs)
            {
                if (bModInstall.Enabled)
                    tbModURL_TextChanged(null, null);
            }
        }

        private void bInstall_Click(object sender, EventArgs e)
        {
            if (GetGame.Short() == "dunno")
            {
                if (Reg.InstallationStatus()["ep1"] == 0)
                    Reg.Install("ep1");
            }
            else
            {
                switch (Reg.InstallationStatus()[GetGame.Short()])
                {
                    case 2: Reg.FixPath(); break;
                    default: Reg.Install(); break;
                }
            }

            UpdateWindow();
        }

        private void bUninstall_Click(object sender, EventArgs e)
        {
            if (GetGame.Short() == "dunno")
            {
                if (Reg.InstallationStatus()["ep1"] == 1)
                    Reg.Uninstall("ep1");
            }
            else Reg.Uninstall();

            UpdateWindow();
        }

        private void bModInstall_Click(object sender, EventArgs e)
        {
            if (Installation.Status == "Idle"
                || Installation.Status == "Cancelled"
                || Installation.Status == "Installed"
                || Installation.Status == "Server error")
            {
                tbModURL.ReadOnly   = true;
                bModPath.Enabled    =
                bModInstall.Enabled = false;
                StartInstallation();
            }
            else if (Installation.Status == "Waiting for path")
            {
                if (Settings.Paths[Installation.Platform] != "")
                {
                    bModInstall.Enabled = false;
                    FinishInstallation();
                }
                else tcMain.SelectTab(tabSettings);
            }
        }

        public void fake_DoTheRest(object sender, AsyncCompletedEventArgs e)
        {
            if (Installation.Total == -1 || Installation.Recieved == Installation.Total)
                DoTheRest();
            else
            {
                Installation.Status = "Server error";
                statusBar.Text      = "Couldn't download full file (server error)";
                UpdateWindow();
            }
        }

        async public void StartInstallation()
        {
            await Task.Run(() =>
            {
                string archive_url = Installation.Link;
                if (Installation.Link.EndsWith("/"))
                    Installation.Link = Installation.Link.Substring(Installation.Link.Length - 1);

                if (File.Exists(archive_url) || Directory.Exists(archive_url))
                {
                    if (Installation.FromDir)
                        Installation.ArchiveDir = archive_url;
                    else
                        Installation.ArchiveName = archive_url;
                    DoTheRest();
                }
                else
                {
                    statusBar.Text = "Connecting to the server...";
                    using (WebClient wc = new WebClient())
                    {
                        bModInstall.Enabled = false;
                        wc.DownloadFileCompleted += new AsyncCompletedEventHandler(fake_DoTheRest);
                        wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);

                        //Download link goes here
                        string url = URL.GetRedirect(archive_url);
                        
                        //Getting file name of the archive
                        if (Installation.ServerHost == "github")
                        {
                            //GitHub's redirect link is something like a request rather than a file "path" on a server
                            Installation.ArchiveName = archive_url.Split('/')[archive_url.Split('/').Length - 1];
                        }
                        else
                        {
                            Installation.ArchiveName = url.Split('/')[url.Split('/').Length - 1];
                        }

                        if (Installation.ServerHost == "gamebanana")
                        {
                            //Well, it seems that GB's counter doesn't increase if you download
                            //the file directly from the redirect url. But I'm not sure that
                            //this works as well
                            url = archive_url;
                        }

                        if (File.Exists(Installation.ArchiveName))
                        {
                            File.Delete(Installation.ArchiveName);
                        }

                        wc.DownloadFileAsync(new Uri(url), Installation.ArchiveName);
                    }
                }
            });
        }

        async public void DoTheRest()
        {
            await Task.Run(() =>
            {
                if (!Installation.FromDir)
                {
                    statusBar.Text = "Extracting downloaded archive...";

                    Installation.ArchiveDir = Installation.ArchiveName + "_extracted";

                    if (File.Exists(Installation.ArchiveName))
                    {
                        if (Directory.Exists(Installation.ArchiveName + "_extracted"))
                            MyDirectory.DeleteRecursively(Installation.ArchiveName + "_extracted");

                        if (Settings.UseLocal7zip)
                            ModArchive.Extract(Installation.ArchiveName, Settings.Paths["7-Zip"]);
                        else
                            ModArchive.Extract(Installation.ArchiveName);
                    }
                }

                if (File.Exists(Installation.ArchiveDir))
                {
                    statusBar.Text = "Couldn't extract archive";
                    Installation.Status = "Idle";
                }

                int cont = -1;

                if (!Directory.Exists(Installation.ArchiveDir))
                {
                    statusBar.Text = "Couldn't extract archive";
                    Installation.Status = "Idle";
                    if (!(File.Exists("7z.exe") || (File.Exists(Settings.Paths["7-Zip"]) && Settings.UseLocal7zip)))
                        statusBar.Text += " (7-Zip not found)";
                }
                else
                {
                    statusBar.Text = "Checking extracted files...";
                    cont = ModArchive.CheckFiles(Installation.ArchiveDir);
                }
                
                if (cont == 0)
                {
                    MyDirectory.DeleteRecursively(Installation.ArchiveDir);
                    statusBar.Text = "Installation was cancelled";
                    Installation.Status = "Cancelled";
                }
                else if (cont == 1)
                {
                    statusBar.Text = "Finding root directories...";

                    var FoundRootDirs = ModArchive.FindRoot(Installation.ArchiveDir);
                    var FoundFiles    = ModArchive.FindFiles(Installation.ArchiveDir);
                    
                    if (FoundRootDirs.Item2 != "???")
                    {
                        Installation.ModRoots = FoundRootDirs.Item1;
                        Installation.Platform = FoundRootDirs.Item2;
                    }
                    else
                    {
                        Installation.ModRoots = FoundFiles.Item1;
                        Installation.Platform = FoundFiles.Item2;
                    }

                    Installation.Status = "Ready to install";

                    tcMain.Invoke(new MethodInvoker(delegate
                    {
                        ContinueInstallation();
                        ;
                    }));
                }
                UpdateWindow();
            });
        }

        public void ContinueInstallation()
        {
            if (Installation.Platform == "???")
            {
                //Status description
                /*  1 - start (open SelectRoots window)
                 *  2 - continue (choose destination directory)
                 *  0 - ok (user selected destination directory)
                 *  -1 - break (user cancelled SelectRoots window)
                 */
                int status = 1;
                var sr = new SelectRoots(Installation.ArchiveDir);

                while (status > 0)
                {
                    if (status == 1)
                    {
                        status = -1;
                        if (sr.ShowDialog() == DialogResult.Yes)
                        {
                            status = 2;
                            Installation.ModRoots = sr.output.ToArray();
                            if (Installation.ModRoots.Length == 0)
                                status = -1;
                        }
                    }
                    else if (status == 2)
                    {
                        status = 1;
                        string path = MyDirectory.Select("test", "dir");
                        if (path != null)
                        {
                            Installation.CustomPath = path;
                            status = 0;
                        }
                    }
                }

                if (status == -1)
                {
                    Installation.Status = "Cancelled";
                    statusBar.Text = "Installation was cancelled";
                    return;
                }
            }
            else if (Installation.Platform == "mixed")
            {
                //TODO: think up a better explanation
                MessageBox.Show("Looks like this thing is complicated, try to install it manually. bye"
                              , "Couldn't install the mod"
                              , MessageBoxButtons.OK
                              , MessageBoxIcon.Exclamation);
                Installation.Status = "Mod is complicated";
                return;
            }
            else
            {
                if (Installation.ModRoots.Length > 1)
                {
                    TooManyMods tmm_form = new TooManyMods(Installation.ModRoots, Installation.Platform);
                    tmm_form.ShowDialog();

                    Installation.ModRoots = tmm_form.mods;
                }

                if (Settings.Paths.ContainsKey(Installation.Platform))
                {
                    if (Settings.Paths[Installation.Platform] == "")
                    {
                        tcMain.SelectTab(tabSettings);

                        MessageBox.Show("Looks like the mod you installed requires a special path to be installed to.\nPlease, specify "
                                      + Installation.Platform + "\nand then press the \"Continue Installation\" button in the \"Download\" tab."
                                      , "Please specify a path"
                                      , MessageBoxButtons.OK
                                      , MessageBoxIcon.Information);
                        Installation.Status = "Waiting for path";
                        UpdateWindow();
                    }
                }
            }
            
            if (Installation.ModRoots.Length > 0)
                Installation.LastMod = Path.GetFileName(Installation.ModRoots[0]);

            if (Installation.Status == "Ready to install")
                FinishInstallation();
        }

        async public void FinishInstallation()
        {
            await Task.Run(() =>
            {
                statusBar.Text = "Installing downloaded mod...";

                foreach (string mod in Installation.ModRoots)
                {
                    string dest;
                    
                    switch (Installation.Platform)
                    {
                        case "pc":      dest = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "mods", Path.GetFileName(mod)); break;
                        case "dolphin": dest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Dolphin Emulator", "Load", "Textures"); break;
                        case "???":     dest = Installation.CustomPath; break;
                        default:        dest = Settings.Paths[Installation.Platform]; break;
                    }
                    
                    if (Directory.Exists(dest) && Installation.Platform == "pc")
                        MyDirectory.DeleteRecursively(dest);
                    
                    if (Installation.Platform != "???")
                        ModArchive.CopyAll(mod, dest);
                    else
                    {
                        if (File.Exists(Path.Combine(Installation.ArchiveDir, mod)))
                            File.Copy(Path.Combine(Installation.ArchiveDir, mod), Path.Combine(dest, Path.GetFileName(mod)));
                        else if (Directory.Exists(Path.Combine(Installation.ArchiveDir, mod)))
                            ModArchive.CopyAll(Path.Combine(Installation.ArchiveDir, mod), Path.Combine(dest, Path.GetFileName(mod)));
                    }
                }

                if (!Installation.FromDir)
                    MyDirectory.DeleteRecursively(Installation.ArchiveDir);

                if (File.Exists(Installation.ArchiveName) && !Installation.Local)
                {
                    if (Settings.SaveDownloadedArchives)
                    {
                        Directory.CreateDirectory(Settings.Paths["DownloadedArhives"]);
                        File.Move(Installation.ArchiveName, Path.Combine(Settings.Paths["DownloadedArhives"], Installation.ArchiveName));
                    }
                    else
                    {
                        File.Delete(Installation.ArchiveName);
                    }
                }

                if (Settings.ExitLaunchManager)
                {
                    if (Installation.Platform == "pc" && File.Exists("Sonic4ModManager.exe"))
                        Process.Start("Sonic4ModManager.exe", "\"" + UltimateWinForm.Installation.LastMod + "\"");
                    Environment.Exit(0);
                }

                statusBar.Text = "Mod installation complete!";
                Installation.Status = "Installed";
                
                tcMain.Invoke(new MethodInvoker(delegate {
                    progressBar.Value = 0;
                    UpdateWindow();
                    ;
                }));
            });
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            string unit;
            int divider;
            long total;

            Installation.Recieved   = e.BytesReceived;
            Installation.Total      = e.TotalBytesToReceive;
            
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
                    statusBar.Text = "Downloading... (" + e.BytesReceived / divider + unit + ")";
                else
                {
                    statusBar.Text = "Downloading... (" + e.BytesReceived / divider + " / " + e.TotalBytesToReceive / divider + " " + unit + ")";
                    progressBar.Value = (int)(1000 * e.BytesReceived / e.TotalBytesToReceive);
                }
                ;
            }));
        }

        private void bIOEp1Visit_Click(object sender, EventArgs e)
        {
            if (Reg.InstallationStatus()["ep1"] < 1) return;
            MyDirectory.OpenExplorer(Reg.InstallationLocation()["ep1"]);
        }

        private void bIOEp2Visit_Click(object sender, EventArgs e)
        {
            if (Reg.InstallationStatus()["ep2"] < 1) return;
            MyDirectory.OpenExplorer(Reg.InstallationLocation()["ep2"]);
        }

        private void bIOEp1Uninstall_Click(object sender, EventArgs e)
        {
            Reg.Uninstall("ep1");
            UpdateWindow();
        }

        private void bIOEp2Uninstall_Click(object sender, EventArgs e)
        {
            Reg.Uninstall("ep2");
            UpdateWindow();
        }

        private void cbUseLocal7zip_CheckedChanged(object sender, EventArgs e)
        {
            tbPath7z.Enabled =
            bPath7z.Enabled  = cbUseLocal7zip.Checked;
            fake_SettingsSave(sender, e);
        }

        private void bPath7z_Click(object sender, EventArgs e)
        {
            string path = MyDirectory.Select("7z.exe", "7z");
            if (path != null)
                tbPath7z.Text = path;
        }

        private void bPathCheatTables_Click(object sender, EventArgs e)
        {
            string path = MyDirectory.Select("Cheat Tables", "dir");
            if (path != null)
                tbPathCheatTables.Text = path;
        }

        private void bPathDownloadedArchives_Click(object sender, EventArgs e)
        {
            string path = MyDirectory.Select("directory where you want to save downloaded archives", "dir");
            if (path != null)
                tbDownloadedArchiveLocation.Text = path;
        }

        private void fake_SettingsLoad()
        {
            Settings.Load();

            cbUseLocal7zip.Checked           = Settings.UseLocal7zip;
            chSaveDownloadedArchives.Checked = Settings.SaveDownloadedArchives;
            cbExitLaunchManager.Checked      = Settings.ExitLaunchManager;
            tbPath7z.Text                    = Settings.Paths["7-Zip"];
            tbPathCheatTables.Text           = Settings.Paths["CheatTables"];
            tbDownloadedArchiveLocation.Text = Settings.Paths["DownloadedArhives"];
        }

        private void fake_SettingsSave(object sender, EventArgs e)
        {
            switch (((Control)sender).Name)
            {
                case "cbUseLocal7zip":              Settings.UseLocal7zip               = cbUseLocal7zip.Checked;           break;
                case "chSaveDownloadedArchives":    Settings.SaveDownloadedArchives     = chSaveDownloadedArchives.Checked; break;
                case "cbExitLaunchManager":         Settings.ExitLaunchManager          = cbExitLaunchManager.Checked;      break;
                case "tbPath7z":                    Settings.Paths["7-Zip"]             = tbPath7z.Text;                    break;
                case "tbPathCheatTables":           Settings.Paths["CheatTables"]       = tbPathCheatTables.Text;           break;
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
                if (Installation.FromArgs)
                {
                    tbModURL.ReadOnly   = true;
                    bModPath.Enabled    = false;
                }
            }
            else
            {
                bModInstall.Enabled = false;
            }
            PrepareInstallation();
        }

        private void bModPath_Click(object sender, EventArgs e)
        {
            string path = MyDirectory.Select("mod", "arc/dir");
            if (path != null)
                tbModURL.Text = path;
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(((Control)sender).Text);
        }
    }
}
