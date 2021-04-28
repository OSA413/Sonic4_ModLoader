using System;
using System.IO;
using System.Windows.Forms;

namespace Sonic4ModManager
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args.Length == 2 && args[0] == "--upgrade" && Directory.Exists(args[1]))
                    Installation.Upgrade(args[1]);

                if (args[0] == "--install")
                    Installation.Install();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Installation.GetInstallationStatus() == Installation.Status.FirstLaunch)
                new FirstLaunch().ShowDialog();

            Settings.Load();
            Application.Run(new MainForm(args));
        }
    }
}
