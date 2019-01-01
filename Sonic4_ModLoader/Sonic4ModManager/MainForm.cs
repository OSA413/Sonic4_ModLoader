using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Sonic4ModManager
{
    public partial class MainForm:Form
    {
        public void RefreshMods()
        {
            listMods.Items.Clear();

            var mod_list = GetMods();

            for (int i = 0; i < mod_list.Count; i++)
            {
                ListViewItem item = new ListViewItem(mod_list[i].Item1); //Mod Name
                item.SubItems.Add(mod_list[i].Item2); //Mod Authors
                item.SubItems.Add(mod_list[i].Item3); //Mod Version
                item.SubItems.Add(mod_list[i].Item4); //Mod Directory
                item.SubItems.Add(mod_list[i].Item5); //Description
                listMods.Items.Add(item);
            }

        }

        public void SetModPriority()
        {
            if (Directory.Exists("mods"))
            {
                bOpenExplorer.Enabled = true;
            }
            else
            {
                bOpenExplorer.Enabled = false;
            }

            if (File.Exists(@"mods\mods.ini"))
            {
                string[] ini_preority = File.ReadAllLines(@"mods\mods.ini");

                for (int i = 0; i < ini_preority.Length; i++)
                {
                    int folder_ind = -1;
                    for (int j = 0; j < listMods.Items.Count; j++)
                    {
                        if (ini_preority[i] == listMods.Items[j].SubItems[3].Text)
                        {
                            folder_ind = j;
                            break;
                        }
                    }

                    if (folder_ind != -1)
                    {
                        var tmp_item = listMods.Items[folder_ind];
                        listMods.Items.RemoveAt(folder_ind);
                        listMods.Items.Insert(0, tmp_item);
                        listMods.Items[0].Checked = true;
                    }
                }
            }
        }

        static List<Tuple<string, string, string, string, string>> GetMods()
        {
            var mod_list = new List<Tuple<string, string, string, string, string>>();

            if (Directory.Exists("mods"))
            {
                string[] dir_names = Directory.GetDirectories("mods");

                for (int i = 0; i < dir_names.Length; i++)
                {
                    dir_names[i] = dir_names[i].Split(Path.DirectorySeparatorChar)[1];
                }

                for (int i = 0; i < dir_names.Length; i++)
                {
                    string mod_name = dir_names[i];
                    string mod_authors = "???";
                    string mod_version = "???";
                    string mod_desctiption = "No description.";

                    string ini_path = Path.Combine("mods", dir_names[i], "mod.ini");

                    if (File.Exists(ini_path))
                    {
                        string[] ini_file = File.ReadAllLines(ini_path);
                        for (int j = 0; j < ini_file.Length; j++)
                        {
                            if (ini_file[j].StartsWith("Name="))
                            {
                                mod_name = String.Join("=", ini_file[j].Split('=').Skip(1));
                            }
                            else if (ini_file[j].StartsWith("Authors="))
                            {
                                mod_authors = String.Join("=", ini_file[j].Split('=').Skip(1));
                            }
                            else if (ini_file[j].StartsWith("Version="))
                            {
                                mod_version = String.Join("=", ini_file[j].Split('=').Skip(1));
                            }
                            else if (ini_file[j].StartsWith("Description="))
                            {
                                mod_desctiption = String.Join("=", ini_file[j].Split('=').Skip(1));
                            }
                        }
                    }
                    mod_list.Add(Tuple.Create(mod_name, mod_authors, mod_version, dir_names[i], mod_desctiption));
                }
            }
            return mod_list;
        }

        public void Save()
        {
            List<string> checked_mods = new List<string>();
            for (int i = 0; i < listMods.Items.Count; i++)
            {
                if (listMods.Items[i].Checked)
                {
                    checked_mods.Insert(0, listMods.Items[i].SubItems[3].Text);
                }
            }
            if (!Directory.Exists("mods"))
            {
                Directory.CreateDirectory("mods");
            }
            File.WriteAllText(@"mods\mods.ini", string.Join("\n", checked_mods.ToArray()));
        }

        static bool Play()
        {
            bool IsThisStarted = true;
            //Episode 1
            if (File.Exists("Sonic_vis.exe"))
            {
                Process.Start("Sonic_vis.exe");
            }
            //Episode 2
            else if (File.Exists("Sonic.exe"))
            {
                Process.Start("Sonic.exe");
            }
            else
            {
                IsThisStarted = false;
            }

            return IsThisStarted;
        }
        
        private void ChangePriority(string direction)
        {
            if (listMods.SelectedIndices.Count > 0)
            {
                int ind = listMods.SelectedIndices[0];

                //TODO: Write everything once
                if (direction == "u")
                {
                    if (ind != 0)
                    {
                        var tmp_item = listMods.Items[ind];
                        listMods.Items.RemoveAt(ind);
                        listMods.Items.Insert(ind - 1, tmp_item);
                    }
                }
                else if (direction == "d")
                {
                    var tmp_item = listMods.Items[ind];
                    listMods.Items.RemoveAt(ind);
                    if (ind == listMods.Items.Count)
                    {
                        listMods.Items.Add(tmp_item);
                    }
                    else
                    {
                        listMods.Items.Insert(ind + 1, tmp_item);
                    }
                }
                else if (direction == "f")
                {
                    var tmp_item = listMods.Items[ind];
                    listMods.Items.RemoveAt(ind);
                    listMods.Items.Insert(0, tmp_item);
                }
                else if (direction == "l")
                {
                    var tmp_item = listMods.Items[ind];
                    listMods.Items.RemoveAt(ind);
                    listMods.Items.Add(tmp_item);
                }
                listMods.Select();
            }
        }

        public void RandomMods()
        {
            Random rnd = new Random();
            for (int i = 0; i < listMods.Items.Count; i++)
            {
                var tmp = listMods.Items[rnd.Next(listMods.Items.Count)];
                Thread.Sleep(1); //I know that using it is not the best solution
                listMods.Items.Remove(tmp);
                listMods.Items.Insert(rnd.Next(listMods.Items.Count), tmp);
                Thread.Sleep(1); //But they are at least more random
            }

            for (int i = 0; i < listMods.Items.Count; i++)
            {
                //I would use Convert.ToBoolean(rnd.Next(2)), but I think it'll be slower
                listMods.Items[i].Checked = rnd.Next(2) == 1;
                Thread.Sleep(1);
            }

            //Final sort
            for (int i = 0; i < listMods.Items.Count; i++)
            {
                if (listMods.Items[i].Checked)
                {
                    var tmp_item = listMods.Items[i];
                    listMods.Items.RemoveAt(i);
                    listMods.Items.Insert(0, tmp_item);
                }
            }
        }

        static string WhereAmI()
        {
            string where = "dunno";

            if (File.Exists("Sonic_vis.exe") && File.Exists("SonicLauncher.exe"))
            {
                where = "Episode 1";
            }
            else if (File.Exists("Sonic.exe") && File.Exists("Launcher.exe"))
            {
                where = "Episode 2";
            }

            return where;
        }

        static string GetGame()
        {
            string game = WhereAmI();

            switch (WhereAmI())
            {
                case "Episode 1":
                    game = "ep1";
                    break;
                case "Episode 2":
                    game = "ep2";
                    break;
            }

            return game;
        }

        public static int GetInstallationStatus()
        {
            /* status description
             * -2   = This directory is not not a game directory
             * -1   = First launch (.cfg file not present)
             * 0    = Not installed
             * 1    = Installed
             */


            int status = -2;

            string game = GetGame();

            if (game != "dunno")
            {
                if (File.Exists("mod_manager.cfg"))
                {
                    string tmp_status = File.ReadAllText("mod_manager.cfg");
                    if (tmp_status != "")
                    {
                        if (int.TryParse(tmp_status, out int n))
                        {
                            status = Convert.ToInt32(tmp_status);
                        }
                    }
                    else
                    {
                        status = 0;
                        if (game == "ep1")
                        {
                            if (File.Exists("Sonic_vis.orig.exe"))
                            {
                                status = 1;
                            }

                        }
                        else if (game == "ep2")
                        {
                            if (File.Exists("Sonic.orig.exe"))
                            {
                                status = 1;
                            }
                        }
                    }
                }
                else
                {
                    status = -1;
                }
            }
            return status;
        }

        public static void Install(int whattodo, int options = 0)
        {
            //whattodo = 1 is install
            //whattodo = 0 is uninstall
            int status = GetInstallationStatus();
            string game = GetGame();

            //Installation
            if ((status == 0 || status == -1) && whattodo == 1)
            {
                //Episode 1
                if (game == "ep1")
                {
                    //We need to make sure that all the file required are present before installation
                    if (File.Exists("Sonic_vis.exe") && File.Exists("PatchLauncher.exe") &&
                        File.Exists("SonicLauncher.exe") && File.Exists("ManagerLauncher.exe"))
                    {
                        //Original game file
                        File.Move("Sonic_vis.exe", "Sonic_vis.orig.exe");
                        //PatchLauncher
                        File.Move("PatchLauncher.exe", "Sonic_vis.exe");
                        //Original launcher
                        File.Move("SonicLauncher.exe", "SonicLauncher.orig.exe");
                        //ManagerLauncher
                        File.Move("ManagerLauncher.exe", "SonicLauncher.exe");

                        //Saving info that installation has been finished
                        File.WriteAllText("mod_manager.cfg", "1");

                        //Renaming save file if present
                        if (File.Exists("Sonic_vis_save.dat") && !File.Exists("Sonic_vis.orig_save.dat"))
                        {
                            File.Move("Sonic_vis_save.dat", "Sonic_vis.orig_save.dat");
                        }
                    }
                }

                //Episode 2
                else if (game == "ep2")
                {
                    if (File.Exists("Sonic.exe") && File.Exists("PatchLauncher.exe") &&
                        File.Exists("Launcher.exe") && File.Exists("ManagerLauncher.exe"))
                    {
                        //Original game file
                        File.Move("Sonic.exe", "Sonic.orig.exe");
                        //PatchLauncher
                        File.Move("PatchLauncher.exe", "Sonic.exe");
                        //Original launcher
                        File.Move("Launcher.exe", "Launcher.orig.exe");
                        //ManagerLauncher
                        File.Move("ManagerLauncher.exe", "Launcher.exe");

                        //Saving info that installation has been finished
                        File.WriteAllText("mod_manager.cfg", "1");

                        //Renaming save file if present
                        if (File.Exists("Sonic_save.dat") && !File.Exists("Sonic.orig_save.dat"))
                        {
                            File.Move("Sonic_save.dat", "Sonic.orig_save.dat");
                        }
                    }
                }
            }

            //Uninstallation
            else if (status == 1 && whattodo == 0)
            {
                //Episode 1
                if (game == "ep1")
                {
                    if (File.Exists("Sonic_vis.exe") && File.Exists("Sonic_vis.orig.exe") &&
                        File.Exists("SonicLauncher.exe") && File.Exists("SonicLauncher.orig.exe"))
                    {
                        //ManagerLauncher
                        File.Move("SonicLauncher.exe", "ManagerLauncher.exe");
                        //Original launcher
                        File.Move("SonicLauncher.orig.exe", "SonicLauncher.exe");
                        //PatchLauncher
                        File.Move("Sonic_vis.exe", "PatchLauncher.exe");
                        //Original game file
                        File.Move("Sonic_vis.orig.exe", "Sonic_vis.exe");

                        //Saving info that installation has been finished
                        File.WriteAllText("mod_manager.cfg", "0");

                        //Renaming save file if present
                        if (File.Exists("Sonic_vis.orig_save.dat") && !File.Exists("Sonic_vis_save.dat"))
                        {
                            File.Move("Sonic_vis.orig_save.dat", "Sonic_vis_save.dat");
                        }
                    }
                }

                //Episode 2
                else if (game == "ep2")
                {
                    if (File.Exists("Sonic.exe") && File.Exists("Sonic.orig.exe") &&
                        File.Exists("Launcher.exe") && File.Exists("Launcher.orig.exe"))
                    {
                        //ManagerLauncher
                        File.Move("Launcher.exe", "ManagerLauncher.exe");
                        //Original launcher
                        File.Move("Launcher.orig.exe", "Launcher.exe");
                        //PatchLauncher
                        File.Move("Sonic.exe", "PatchLauncher.exe");
                        //Original game file
                        File.Move("Sonic.orig.exe", "Sonic.exe");

                        //Saving info that uninstallation has been finished
                        File.WriteAllText("mod_manager.cfg", "0");

                        //Renaming save file if present
                        if (File.Exists("Sonic.orig_save.dat") && !File.Exists("Sonic_save.dat"))
                        {
                            File.Move("Sonic.orig_save.dat", "Sonic_save.dat");
                        }
                    }
                }

                //Options

                //Recover original files
                if ((options & 1) == 1)
                {
                    Process.Start("AMBPatcher.exe", "recover").WaitForExit();
                    if ((options & 2) == 2)
                    {
                        if (Directory.Exists("mods_sha"))
                        Directory.Delete("mods_sha", true);
                    }
                }

                //Delete Mod Loader files
                {
                    if ((options & 2) == 2)
                    {
                        foreach (string file in new string[] { "7z.exe", "7z.dll",
                                                           "AMBPatcher.exe", "CsbEditor.exe",
                                                           "LICENSE-Sonic4_ModLoader",
                                                           "LICENSE-Sonic4_ModLoader_files",
                                                           "LICENSE-SonicAudioTools",
                                                           "LICENSE-SonicAudioTools_files",
                                                           "ManagerLauncher.exe",
                                                           "Mod Loader - Whats new.txt",
                                                           "PatchLauncher.exe",
                                                           "README.rtf", "README.txt",
                                                           "README-tldr.txt",
                                                           "SonicAudioLib.dll",
                                                           "LICENSE-7-Zip",
                                                           "LICENSE-7-Zip_files",
                                                           "mod_manager.cfg",
                                                           "AMBPatcher.cfg"})
                        {
                            if (File.Exists(file))
                            {
                                File.Delete(file);
                            }
                        }

                        //Sonic4ModManager.exe
                        //The only (easy and fast) way to delete an open program is to create a .bat file
                        //that deletes the .exe file and itself.
                        string bat = "taskkill /IM Sonic4ModManager.exe /F\n" +
                                     "DEL Sonic4ModManager.exe\n" +
                                     "DEL tmp.bat";
                        File.WriteAllText("tmp.bat", bat);

                        Process.Start("tmp.bat");
                    }
                }
            }
        }

        public MainForm(string[] args)
        {
            if (GetInstallationStatus() == -1)
            {
                FirstLaunch f = new FirstLaunch();
                f.ShowDialog();
            }
            InitializeComponent();
            RefreshMods();
            SetModPriority();

            if (args.Length == 1)
            {
                for (int i = 0; i < listMods.Items.Count; i++)
                {
                    if (listMods.Items[i].SubItems[3].Text == args[0])
                    {
                        listMods.Items[i].Selected = true;
                        listMods.TopItem = listMods.Items[i];
                        listMods.Select();
                        break;
                    }
                }
            }
            
            //TOP SECRET EASTER EGG
            string today = System.DateTime.Now.ToString("dd.MM");
            if (new string[] {
                "01.01", //New Year
                "13.01", //OSA413's BD
                "19.01", //Sonic 4:1's release day
                "29.02", //Leap year day
                "15.05", //Sonic 4:2's release day
                "23.06", //Sonic's BD
                "31.12"  //New Year
            }.Contains(today))
            { bRandom.Text = "I'm Feeling Lucky"; }

            string whats_new = "\n\n[c][b][i]What's new:[\\i][\\b]\n";
            if (File.Exists("Mod Loader - Whats new.txt"))
            {
                whats_new += File.ReadAllText("Mod Loader - Whats new.txt");
            }
            else
            {
                whats_new += "File \"Mod Loader - Whats new.txt\" not found.";
            }

            whats_new += "\n\nHome page: https://github.com/OSA413/Sonic4_ModLoader";

            rtb_mod_description.Text += whats_new;
            
            FormatDescription();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void bSaveAndPlay_Click(object sender, EventArgs e)
        {
            Save();
            if (Play())
            {
                Application.Exit();
            }
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshMods();
            SetModPriority();
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bPriorityUp_Click(object sender, EventArgs e)
        {
            ChangePriority("u");
        }

        private void bPriorityDown_Click(object sender, EventArgs e)
        {
            ChangePriority("d");
        }

        private void bAbout_Click(object sender, EventArgs e)
        {
            Settings f = new Settings();
            f.ShowDialog();
        }

        private void bPriorityFirst_Click(object sender, EventArgs e)
        {
            ChangePriority("f");
        }

        private void bPriorityLast_Click(object sender, EventArgs e)
        {
            ChangePriority("l");
        }

        private void bRandom_Click(object sender, EventArgs e)
        {
            RandomMods();
        }

        private void bOpenExplorer_Click(object sender, EventArgs e)
        {
            if (Directory.Exists("mods"))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "explorer",
                    Arguments = "mods"
                };
                Process.Start(startInfo);
            }
            else
            {
                bOpenExplorer.Enabled = false;
            }
        }

        public static void LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            { Process.Start(e.LinkText); }
            catch (Exception err)
            { MessageBox.Show("Clicking on the link raised following exception:\n\n"+err.Message, "Watch out! You are going to crash!"); }
        }

        public void FormatDescription()
        {
            //Newline character
            rtb_mod_description.Text = rtb_mod_description.Text.Replace("\\n", "\n");

            //Tab character
            rtb_mod_description.Text = rtb_mod_description.Text.Replace("\\t", "\t");

            //Bullet character at the biginning of a line
            rtb_mod_description.Text = rtb_mod_description.Text.Replace("\n* ", "\n • ");

            //Description from mod.ini
            foreach (string i in new string[] { "b", "i", "u", "strike" })
            {
                while (rtb_mod_description.Text.Contains("[" + i + "]"))
                {
                    //Getting the list of all [i] and [\i]
                    int ind = 0;

                    List<int> end_lst = new List<int> { };
                    while (rtb_mod_description.Text.Substring(ind).Contains("[\\" + i + "]"))
                    {
                        end_lst.Add(rtb_mod_description.Text.Substring(ind).IndexOf("[\\" + i + "]") + ind);
                        ind = end_lst[end_lst.Count - 1] + 1;
                    }

                    int start_ind = rtb_mod_description.Text.IndexOf("[" + i + "]");

                    //Formating the original text
                    if (end_lst.Count == 0)
                    {
                        end_lst.Add(rtb_mod_description.Text.Length);
                    }
                    foreach (int j in end_lst)
                    {
                        if (j > start_ind)
                        {
                            for (int k = 0; k < j - start_ind; k++)
                            {
                                rtb_mod_description.Select(start_ind + k, 1);

                                if (i == "b")
                                { rtb_mod_description.SelectionFont = new Font(rtb_mod_description.SelectionFont, FontStyle.Bold | rtb_mod_description.SelectionFont.Style); }
                                else if (i == "i")
                                { rtb_mod_description.SelectionFont = new Font(rtb_mod_description.SelectionFont, FontStyle.Italic | rtb_mod_description.SelectionFont.Style); }
                                else if (i == "u")
                                { rtb_mod_description.SelectionFont = new Font(rtb_mod_description.SelectionFont, FontStyle.Underline | rtb_mod_description.SelectionFont.Style); }
                                else if (i == "strike")
                                { rtb_mod_description.SelectionFont = new Font(rtb_mod_description.SelectionFont, FontStyle.Strikeout | rtb_mod_description.SelectionFont.Style); }
                            }

                            //Remember folks, you can't delete any text (= "") if the richtextbox is ReadOnly
                            //I don't know why, but you can replace text this way.
                            rtb_mod_description.ReadOnly = false;
                            rtb_mod_description.Select(j, 3 + i.Length);
                            rtb_mod_description.SelectedText = "";
                            rtb_mod_description.Select(start_ind, 2 + i.Length);
                            rtb_mod_description.SelectedText = "";
                            rtb_mod_description.ReadOnly = true;

                            break;
                        }
                    }
                }
            }

            //Text alignment
            while (rtb_mod_description.Text.Contains("[l]") ||
                   rtb_mod_description.Text.Contains("[c]") ||
                   rtb_mod_description.Text.Contains("[r]"))
            {
                int ind_l = rtb_mod_description.Text.IndexOf("[l]");
                if (ind_l == -1) { ind_l = rtb_mod_description.Text.Length; }
                int ind_c = rtb_mod_description.Text.IndexOf("[c]");
                if (ind_c == -1) { ind_c = rtb_mod_description.Text.Length; }
                int ind_r = rtb_mod_description.Text.IndexOf("[r]");
                if (ind_r == -1) { ind_r = rtb_mod_description.Text.Length; }

                //Nearest tag
                int ind = Math.Min(Math.Min(ind_l, ind_c), ind_r);
                string tag = rtb_mod_description.Text[ind+1].ToString();


                rtb_mod_description.Select(ind, 3);

                if (tag == "c")
                { rtb_mod_description.SelectionAlignment = HorizontalAlignment.Center; }
                else if (tag == "r")
                { rtb_mod_description.SelectionAlignment = HorizontalAlignment.Right; }
                else
                { rtb_mod_description.SelectionAlignment = HorizontalAlignment.Left; }

                rtb_mod_description.ReadOnly = false;
                rtb_mod_description.SelectedText = "";
                rtb_mod_description.ReadOnly = true;
            }
        }

        private void listMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Do nothing if nothing selected (it also deselects when changing)
            if (listMods.SelectedItems.Count == 0) { return; }

            //Updating description
            rtb_mod_description.Text = listMods.Items[listMods.SelectedIndices[0]].SubItems[4].Text;

            //Updating description if there's a description link
            if (rtb_mod_description.Text.StartsWith("file="))
            {
                string desciption_file = Path.Combine("mods", listMods.Items[listMods.SelectedIndices[0]].SubItems[3].Text, rtb_mod_description.Text.Substring(5));

                if (File.Exists(desciption_file))
                {
                    if (desciption_file.EndsWith(".TXT", StringComparison.OrdinalIgnoreCase))
                    {
                        rtb_mod_description.Text = File.ReadAllText(desciption_file);
                    }
                    else
                    {
                        rtb_mod_description.Text = "Error: unsupported format of \"" + desciption_file + "\" file.";
                    }
                }
                else
                {
                    rtb_mod_description.Text = "Error: \"" + desciption_file + "\" file not found.";
                }
            }
            FormatDescription();
        }
    }
}
