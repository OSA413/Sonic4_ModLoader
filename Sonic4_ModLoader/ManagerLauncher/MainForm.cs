using System;
using System.Windows.Forms;

using Common.Launcher;

namespace ManagerLauncher
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();

        private void ClickExitWrapper(Func<bool> action) {
            if (action())
                Application.Exit();
        }

        private void bPlayClick(object sender, EventArgs e) => ClickExitWrapper(Launcher.LaunchGame);
        private void bConfClick(object sender, EventArgs e) => ClickExitWrapper(Launcher.LaunchConfig);
        private void bManagerClick(object sender, EventArgs e) => ClickExitWrapper(() => Launcher.LaunchModManager());
    }
}
