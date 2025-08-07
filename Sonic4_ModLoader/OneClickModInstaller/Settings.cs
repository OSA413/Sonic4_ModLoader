using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Common.Ini;
using Common.ValueUpdater;

namespace OneClickModInstaller
{
    public static class Settings
    {
        public static bool UseLocal7zip;
        public static bool SaveDownloadedArchives;
        public static bool ExitLaunchManager;

        public static Dictionary<string, string> Paths;

        public static void Load()
        {
            UseLocal7zip = false;
            SaveDownloadedArchives = false;
            ExitLaunchManager = true;

            Paths = new Dictionary<string, string>
            {
                { "7-Zip", "" },
                { "DownloadedArhives", "mods_downloaded" }
            };

            var cfg = IniReader.Read("OneClickModInstaller.cfg");

            ValueUpdater.UpdateIfKeyPresent(cfg, "UseLocal7zip", ref UseLocal7zip);
            ValueUpdater.UpdateIfKeyPresent(cfg, "SaveDownloadedArchives", ref SaveDownloadedArchives);
            ValueUpdater.UpdateIfKeyPresent(cfg, "ExitLaunchManager", ref ExitLaunchManager);
        }

        public static void Save()
        {
            var text = new List<string> { };

            text.Add("UseLocal7zip="           + Convert.ToInt32(UseLocal7zip));
            text.Add("SaveDownloadedArchives=" + Convert.ToInt32(SaveDownloadedArchives));
            text.Add("ExitLaunchManager="      + Convert.ToInt32(ExitLaunchManager));

            text.Add("[Paths]");
            foreach (var path in Paths.Keys)
                text.Add(path + "=" + Paths[path]);

            File.WriteAllLines("OneClickModInstaller.cfg", text);
        }
    }
}