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
        public static bool local { set; get; }

        public DownloadForm(string[] args)
        {
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
                lURL.Text = args[1];

                lType.Text = lModID.Text = label2.Text = label4.Text = "";

                label1.Text = "Path to the mod:";
            }
            else
            {
                label3.Text = label3.Text.Replace("{0}", "download a mod from GameBanana");

                string[] parameters = args[0].Split(',');
                if (parameters.Length > 0) { lURL.Text =    parameters[0]; }
                if (parameters.Length > 1) { lType.Text =   parameters[1]; }
                if (parameters.Length > 2) { lModID.Text =  parameters[2]; }
            }
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
                Arguments = "x \"" + file_name + "\" -o" + "extracted_mod"
            };
            Process.Start(startInfo).WaitForExit();
        }
        
        private int CheckFiles(string dir_name)
        {
            string[] good_formats = "TXT,INI,DDS,TXB,AMA,AME,ZNO,TXB,ZNM,ZNV,DC,EV,RG,MD,MP,AT,DF,DI,PSH,VSH,LTS,XNM,MFS,SSS,GPB,MSG,AYK".Split(',');

            string[] all_files = Directory.GetFiles(dir_name, "*", SearchOption.AllDirectories);
            List<string> suspicious_files = new List<string>();

            foreach (string file in all_files)
            {
                if (int.TryParse(Path.GetFileName(file), out int n) && file.Contains("DEMO\\WORLDMAP\\WORLDMAP.AMB"))
                {
                    continue;
                }
                
                if (good_formats.Contains(Path.GetExtension(file).Substring(1), StringComparer.OrdinalIgnoreCase))
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
            if (File.Exists(lModID.Text + ".zip"))
            {
                File.Delete(lModID.Text + ".zip");
            }
            if (local)
            {
                toolStripStatusLabel1.Text = "Copying local archive...";
                Console.WriteLine(Path.GetFileNameWithoutExtension(lURL.Text) + ".zip");
                File.Copy(lURL.Text, Path.GetFileNameWithoutExtension(lURL.Text) + ".zip", true);
                DoTheRest();
            }
            else
            {
                using (WebClient wc = new WebClient())
                {
                    toolStripStatusLabel1.Text = "Downloading...";
                    bDownload.Enabled = false;
                    wc.DownloadFileCompleted += new AsyncCompletedEventHandler(fake_DoTheRest);
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    //Download link goes here
                    Uri url = new Uri(lURL.Text);
                    wc.DownloadFileAsync(url, lModID.Text + ".zip");
                }
            }
        }

        private void fake_DoTheRest(object sender, AsyncCompletedEventArgs e)
        {
            DoTheRest();
        }

        private void DoTheRest()
        {
            toolStripStatusLabel1.Text = "Extracting downloaded archive...";
            
            string mod_archive = lModID.Text + ".zip";
            if (local)
            { mod_archive = Path.GetFileNameWithoutExtension(lURL.Text) + ".zip"; }

            string mod_dir = "extracted_mod";
            if (Directory.Exists(mod_dir))
            {
                Directory.Delete(mod_dir, true);
            }
            ExtractArchive(mod_archive);

            toolStripStatusLabel1.Text = "Checking extracted files...";
            int cont = CheckFiles(mod_dir);

            if (cont == -1) { Application.Exit(); }

            toolStripStatusLabel1.Text = "Finding root directories...";
            string[] mod_roots = FindRootDirs(mod_dir);

            if (mod_roots.Length > 1)
            {
                TooManyMods tmm_form = new TooManyMods(mod_roots);
                tmm_form.ShowDialog();

                mod_roots = tmm_form.mods;
            }

            toolStripStatusLabel1.Text = "Installing downloaded mod...";

            foreach (string mod in mod_roots)
            {
                string dest = Path.Combine("mods", Path.GetFileName(mod));
                if (Directory.Exists(dest)) { Directory.Delete(dest, true); }
                CopyAll(mod, dest);
            }

            DFtEM enable_mod = new DFtEM();
            enable_mod.ShowDialog();

            Directory.Delete("extracted_mod", true);
            if (!local) { File.Delete(mod_archive); }
            Application.Exit();


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

            toolStripStatusLabel1.Text = "Downloading... (" + e.BytesReceived/divider + unit + " / " + e.TotalBytesToReceive/divider + unit + ")";
            Console.WriteLine(e.BytesReceived);
            Console.WriteLine(e.TotalBytesToReceive);
        }
    }
}
