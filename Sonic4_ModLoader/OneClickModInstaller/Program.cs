using System;
using System.Windows.Forms;

using Common.Launcher;

namespace OneClickModInstaller
{
    static class Program
    {
        public static HandlerInstallerWrapper hiWrapper = new HandlerInstallerWrapper();

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                GAME? game = null;
                if (args.Length > 1) game = Launcher.GetGameFromShort(args[1]);

                switch (args[0])
                {
                    case "--install":   hiWrapper.Install(game);   Environment.Exit(0); break;
                    case "--uninstall": hiWrapper.Uninstall(game); Environment.Exit(0); break;
                    case "--fix":       hiWrapper.FixPath(game);   Environment.Exit(0); break;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UltimateWinForm(args));
        }
    }
}