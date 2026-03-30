using System.IO;
using System.Diagnostics;

namespace Common.Launcher
{
    public static class Launcher
    {
        public static bool LaunchCsbEditor(string args="")
        {
            if (!File.Exists("CsbEditor.exe")) return false;
            Process.Start("CsbEditor.exe", args).WaitForExit();
            return true;
        }
    }
}