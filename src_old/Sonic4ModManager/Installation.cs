using System;
using System.Collections.Generic;
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
                if (File.Exists("SonicLauncher.orig.exe"))
                    return Status.Installed;

            if (game == GAME.Episode2)
                if (File.Exists("Launcher.orig.exe"))
                    return Status.Installed;

            if (!File.Exists("ModManager.cfg"))
                return Status.FirstLaunch;

            return Status.NotInstalled;
        }

        public static List<(string orig, string newName, bool modloaderFile)> GetInstallationInstructions()
        {
            var game = Launcher.GetCurrentGame();
            var renameList = new List<(string orig, string newName, bool modloaderFile)>();

            var originalLauncher = "";
            var managerLauncher = "ManagerLauncher.exe";

            if (game == GAME.Episode1)
                originalLauncher = "SonicLauncher.exe";
            else if (game == GAME.Episode2)
                originalLauncher = "Launcher.exe";

            var originalLauncherBkp = Path.GetFileNameWithoutExtension(originalLauncher) + ".orig.exe";

            //Original files
            renameList.Add((originalLauncher, originalLauncherBkp, false));
            renameList.Add((managerLauncher, originalLauncher, true));
            
            //Mod Loader files
            renameList.Add(("7z.exe", null, true));
            renameList.Add(("7z.dll", null, true));
            renameList.Add(("AMBPatcher.exe", null, true));
            renameList.Add(("CsbEditor.exe", null, true));
            renameList.Add(("Mod Loader - Whats new.txt", null, true));
            renameList.Add(("README.md", null, true));
            renameList.Add(("SonicAudioLib.dll", null, true));

            renameList.Add(("AMBPatcher.cfg", null, true));
            renameList.Add(("CsbEditor.exe.config", null, true));
            renameList.Add(("ModManager.cfg", null, true));

            renameList.Add(("Mod Loader - licenses", null, true));
            renameList.Add(("AML", null, true));
            renameList.Add(("d3d9.dll", null, true));

            return renameList;
        }

        public static void Install()
        {
            var status = GetInstallationStatus();

            if (status == Status.NotInstalled || status == Status.FirstLaunch)
            {
                var instructions = GetInstallationInstructions();
                foreach (var i in instructions)
                    if (File.Exists(i.orig) && i.newName != null && !File.Exists(i.newName))
                        File.Move(i.orig, i.newName);

                Settings.PatcherDir = "AMBPatcher.exe";
                Settings.Save();
            }
        }

        public static void Uninstall(UninstallationOptions options)
        {
            var renameList = GetInstallationInstructions();
            renameList.Reverse();

            foreach (var i in renameList)
                if (File.Exists(i.newName) && !File.Exists(i.orig))
                    File.Move(i.newName, i.orig);

            Settings.PatcherDir = "";
            Settings.Save();

            if (options.RecoverOriginalFiles)
            {
                Process.Start("AMBPatcher.exe", "recover").WaitForExit();

                if (options.DeleteAllModLoaderFiles && Directory.Exists("mods_sha"))
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
                    if (file.modloaderFile && (File.Exists(file.orig) || Directory.Exists(file.orig)))
                        if (!options.KeepSettings ||
                            !(options.KeepSettings && (file.orig.EndsWith(".cfg") || file.orig.EndsWith(".config"))))
                        {
                            if (File.Exists(file.orig))
                                File.Delete(file.orig);
                            else if (Directory.Exists(file.orig))
                                Directory.Delete(file.orig, true);
                        }

                var bat = "taskkill /IM Sonic4ModManager.exe /F\n" + 
                    "DEL Sonic4ModManager.exe\n" +
                    "DEL FinishInstallation.bat";
                File.WriteAllText("FinishInstallation.bat", bat);

                Process.Start("FinishInstallation.bat");
                Environment.Exit(0);
            }
        }

        public static void Upgrade(string dirToNewVersion)
        {
            var myDir = Path.GetDirectoryName(AppContext.BaseDirectory);
            if (Directory.Exists(dirToNewVersion))
            {
                var installFrom = dirToNewVersion;
                var argInstall = GetInstallationStatus() == Status.Installed ? " --install" : "";

                var possibleInstallDir = Path.Combine(dirToNewVersion, "Sonic4ModLoader");
                if (Directory.Exists(possibleInstallDir))
                    installFrom = possibleInstallDir;

                Uninstall(new UninstallationOptions());

                foreach (var file in Directory.GetFileSystemEntries(installFrom))
                {
                    if (file.EndsWith("Sonic4ModManager.exe")) continue;
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

                var bat = "taskkill /IM Sonic4ModManager.exe /F\n"+
                    "MOVE /Y \"" + installFrom + "\"\\Sonic4ModManager.exe \"" + myDir + "\"\\Sonic4ModManager.exe\n"+
                    "RMDIR /Q /S \"" + dirToNewVersion + "\"\n"+
                    "START \"\" /D \"" + myDir + "\" Sonic4ModManager.exe" + argInstall + "\n" +
                    "DEL FinishUpgrade.bat";

                File.WriteAllText("FinishUpgrade.bat", bat);
                Process.Start("FinishUpgrade.bat");
                Environment.Exit(0);
            }
        }
    }
}
