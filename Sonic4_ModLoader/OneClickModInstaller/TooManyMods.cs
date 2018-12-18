using System;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    public partial class TooManyMods : Form
    {
        public string[] mods { get; set; }

        public TooManyMods(string[] args)
        {
            InitializeComponent();
            foreach (string file in args)
            {
                checkedListBox1.Items.Add(file, true);
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
