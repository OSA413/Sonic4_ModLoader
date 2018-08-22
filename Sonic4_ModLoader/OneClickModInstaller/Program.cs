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
            if (args.Length == 0)
            {
                Application.Run(new Install(args));
            }
            else if (args.Length == 1 && (args[0] == "--install" || args[0] == "--uninstall"))
            {
                Application.Run(new Install(args));
            }
            else
            {
                Application.Run(new Form1(args));
            }
        }
    }
}
