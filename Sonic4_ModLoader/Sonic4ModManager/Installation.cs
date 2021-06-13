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

        public class UninstallationOptions
        {
            public bool RecoverOriginalFiles;
            public bool KeepSettings;
            public bool UninstallAndDeleteOCMI;
            public bool DeleteAllModLoaderFiles;
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

            if (status == Status.NotInstalled || status == Status.FirstLaunch)
            {
                var instructions = GetInstallationInstructions();
                foreach (var i in instructions)
                    if (File.Exists(i.orig) && !File.Exists(i.newName))
                        File.Move(i.orig, i.newName);

                Settings.Save();
            }
        }

        public static void Uninstall(UninstallationOptions options)
        {
            //TODO: add deletion of config files
            var renameList = GetInstallationInstructions();
            renameList.Reverse();

            foreach (var i in renameList)
                if (File.Exists(i.newName) && !File.Exists(i.orig))
                    File.Move(i.newName, i.orig);

            if (options.RecoverOriginalFiles)
            {
                Process.Start("AMBPatcher.exe", "recover").WaitForExit();

                if (options.DeleteAllModLoaderFiles)
                    if (Directory.Exists("mods_sha"))
                        Directory.Delete("mods_sha", true);
            }

            if (options.UninstallAndDeleteOCMI)
            {
                if (File.Exists("OneClickModInstaller.exe"))
                {
                    Process.Start("OneClickModInstaller.exe", "--uninstall").WaitForExit();
                    File.Delete("OneClickModInstaller.exe");
                }

                if (!options.KeepSettings && File.Exists("OneClickModInstaller.cfg"))
                    File.Delete("OneClickModInstaller.cfg");
            }

            if (options.DeleteAllModLoaderFiles)
            {
                foreach (var file in renameList)
                    if (file.modloaderFile && File.Exists(file.orig))
                        File.Delete(file.orig);

                if (Directory.Exists("Mod Loader - licenses"))
                    Directory.Delete("Mod Loader - licenses", true);

                //TODO: rewrite this as an executable command instead of file call
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

        public static void Upgrade(string dirToNewVersion)
        {
            string myDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            if (Directory.Exists(dirToNewVersion))
            {
                var installFrom = dirToNewVersion;
                var status = GetInstallationStatus();
                var argInstall = "";

                if (status == Status.Installed)
                    argInstall = " --install";

                if (Directory.Exists(Path.Combine(dirToNewVersion, "Sonic4ModLoader")))
                    installFrom = Path.Combine(installFrom, "Sonic4ModLoader");

                var options = new UninstallationOptions();
                options.DeleteAllModLoaderFiles = true;
                Uninstall(options);

                var filesToMove = Directory.GetFileSystemEntries(installFrom).ToList();
                filesToMove.Remove(Path.Combine(installFrom, "Sonic4ModManager.exe"));

                foreach (var file in filesToMove)
                {
                    var myFile = Path.Combine(myDir, Path.GetFileName(file));
                    if (File.Exists(file))
                    {
                        if (File.Exists(myFile))
                            File.Delete(myFile);
                        File.Move(file, myFile);
                    }
                    else
                    {
                        MyDirectory.CopyAll(file, myFile);
                        Directory.Delete(file, true);
                    }
                }

                string[] bat =
                {
                    "taskkill /IM Sonic4ModManager.exe /F",
                    "MOVE /Y \"" + installFrom + "\"\\Sonic4ModManager.exe \"" + myDir + "\"\\Sonic4ModManager.exe",
                    "RMDIR /Q /S \"" + dirToNewVersion + "\"",
                    "START \"\" /D \"" + myDir + "\" Sonic4ModManager.exe" + argInstall,
                    "DEL FinishUpgrade.bat"
                };
                File.WriteAllLines("FinishUpgrade.bat", bat);
                Process.Start("FinishUpgrade.bat");
                Environment.Exit(0);
            }
        }
    }
}
