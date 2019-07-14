using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OneClickModInstaller
{
    public partial class UltimateWinForm:Form
    {
        //This class stores variables related to mod installation
        public static class Installation
        {
            public static string Link        { set; get; }
            public static string ServerHost  { set; get; }
            public static string ArchiveName { set; get; }
            public static string ArchiveDir  { set; get; }
            public static string LastMod     { set; get; }
            public static string[] ModRoots  { set; get; }
            public static string Type        { set; get; }
            public static string Status      { set; get; }
        }
        
        public static class Settings
        {
            public static bool UseLocal7zip { set; get; }

            public static Dictionary<string, string> Paths { set; get; }

            public static void Load()
            {
                Settings.Paths = new Dictionary<string, string> ();
                Settings.Paths.Add("CheatTables", "");
                Settings.Paths.Add("7-Zip", "");

                if (File.Exists("OneClickModInstaller.cfg"))
                {
                    string[] cfg_file = File.ReadAllLines("OneClickModInstaller.cfg");

                    foreach (string line in cfg_file)
                    {
                        if (!line.Contains("=")) { continue; }
                        string formatted_line = line.Substring(line.IndexOf("=") + 1);

                        if (line.StartsWith("UseLocal7zip="))
                        { Settings.UseLocal7zip = Convert.ToBoolean(Convert.ToInt32(formatted_line)); }

                        else if (line.StartsWith("CheatTables="))
                        { Settings.Paths["CheatTables"] = formatted_line.Replace("\\", "/"); }

                        else if (line.StartsWith("7-Zip="))
                        { Settings.Paths["7-Zip"] = formatted_line.Replace("\\", "/"); }

                    }
                }
            }

            public static void Save()
            {
                var text = new List<string> { };

                text.Add("UseLocal7zip=" + Convert.ToInt32(Settings.UseLocal7zip));
                
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

            //Dealing with arguments
            if (args.Length > 0)
            {
                //If this is not a 1-click installation call
                if (!(args[0].StartsWith("sonic4mmep1:") ||
                    args[0].StartsWith("sonic4mmep2:")))
                {
                    //Drag&Drop mod installation
                    if (File.Exists(args[0]) || Directory.Exists(args[0]))
                    {
                        bDownload.Text = "Install";
                        lDownloadTrying.Text = lDownloadTrying.Text.Replace("{0}", "install a mod from hard drive");
                        Installation.Link = args[0];
                        lURL.Text = args[0].Replace(' ', '\u2007');

                        lType.Text = lModID.Text = lDownloadType.Text = lDownloadID.Text = null;

                        lDownloadLink.Text = "Path to the mod:";
                        tcMain.SelectTab(tabDownload);
                    }
                }
                else
                {
                    //a 1-lick installation call
                    //sonic4mmepx:url,mod_type,mod_id
                    tcMain.SelectTab(tabDownload);
                    var tmp_args = args[0].Substring(12).Split(',');
                    lURL.Text = tmp_args[0];
                    Installation.Link = tmp_args[0];

                    lDownloadTrying.Text = lDownloadTrying.Text.Replace("{0}", "download a mod from {1}");

                    if (tmp_args.Length > 1) { lType.Text  = tmp_args[1]; }
                    if (tmp_args.Length > 2) { lModID.Text = tmp_args[2]; }

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

            Settings.Load();
            Installation.Status = "Idle";
            UpdateWindow();
        }
        
        private void UpdateWindow()
        {
            ////////////////
            //Installation//
            ////////////////

            ///////////
            //Overall//
            ///////////

            var statuses  = Reg.InstallationStatus();
            var locations = Reg.InstallationLocation();

            foreach (string key in statuses.Keys)
            {
                Console.WriteLine(key + " " + statuses[key]);
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

                    if (Admin.AmI()) { bUninstall.Image = null; }
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
                bInstall.Image = null;
                bUninstall.Image = null;
            }

            if (current_game == "dunno")
            {
                lGameName.Text           = "Not found";
                lInstallationStatus.Text = "None";
                bInstall.Text      = "Install as fake Episode 1";
                bUninstall.Text    = "Uninstall fake Episode 1";
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

                bInstall.Enabled   =
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
        }
        
        private void bInstall_Click(object sender, EventArgs e)
        {
            if (GetGame.Short() == "dunno")
            {
                if (Reg.InstallationStatus()["ep1"] == 0)
                { Reg.Install("ep1"); }
            }
            else
            {
                switch (Reg.InstallationStatus()[GetGame.Short()])
                {
                    case 2:  Reg.FixPath(); break;
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
                { Reg.Uninstall("ep1"); }
            }
            else { Reg.Uninstall(); }

            UpdateWindow();
        }

        private void bDownload_Click(object sender, EventArgs e)
        {
            if (Installation.Status == "Idle")
            {
                string archive_url = Installation.Link;
                if (File.Exists(archive_url) || Directory.Exists(archive_url))
                {
                    Installation.ArchiveName = archive_url;
                    DoTheRest();
                }
                else
                {
                    using (WebClient wc = new WebClient())
                    {
                        toolStripStatusLabel1.Text = "Connecting to the server...";
                        bDownload.Enabled = false;
                        wc.DownloadFileCompleted += new AsyncCompletedEventHandler(fake_DoTheRest);
                        wc.DownloadProgressChanged += wc_DownloadProgressChanged;

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
                            //this works, too. And this is slower as well.
                            url = archive_url;
                        }

                        if (File.Exists(Installation.ArchiveName))
                        {
                            File.Delete(Installation.ArchiveName);
                        }

                        wc.DownloadFileAsync(new Uri(url), Installation.ArchiveName);
                    }
                }
            }
            else if (Installation.Status == "Waiting for path")
            {
                if (tbPathCheatTables.Text != "")
                {
                    Settings.Save();
                    FinishInstallation();
                }
                else { tcMain.SelectTab(tabSettings); }
            }
        }

        public void fake_DoTheRest(object sender, AsyncCompletedEventArgs e)
        {
            DoTheRest();
        }

        async public void DoTheRest()
        {
            await Task.Run(() =>
            {
                toolStripStatusLabel1.Text = "Extracting downloaded archive...";

                Installation.ArchiveDir = Installation.ArchiveName + "_extracted";

                if (File.Exists(Installation.ArchiveName))
                {
                    if (Directory.Exists(Installation.ArchiveName + "_extracted"))
                    { MyDirectory.DeleteRecursively(Installation.ArchiveName + "_extracted"); }

                    ModArchive.Extract(Installation.ArchiveName);
                }
                else if (Directory.Exists(Installation.ArchiveName))
                {
                    //This means that mod will be installed from a directory
                    Installation.ArchiveDir = Installation.ArchiveName;
                }

                toolStripStatusLabel1.Text = "Checking extracted files...";
                int cont = ModArchive.CheckFiles(Installation.ArchiveDir);

                if (cont != 1)
                {
                    MyDirectory.DeleteRecursively(Installation.ArchiveDir);
                    Environment.Exit(0);
                }

                toolStripStatusLabel1.Text = "Finding root directories...";

                var FoundRootDirs = ModArchive.FindRoot(Installation.ArchiveDir);
                Installation.ModRoots = FoundRootDirs.Item1;
                Installation.Type = FoundRootDirs.Item2;
                Installation.Status = "Ready to install";

                if (Installation.ModRoots.Length > 1)
                {
                    TooManyMods tmm_form = new TooManyMods(Installation.ModRoots, Installation.Type);
                    tmm_form.ShowDialog();

                    Installation.ModRoots = tmm_form.mods;
                }
                else if (Installation.ModRoots.Length == 0)
                {
                    var FoundFiles = ModArchive.FindFiles(Installation.ArchiveDir);
                    Installation.Type = FoundFiles.Item2;
                    Installation.ModRoots = FoundFiles.Item1;

                    if (Installation.Type == "???")
                    {
                        MessageBox.Show("One-Click Mod Installer couldn't find any root directories of the mod. The mod won't be installed. Try to install the mod manually or try to contact the mod creator or the creator of the Mod Loader."
                                      , "Couldn't install the mod"
                                      , MessageBoxButtons.OK
                                      , MessageBoxIcon.Asterisk);
                        Environment.Exit(0);
                    }
                    else if (Installation.Type == "mixed")
                    {
                        //TODO: think up a better explanation
                        MessageBox.Show("Looks like this thing is complicated, try to install it manually. bye"
                                      , "Couldn't install the mod"
                                      , MessageBoxButtons.OK
                                      , MessageBoxIcon.Exclamation);
                        Environment.Exit(0);
                    }
                    else
                    {
                        if (Installation.ModRoots.Length > 1)
                        {
                            TooManyMods tmm_form = new TooManyMods(Installation.ModRoots, Installation.Type);
                            tmm_form.ShowDialog();

                            Installation.ModRoots = tmm_form.mods;
                        }
                        
                        if (Settings.Paths[Installation.Type] == "")
                        {
                            //Oh, some hacky code over here
                            if (tcMain.InvokeRequired)
                            {
                                tcMain.Invoke(new MethodInvoker(delegate {
                                    //Code goes here
                                    tcMain.SelectTab(tabSettings);
                                    bDownload.Text = "Continue Installation";
                                ; }));
                            }

                            MessageBox.Show("Looks like the mod you installed requires a special path to be installed to.\nPlease, specify "
                                          + Installation.Type + "\nand then press the \"Continue Installation\" button in the \"Download\" tab."
                                          , "Please specify a path"
                                          , MessageBoxButtons.OK
                                          , MessageBoxIcon.Information);
                            Installation.Status = "Waiting for path";
                        }
                    }
                }

                Installation.LastMod = Path.GetFileName(Installation.ModRoots[0]);

                if (Installation.Status == "Ready to install")
                { FinishInstallation(); }
                UpdateWindow();
            });
        }

        async public void FinishInstallation()
        {
            await Task.Run(() =>
            {
                toolStripStatusLabel1.Text = "Installing downloaded mod...";

                foreach (string mod in Installation.ModRoots)
                {
                    string dest;
                    if (Installation.Type == "pc")
                    { dest = Path.Combine("mods", Path.GetFileName(mod)); }
                    else if (Installation.Type == "dolphin")
                    { dest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Dolphin Emulator", "Load", "Textures"); }
                    else
                    { dest = Settings.Paths[Installation.Type]; }
                    
                    if (Directory.Exists(dest) && Installation.Type == "pc") { MyDirectory.DeleteRecursively(dest); }
                    ModArchive.CopyAll(mod, dest);
                }

                DFtEM enable_mod = new DFtEM();
                enable_mod.ShowDialog();

                if (Installation.ArchiveName != Installation.ArchiveDir)
                { MyDirectory.DeleteRecursively(Installation.ArchiveDir); }

                if (File.Exists(Installation.ArchiveName))
                { File.Delete(Installation.ArchiveName); }

                Environment.Exit(0);
            });
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;

            string unit;
            int divider;

            if (e.TotalBytesToReceive > 1024 * 1024 * 16)
            { unit = "MBs"; divider = 1024 * 1024; }
            else if (e.TotalBytesToReceive > 1024 * 16)
            { unit = "KBs"; divider = 1024; }
            else
            { unit = "Bytes"; divider = 1; }

            //Yep, sometimes TotalBytesToReceive equals -1
            if (e.TotalBytesToReceive == -1)
            {
                toolStripStatusLabel1.Text = "Downloading... (" + e.BytesReceived / divider + unit + ")";
            }
            else
            {
                toolStripStatusLabel1.Text = "Downloading... (" + e.BytesReceived / divider + " / " + e.TotalBytesToReceive / divider + unit + ")";
            }
        }

        private void bIOEp1Visit_Click(object sender, EventArgs e)
        {
            if (Reg.InstallationStatus()["ep1"] < 1) { return; }
            MyDirectory.OpenExplorer(Reg.InstallationLocation()["ep1"]);
        }

        private void bIOEp2Visit_Click(object sender, EventArgs e)
        {
            if (Reg.InstallationStatus()["ep2"] < 1) { return; }
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
            tbPath7z.Enabled = cbUseLocal7zip.Checked;
        }

        private void bPath7z_Click(object sender, EventArgs e)
        {
            string path = MyDirectory.SelectionDialog("7-Zip");
            if (path != null)
            { tbPath7z.Text = path; }
        }

        private void bPathCheatTables_Click(object sender, EventArgs e)
        {
            string path = MyDirectory.SelectionDialog("Cheat Tables");
            if (path != null)
            { tbPathCheatTables.Text = path; }
        }

        private void bSettingsSave_Click(object sender, EventArgs e)
        {
            Settings.Paths["7-Zip"]       = tbPath7z.Text;
            Settings.Paths["CheatTables"] = tbPathCheatTables.Text;
            Settings.Save();
        }
    }
}
