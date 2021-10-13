using System.Windows.Forms;

namespace OneClickModInstaller
{
    public partial class Suspicious : Form
    {
        public Suspicious(string[] args)
        {
            InitializeComponent();
            foreach (var file in args) listView1.Items.Add(file);
            //Set width to the longest item size
            listView1.Columns[0].Width = -1;
        }
    }
}
