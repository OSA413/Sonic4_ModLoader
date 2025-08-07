using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Common.Mods;
using Common.Launcher;
using Common.MyIO;

namespace Sonic4ModManager
{
    public partial class MainForm:Form
    {
        private Random rnd = new();
        private Dictionary<string, Mod> ModsDict = new();

        public void RefreshMods()
        {
            bOpenExplorer.Enabled = Directory.Exists("mods");
            listMods.Items.Clear();
            ModsDict.Clear();
            
            foreach (var mod in Mods.GetMods())
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
            var checkedMods = new List<string>();
            for (int i = 0; i < listMods.Items.Count; i++)
                if (listMods.Items[i].Checked)
                    checkedMods.Add(listMods.Items[i].SubItems[3].Text);

            Directory.CreateDirectory("mods");
            File.WriteAllLines(Path.Combine("mods","mods.ini"), checkedMods.ToArray());
        }

        private void ChangePriority(int direction)
        {
            for (int i = 0; i < listMods.SelectedIndices.Count; i++)
            {
                var index = listMods.SelectedIndices[i];
                var insertTo = index;

                switch (direction)
                {
                    case -1: if (index != 0) insertTo = index - 1; break;
                    case  1: if (index != listMods.Items.Count - 1) insertTo = index + 1; break;
                    case -2: insertTo = 0; break;
                    case  2: insertTo = listMods.Items.Count - 1; break;
                }
                listMods.MoveItem(index, insertTo);                
            }
            listMods.Select();

            if (listMods.Items.Count > 0)
                listMods.EnsureVisible(listMods.SelectedIndices[0]);
        }

        public void RandomMods()
        {
            foreach (ListViewItem o in listMods.Items)
            {
                o.Checked = rnd.Next(2) == 1;
                if (o.Checked)
                    listMods.MoveItem(o.Index, rnd.Next(listMods.Items.Count));
            }

            foreach (ListViewItem o in listMods.Items)
                if (o.Checked)
                    listMods.MoveItem(o.Index, 0);
        }

        public MainForm(string[] args)
        {
            InitializeComponent();
            this.Text = "Sonic 4 Mod Loader [Version: " + Settings.ModLoaderVersion + "]";
            RefreshMods();
            
            var whatsNew = "Select a mod to read its description\n\n[c][b][i]What's new:[\\i][\\b]\n";
            if (File.Exists("Mod Loader - Whats new.txt"))
                whatsNew += File.ReadAllText("Mod Loader - Whats new.txt");
            else
                whatsNew += "File \"Mod Loader - Whats new.txt\" not found.";
            whatsNew += "\n\nHome page: https://github.com/OSA413/Sonic4_ModLoader";

            rtb_mod_description.Text = whatsNew;
            rtb_mod_description.Format();

            //The call after 1CMI installation
            if (args.Length == 1)
            {
                for (int i = 0; i < listMods.Items.Count; i++)
                {
                    if (listMods.Items[i].SubItems[3].Text == args[0])
                    {
                        listMods.Items[i].Selected = true;                
                        listMods.EnsureVisible(i);
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
                MyDirectory.OpenInFileManager("mods");
            else
                bOpenExplorer.Enabled = false;
        }

        public void LinkClicked(object sender, LinkClickedEventArgs e) => Process.Start(e.LinkText);
        private void listMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listMods.SelectedItems.Count == 0) return;
            rtb_mod_description.Text = ModsDict[listMods.Items[listMods.SelectedIndices[0]].SubItems[3].Text].Description;
            rtb_mod_description.Format();
        }
    }
}
