using System;
using System.IO;

namespace AMBPatcher
{
    public static class Log
    {
        public static void Write(string Message)
        {
            if (!Settings.GenerateLog) return;
            File.AppendAllText("AMBPatcher.log", Message + Environment.NewLine);
        }

        public static void Reset()
        {
            if (!Settings.GenerateLog) return;
            File.WriteAllText("AMBPatcher.log", "");
        }
    }
}
