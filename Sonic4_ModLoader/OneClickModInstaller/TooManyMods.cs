using System;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    public partial class TooManyMods : Form
    {
        public string[] mods;

        public TooManyMods(string[] list, ModType type = ModType.PC)
        {
            InitializeComponent();
            foreach (var file in list)
                checkedListBox1.Items.Add(file, true);

            label1.Text = label1.Text.Replace("{0}", type == ModType.PC ? "mods" : "texture mods");
        }

        private void bContinue_Click(object sender, EventArgs e)
        {
            mods = new string[checkedListBox1.CheckedItems.Count];
            for (int i = 0; i < mods.Length; i++ )
                mods[i] = checkedListBox1.CheckedItems[i].ToString();
        }
    }
}
