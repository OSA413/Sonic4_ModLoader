using System;

using Common.Launcher;

namespace PatchLauncher
{
    static class Program
    {
        static void Main()
        {
            Launcher.LaunchAMBPatcher();
            if (!Launcher.LaunchGame(true))
            {
                Console.Write("No game executable found. Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
