using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OneClickModInstaller
{
    public static class Admin
    {
        public static bool AmI()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void RunAs(string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = AppDomain.CurrentDomain.FriendlyName,
                Arguments = args,
                Verb = "runas"
            };
            
            Process.Start(startInfo).WaitForExit();
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
        public static void FixPath(string game = null)
        {
            if (Admin.AmI())
            {
                if (game == null)    { game = GetGame.Short(); }
                if (game == "dunno") { return; }

                string root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;

                Registry.SetValue(root_key + "\\Shell\\Open\\Command", "", "\"" + Assembly.GetEntryAssembly().Location + "\" \"%1\"");
            }
            else
            {
                string extra_arg = "";
                if (game != null) { extra_arg = " " + game; }

                Admin.RunAs("--fix " + extra_arg);
            }
        }

        public static void Install(string game = null)
        {
            if (Admin.AmI())
            {
                if (game == null)    { game = GetGame.Short(); }
                if (game == "dunno") { return; }

                string root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;

                Registry.SetValue(root_key, "", "URL:Sonic 4 Mod Loader's 1-Click Installer protocol");
                Registry.SetValue(root_key, "URL Protocol", "");
                Registry.SetValue(root_key + "\\DefaultIcon", "", "OneClickModInstaller.exe");
                Registry.SetValue(root_key + "\\Shell\\Open\\Command", "", "\"" + Assembly.GetEntryAssembly().Location + "\" \"%1\"");

            }
            else
            {
                string extra_arg = "";
                if (game != null) { extra_arg = " " + game; }

                Admin.RunAs("--install " + extra_arg);
            }
        }

        public static void Uninstall(string game = null)
        {
            if (Admin.AmI())
            {
                if (game == null)    { game = GetGame.Short(); }
                if (game == "dunno") { return; }

                string root_key = "sonic4mm" + game;

                if (Registry.ClassesRoot.OpenSubKey(root_key) != null)
                { Registry.ClassesRoot.DeleteSubKeyTree(root_key); }
            }
            else
            {
                string extra_arg = "";
                if (game != null) { extra_arg = " " + game; }

                Admin.RunAs("--uninstall " + extra_arg);
            }
        }

        public static Dictionary<string, int> InstallationStatus()
        {
            /* status description
             * 0    = Not installed
             * 1    = Properly installed
             * -1   = Improperly installed (something is not installed)
             * 2    = Another installation present (different path in registry)
             */

            var statuses = new Dictionary<string, int> { { "dunno", 0 } };

            string[] games = { "ep1", "ep2"};

            foreach (string game in games)
            {
                int status = 0;
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

                statuses.Add(game, status);
            }
            
            return statuses;
        }

        public static Dictionary<string, string> InstallationLocation()
        {
            var locations = new Dictionary<string, string> { { "dunno", "" } };

            string[] games = { "ep1", "ep2" };

            foreach (string game in games)
            {
                string root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;
                string location = (string)Registry.GetValue(root_key + "\\Shell\\Open\\Command", "", null);

                if (location != null)
                {
                    location = location.Substring(1, location.Length - 7);
                }
                locations.Add(game, location);
            }

            return locations;
        }
    }

    public static class URL
    {
        public static string GetRedirect(string url)
        {
            var t = WebRequest.Create(url);
            var r = t.GetResponse();
            var y = r.ResponseUri;
            r.Close();
            return y.ToString();
        }
    }

    public static class MyFile
    {
        public static void DeleteAnyway(string file)
        {
            //Program crashes if it tries to delete a read-only file
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }
    }

    public static class MyDirectory
    {
        //Same reason as for MyFile.DeleteAnyway
        public static void DeleteRecursively(string dir)
        {
            foreach (string dirr in Directory.GetDirectories(dir))
            {
                MyDirectory.DeleteRecursively(dirr);
            }

            foreach (string file in Directory.GetFiles(dir))
            {
                MyFile.DeleteAnyway(file);
            }

            Directory.Delete(dir);
        }

        public static void OpenExplorer(string path)
        {
            if (File.Exists(path)) { path = Path.GetDirectoryName(path); }

            string local_explorer = "";
            switch ((int)Environment.OSVersion.Platform)
            {
                //Windows
                case 2: local_explorer = "explorer"; break;
                //Linux (with xdg)
                case 4: local_explorer = "xdg-open"; break;
                //MacOS (not tested)
                case 6: local_explorer = "open"; break;
            }

            Process.Start(local_explorer, path);
        }
    }

    public static class ModArchive
    {
        public static bool IsFSCS()
        {
            //This checks if file system is case-sensitive
            bool res;

            File.WriteAllText("case_sensitivity_test", "");
            res = !File.Exists("CASE_SENSITIVITY_TEST");
            File.Delete("case_sensitivity_test");
            return res;
        }

        public static void Extract(string file)
        {
            //Need 7-zip to work
            if (!File.Exists("7z.exe")) { return; }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "7z.exe",
                Arguments = "x \"" + file + "\" -o\"" + file + "_extracted" + "\""
            };
            Process.Start(startInfo).WaitForExit();
        }

        public static int CheckFiles(string dir_name)
        {
            string[] good_formats = "TXT,INI,DDS,TXB,AMA,AME,ZNO,ZNM,ZNV,DC,EV,RG,MD,MP,AT,DF,DI,PSH,VSH,LTS,XNM,MFS,SSS,GPB,MSG,AYK,ADX,AMB,CPK,CSB,PNG,CT".Split(',');

            string[] all_files = Directory.GetFiles(dir_name, "*", SearchOption.AllDirectories);
            List<string> suspicious_files = new List<string>();

            foreach (string file in all_files)
            {
                if (int.TryParse(Path.GetFileName(file), out int n) && file.Contains("DEMO\\WORLDMAP\\WORLDMAP.AMB"))
                {
                    continue;
                }

                int extension_len = Path.GetExtension(file).Length;
                if (extension_len != 0) { extension_len = 1; }

                if (good_formats.Contains(Path.GetExtension(file).Substring(extension_len), StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                suspicious_files.Add(file);
            }


            int cont = 1;
            if (suspicious_files.Count != 0)
            {
                cont = 0;
                Suspicious SuspiciousDialog = new Suspicious(suspicious_files.ToArray());

                DialogResult result = SuspiciousDialog.ShowDialog();

                //Continue
                if (result == DialogResult.Yes)
                {
                    cont = 1;
                }
            }
            return cont;
        }

        public static Tuple<string[], string> FindRoot(string dir_name)
        {
            string platform = "???";
            List<string> mod_roots = new List<string>();

            string[] platforms = new string[2] { "pc", "dolphin" };
            string[] game_folders_array = new string[2] { "CUTSCENE,DEMO,G_COM,G_SS,G_EP1COM,G_EP1ZONE2,G_EP1ZONE3,G_EP1ZONE4,G_ZONE1,G_ZONE2,G_ZONE3,G_ZONE4,G_ZONEF,MSG,NNSTDSHADER,SOUND"
                                                        , "WSNE8P,WSNP8P,WSNJ8P"};

            for (int i = 0; i < platforms.Length; i++)
            {
                string[] game_folders = game_folders_array[i].Split(',');

                foreach (string folder in game_folders)
                {
                    foreach (string mod_folder in Directory.GetDirectories(dir_name, folder, SearchOption.AllDirectories))
                    {
                        string tmp_root = Path.GetDirectoryName(mod_folder);
                        if (!mod_roots.Contains(tmp_root))
                        {
                            mod_roots.Add(tmp_root);
                        }
                    }
                }

                if (mod_roots.Count > 0)
                {
                    platform = platforms[i];
                    break;
                }
            }

            return Tuple.Create(mod_roots.ToArray(), platform);
        }

        public static Tuple<string[], string> FindFiles(string dir_name)
        {
            List<string> file_roots = new List<string>();
            string type = "???";

            string[] types      = new string[1] { "CheatTables" };
            string[] extensions = new string[1] { "ct" };

            for (int i = 0; i < types.Length; i++)
            {
                if (type == "mixed") { break; }
                string[] extensions_to_find = extensions[i].Split(',');

                foreach (string extension in extensions_to_find)
                {
                    if (type == "mixed") { break; }
                    foreach (string mod_file in Directory.GetFiles(dir_name, "*." + extension, SearchOption.AllDirectories))
                    {
                        if (type == "???")    { type = types[i]; }
                        if (type != types[i]) { type = "mixed"; break; }

                        string tmp_root = Path.GetDirectoryName(mod_file);
                        if (!file_roots.Contains(tmp_root))
                        {
                            file_roots.Add(tmp_root);
                        }
                    }
                }
            }
            if (type == "mixed") { file_roots.RemoveRange(0, file_roots.Count); }

            return Tuple.Create(file_roots.ToArray(), type);
        }

        public static void CopyAll(string source, string destination)
        {
            if (ModArchive.IsFSCS())
            { if (source == destination) { return; } }
            else { { if (source.ToLower() == destination.ToLower()) { return; } } }

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
                ModArchive.CopyAll(Path.Combine(source, dir_name), Path.Combine(destination, dir_name));
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

            //Now the 1CMI installation thing will be here
            if (args.Length > 0)
            {
                string extra_arg = null;
                if (args.Length > 1) { extra_arg = args[1]; }

                switch (args[0])
                {
                    case "--install":   Reg.Install(extra_arg);   Environment.Exit(0); break;
                    case "--uninstall": Reg.Uninstall(extra_arg); Environment.Exit(0); break;
                    case "--fix":       Reg.FixPath(extra_arg);   Environment.Exit(0); break;
                }
            }

            //This will fix problems when you launch 1CMI from terminal
            if (!Application.ExecutablePath.Contains(Path.Combine("bin", "Debug")))
            {
                Environment.CurrentDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            }

            Application.Run(new UltimateWinForm(args));
        }
    }
}