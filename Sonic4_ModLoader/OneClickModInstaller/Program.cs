using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 1)
            {
                if (args[0].StartsWith("sonic4mmep1:") ||
                    args[0].StartsWith("sonic4mmep2:"))
                {
                    Application.Run(new DownloadForm(args));
                }
                else if (File.Exists(args[0]))
                {
                    Application.Run(new DownloadForm(new string[] { "--local", args[0] }));
                }
                else
                { Application.Run(new InstallationForm(args)); }
            }
            else
            {
                Application.Run(new InstallationForm(args));
            }
        }
    }
}