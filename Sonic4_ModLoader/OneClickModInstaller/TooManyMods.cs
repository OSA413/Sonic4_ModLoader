using System;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    public partial class TooManyMods : Form
    {
        public string[] mods;

        public TooManyMods(string[] list, string type = "pc")
        {
            InitializeComponent();
            foreach (string file in list)
            {
                checkedListBox1.Items.Add(file, true);
            }

            switch (type)
            {
                case "pc":      label1.Text = label1.Text.Replace("{0}", "mods");         break;
                case "dolphin": label1.Text = label1.Text.Replace("{0}", "texture mods"); break;
                case "ct":      label1.Text = label1.Text.Replace("{0}", "cheat tables"); break;
            }
        }

        private void bContinue_Click(object sender, EventArgs e)
        {
            mods = new string[checkedListBox1.CheckedItems.Count];
            for (int i = 0; i < mods.Length; i++ )
            {
                mods[i] = checkedListBox1.CheckedItems[i].ToString();
            }
        }
    }
}
