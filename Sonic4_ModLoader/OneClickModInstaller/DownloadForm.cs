using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OneClickModInstaller
{
    public partial class DownloadForm : Form
    {
        public static bool      local { set; get; }
        public static string    archive_name { set; get; }
        public static string    archive_url { set; get; }
        public static string    archive_dir { set; get; }
        public static string    server_host { set; get; }

        public DownloadForm(string[] args)
        {
            if (!Application.ExecutablePath.Contains(Path.Combine("bin","Debug")))
            {
                Environment.CurrentDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            }

            if (!File.Exists("7z.exe") || !File.Exists("7z.dll"))
            {
                MessageBox.Show("Please, reinstall the Mod Loader (or place \"7z.exe\" and \"7z.dll\" here). 1-Click Mod Installer will be closed.", "7-Zip not found");
                Environment.Exit(0);
            }

            local = false;
            if (args[0] == "--local")
            { local = true; }

            InitializeComponent();

            string tmp = "download";
            if (local) { tmp = "installation"; }
            toolStripStatusLabel1.Text = toolStripStatusLabel1.Text.Replace("{0}", tmp);
            
            if (local)
            {
                bDownload.Text = "Install";
                label3.Text = label3.Text.Replace("{0}", "install a mod from hard drive");
                archive_url = args[1];
                lURL.Text = archive_url.Replace(' ', '\u2007');

                lType.Text = lModID.Text = label2.Text = label4.Text = "";

                label1.Text = "Path to the mod:";
            }
            else
            {
                label3.Text = label3.Text.Replace("{0}", "download a mod from {1}");
                
                string[] parameters = args[0].Substring(12).Split(',');
                archive_url = lURL.Text                   = parameters[0];
                if (parameters.Length > 1) { lType.Text   = parameters[1]; }
                if (parameters.Length > 2) { lModID.Text  = parameters[2]; }

                if (lURL.Text.Contains("gamebanana.com"))
                {
                    label3.Text = label3.Text.Replace("{1}", "GameBanana");
                    server_host = "gamebanana";
                }
                else if (lURL.Text.Contains("github.com"))
                {
                    label3.Text = label3.Text.Replace("{1}", "GitHub");
                    server_host = "github";
                }
                else
                {
                    label3.Text = label3.Text.Replace("{1}", "the Internet");
                    server_host = "else";
                }
            }
            
            if (lType.Text == "???")
            {
                lType.Text = "";
                label4.Text = "";
            }
            if (lModID.Text == "???")
            {
                lModID.Text = "";
                label2.Text = "";
            }
        }
        
        static string GetRedirectURL(string url)
        {
            var t = WebRequest.Create(url);
            var r = t.GetResponse();
            var y = r.ResponseUri;
            r.Close();
            return y.ToString();
        }

        static void CopyAll(string source, string destination)
        {
            if (source.ToLower() == destination.ToLower())
            { return; }

            Directory.CreateDirectory(destination);
            
            foreach (string file in Directory.GetFiles(source))
            {
                File.Copy(file, Path.Combine(destination, Path.GetFileName(file)), true);
            }
            
            foreach (string dir in Directory.GetDirectories(source))
            {
                string dir_name = Path.GetFileName(dir);
                Directory.CreateDirectory(Path.Combine(destination, dir_name));
                CopyAll(Path.Combine(source, dir_name), Path.Combine(destination, dir_name));
            }
        }

        static void ExtractArchive(string file_name)
        {
            //Need 7-zip to work
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "7z.exe",
                Arguments = "x \"" + file_name + "\" -o" + archive_dir
            };
            Process.Start(startInfo).WaitForExit();
        }
        
        private int CheckFiles(string dir_name)
        {
            string[] good_formats = "TXT,INI,DDS,TXB,AMA,AME,ZNO,TXB,ZNM,ZNV,DC,EV,RG,MD,MP,AT,DF,DI,PSH,VSH,LTS,XNM,MFS,SSS,GPB,MSG,AYK,ADX,AMB,CPK,CBS".Split(',');

            string[] all_files = Directory.GetFiles(dir_name, "*", SearchOption.AllDirectories);
            List<string> suspicious_files = new List<string>();

            foreach (string file in all_files)
            {
                if (int.TryParse(Path.GetFileName(file), out int n) && file.Contains("DEMO\\WORLDMAP\\WORLDMAP.AMB"))
                {
                    continue;
                }

                int extension_len = Path.GetExtension(file).Length;
                if (extension_len != 0) { extension_len = 1; }

                if (good_formats.Contains(Path.GetExtension(file).Substring(extension_len), StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }
                
                suspicious_files.Add(file);
            }


            int cont = 1;
            if (suspicious_files.Count != 0)
            {
                cont = 0;
                Suspicious SuspiciousDialog = new Suspicious(suspicious_files.ToArray());
                
                DialogResult result = SuspiciousDialog.ShowDialog();

                //Continue
                if (result == DialogResult.Yes)
                {
                    cont = 1;
                }
            }
            return cont;
        }

        static string[] FindRootDirs(string dir_name)
        {
            List<string> mod_roots = new List<string>();
            string[] game_folders = "CUTSCENE,DEMO,G_COM,G_SS,G_EP1COM,G_EP1ZONE2,G_EP1ZONE3,G_EP1ZONE4,G_ZONE1,G_ZONE2,G_ZONE3,G_ZONE4,G_ZONEF,MSG,NNSTDSHADER,SOUND".Split(',');

            foreach (string folder in game_folders)
            {
                foreach (string mod_folder in Directory.GetDirectories(dir_name, folder, SearchOption.AllDirectories))
                {
                    string tmp_root = Path.GetDirectoryName(mod_folder);
                    if (!mod_roots.Contains(tmp_root))
                    {
                        mod_roots.Add(tmp_root);
                    }
                }
            }

            return mod_roots.ToArray();
        }
        
        private void bDownload_Click(object sender, EventArgs e)
        {
            if (local)
            {
                archive_name = Path.GetFileName(archive_url);
                archive_dir = Path.GetFileNameWithoutExtension(archive_name);

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
                    string url = GetRedirectURL(archive_url);

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

                    archive_dir = archive_name;

                    if (File.Exists(archive_name))
                    {
                        File.Delete(archive_name);
                    }
                    
                    wc.DownloadFileAsync(new Uri(url), archive_name);
                }
            }
        }

        static void DirectoryRemoveRecursively(string dir)
        {
            foreach (string file in Directory.GetFiles(dir))
            {
                //Program crashes if it tries to delete a read-only file
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dirr in Directory.GetDirectories(dir))
            {
                DirectoryRemoveRecursively(dirr);
            }
            Directory.Delete(dir);
        }

        private void fake_DoTheRest(object sender, AsyncCompletedEventArgs e)
        {
            DoTheRest();
        }

        private void DoTheRest()
        {
            toolStripStatusLabel1.Text = "Extracting downloaded archive...";
            Refresh();

            if (Directory.Exists(archive_dir))
            {
                DirectoryRemoveRecursively(archive_dir);
            }
            if (local)
            { ExtractArchive(archive_url); }
            else
            { ExtractArchive(archive_name); }

            toolStripStatusLabel1.Text = "Checking extracted files...";
            Refresh();
            int cont = CheckFiles(archive_dir);

            if (cont != 1)
            {
                DirectoryRemoveRecursively(archive_dir);
                Environment.Exit(0);
            }
            
            toolStripStatusLabel1.Text = "Finding root directories...";
            Refresh();
            string[] mod_roots = FindRootDirs(archive_dir);
            
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

            toolStripStatusLabel1.Text = "Installing downloaded mod...";
            Refresh();
            
            foreach (string mod in mod_roots)
            {
                string dest = Path.Combine("mods", Path.GetFileName(mod));
                if (Directory.Exists(dest)) { DirectoryRemoveRecursively(dest); }
                CopyAll(mod, dest);
            }

            DFtEM enable_mod = new DFtEM();
            enable_mod.ShowDialog();

            DirectoryRemoveRecursively(archive_dir);
            if (!local)
            {
                File.Delete(archive_name);
            }
            Environment.Exit(0);
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            
            string unit;
            int divider;

            if (e.TotalBytesToReceive > 1024*1024*5)
            {unit = "MBs"; divider = 1024*1024;}
            else if (e.TotalBytesToReceive > 1024*5)
            {unit = "KBs"; divider = 1024;}
            else
            {unit = "Bytes"; divider = 1;}

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
    }
}
