using System;
using System.Collections.Generic;

using Common.Ini;
using Common.ValueUpdater;

namespace Sonic4ModManager
{
    static class Settings
    {
        public static int OnlineUpdateCheckPeriod;
        public static bool CheckOnlineUpdates;

        public static void Load()
        {
            OnlineUpdateCheckPeriod = 30;
            CheckOnlineUpdates = false;

            var cfg = IniReader.Read("ModManager.cfg");
            if (!cfg.ContainsKey(IniReader.DEFAULT_SECTION)) return;

            ValueUpdater.UpdateIfKeyPresent(cfg, "OnlineUpdateCheckPeriod", ref OnlineUpdateCheckPeriod);
            ValueUpdater.UpdateIfKeyPresent(cfg, "CheckOnlineUpdates", ref CheckOnlineUpdates);
        }

        public static void Save()
        {
            var ini = IniWriter.CreateIni();
            ini[IniReader.DEFAULT_SECTION] = new Dictionary<string, string>();
            ini[IniReader.DEFAULT_SECTION]["OnlineUpdateCheckPeriod"] = OnlineUpdateCheckPeriod.ToString();
            ini[IniReader.DEFAULT_SECTION]["CheckOnlineUpdates"] = Convert.ToInt32(CheckOnlineUpdates).ToString();
            IniWriter.Write(ini, "ModManager.cfg");
        }
    }
}
