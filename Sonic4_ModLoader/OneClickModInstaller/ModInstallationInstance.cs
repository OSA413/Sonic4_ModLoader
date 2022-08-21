using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;

using Common.MyIO;
using Common.Launcher;

namespace OneClickModInstaller
{
    public enum ModInstallationStatus
    {
        Beginning,
        Downloading,
        ServerError,
        Downloaded,
        Extracting,
        Extracted,
        Scanning,
        Scanned,
        Installing,
        Installed,
        ModIsComplicated
    }

    public class ModInstallationInstance
    {
        private string link;
        public string Link { get => link; set { if (!Locked) link = value; } }
        public (string root, ModType Platform)[] ModRoots;
        private string currentPath;

        public Downloader Downloader = new ();
        public ModInstaller Installer;

        private (string Link, bool Correct, bool FromArchive, bool FromDir, Downloader.ServerHost Host) initialInfo;
        public bool FromArgs => Args != null;
        public readonly ModArgs Args;
        public bool Locked = false;

        public ModInstallationStatus Status;// { get; private set; }
        public bool Cancelled;// { get; private set; }

        public ModInstallationInstance(ModArgs args = null)
        {
            Status = ModInstallationStatus.Beginning;
            if (args == null) return;
            Args = args;
            Link = Args.Path;
            Locked = true;
        }

        public static (string Link, bool Correct, bool FromArchive, bool FromDir, Downloader.ServerHost Host) GetInformationFromLink(string url)
        {
            if (File.Exists(url))
                return (Link: url, Correct: true, FromArchive: true, FromDir: false, Host: Downloader.ServerHost.Unknown);
            else if (Directory.Exists(url))
                return (Link: url, Correct: true, FromArchive: false, FromDir: true, Host: Downloader.ServerHost.Unknown);
            else if (url.StartsWith("https://"))
                return (Link: url, Correct: true, FromArchive: false, FromDir: false, Host: Downloader.GetServerHost(url));
            return (Link: url, Correct: false, FromArchive: false, FromDir: false, Host: 0);
        }

        public void ContinueInstallation()
        {
            while (DoNextStep())
                ContinueInstallation();
        }

        public bool DoNextStep() => Status switch
        {
            ModInstallationStatus.Beginning => Prepare(),
            ModInstallationStatus.Downloading => Download(),
            ModInstallationStatus.Downloaded => ExtractMod(),
            ModInstallationStatus.Extracted => FindRoots(),
            ModInstallationStatus.Scanned => InstallFromModRoots(),
            ModInstallationStatus.ServerError => throw new NotImplementedException(),
            ModInstallationStatus.Extracting => throw new NotImplementedException(),
            ModInstallationStatus.Scanning => throw new NotImplementedException(),
            ModInstallationStatus.Installing => throw new NotImplementedException(),
            ModInstallationStatus.Installed => throw new NotImplementedException(),
            ModInstallationStatus.ModIsComplicated => throw new NotImplementedException(),
            _ => false,
        };

        public bool Prepare()
        {
            if (Locked) return false;
            var info = GetInformationFromLink(Link);
            if (!info.Correct) return false;
            Locked = true;
            initialInfo = info;

            Status = ModInstallationStatus.Downloading;
            if (info.FromArchive)
                Status = ModInstallationStatus.Downloaded;
            else if (info.FromDir)
                Status = ModInstallationStatus.Extracted;
            return true;
        }

        public bool Download()
        {
            return false;
        }

        public bool ExtractMod()
        {
            if (!mod.FromDir)
            {
                mod.ArchiveDir = mod.ArchiveName + "_extracted";

                if (File.Exists(mod.ArchiveName))
                {
                    if (Directory.Exists(mod.ArchiveName + "_extracted"))
                        MyDirectory.DeleteRecursively(mod.ArchiveName + "_extracted");

                    if (Settings.UseLocal7zip)
                        ModArchive.Extract(mod.ArchiveName, Settings.Paths["7-Zip"]);
                    else
                        ModArchive.Extract(mod.ArchiveName);
                }
            }
            return true;
        }

        public bool FindRoots()
        {
            var cont = -1;

            //Check this branch
            if (!Directory.Exists(mod.ArchiveDir))
            {
                Status = ModInstallationStatus.Downloaded;
            }
            else
                cont = ModArchive.CheckFiles(mod.ArchiveDir);

            if (cont == 0)
            {
                MyDirectory.DeleteRecursively(mod.ArchiveDir);
                Cancelled = true;
            }
            else if (cont == 1)
            {
                var FoundRootDirs = ModArchive.FindRoot(mod.ArchiveDir);

                if (FoundRootDirs.Item2 != ModType.Unknown)
                {
                    ModRoots = FoundRootDirs.Item1;
                    mod.Platform = FoundRootDirs.Item2;
                }

                Status = ModInstallationStatus.Scanned;
            }
            return true;
        }

        public void HowToInstallByType((ModType Type, string SelectRoots) mod)
        {
            if (mod.Type == ModType.ModLoader)
            {
                File.Delete(mod.ArchiveName);
                {
                    Launcher.LaunchModManager("--upgrade \"" + mod.ArchiveDir + "\"");
                    Application.Exit();
                }
            }
            else if (mod.Type == ModType.Unknown)
            {
                //Status description
                /*  1 - start (open SelectRoots window)
                    *  2 - continue (choose destination directory)
                    *  0 - ok (user selected destination directory)
                    *  -1 - break (user cancelled SelectRoots window)
                    */
                var status = 1;
                var sr = new SelectRoots(mod.ArchiveDir);

                while (status > 0)
                {
                    if (status == 1)
                    {
                        status = -1;
                        if (sr.ShowDialog() == DialogResult.Yes)
                        {
                            status = 2;
                            ModRoots = sr.output.ToArray();
                            if (ModRoots.Length == 0)
                                status = -1;
                        }
                    }
                    else if (status == 2)
                    {
                        status = 1;
                        var path = MyDirectory.Select("test", "dir");
                        if (path != null)
                        {
                            mod.CustomPath = path;
                            status = 0;
                        }
                    }
                }

                if (status == -1)
                {
                    Cancelled = true;
                    return;
                }
            }
            else if (mod.Type == ModType.Mixed)
            {
                //TODO: think up a better explanation
                MessageBox.Show("Looks like this thing is complicated, try to install it manually. bye"
                                , "Couldn't install the mod"
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Exclamation);
                Status = ModInstallationStatus.ModIsComplicated;
                return;
            }
            else
            {
                if (ModRoots.Length > 1)
                {
                    var tmm = new TooManyMods(ModRoots);
                    tmm.ShowDialog();
                    ModRoots = tmm.mods;
                }
            }

            Status = ModInstallationStatus.Scanned;
        }

        public bool InstallFromModRoots()
        {
            var lastMod = ModRoots.First();
            foreach (var mod in ModRoots)
            {
                string dest = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "mods", Path.GetFileName(mod.root));

                if (Directory.Exists(dest) && mod.Platform == ModType.PC)
                    MyDirectory.DeleteRecursively(dest);

                if (mod.Platform != ModType.Unknown)
                {
                    MyDirectory.CopyAll(mod.root, dest);
                    lastMod = mod;
                }
            }
            Status = ModInstallationStatus.Installed;

            if (Settings.ExitLaunchManager)
                if (lastMod.Platform == ModType.PC)
                    if (Launcher.LaunchModManager("\"" + Path.GetFileName(lastMod.root) + "\""))            
                        Environment.Exit(0);
            return true;
        }
    }
}
