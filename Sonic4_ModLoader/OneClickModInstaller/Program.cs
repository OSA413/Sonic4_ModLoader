using System;
using System.Collections.Generic;
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
            if (args.Length > 0)
            {
                if ((args.Length == 1 && (args[0].StartsWith("sonic4mmep1:") || args[0].StartsWith("sonic4mmep2:")))
                    || (args.Length == 2 && args[0] == "--local"))
                {
                    Application.Run(new Form1(args));
                }
                else
                {
                    Application.Run(new Install(args));
                }
            }
            else
            {
                Application.Run(new Install(args));
            }
        }
    }
}
