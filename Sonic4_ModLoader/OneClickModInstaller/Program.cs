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

using Common.MyIO;
using Common.Launcher;

namespace OneClickModInstaller
{
    public static class Admin
    {
        private static bool? isAdmin;

        public static bool AmI()
        {
            if (isAdmin == null)
            {
                var id = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(id);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            return (bool)isAdmin;
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

    public static class Reg
    {
        public static void FixPath(GAME? game = null)
        {
            if (game == null) game = Launcher.GetCurrentGame();
            if (game == GAME.Unknown) return;
            var shrt = Launcher.GetShortGame(game);

            if (Admin.AmI())
                Registry.SetValue("HKEY_CLASSES_ROOT\\sonic4mm" + shrt + "\\Shell\\Open\\Command",
                    "", "\"" + Assembly.GetEntryAssembly().Location + "\" \"%1\"");
            else
                Admin.RunAs("--fix " + shrt);
        }

        public static void Install(GAME? game = null)
        {
            if (game == null) game = Launcher.GetCurrentGame();
            if (game == GAME.Unknown) return;
            var shrt = Launcher.GetShortGame(game);
            
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (Admin.AmI())
                {
                    var root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + shrt;

                    Registry.SetValue(root_key, "", "URL:OSA413's One-Click Mod Installer protocol");
                    Registry.SetValue(root_key, "URL Protocol", "");
                    Registry.SetValue(root_key + "\\DefaultIcon", "", "OneClickModInstaller.exe");
                    Registry.SetValue(root_key + "\\Shell\\Open\\Command", "", "\"" + Assembly.GetEntryAssembly().Location + "\" \"%1\"");
                }
                else
                    Admin.RunAs("--install " + shrt);
            }

            //Not tested, redo
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var desktopFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.local/share/applications/sonic4mm" + shrt +".desktop";

                File.WriteAllText(desktopFile
                                , "[Desktop Entry]"
                                + "\nType=Application"
                                + "\nExec=mono \"" + Application.ExecutablePath + "\" %U"
                                + "\nStartupNotify=true"
                                + "\nTerminal=false"
                                + "\nMimeType=x-scheme-handler/sonic4mm" + shrt
                                + "\nName=One-Click Mod Installer"
                                + "\nComment=OSA413's One-Click Mod Installer");

                Process.Start("xdg-mime", "default sonic4mm" + shrt +".desktop x-scheme-handler/sonic4mm" + shrt +".desktop").WaitForExit();
            }
        }

        public static void Uninstall(GAME? game = null)
        {
            if (game == null) game = Launcher.GetCurrentGame();
            if (game == GAME.Unknown) return;
            var shrt = Launcher.GetShortGame(game);

            if (Reg.InstallationStatus()[shrt] == 0)
                return;
            
            var root_key = "sonic4mm" + shrt;
            if (Admin.AmI())
            {
                if (Registry.ClassesRoot.OpenSubKey(root_key) != null)
                    Registry.ClassesRoot.DeleteSubKeyTree(root_key);
            }
            else
                Admin.RunAs("--uninstall " + shrt);
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

            foreach (var game in games)
            {
                var status = 0;
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    var root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;

                    if ((string)Registry.GetValue(root_key, "", null) == "URL:OSA413's One-Click Mod Installer protocol")
                        if ((string)Registry.GetValue(root_key, "URL Protocol", null) == "")
                            if ((string)Registry.GetValue(root_key + "\\DefaultIcon", "", null) == "OneClickModInstaller.exe")
                                if ((string)Registry.GetValue(root_key + "\\Shell\\Open\\Command", "", null) == "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\"")
                                    status = 1;
                                else status = 2;
                            else status = -1;
                        else status = -1;
                }

                else if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    var desktop_file = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.local/share/applications/sonic4mm" + game +".desktop";
                    var process = Process.Start(new ProcessStartInfo
                    {
                        FileName = "xdg-mime",
                        Arguments = "query default x-scheme-handler/sonic4mm" + game,
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    });

                    var output = process.StandardOutput.ReadToEnd();

                    status = -1;
                    if (output == "sonic4mm" + game + ".desktop\n")
                        if (File.Exists(desktop_file))
                            foreach (string line in File.ReadAllLines(desktop_file))
                                if (line.StartsWith("Exec="))
                                {
                                    status = 2;
                                    if (line == "Exec=mono \"" + Application.ExecutablePath + "\" %U")
                                        status = 1;
                                    break;
                                }
                }
                statuses.Add(game, status);
            }
            
            return statuses;
        }

