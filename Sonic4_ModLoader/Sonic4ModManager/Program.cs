using System;
using System.IO;
using System.Windows.Forms;

namespace Sonic4ModManager
{
    //TODO: move common parts to a separated file
    public static class MyDirectory
    {
        public static void CopyAll(string source, string destination)
        {
            Directory.CreateDirectory(destination);

            foreach (string file in Directory.GetFiles(source))
            {
                File.Copy(file, Path.Combine(destination, Path.GetFileName(file)), true);
                File.SetAttributes(file, FileAttributes.Normal);
            }

            foreach (string dir in Directory.GetDirectories(source))
            {
                string dir_name = Path.GetFileName(dir);
                Directory.CreateDirectory(Path.Combine(destination, dir_name));
                MyDirectory.CopyAll(Path.Combine(source, dir_name), Path.Combine(destination, dir_name));
            }
        }
    }

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
            Application.Run(new MainForm(args));
        }
    }
}
