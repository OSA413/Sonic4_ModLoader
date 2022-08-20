using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneClickModInstaller
{
    public class ModInstallationInstance
    {
        public string Link;
        public Downloader.ServerHost ServerHost;
        public string ArchiveName;
        public string ArchiveDir;
        public bool Local;
        public string LastMod;
        public string[] ModRoots;
        public ModType Platform;
        public string Status;
        public bool FromArgs;
        public string CustomPath;
        public bool FromDir;
        //Sometimes server may break connection when file is not fully downloaded
        //User will be offered to redownload it
        public long Recieved;
        public long Total;
    }
}
