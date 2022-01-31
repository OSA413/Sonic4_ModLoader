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
            Settings.UseLocal7zip           = false;
            Settings.SaveDownloadedArchives = false;
            Settings.ExitLaunchManager      = true;

            Settings.Paths = new Dictionary<string, string>();
            Settings.Paths.Add("7-Zip", "");
            Settings.Paths.Add("DownloadedArhives", "mods_downloaded");

            var cfg = IniReader.Read("OneClickModInstaller.cfg");

            if (cfg.ContainsKey(IniReader.DEFAULT_SECTION))
            {
                var def = cfg[IniReader.DEFAULT_SECTION];
                ValueUpdater.UpdateIfKeyPresent(def, "UseLocal7zip", ref Settings.UseLocal7zip);
                ValueUpdater.UpdateIfKeyPresent(def, "SaveDownloadedArchives", ref Settings.SaveDownloadedArchives);
                ValueUpdater.UpdateIfKeyPresent(def, "ExitLaunchManager", ref Settings.ExitLaunchManager);
            }

            if (cfg.ContainsKey("Paths"))
            {
                var cfgPaths = cfg["Paths"];
                foreach (var p in Settings.Paths.Keys.ToList())
                {
                    ValueUpdater.UpdateIfKeyPresent(cfgPaths, p, Settings.Paths);
                    Settings.Paths[p] = Settings.Paths[p].Replace("\\", "/");
                }
            }
        }

        public static void Save()
        {
            var text = new List<string> { };

            text.Add("UseLocal7zip="           + Convert.ToInt32(Settings.UseLocal7zip));
            text.Add("SaveDownloadedArchives=" + Convert.ToInt32(Settings.SaveDownloadedArchives));
            text.Add("ExitLaunchManager="      + Convert.ToInt32(Settings.ExitLaunchManager));

            text.Add("[Paths]");
            foreach (var path in Settings.Paths.Keys)
                text.Add(path + "=" + Settings.Paths[path]);

            File.WriteAllLines("OneClickModInstaller.cfg", text);
        }
    }
}