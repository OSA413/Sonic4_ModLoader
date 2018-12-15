using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace OneClickModInstaller
{
    public partial class DownloadForm : Form
    {
        public static bool local { set; get; }

        public DownloadForm(string[] args)
        {
            bool install = false;

            if (args.Length == 1)
            {
                if (args[0] == "--install")
                {
                    install = true;
                }
            }

            if (args.Length == 0 || install)
            {
                InstallationForm install_form = new InstallationForm(args);
                install_form.ShowDialog();
                Application.Exit();
            }
            else
            {
                InitializeComponent();
                toolStripStatusLabel1.Text = "A wild download button appeared!";

                local = false;

                if (args.Length == 1)
                {
                    string[] gb_parameters = args[0].Split(',');
                    if (gb_parameters.Length > 0) { lURL.Text = gb_parameters[0]; }
                    if (gb_parameters.Length > 1) { lType.Text = gb_parameters[1]; }
                    if (gb_parameters.Length > 2) { lModID.Text = gb_parameters[2]; }
                }
                else if (args.Length == 2)
                {
                    if (args[0] == "--local")
                    {
                        local = true;
                        lURL.Text = args[1];
                    }
                }
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
                string dir_name = Path.GetDirectoryName(dir);
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
                Arguments = "x " + file_name + " -o" + "extracted_mod"
            };
            Process.Start(startInfo).WaitForExit();
        }
        
        private int CheckFiles(string dir_name)
        {
            string[] good_formats = "TXT,INI,DDS,TXB,AMA,AME,ZNO,TXB,ZNM,ZNV,DC,EV,RG,MD,MP,AT,DF,DI,PSH,VSH,LTS,XNM,MFS,SSS,GPB,MSG,AYK".Split(',');

            string[] all_files = Directory.GetFiles(dir_name, "*", SearchOption.AllDirectories);
            List<string> suspicious_files = new List<string>();

            for (int i = 0; i < all_files.Length; i++)
            {
                bool does_it_end = false;

                if (int.TryParse(Path.GetFileName(all_files[i]), out int n) && all_files[i].Contains("DEMO\\WORLDMAP\\WORLDMAP.AMB"))
                {
                    does_it_end = true;
                }

                for (int j = 0; j < good_formats.Length; j++)
                {
                    if (all_files[i].ToUpper().EndsWith("." + good_formats[j]))
                    {
                        does_it_end = true;
                    }
                    if (does_it_end) { break; }
                }

                if (!does_it_end)
                {
                    suspicious_files.Add(all_files[i]);
                }
            }

            int cont = 0;
            if (suspicious_files.Count > 0)
            {
                Suspicious SuspiciousDialog = new Suspicious();

                foreach (string file in suspicious_files)
                {
                    SuspiciousDialog.listView1.Items.Add(file);
                }

                DialogResult result = SuspiciousDialog.ShowDialog();

                //Continue
                if (result == DialogResult.Yes)
                {
                    cont = 1;
                }
                //Delete those files and Continue
                else if (result == DialogResult.OK)
                {
                    foreach (string file in suspicious_files)
                    {
                        File.Delete(file);
                    }
                    cont = 1;
                }
                else {cont = -1;}
            }
            return cont;
        }

        static string FindRootDirectory(string dir_name)
        {
            string root_name = "???";
            List<string> mod_possible_root = new List<string>();
            string[] game_folders = "CUTSCENE,DEMO,G_COM,G_SS,G_EP1COM,G_EP1ZONE2,G_EP1ZONE3,G_EP1ZONE4,G_ZONE1,G_ZONE2,G_ZONE3,G_ZONE4,G_ZONEF,MSG,NNSTDSHADER,SOUND".Split(',');

            foreach (string folder in game_folders)
            {
                foreach (string mod_folder in Directory.GetDirectories(dir_name, folder, SearchOption.AllDirectories))
                {
                    string tmp_root = Path.GetDirectoryName(mod_folder);
                    if (!mod_possible_root.Contains(tmp_root))
                    {
                        mod_possible_root.Add(tmp_root);
                    }
                }
            }

            if (mod_possible_root.Count == 1)
            {
                root_name = mod_possible_root[0];
            }

            return root_name;
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
                File.Copy(lURL.Text, lModID.Text + ".zip");
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

            string mod_arc = lModID.Text + ".zip";

            string mod_dir = "extracted_mod";
            if (Directory.Exists(mod_dir))
            {
                Directory.Delete(mod_dir, true);
            }
            ExtractArchive(mod_arc);

            toolStripStatusLabel1.Text = "Checking extracted files...";
            int cont = CheckFiles(mod_dir);

            if (cont == -1) { Application.Exit(); }

            toolStripStatusLabel1.Text = "Trying to find mod root directory...";
            string mod_root = FindRootDirectory(mod_dir);

            if (mod_root != "???")
            {
                Console.WriteLine(mod_root);

                string mod_root_name = mod_root.Split(Path.DirectorySeparatorChar)[mod_root.Split(Path.DirectorySeparatorChar).Length - 1];

                toolStripStatusLabel1.Text = "Installing downloaded mod...";
                CopyAll(mod_root, Path.Combine("mods", mod_root_name));

                DFtEM enable_mod = new DFtEM();
                enable_mod.ShowDialog();

                Directory.Delete("extracted_mod", true);
                File.Delete(mod_arc);
                Application.Exit();
            }
            else
            {
                //replace with a dialog to choose which mods to install
                //WrongModStructureForm wmsf = new WrongModStructureForm();
                //wmsf.ShowDialog();

                Application.Exit();
            }
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
