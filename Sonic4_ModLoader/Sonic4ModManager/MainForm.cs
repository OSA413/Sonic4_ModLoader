using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Common.Mods;
using Common.Launcher;

namespace Sonic4ModManager
{
    public partial class MainForm:Form
    {
        
        private Random rnd = new Random();
        private List<Mod> CurrentMods;
        private Dictionary<string, Mod> ModsDict = new Dictionary<string, Mod>();

        public void RefreshMods()
        {
            bOpenExplorer.Enabled = Directory.Exists("mods");
            listMods.Items.Clear();
            CurrentMods = Mods.GetMods();

            ModsDict.Clear();
            foreach (var mod in CurrentMods)
            {
                ModsDict[mod.Path] = mod;

                var item = new ListViewItem(mod.Name);
                item.SubItems.Add(mod.Authors);
                item.SubItems.Add(mod.Version);
                item.SubItems.Add(mod.Path);
                item.Checked = mod.Enabled;
                listMods.Items.Add(item);
            }
        }

        public void Save()
        {
            Directory.CreateDirectory("mods");
            var checkedMods = new List<string>();
            for (int i = 0; i < listMods.Items.Count; i++)
                if (listMods.Items[i].Checked)
                    checkedMods.Add(listMods.Items[i].SubItems[3].Text);

            File.WriteAllLines(Path.Combine("mods","mods.ini"), checkedMods.ToArray());
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
                    case -2: listMods.MoveItem(ind, 0); break;
                    case  2: listMods.MoveItem(ind, listMods.Items.Count - 1); break;
                }
            }
            listMods.Select();

            if (listMods.Items.Count > 0)
                listMods.EnsureVisible(listMods.SelectedIndices[0]);
        }

        public void RandomMods()
        {
            for (int i = 0; i < listMods.Items.Count; i++)
            {
                listMods.Items[i].Checked = Convert.ToBoolean(rnd.Next(2));
                listMods.MoveItem(i, rnd.Next(listMods.Items.Count));
            }

            //Randomize enabled mods
            for (int i = 0; i < listMods.Items.Count; i++)
                if (listMods.Items[i].Checked)
                    listMods.MoveItem(i, 0);
        }

        public MainForm(string[] args)
        {
            InitializeComponent();
            RefreshMods();
            
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

        private void bRefresh_Click(object sender, EventArgs e) => RefreshMods();
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
            { 
                Process.Start(e.LinkText); 
            }
            catch (Exception err)
            {
                MessageBox.Show("Clicking on the link raised the following exception:\n\n"+err.Message,
                                "Watch out! You are going to crash!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }
        
        private void listMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listMods.SelectedItems.Count == 0) return;
            rtb_mod_description.Text = ModsDict[listMods.Items[listMods.SelectedIndices[0]].SubItems[3].Text].Description;
            rtb_mod_description.Format();
        }
    }
}
