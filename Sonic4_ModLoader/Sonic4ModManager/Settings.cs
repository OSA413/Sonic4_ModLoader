using System;
using System.IO;
using System.Collections.Generic;

using Common.Ini;
using Common.ValueUpdater;

namespace Sonic4ModManager
{
    static class Settings
    {
        public static string ModLoaderVersion => Properties.Resources.version;
        public static string PatcherDir;
        public static int OnlineUpdateCheckPeriod;
        public static bool CheckOnlineUpdates;

        public static void Load()
        {
            PatcherDir = "";
            OnlineUpdateCheckPeriod = 30;
            CheckOnlineUpdates = false;

            var cfg = IniReader.Read("ModManager.cfg");
            if (cfg.ContainsKey(IniReader.DEFAULT_SECTION))
            {
                ValueUpdater.UpdateIfKeyPresent(cfg, "OnlineUpdateCheckPeriod", ref OnlineUpdateCheckPeriod);
                ValueUpdater.UpdateIfKeyPresent(cfg, "CheckOnlineUpdates", ref CheckOnlineUpdates);
            }

            if (!File.Exists("AML/AliceML.ini")) return;
            var cfgAml = File.ReadAllLines("AML/AliceML.ini");
            for (int i = 0; i < cfgAml.Length; i++)
            {
                if (cfgAml[i].StartsWith("PatcherDir="))
                {
                    PatcherDir = cfgAml[i]["PatcherDir=\"".Length..];
                    PatcherDir = PatcherDir[0..^1];
                    break;
                }
            }
        }

        public static void Save()
        {
            var ini = IniWriter.CreateIni();
            ini[IniReader.DEFAULT_SECTION] = new Dictionary<string, string>();
            ini[IniReader.DEFAULT_SECTION]["OnlineUpdateCheckPeriod"] = OnlineUpdateCheckPeriod.ToString();
            ini[IniReader.DEFAULT_SECTION]["CheckOnlineUpdates"] = Convert.ToInt32(CheckOnlineUpdates).ToString();
            IniWriter.Write(ini, "ModManager.cfg");

            var iniAml = File.ReadAllLines("AML/AliceML.ini");
            for (int i = 0; i < iniAml.Length; i++)
            {
                if (iniAml[i].StartsWith("PatcherDir="))
                {
                    iniAml[i] = "PatcherDir=\""+PatcherDir+"\"";
                    break;
                }
            }
            
            File.WriteAllLines("AML/AliceML.ini", iniAml);
        }
    }
}
