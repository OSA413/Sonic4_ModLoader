using System;
using System.Collections.Generic;
using System.IO;

using Common.Ini;
using Common.ValueUpdater;

namespace Sonic4ModManager
{
    static class Settings
    {
        public static int OnlineUpdateCheckPeriod;
        public static bool CheckOnlineUpdates;
        public static bool SHACheck;

        public static void Load()
        {
            Settings.CheckOnlineUpdates = false;

            var cfg = IniReader.Read("ModManager.cfg");
            if (!cfg.ContainsKey(IniReader.DEFAULT_SECTION)) return;

            ValueUpdater.UpdateIfKeyPresent(cfg, "CheckOnlineUpdates", ref Settings.CheckOnlineUpdates);
        }

        public static void Save()
        {
            var text = new List<string> { };

            text.Add("CheckOnlineUpdates=" + Convert.ToInt32(Settings.CheckOnlineUpdates));
            File.WriteAllLines("ModManager.cfg", text);
        }
    }
}
