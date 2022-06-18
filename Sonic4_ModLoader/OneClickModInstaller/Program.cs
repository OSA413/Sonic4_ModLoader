using System;
using System.IO;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    static class Program
    {
        public static HandlerInstallerWrapper hiWrapper = new HandlerInstallerWrapper();

        [STAThread]
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
            ArgsHandler.Handle(args);
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.SetCompatibleTextRenderingDefault(false);
            Settings.Load();
            Application.Run(new UltimateWinForm());
        }
    }
}
