using System.Diagnostics;
using System.IO;

namespace PatchLauncher
{
    static class Program
    {
        static void Main()
        {
            Process.Start("AMBPatcher").WaitForExit();

            //Episode 1
            if  (File.Exists("Sonic_vis.orig.exe"))
            {
                if (File.Exists("main.conf"))
                {
                    Process.Start("Sonic_vis.orig.exe");
                }
                else
                {
                    Process.Start("SonicLauncher.orig.exe");
                }
            }

            //Episode 2
            else if (File.Exists("Sonic.orig.exe"))
            {
                Process.Start("Sonic.orig.exe");
            }
        }
    }
}
