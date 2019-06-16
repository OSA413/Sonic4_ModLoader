using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;

namespace OneClickModInstaller
{
    ///////////////////////
    //Installation things//
    ///////////////////////

    public static class Admin
    {
        public static bool AmI()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void RestartAs(string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = AppDomain.CurrentDomain.FriendlyName,
                Arguments = args,
                Verb = "runas"
            };

            Process.Start(startInfo);
            Application.Exit();
        }
    }

    public static class GetGame
    {
        public static string Full()
        {
            string where = "dunno";

            if (File.Exists("Sonic_vis.exe") && File.Exists("SonicLauncher.exe"))
            { where = "Episode 1"; }
            else if (File.Exists("Sonic.exe") && File.Exists("Launcher.exe"))
            { where = "Episode 2"; }

            return where;
        }

        public static string Short()
        {
            switch (GetGame.Full())
            {
                case "Episode 1": return "ep1";
                case "Episode 2": return "ep2";
                default: return GetGame.Full();
            }
        }
    }

    public static class Reg
    {
        public static void FixPath()
        {
            if (Admin.AmI())
            {
                string game = GetGame.Short();

                if (game != "dunno")
                {
                    string root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;

                    Registry.SetValue(root_key + "\\Shell\\Open\\Command", "", "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\"");
                }
            }
            else
            {
                Admin.RestartAs("--fix");
            }


        }

        public static void Install()
        {
            if (Admin.AmI())
            {
                string game = GetGame.Short();

                if (game != "dunno")
                {
                    string root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;

                    Registry.SetValue(root_key, "", "URL:Sonic 4 Mod Loader's 1-Click Installer protocol");
                    Registry.SetValue(root_key, "URL Protocol", "");
                    Registry.SetValue(root_key + "\\DefaultIcon", "", "OneClickModInstaller.exe");
                    Registry.SetValue(root_key + "\\Shell\\Open\\Command", "", "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\"");
                }
            }
            else
            {
                Admin.RestartAs("--install");
            }
        }

        public static void Uninstall()
        {
            if (Admin.AmI())
            {
                string game = GetGame.Short();

                if (game != "dunno")
                {
                    string root_key = "sonic4mm" + game;

                    if (Registry.ClassesRoot.OpenSubKey(root_key) != null)
                    { Registry.ClassesRoot.DeleteSubKeyTree(root_key); }
                }
            }
            else
            {
                Admin.RestartAs("--uninstall");
            }
        }

        public static int InstallationStatus()
        {
            /* status description
             * 0    = Not installed
             * 1    = Properly installed
             * -1   = Improperly installed (something is not installed)
             * 2    = Another installation present (different path in registry)
             */

            int status = 0;

            string game = GetGame.Short();

            if (game != "dunno")
            {
                string root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;

                if ((string)Registry.GetValue(root_key, "", null) == "URL:Sonic 4 Mod Loader's 1-Click Installer protocol")
                {
                    if ((string)Registry.GetValue(root_key, "URL Protocol", null) == "")
                    {
                        if ((string)Registry.GetValue(root_key + "\\DefaultIcon", "", null) == "OneClickModInstaller.exe")
                        {
                            if ((string)Registry.GetValue(root_key + "\\Shell\\Open\\Command", "", null) == "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\"")
                            {
                                status = 1;
                            }
                            else { status = 2; }
                        }
                        else { status = -1; }
                    }
                    else { status = -1; }
                }
            }

            return status;
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

            //This will fix problems when you launch 1CMI from terminal
            if (!Application.ExecutablePath.Contains(Path.Combine("bin", "Debug")))
            {
                Environment.CurrentDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            }

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
            { Application.Run(new InstallationForm(args)); }
        }
    }
}