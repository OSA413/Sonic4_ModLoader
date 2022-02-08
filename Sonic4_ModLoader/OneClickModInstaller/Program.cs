using System;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    static class Program
    {
        public static HandlerInstallerWrapper hiWrapper = new HandlerInstallerWrapper();

        [STAThread]
        static void Main(string[] args)
        {
            ArgsHandler.Handle(args);
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.SetCompatibleTextRenderingDefault(false);
            Settings.Load();
            Application.Run(new UltimateWinForm());
        }
    }
}
