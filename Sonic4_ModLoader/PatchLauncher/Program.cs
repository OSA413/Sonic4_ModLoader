using System;
using System.Diagnostics;

using Common.Launcher;

namespace PatchLauncher
{
    static class Program
    {
        static void Main()
        {
            Process.Start("AMBPatcher").WaitForExit();
            if (!Launcher.LaunchGame())
                Console.Write("No game executable found. Press Enter to exit.");
        }
    }
}
