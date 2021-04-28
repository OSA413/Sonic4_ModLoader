using Common.Ini;
using Common.ValueUpdater;

namespace AMBPatcher
{
    public static class Settings
    {
        public static bool GenerateLog;
        public static bool SHACheck;
        public static void Load()
        {
            ProgressBar.Enabled = true;
            GenerateLog = false;
            SHACheck = true;

            var cfg = IniReader.Read("AMBPatcher.cfg");
            if (!cfg.ContainsKey(IniReader.DEFAULT_SECTION)) return;

            ValueUpdater.UpdateIfKeyPresent(cfg, "ProgressBar", ref ProgressBar.Enabled);
            ValueUpdater.UpdateIfKeyPresent(cfg, "GenerateLog", ref GenerateLog);
            ValueUpdater.UpdateIfKeyPresent(cfg, "SHACheck", ref SHACheck);
        }
    }

}
