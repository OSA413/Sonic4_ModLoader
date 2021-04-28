﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using Common.Mods;
using Common.Ini;
using Common.MyIO;
using Common.Launcher;

namespace Sonic4ModManager
{
    public partial class MainForm:Form
    {
        public void RefreshMods()
        {
            listMods.Items.Clear();
            var mods = Mods.GetMods();
            foreach (var mod in mods)
            {
                var item = new ListViewItem(mod.Name); //Mod Name
                item.SubItems.Add(mod.Authors); //Mod Authors
                item.SubItems.Add(mod.Version); //Mod Version
                item.SubItems.Add(mod.Path); //Mod Directory
                item.SubItems.Add(mod.Description); //Description
                listMods.Items.Add(item);
            }
        }

        public void SetModPriority()
        {
            if (Directory.Exists("mods"))
                bOpenExplorer.Enabled = true;
            else
                bOpenExplorer.Enabled = false;

            if (File.Exists(@"mods\mods.ini"))
            {
                string[] ini_priority = File.ReadAllLines(@"mods\mods.ini");

                for (int i = 0; i < ini_priority.Length; i++)
                {
                    int folder_ind = -1;
                    for (int j = 0; j < listMods.Items.Count; j++)
                    {
                        if (ini_priority[i] == listMods.Items[j].SubItems[3].Text)
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

        public void Save()
        {
            var checked_mods = new List<string>();
            for (int i = 0; i < listMods.Items.Count; i++)
                if (listMods.Items[i].Checked)
                    checked_mods.Insert(0, listMods.Items[i].SubItems[3].Text);

            if (!Directory.Exists("mods"))
                Directory.CreateDirectory("mods");

            File.WriteAllLines(Path.Combine("mods","mods.ini"), checked_mods.ToArray());
        }

        private void ChangePriority(int direction)
        {
            for (int i = 0; i < listMods.SelectedIndices.Count; i++)
            {
                var ind = listMods.SelectedIndices[i];

                switch (direction)
                {
                    case -1: if (ind != 0) listMods.MoveItem(ind, ind - 1); break;
                    case  1: if (ind != listMods.Items.Count - 1) listMods.MoveItem(ind, ind + 1); break;
                    case -2: /**********************************/  listMods.MoveItem(ind, 0); break;
                    case  2: /**********************************/  listMods.MoveItem(ind, listMods.Items.Count - 1); break;
                }
            }
            listMods.Select();

            if (listMods.Items.Count > 0)
                listMods.EnsureVisible(listMods.SelectedIndices[0]);
        }

        public void RandomMods()
        {
            Random rnd = new Random();
            //Placement
            for (int i = 0; i < listMods.Items.Count; i++)
            {
                //I know that using it is not the best solution but they are at least more random
                //TODO: remove later
                Thread.Sleep(1);
                listMods.MoveItem(i, rnd.Next(listMods.Items.Count));
            }

            //Enabling
            for (int i = 0; i < listMods.Items.Count; i++)
            {
                Thread.Sleep(1);
                listMods.Items[i].Checked = Convert.ToBoolean(rnd.Next(2));
            }

            //Final sort
            for (int i = 0; i < listMods.Items.Count; i++)
                if (listMods.Items[i].Checked)
                    listMods.MoveItem(i, 0);
        }

        public static int GetInstallationStatus()
        {
            var game = Launcher.GetCurrentGame();
            if (game == GAME.Unknown) return -2;
            if (game == GAME.Episode1)
            {
                if (File.Exists("Sonic_vis.orig.exe")
                    && File.Exists("SonicLauncher.orig.exe"))
                    return 1;
            }
            else if (game == GAME.Episode2)
            {
                if (File.Exists("Sonic.orig.exe")
                    && File.Exists("Launcher.orig.exe"))
                    return 1;
            }

            if (!File.Exists("ModManager.cfg")) return -1; //First launch
            return 0;
        }

        public static void Install(int whattodo, int options = 0)
        {
            //whattodo = 1 is install
            //whattodo = 0 is uninstall
            int status = GetInstallationStatus();
            var game = Launcher.GetCurrentGame();

            var rename_list = new List<string[]> { };

            string original_exe = "";
            string original_launcher = "";
            string save_file_orig = "";
            string save_file_new = "";
            string original_exe_bkp = "";
            string original_launcher_bkp = "";
            string patch_launcher = "PatchLauncher.exe";
            string manager_launcher = "ManagerLauncher.exe";

            switch (game)
            {
                case GAME.Episode1: original_exe = "Sonic_vis.exe"; original_launcher = "SonicLauncher.exe"; break;
                case GAME.Episode2: original_exe = "Sonic.exe"; original_launcher = "Launcher.exe"; break;
            }

            save_file_orig = Path.GetFileNameWithoutExtension(original_exe) + "_save.dat";
            save_file_new = Path.GetFileNameWithoutExtension(original_exe) + ".orig_save.dat";
            original_exe_bkp = Path.GetFileNameWithoutExtension(original_exe) + ".orig.exe";
            original_launcher_bkp = Path.GetFileNameWithoutExtension(original_launcher) + ".orig.exe";

            rename_list.Add(new string[] { original_exe, original_exe_bkp });
            rename_list.Add(new string[] { patch_launcher, original_exe });
            rename_list.Add(new string[] { original_launcher, original_launcher_bkp });
            rename_list.Add(new string[] { manager_launcher, original_launcher });
            rename_list.Add(new string[] { save_file_orig, save_file_new });

            //Installation
            if ((status == 0 || status == -1) && whattodo == 1)
            {
                for (int i = 0; i < rename_list.Count; i++)
                    if (File.Exists(rename_list[i][0]) && !File.Exists(rename_list[i][1]))
                        File.Move(rename_list[i][0], rename_list[i][1]);

                Settings.Save();
            }

            //Uninstallation
            else if (whattodo == 0)
            {
                rename_list.Reverse();
                for (int i = 0; i < rename_list.Count; i++)
                {
                    Console.WriteLine(rename_list[i][1]);
                    Console.WriteLine(rename_list[i][0]);
                    if (File.Exists(rename_list[i][1]) && !File.Exists(rename_list[i][0]))
                        File.Move(rename_list[i][1], rename_list[i][0]);
                }

                Settings.Save();

                //Options

                //Recover original files
                if ((options & 1) != 0)
                {
                    Process.Start("AMBPatcher.exe", "recover").WaitForExit();

                    if ((options & 2) != 0)
                        if (Directory.Exists("mods_sha"))
                            Directory.Delete("mods_sha", true);
                }

                //Uninstall and remove OCMI
                if ((options & 4) != 0)
                {
                    if (File.Exists("OneClickModInstaller.exe"))
                    {
                        Process.Start("OneClickModInstaller.exe", "--uninstall").WaitForExit();
                        File.Delete("OneClickModInstaller.exe");
                    }

                    if (File.Exists("OneClickModInstaller.cfg"))
                        File.Delete("OneClickModInstaller.cfg");
                }

                //Delete Mod Loader files
                if ((options & 2) != 0)
                {
                    var to_delete_list =
                        new List<string> {"7z.exe",
                                            "7z.dll",
                                            "AMBPatcher.exe",
                                            "AMBPatcher.log",
                                            "CsbEditor.exe",
                                            "ManagerLauncher.exe",
                                            "Mod Loader - Whats new.txt",
                                            "PatchLauncher.exe",
                                            "README.rtf",
                                            "README.md",
                                            "SonicAudioLib.dll"};

                    //Delete config
                    if ((options & 8) == 0)
                    {
                        to_delete_list.AddRange(new string[] { "ModManager.cfg"
                                                                ,"AMBPatcher.cfg"
                                                                ,"CsbEditor.exe.config"});
                        if ((options & 4) != 0)
                            to_delete_list.Add("OneClickModInstaller.cfg");
                    }

                    foreach (string file in to_delete_list)
                        if (File.Exists(file))
                            File.Delete(file);

                    if (Directory.Exists("Mod Loader - licenses"))
                        Directory.Delete("Mod Loader - licenses", true);

                    //Sonic4ModManager.exe
                    if ((options & 16) != 0)
                    {
                        //The only (easy and fast) way to delete an open program is to create a .bat file
                        //that deletes the .exe file and itself.
                        string[] bat =
                        {
                            "taskkill /IM Sonic4ModManager.exe /F",
                            "DEL Sonic4ModManager.exe",
                            "DEL FinishInstallation.bat"
                        };
                        File.WriteAllLines("FinishInstallation.bat", bat);

                        Process.Start("FinishInstallation.bat");
                        Environment.Exit(0);
                    }
                }
            }
        }

        public static void Upgrade(string dir_to_new_version, int options = 0)
        {
            string my_dir = Path.GetDirectoryName(Application.ExecutablePath);
            if (Directory.Exists(dir_to_new_version))
            {
                string install_from = dir_to_new_version;
                int status = GetInstallationStatus();
                string arg_install = "";

                if (status == 1)
                    arg_install = " --install";

                if (Directory.Exists(Path.Combine(dir_to_new_version, "Sonic4ModLoader")))
                    install_from = Path.Combine(install_from, "Sonic4ModLoader");

                Install(0, 0b10);
                
                var files_to_move = Directory.GetFileSystemEntries(install_from).ToList();
                files_to_move.Remove(Path.Combine(install_from, "Sonic4ModManager.exe"));

                foreach (string file in files_to_move)
                {
                    string my_file = Path.Combine(my_dir, Path.GetFileName(file));
                    if (File.Exists(file))
                    {
                        if (File.Exists(my_file))
                            File.Delete(my_file);
                        File.Move(file, my_file);
                    }
                    else
                    {
                        MyDirectory.CopyAll(file, my_file);
                        Directory.Delete(file, true);
                    }
                }

                string[] bat =
                {
                    "taskkill /IM Sonic4ModManager.exe /F",
                    "MOVE /Y \"" + install_from + "\"\\Sonic4ModManager.exe \"" + my_dir + "\"\\Sonic4ModManager.exe",
                    "RMDIR /Q /S \"" + dir_to_new_version + "\"",
                    "START \"\" /D \"" + my_dir + "\" Sonic4ModManager.exe" + arg_install,
                    "DEL FinishUpgrade.bat"
                };
                File.WriteAllLines("FinishUpgrade.bat", bat);
                Process.Start("FinishUpgrade.bat");
                Environment.Exit(0);
            }
        }

        public MainForm(string[] args)
        {
            if (args.Length > 0)
            {
                if (args.Length > 1)
                    if (args[0] == "--upgrade" && Directory.Exists(args[1]))
                        Upgrade(args[1]);

                switch (args[0])
                {
                    case "--install": Install(1);   break;
                }
            }

            if (GetInstallationStatus() == -1)
                new FirstLaunch().ShowDialog();
            
            InitializeComponent();
            RefreshMods();
            SetModPriority();
            
            string whats_new = "\n\n[c][b][i]What's new:[\\i][\\b]\n";
            if (File.Exists("Mod Loader - Whats new.txt"))
                whats_new += File.ReadAllText("Mod Loader - Whats new.txt");
            else
                whats_new += "File \"Mod Loader - Whats new.txt\" not found.";

            whats_new += "\n\nHome page: https://github.com/OSA413/Sonic4_ModLoader";

            rtb_mod_description.Text += whats_new;

            rtb_mod_description.Format();

            //The call after 1CMI installation
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
        }

        private void bSave_Click(object sender, EventArgs e) => Save();
        private void bSaveAndPlay_Click(object sender, EventArgs e)
        {
            Save();
            if (Launcher.LaunchGame())
                Application.Exit();
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshMods();
            SetModPriority();
        }

        private void bExit_Click(object sender, EventArgs e) => Application.Exit();

        private void bPriorityUp_Click(object sender, EventArgs e) => ChangePriority(-1);
        private void bPriorityDown_Click(object sender, EventArgs e) => ChangePriority(1);
        private void bPriorityFirst_Click(object sender, EventArgs e) => ChangePriority(-2);
        private void bPriorityLast_Click(object sender, EventArgs e) => ChangePriority(2);

        private void bAbout_Click(object sender, EventArgs e) => new SettingsForm().ShowDialog();        
        private void bRandom_Click(object sender, EventArgs e) => RandomMods();

        private void bOpenExplorer_Click(object sender, EventArgs e)
        {
            if (Directory.Exists("mods"))
            {
                string local_explorer = "";
                switch ((int) Environment.OSVersion.Platform)
                {
                    //Windows
                    case 2: local_explorer = "explorer"; break;
                    //Linux (with xdg)
                    case 4: local_explorer = "xdg-open"; break;
                    //MacOS (not tested)
                    case 6: local_explorer = "open"; break;
                }

                Process.Start(local_explorer, "mods");
            }
            else
                bOpenExplorer.Enabled = false;
        }

        public void LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            { Process.Start(e.LinkText); }
            catch (Exception err)
            { MessageBox.Show("Clicking on the link raised the following exception:\n\n"+err.Message,
                                "Watch out! You are going to crash!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning); }
        }
        
        private void listMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Do nothing if nothing is selected (it also automatically deselects when changing)
            if (listMods.SelectedItems.Count == 0) return;

            //Updating description
            rtb_mod_description.Text = listMods.Items[listMods.SelectedIndices[0]].SubItems[4].Text;
            
            rtb_mod_description.Format();
        }
    }

    public static class Extensions
    {
        //Formats Rich Text Box
        //https://github.com/OSA413/Sonic4_ModLoader/blob/master/docs/Mod%20structure.md#description-formating
        public static void Format(this RichTextBox rtb)
        {
            //Remember folks, you can't delete any text if the richtextbox is ReadOnly
            rtb.ReadOnly = false;

            //Newline character
            rtb.Text = rtb.Text.Replace("\\n", "\n");

            //Tab character
            rtb.Text = rtb.Text.Replace("\\t", "\t");

            //Bullet character at the biginning of a line
            rtb.Text = rtb.Text.Replace("\n* ", "\n • ");

            //Description from mod.ini
            foreach (string i in new string[] { "b", "i", "u", "strike" })
            {
                while (rtb.Text.Contains("[" + i + "]"))
                {
                    //Getting the list of all [i] and [\i]
                    int ind = 0;

                    List<int> end_lst = new List<int> { };
                    while (rtb.Text.Substring(ind).Contains("[\\" + i + "]"))
                    {
                        end_lst.Add(rtb.Text.Substring(ind).IndexOf("[\\" + i + "]") + ind);
                        ind = end_lst[end_lst.Count - 1] + 1;
                    }

                    int start_ind = rtb.Text.IndexOf("[" + i + "]");

                    //Formating the original text
                    if (end_lst.Count == 0)
                    {
                        end_lst.Add(rtb.Text.Length);
                    }
                    foreach (int j in end_lst)
                    {
                        if (j > start_ind)
                        {
                            for (int k = 0; k < j - start_ind; k++)
                            {
                                rtb.Select(start_ind + k, 1);

                                FontStyle new_style = FontStyle.Regular;
                                switch (i)
                                {
                                    case "b":      new_style = FontStyle.Bold;      break;
                                    case "i":      new_style = FontStyle.Italic;    break;
                                    case "u":      new_style = FontStyle.Underline; break;
                                    case "strike": new_style = FontStyle.Strikeout; break;
                                }

                                rtb.SelectionFont = new Font(rtb.SelectionFont, new_style | rtb.SelectionFont.Style);
                            }

                            rtb.Select(j, 3 + i.Length);
                            rtb.SelectedText = "";
                            rtb.Select(start_ind, 2 + i.Length);
                            rtb.SelectedText = "";
                            
                            break;
                        }
                    }
                }
            }

            //Text alignment
            while (rtb.Text.Contains("[l]") ||
                   rtb.Text.Contains("[c]") ||
                   rtb.Text.Contains("[r]"))
            {
                int ind_l = rtb.Text.Contains("[l]") ? rtb.Text.IndexOf("[l]") : rtb.Text.Length;
                int ind_c = rtb.Text.Contains("[c]") ? rtb.Text.IndexOf("[c]") : rtb.Text.Length;
                int ind_r = rtb.Text.Contains("[r]") ? rtb.Text.IndexOf("[r]") : rtb.Text.Length;

                //Nearest tag
                int ind = Math.Min(Math.Min(ind_l, ind_c), ind_r);
                string tag = rtb.Text[ind+1].ToString();

                rtb.Select(ind, 3);

                switch(tag)
                {
                    case "c": rtb.SelectionAlignment = HorizontalAlignment.Center;  break;
                    case "r": rtb.SelectionAlignment = HorizontalAlignment.Right;   break;
                    default:  rtb.SelectionAlignment = HorizontalAlignment.Left;    break;
                }

                rtb.SelectedText = "";
            }

            rtb.ReadOnly = true;
        }

        //Moves a line with [index] in ListView to [insert_to] index
        public static void MoveItem(this ListView lv, int index, int insert_to)
        {
            var item = lv.Items[index];
            lv.Items.RemoveAt(index);
            if (insert_to >= lv.Items.Count)
                lv.Items.Add(item);
            else
                lv.Items.Insert(insert_to, item);
        }

    }
}
