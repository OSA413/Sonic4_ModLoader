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

        public static List<(string orig, string newName, bool modloaderFile)> GetInstallationInstructions()
        {
            var game = Launcher.GetCurrentGame();
            var renameList = new List<(string orig, string newName, bool modloaderFile)>();

            var originalExe = "";
            var originalLauncher = "";
            var patchLauncher = "PatchLauncher.exe";
            var managerLauncher = "ManagerLauncher.exe";

            if (game == GAME.Episode1)
            {
                originalExe = "Sonic_vis.exe";
                originalLauncher = "SonicLauncher.exe";
            }
            else if (game == GAME.Episode2)
            {
                originalExe = "Sonic.exe";
                originalLauncher = "Launcher.exe";
            }

            var saveFileOrig = Path.GetFileNameWithoutExtension(originalExe) + "_save.dat";
            var saveFileNew = Path.GetFileNameWithoutExtension(originalExe) + ".orig_save.dat";
            var originalExeBkp = Path.GetFileNameWithoutExtension(originalExe) + ".orig.exe";
            var originalLauncherBkp = Path.GetFileNameWithoutExtension(originalLauncher) + ".orig.exe";

            renameList.Add((originalExe, originalExeBkp, false));
            renameList.Add((patchLauncher, originalExe, true));
            renameList.Add((originalLauncher, originalLauncherBkp, false));
            renameList.Add((managerLauncher, originalLauncher, true));
            renameList.Add((saveFileOrig, saveFileNew, false));

            renameList.Add(("7z.exe", null, true));
            renameList.Add(("7z.dll", null, true));
            renameList.Add(("AMBPatcher.exe", null, true));
            renameList.Add(("CsbEditor.exe", null, true));
            renameList.Add(("ManagerLauncher.exe", null, true));
            renameList.Add(("Mod Loader - Whats new.txt", null, true));
            renameList.Add(("README.md", null, true));
            renameList.Add(("SonicAudioLib.dll", null, true));

            return renameList;
        }

        public static void Install()
        {
            var status = GetInstallationStatus();

            //Installation
            if (status == Status.NotInstalled || status == Status.FirstLaunch)
            {
                var instructions = GetInstallationInstructions();
                foreach (var i in instructions)
                    if (File.Exists(i.orig) && !File.Exists(i.newName))
                        File.Move(i.orig, i.newName);

                Settings.Save();
            }
        }

        public static void Uninstall(int options = 0)
        {
            var renameList = GetInstallationInstructions();
            renameList.Reverse();

            foreach (var i in renameList)
                if (File.Exists(i.newName) && !File.Exists(i.orig))
                    File.Move(i.newName, i.orig);

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
                foreach (var file in renameList)
                    if (file.modloaderFile && File.Exists(file.orig))
                        File.Delete(file.orig);

                if (Directory.Exists("Mod Loader - licenses"))
                    Directory.Delete("Mod Loader - licenses", true);

                if ((options & 2) != 0)
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

        public static void Upgrade(string dir_to_new_version, int options = 0)
        {
            string my_dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            if (Directory.Exists(dir_to_new_version))
            {
                var installFrom = dir_to_new_version;
                var status = GetInstallationStatus();
                var argInstall = "";

                if (status == Status.Installed)
                    argInstall = " --install";

                if (Directory.Exists(Path.Combine(dir_to_new_version, "Sonic4ModLoader")))
                    installFrom = Path.Combine(installFrom, "Sonic4ModLoader");

                Uninstall(0b10);

                var filesToMove = Directory.GetFileSystemEntries(installFrom).ToList();
                filesToMove.Remove(Path.Combine(installFrom, "Sonic4ModManager.exe"));

                foreach (var file in filesToMove)
                {
                    var my_file = Path.Combine(my_dir, Path.GetFileName(file));
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
                    "MOVE /Y \"" + installFrom + "\"\\Sonic4ModManager.exe \"" + my_dir + "\"\\Sonic4ModManager.exe",
                    "RMDIR /Q /S \"" + dir_to_new_version + "\"",
                    "START \"\" /D \"" + my_dir + "\" Sonic4ModManager.exe" + argInstall,
                    "DEL FinishUpgrade.bat"
                };
                File.WriteAllLines("FinishUpgrade.bat", bat);
                Process.Start("FinishUpgrade.bat");
                Environment.Exit(0);
            }
        }
    }
}
