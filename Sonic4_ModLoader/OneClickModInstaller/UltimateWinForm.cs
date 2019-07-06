using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Threading.Tasks;

namespace OneClickModInstaller
{
    public partial class UltimateWinForm:Form
    {
        public static string server_host  { set; get; }
        public static string archive_name { set; get; }
        public static string archive_dir  { set; get; }
        public static string last_mod     { set; get; }

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
                        lURL.Text = lURL.Text.Replace(' ', '\u2007');

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

                    lDownloadTrying.Text = lDownloadTrying.Text.Replace("{0}", "download a mod from {1}");

                    if (tmp_args.Length > 1) { lType.Text  = tmp_args[1]; }
                    if (tmp_args.Length > 2) { lModID.Text = tmp_args[2]; }

                    if (lURL.Text.Contains("gamebanana.com"))
                    {
                        lDownloadTrying.Text = lDownloadTrying.Text.Replace("{1}", "GameBanana");
                        server_host = "gamebanana";
                    }
                    else if (lURL.Text.Contains("github.com"))
                    {
                        lDownloadTrying.Text = lDownloadTrying.Text.Replace("{1}", "GitHub");
                        server_host = "github";
                    }
                    else
                    {
                        lDownloadTrying.Text = lDownloadTrying.Text.Replace("{1}", "the Internet");
                        server_host = "else";
                    }
                }
            }

            ////////////////////
            //Installation tab//
            ////////////////////

            lGameName.Text = "Sonic 4: " + GetGame.Full();
            if (Admin.AmI())
            {
                lInstallAdmin.Text = "";
                bInstall.Image = null;
                bUninstall.Image = null;
            }
            
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
                Label  lStatus;
                Label  lPath;
                Button bUninstall;
                Button bVisit;

                switch (key)
                {
                    case "ep1":
                        lStatus    = lIOEp1Stat;
                        lPath      = lIOEp1Path;
                        bUninstall = bIOEp1Uninstall;
                        bVisit     = bIOEp1Visit;
                        break;
                    case "ep2":
                        lStatus    = lIOEp2Stat;
                        lPath      = lIOEp2Path;
                        bUninstall = bIOEp2Uninstall;
                        bVisit     = bIOEp2Visit;
                        break;
                    default: continue;
                }

                if (statuses[key] > 0)
                {
                    lStatus.Text = "Installed";
                    lPath.Text   = ("Path: " + locations[key]).Replace(' ', '\u2007');
                    bUninstall.Enabled =
                    bVisit.Enabled     = true;

                    if (Admin.AmI()) { bUninstall.Image = null; }
                }
                else
                {
                    lStatus.Text = "Not installed";
                    lPath.Text   = "";
                    bUninstall.Enabled =
                    bVisit.Enabled     = false;
                }
            }

            ///////////
            //Current//
            ///////////

            string current_game = GetGame.Short();

            if (current_game == "dunno")
            {
                switch (statuses["ep1"])
                {
                    case 0:
                        break;
                    default:
                        break;
                }
            }
            else
            {
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
            switch (Reg.InstallationStatus()[GetGame.Short()])
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

        private void bDownload_Click(object sender, EventArgs e)
        {
            string archive_url = lURL.Text;
            if (File.Exists(archive_url) || Directory.Exists(archive_url))
            {
                archive_name = archive_url;
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
                    if (server_host == "github")
                    {
                        //GitHub's redirect link is something like a request rather than a file "path" on a server
                        archive_name = archive_url.Split('/')[archive_url.Split('/').Length - 1];
                    }
                    else
                    {
                        archive_name = url.Split('/')[url.Split('/').Length - 1];
                    }

                    if (server_host == "gamebanana")
                    {
                        //Well, it seems that GB's counter doesn't increase if you download
                        //the file directly from the redirect url. But I'm not sure that
                        //this works, too. And this is slower as well.
                        url = archive_url;
                    }

                    if (File.Exists(archive_name))
                    {
                        File.Delete(archive_name);
                    }

                    wc.DownloadFileAsync(new Uri(url), archive_name);
                }
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

                archive_dir = archive_name + "_extracted";

                if (File.Exists(archive_name))
                {
                    if (Directory.Exists(archive_name + "_extracted"))
                    { MyDirectory.DeleteRecursively(archive_name + "_extracted"); }

                    ModArchive.Extract(archive_name);
                }
                else if (Directory.Exists(archive_name))
                {
                    //This means that mod will be installed from a directory
                    archive_dir = archive_name;
                }

                toolStripStatusLabel1.Text = "Checking extracted files...";
                int cont = ModArchive.CheckFiles(archive_dir);

                if (cont != 1)
                {
                    MyDirectory.DeleteRecursively(archive_dir);
                    Environment.Exit(0);
                }

                toolStripStatusLabel1.Text = "Finding root directories...";

                var FoundRootDirs = ModArchive.FindRoot(archive_dir);
                string[] mod_roots = FoundRootDirs.Item1;
                string platform = FoundRootDirs.Item2;

                if (mod_roots.Length > 1)
                {
                    TooManyMods tmm_form = new TooManyMods(mod_roots);
                    tmm_form.ShowDialog();

                    mod_roots = tmm_form.mods;
                }
                else if (mod_roots.Length == 0)
                {
                    MessageBox.Show("1-Click Mod Installer couldn't find any root directories of the mod. The mod won't be installed. Try to install the mod manually or try to contact the mod creator or the creator of the Mod Loader.");
                    Environment.Exit(0);
                }

                last_mod = Path.GetFileName(mod_roots[0]);

                toolStripStatusLabel1.Text = "Installing downloaded mod...";

                foreach (string mod in mod_roots)
                {
                    string dest;
                    if (platform == "pc")
                    { dest = Path.Combine("mods", Path.GetFileName(mod)); }
                    else
                    { dest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Dolphin Emulator", "Load", "Textures"); }

                    if (Directory.Exists(dest) && platform == "pc") { MyDirectory.DeleteRecursively(dest); }
                    ModArchive.CopyAll(mod, dest);
                }

                DFtEM enable_mod = new DFtEM();
                enable_mod.ShowDialog();

                if (archive_name != archive_dir)
                { MyDirectory.DeleteRecursively(archive_dir); }
                if (File.Exists(archive_name))
                { File.Delete(archive_name); }

                Environment.Exit(0);
            });
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;

            string unit;
            int divider;

            if (e.TotalBytesToReceive > 1024 * 1024 * 5)
            { unit = "MBs"; divider = 1024 * 1024; }
            else if (e.TotalBytesToReceive > 1024 * 5)
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
                toolStripStatusLabel1.Text = "Downloading... (" + e.BytesReceived / divider + unit + " / " + e.TotalBytesToReceive / divider + unit + ")";
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
    }
}
