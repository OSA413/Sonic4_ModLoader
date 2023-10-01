using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;

using Common.MyIO;
using Common.Launcher;
using System.ComponentModel;
using System.Net;

namespace OneClickModInstaller
{
    public enum ModInstallationStatus
    {
        Beginning,
        Downloading,
        ServerError,
        Downloaded,
        Extracted,
        Scanned,
        Installed,
    }

    public class ModInstallationInstance
    {
        private string link;
        public string Link { get => link; set { if (!Locked) link = value; } }
        public (string root, ModType Platform)[] ModRoots;
        private string currentPath;

        public Downloader Downloader = new();
        public ModInstaller Installer;

        private (string Link, bool Correct, bool FromArchive, bool FromDir, Downloader.ServerHost Host) initialInfo;
        public bool FromArgs => Args != null;
        public readonly ModArgs Args;
        public bool Locked = false;

        public string ModDirectory;
        public string ModArchivePath;

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
            ModInstallationStatus.ServerError => false,
            ModInstallationStatus.Installed => false,
            _ => false,
        };

        public bool Prepare()
        {
            var info = GetInformationFromLink(Link);
            if (!info.Correct) return false;
            Locked = true;
            initialInfo = info;

            Status = ModInstallationStatus.Downloading;
            if (info.FromArchive)
            {
                ModArchivePath = Link;
                Status = ModInstallationStatus.Downloaded;
            }
            else if (info.FromDir)
            {
                ModDirectory = Link;
                Status = ModInstallationStatus.Extracted;
            }
            return true;
        }

        public void afterDownload(object o, AsyncCompletedEventArgs e) {
            Status = ModInstallationStatus.Downloaded;
            ContinueInstallation();
        }


        public bool Download()
        {
            var output = Downloader.Download(Link, afterDownload, (object o, DownloadProgressChangedEventArgs e) => { });
            ModArchivePath = output;
            return false;
        }

        public bool ExtractMod()
        {
            ModDirectory = ModArchivePath + "_extracted";

            if (Directory.Exists(ModDirectory))
                MyDirectory.DeleteRecursively(ModDirectory);

            if (Settings.UseLocal7zip)
                ModArchive.Extract(ModArchivePath, Settings.Paths["7-Zip"]);
            else
                ModArchive.Extract(ModArchivePath);

            Status = ModInstallationStatus.Extracted;
            return true;
        }

        public bool FindRoots()
        {
            var cont = -1;

            //Check this branch
            if (!Directory.Exists(ModDirectory))
            {
                Status = ModInstallationStatus.Downloaded;
            }
            else
                cont = ModArchive.CheckFiles(ModDirectory);

            if (cont == 0)
            {
                MyDirectory.DeleteRecursively(ModDirectory);
                Cancelled = true;
            }
            else if (cont == 1)
            {
                var FoundRootDirs = ModArchive.FindRoot(ModDirectory);

                ModRoots = FoundRootDirs;

                if (ModRoots.Length > 1)
                {
                    var tmm = new TooManyMods(ModRoots);
                    tmm.ShowDialog();
                    ModRoots = tmm.mods;
                }

                Status = ModInstallationStatus.Scanned;
            }
            return true;
        }

        public void HowToInstallByType((ModType Type, string SelectRoots) mod)
        {
            if (mod.Type == ModType.ModLoader)
            {
                File.Delete(ModArchivePath);
                {
                    Launcher.LaunchModManager("--upgrade \"" + ModDirectory + "\"");
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
                var sr = new SelectRoots(ModArchivePath);

                while (status > 0)
                {
                    if (status == 1)
                    {
                        status = -1;
                        if (sr.ShowDialog() == DialogResult.Yes)
                        {
                            ModRoots = sr.output.Select(x => (root: x, Platform: ModType.PC)).ToArray();
                            if (ModRoots.Length == 0)
                                status = -1;
                        }
                    }
                }

                if (status == -1)
                {
                    Cancelled = true;
                    return;
                }
                return;
            }
        }

        public bool InstallFromModRoots()
        {
            foreach (var mod in ModRoots)
            {
                var dest = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "mods", Path.GetFileName(mod.root));

                if (Directory.Exists(dest) && mod.Platform == ModType.PC)
                    MyDirectory.DeleteRecursively(dest);

                if (mod.Platform != ModType.Unknown)
                    MyDirectory.CopyAll(mod.root, dest);
            }

            Status = ModInstallationStatus.Installed;
            return true;
        }
    }
}
