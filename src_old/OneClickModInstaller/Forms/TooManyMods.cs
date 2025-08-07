using System;
using System.Linq;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    public partial class TooManyMods : Form
    {
        public (string Root, ModType Type)[] mods;

        public TooManyMods((string, ModType)[] list)
        {
            mods = list;
            InitializeComponent();
            foreach (var file in list)
                checkedListBox1.Items.Add(file.Item1, true);
        }

        private void bContinue_Click(object sender, EventArgs e)
        {
            var cache = mods.ToDictionary(x => x.Root, x => x.Type);
            var result = new (string, ModType)[checkedListBox1.CheckedItems.Count];
            for (int i = 0; i < result.Length; i++)
            {
                var root = checkedListBox1.CheckedItems[i].ToString();
                result[i] = (root, cache[root]);
            }
        }
    }
}
