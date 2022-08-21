using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

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
        Cancelled,
        ModIsComplicated
    }

    public class ModInstallationInstance
    {
        public string Link;
        public string ArchiveName;
        public string ArchiveDir;
        public bool Local;
        public string LastMod;
        public string[] ModRoots;
        public ModType Platform;
        public string CustomPath;
        public bool FromDir;

        public Downloader Downloader = new ();
        public ModInstaller Installer;

        public bool FromArgs => Args != null;
        public readonly ModArgs Args;
        public bool Locked = false;

        public ModInstallationStatus Status;// { get; private set; }

        public ModInstallationInstance(ModArgs args = null)
        {
            Status = ModInstallationStatus.Beginning;
            if (args == null) return;
            Args = args;
            Link = Args.Path;
            Locked = true;
        }

        public static (bool Correct, bool FromArchive, bool FromDir, Downloader.ServerHost Host) GetInformationFromLink(string url)
        {
            if (File.Exists(url))
                return (Correct: true, FromArchive: true, FromDir: false, Host: Downloader.ServerHost.Unknown);
            else if (Directory.Exists(url))
                return (Correct: true, FromArchive: false, FromDir: true, Host: Downloader.ServerHost.Unknown);
            else if (url.StartsWith("https://"))
                return (Correct: true, FromArchive: false, FromDir: false, Host: Downloader.GetServerHost(url));
            return (Correct: false, FromArchive: false, FromDir: false, Host: 0);
        }

        public void Prepare()
        {
            var info = GetInformationFromLink(Link);
            if (!info.Correct) return;
            Locked = true;

            Status = ModInstallationStatus.Downloading;
            if (info.FromArchive)
                Status = ModInstallationStatus.Downloaded;
            else if (info.FromDir)
                Status = ModInstallationStatus.Extracted;
        }

        public void Download()
        {

        }
    }
}