        public static Dictionary<string, string> InstallationLocation()
        {
            var locations = new Dictionary<string, string> { { "dunno", "" } };

            string[] games = { "ep1", "ep2" };

            foreach (var game in games)
            {
                string location = null;
                switch ((int) Environment.OSVersion.Platform)
                {
                    //Windows
                    case 2:
                    {
                        var root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;
                        location = (string)Registry.GetValue(root_key + "\\Shell\\Open\\Command", "", null);

                        if (location != null)
                            location = location.Substring(1, location.Length - 7);
                    } break;

                    //Linux
                    case 4:
                    {
                        var desktop_file = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.local/share/applications/sonic4mm" + game +".desktop";
                        
                        if (File.Exists(desktop_file))
                            foreach (string line in File.ReadAllLines(desktop_file))
                                if (line.StartsWith("Exec="))
                                    location = line.Substring(12, line.Length - 16);
                    } break;
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
            var reqiest = WebRequest.Create(url);
            var response = reqiest.GetResponse();
            var redir = response.ResponseUri;
            response.Close();
            return redir.ToString();
        }
    }

    public static class ModArchive
    {
        public static bool IsFSCS()
        {
            //This checks if file system is case-sensitive
            File.WriteAllText("case_sensitivity_test", "");
            var res = !File.Exists("CASE_SENSITIVITY_TEST");
            File.Delete("case_sensitivity_test");
            return res;
        }

        public static void Extract(string file, string path_to_7z = "7z.exe")
        {
            //Need 7-zip to work
            if (!File.Exists(path_to_7z))
            {
                //Try to use bundled copy if local not found
                if (File.Exists("7z.exe"))
                    path_to_7z = "7z.exe";
                else
                    return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = path_to_7z,
                Arguments = "x \"" + file + "\" -o\"" + file + "_extracted" + "\""
            };
            Process.Start(startInfo).WaitForExit();
        }

        public static int CheckFiles(string dir_name)
        {
            var good_formats = "TXT,INI,DDS,TXB,AMA,AME,ZNO,ZNM,ZNV,DC,EV,RG,MD,MP,AT,DF,DI,PSH,VSH,LTS,XNM,MFS,SSS,GPB,MSG,AYK,ADX,AMB,CPK,CSB,PNG,CT,TGA".Split(',');

            var all_files = Directory.GetFiles(dir_name, "*", SearchOption.AllDirectories);
            var suspicious_files = new List<string>();

            foreach (string file in all_files)
            {
                var file_short = file.Substring(dir_name.Length + 1);

                if (int.TryParse(Path.GetFileName(file_short), out int n) && file_short.Contains(Path.Combine("DEMO", "WORLDMAP", "WORLDMAP.AMB")))
                    continue;
 
                var extension_len = Path.GetExtension(file_short).Length;
                if (extension_len != 0) extension_len = 1;

                if (good_formats.Contains(Path.GetExtension(file_short).Substring(extension_len), StringComparer.OrdinalIgnoreCase))
                    continue;

                suspicious_files.Add(file_short);
            }


            var cont = 1;
            if (suspicious_files.Count != 0)
            {
                cont = 0;
                Suspicious SuspiciousDialog = new Suspicious(suspicious_files.ToArray());

                DialogResult result = SuspiciousDialog.ShowDialog();

                //Continue
                if (result != DialogResult.Cancel)
                    cont = 1;

                if (result == DialogResult.Yes)
                    foreach (string file in suspicious_files)
                        MyFile.DeleteAnyway(Path.Combine(dir_name, file));
            }
            return cont;
        }

        public static Tuple<string[], string> FindRoot(string dir_name)
        {
            var platform = "???";
            var mod_roots = new List<string>();

            var platforms = new string[] { "pc", "dolphin", "modloader" };
            var game_folders_array = new string[] { "CUTSCENE,DEMO,G_COM,G_SS,G_EP1COM,G_EP1ZONE2,G_EP1ZONE3,G_EP1ZONE4,G_ZONE1,G_ZONE2,G_ZONE3,G_ZONE4,G_ZONEF,MSG,NNSTDSHADER,SOUND"
                                                        , "WSNE8P,WSNP8P,WSNJ8P"
                                                        , "Sonic4ModLoader"};

            for (int i = 0; i < platforms.Length; i++)
            {
                var game_folders = game_folders_array[i].Split(',');

                foreach (var folder in game_folders)
                {
                    foreach (var mod_folder in Directory.GetDirectories(dir_name, folder, SearchOption.AllDirectories))
                    {
                        var tmp_root = Path.GetDirectoryName(mod_folder);
                        if (!mod_roots.Contains(tmp_root))
                            mod_roots.Add(tmp_root);
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
            var file_roots = new List<string>();
            var type = "???";

            var types      = new string[] { "CheatTables" };
            var extensions = new string[] { "ct" };

            for (int i = 0; i < types.Length; i++)
            {
                if (type == "mixed") break;
                string[] extensions_to_find = extensions[i].Split(',');

                foreach (string extension in extensions_to_find)
                {
                    if (type == "mixed") break;
                    foreach (string mod_file in Directory.GetFiles(dir_name, "*." + extension, SearchOption.AllDirectories))
                    {
                        if (type == "???")    type = types[i];
                        if (type != types[i]) { type = "mixed"; break; }

                        var tmp_root = Path.GetDirectoryName(mod_file);
                        if (!file_roots.Contains(tmp_root))
                            file_roots.Add(tmp_root);
                    }
                }
            }
            if (type == "mixed")
                file_roots.RemoveRange(0, file_roots.Count);

            return Tuple.Create(file_roots.ToArray(), type);
        }

        //Copies everything from source to dest
        public static void CopyAll(string source, string destination)
        {
            if (ModArchive.IsFSCS())
                if (source == destination) return;
            else
                if (source.ToLower() == destination.ToLower()) return;

            Directory.CreateDirectory(destination);

            foreach (var file in Directory.GetFiles(source))
            {
                File.Copy(file, Path.Combine(destination, Path.GetFileName(file)), true);
                File.SetAttributes(file, FileAttributes.Normal);
            }

            foreach (var dir in Directory.GetDirectories(source))
            {
                var dir_name = Path.GetFileName(dir);
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
                GAME? game = null;
                if (args.Length > 1) game = Launcher.GetGameFromShort(args[1]);

                switch (args[0])
                {
                    case "--install":   Reg.Install(game);   Environment.Exit(0); break;
                    case "--uninstall": Reg.Uninstall(game); Environment.Exit(0); break;
                    case "--fix":       Reg.FixPath(game);   Environment.Exit(0); break;
                }
            }

            //This will fix problems when you launch 1CMI from terminal
            if (!Application.ExecutablePath.Contains(Path.Combine("bin", "Debug")))
                Environment.CurrentDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            Application.Run(new UltimateWinForm(args));
        }
    }
}