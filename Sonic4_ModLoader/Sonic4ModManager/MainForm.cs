using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Sonic4ModManager
{
    public partial class MainForm : Form
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

                    string ini_path = "mods" + Path.DirectorySeparatorChar + dir_names[i] + Path.DirectorySeparatorChar + "mod.ini";

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

        static void Play()
        {
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
                Thread.Sleep(1);
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

        public MainForm()
        {
            if (!File.Exists("mod_manager.cfg"))
            {
                FirstLaunch f = new FirstLaunch();
                f.ShowDialog();
            }
            InitializeComponent();
            RefreshMods();
            SetModPriority();
        }
        
        private void bSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void bSaveAndPlay_Click(object sender, EventArgs e)
        {
            Save();
            Play();
            Application.Exit();
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
            About f = new About();
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

        private void listMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Do nothing if selected nothing (it also deselects when changing)
            if (listMods.SelectedItems.Count == 0) { return; }
            rtb_mod_description.Lines = listMods.Items[listMods.SelectedIndices[0]].SubItems[4].Text
                                        .Split(new[] {"\\n"}, StringSplitOptions.None);
        }
    }
}
