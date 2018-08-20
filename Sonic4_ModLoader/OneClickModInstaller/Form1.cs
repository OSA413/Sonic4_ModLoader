using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace OneClickModInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "A wild download button appeared!";
        }
        
        //From https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            // Check if the target directory exists, if not, create it.
            if (!Directory.Exists(target.FullName))
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
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
            string[] good_formats = "AMA,AMB,ADX,DDS,INI,TXT,TXB".Split(',');

            string[] all_files = Directory.GetFiles(dir_name, "*", SearchOption.AllDirectories);
            List<string> suspicious_files = new List<string>();

            for (int i = 0; i < all_files.Length; i++)
            {
                bool does_it_end = false;
                for (int j = 0; j < good_formats.Length; j++)
                {
                    if (all_files[i].ToUpper().EndsWith("." + good_formats[j]))
                    {
                        does_it_end = true;
                        break;
                    }
                }

                if (!does_it_end)
                {
                    suspicious_files.Add(all_files[i]);
                }
            }

            int cont = 0;
            if (suspicious_files.Count > 0)
            {
                Form2 SuspiciousDialog = new Form2();

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
            using (WebClient wc = new WebClient())
            {
                toolStripStatusLabel1.Text = "Downloading...";
                bDownload.Enabled = false;
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(DoTheRest);
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                //Download link goes here
                Uri url = new Uri(/* removed */);
                wc.DownloadFileAsync(url, lModID.Text + ".zip");
            }
        }

        private void DoTheRest(object sender, AsyncCompletedEventArgs e)
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

            if (cont == -1) {Application.Exit();}

            toolStripStatusLabel1.Text = "Trying to find mod root directory...";
            string mod_root = FindRootDirectory(mod_dir);

            if (mod_root != "???")
            {
                Console.WriteLine(mod_root);

                string mod_root_name = mod_root.Split(Path.DirectorySeparatorChar)[mod_root.Split(Path.DirectorySeparatorChar).Length - 1];

                toolStripStatusLabel1.Text = "Installing downloaded mod...";
                CopyAll(new DirectoryInfo(mod_root), new DirectoryInfo("mods" + Path.DirectorySeparatorChar + mod_root_name));

                DFtEM enable_mod = new DFtEM();
                enable_mod.ShowDialog();

                Directory.Delete("extracted_mod", true);
                File.Delete(mod_arc);
                Application.Exit();
            }
            else
            {
                WrongModStructureForm wmsf = new WrongModStructureForm();
                wmsf.ShowDialog();

                Application.Exit();
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;

            string unit;
            int divider;

            if (e.TotalBytesToReceive > 1024*1024*10)
            {unit = "MBs"; divider = 1024*1024;}
            else if (e.TotalBytesToReceive > 1024*10)
            {unit = "KBs"; divider = 1024;}
            else
            {unit = "Bytes"; divider = 1;}

            toolStripStatusLabel1.Text = "Downloading... (" + e.BytesReceived/divider + unit + " / " + e.TotalBytesToReceive/divider + unit + ")";
            Console.WriteLine(e.BytesReceived);
            Console.WriteLine(e.TotalBytesToReceive);
            //DoTheRest(lModID.Text + ".zip");
        }
    }
}
