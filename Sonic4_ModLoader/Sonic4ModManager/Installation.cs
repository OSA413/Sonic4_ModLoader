using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

using Common.MyIO;
using Common.Launcher;

namespace Sonic4ModManager
{
    class Installation
    {
        public enum Status
        {
            Installed,
            NotInstalled,
            FirstLaunch,
            NotGameDirectory
        }

        public static Status GetInstallationStatus()
        {
            var game = Launcher.GetCurrentGame();

            if (game == GAME.Unknown)
                return Status.NotGameDirectory;

            if (game == GAME.Episode1)
                if (File.Exists("Sonic_vis.orig.exe") && File.Exists("SonicLauncher.orig.exe"))
                    return Status.Installed;

            else if (game == GAME.Episode2)
                if (File.Exists("Sonic.orig.exe") && File.Exists("Launcher.orig.exe"))
                    return Status.Installed;

            if (!File.Exists("ModManager.cfg"))
                return Status.FirstLaunch;

            return Status.NotInstalled;
        }

        public static void Install(int whattodo, int options = 0)
        {
            //whattodo = 1 is install
            //whattodo = 0 is uninstall
            var status = GetInstallationStatus();
            var game = Launcher.GetCurrentGame();

            var rename_list = new List<string[]> { };

            string original_exe = "";
            string original_launcher = "";
            string save_file_orig = "";
            string save_file_new = "";
            string original_exe_bkp = "";
            string original_launcher_bkp = "";
            string patch_launcher = "PatchLauncher.exe";
            string manager_launcher = "ManagerLauncher.exe";

            switch (game)
            {
                case GAME.Episode1: original_exe = "Sonic_vis.exe"; original_launcher = "SonicLauncher.exe"; break;
                case GAME.Episode2: original_exe = "Sonic.exe"; original_launcher = "Launcher.exe"; break;
            }

            save_file_orig = Path.GetFileNameWithoutExtension(original_exe) + "_save.dat";
            save_file_new = Path.GetFileNameWithoutExtension(original_exe) + ".orig_save.dat";
            original_exe_bkp = Path.GetFileNameWithoutExtension(original_exe) + ".orig.exe";
            original_launcher_bkp = Path.GetFileNameWithoutExtension(original_launcher) + ".orig.exe";

            rename_list.Add(new string[] { original_exe, original_exe_bkp });
            rename_list.Add(new string[] { patch_launcher, original_exe });
            rename_list.Add(new string[] { original_launcher, original_launcher_bkp });
            rename_list.Add(new string[] { manager_launcher, original_launcher });
            rename_list.Add(new string[] { save_file_orig, save_file_new });

            //Installation
            if ((status == Status.NotInstalled || status == Status.FirstLaunch) && whattodo == 1)
            {
                for (int i = 0; i < rename_list.Count; i++)
                    if (File.Exists(rename_list[i][0]) && !File.Exists(rename_list[i][1]))
                        File.Move(rename_list[i][0], rename_list[i][1]);

                Settings.Save();
            }

            //Uninstallation
            else if (whattodo == 0)
            {
                rename_list.Reverse();
                for (int i = 0; i < rename_list.Count; i++)
                {
                    Console.WriteLine(rename_list[i][1]);
                    Console.WriteLine(rename_list[i][0]);
                    if (File.Exists(rename_list[i][1]) && !File.Exists(rename_list[i][0]))
                        File.Move(rename_list[i][1], rename_list[i][0]);
                }

                Settings.Save();

                //Options

                //Recover original files
                if ((options & 1) != 0)
                {
                    Process.Start("AMBPatcher.exe", "recover").WaitForExit();

                    if ((options & 2) != 0)
                        if (Directory.Exists("mods_sha"))
                            Directory.Delete("mods_sha", true);
                }

                //Uninstall and remove OCMI
                if ((options & 4) != 0)
                {
                    if (File.Exists("OneClickModInstaller.exe"))
                    {
                        Process.Start("OneClickModInstaller.exe", "--uninstall").WaitForExit();
                        File.Delete("OneClickModInstaller.exe");
                    }

                    if (File.Exists("OneClickModInstaller.cfg"))
                        File.Delete("OneClickModInstaller.cfg");
                }

                //Delete Mod Loader files
                if ((options & 2) != 0)
                {
                    var to_delete_list =
                        new List<string> {"7z.exe",
                                            "7z.dll",
                                            "AMBPatcher.exe",
                                            "AMBPatcher.log",
                                            "CsbEditor.exe",
                                            "ManagerLauncher.exe",
                                            "Mod Loader - Whats new.txt",
                                            "PatchLauncher.exe",
                                            "README.rtf",
                                            "README.md",
                                            "SonicAudioLib.dll"};

                    //Delete config
                    if ((options & 8) == 0)
                    {
                        to_delete_list.AddRange(new string[] { "ModManager.cfg"
                                                                ,"AMBPatcher.cfg"
                                                                ,"CsbEditor.exe.config"});
                        if ((options & 4) != 0)
                            to_delete_list.Add("OneClickModInstaller.cfg");
                    }

                    foreach (string file in to_delete_list)
                        if (File.Exists(file))
                            File.Delete(file);

                    if (Directory.Exists("Mod Loader - licenses"))
                        Directory.Delete("Mod Loader - licenses", true);

                    //Sonic4ModManager.exe
                    if ((options & 16) != 0)
                    {
                        //The only (easy and fast) way to delete an open program is to create a .bat file
                        //that deletes the .exe file and itself.
                        string[] bat =
                        {
                            "taskkill /IM Sonic4ModManager.exe /F",
                            "DEL Sonic4ModManager.exe",
                            "DEL FinishInstallation.bat"
                        };
                        File.WriteAllLines("FinishInstallation.bat", bat);

                        Process.Start("FinishInstallation.bat");
                        Environment.Exit(0);
                    }
                }
            }
        }

        public static void Upgrade(string dir_to_new_version, int options = 0)
        {
            string my_dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            if (Directory.Exists(dir_to_new_version))
            {
                string install_from = dir_to_new_version;
                var status = GetInstallationStatus();
                string arg_install = "";

                if (status == Status.Installed)
                    arg_install = " --install";

                if (Directory.Exists(Path.Combine(dir_to_new_version, "Sonic4ModLoader")))
                    install_from = Path.Combine(install_from, "Sonic4ModLoader");

                Install(0, 0b10);

                var files_to_move = Directory.GetFileSystemEntries(install_from).ToList();
                files_to_move.Remove(Path.Combine(install_from, "Sonic4ModManager.exe"));

                foreach (string file in files_to_move)
                {
                    string my_file = Path.Combine(my_dir, Path.GetFileName(file));
                    if (File.Exists(file))
                    {
                        if (File.Exists(my_file))
                            File.Delete(my_file);
                        File.Move(file, my_file);
                    }
                    else
                    {
                        MyDirectory.CopyAll(file, my_file);
                        Directory.Delete(file, true);
                    }
                }

                string[] bat =
                {
                    "taskkill /IM Sonic4ModManager.exe /F",
                    "MOVE /Y \"" + install_from + "\"\\Sonic4ModManager.exe \"" + my_dir + "\"\\Sonic4ModManager.exe",
                    "RMDIR /Q /S \"" + dir_to_new_version + "\"",
                    "START \"\" /D \"" + my_dir + "\" Sonic4ModManager.exe" + arg_install,
                    "DEL FinishUpgrade.bat"
                };
                File.WriteAllLines("FinishUpgrade.bat", bat);
                Process.Start("FinishUpgrade.bat");
                Environment.Exit(0);
            }
        }
    }
}
