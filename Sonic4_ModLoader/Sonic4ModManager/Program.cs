using System;
using System.Windows.Forms;

namespace Sonic4ModManager
{
    static class Program
    {


        [STAThread]
        static void Main(string[] args)
        {
            Settings.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(args));
        }
    }
}
