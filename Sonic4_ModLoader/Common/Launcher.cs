using System.IO;
using System.Diagnostics;

namespace Common.Launcher
{
    public enum GAME
    {
        Unknown,
        Episode1,
        Episode2,
        NonSteam
    }

    public static class Launcher
    {
        private static GAME? currentGame;
        public static string GetShortGame(GAME? game)
        {
            if (game == GAME.Episode1) return "ep1";
            if (game == GAME.Episode2) return "ep2";
            return "";
        }
        public static GAME GetGameFromShort(string shrt)
        {
            if (shrt == "ep1") return GAME.Episode1;
            if (shrt == "ep2") return GAME.Episode2;
            return GAME.Unknown;
        }
        public static GAME GetCurrentGame()
        {
            if (currentGame is null)
                currentGame = GetGame();
            return (GAME)currentGame;
        }

        public static GAME GetGame(string path = "")
        {
            if (File.Exists(Path.Combine(path, "Sonic.exe"))
                || File.Exists(Path.Combine(path, "Launcher.exe"))
                || File.Exists(Path.Combine(path, "Sonic_vis.exe"))
                || File.Exists(Path.Combine(path, "SonicLauncher.exe")))
            {
                if (File.Exists(Path.Combine(path, "Sonic_vis.exe"))
                    && File.Exists(Path.Combine(path, "SonicLauncher.exe")))
                    return GAME.Episode1;
                else if (File.Exists(Path.Combine(path, "Sonic.exe"))
                    && File.Exists(Path.Combine(path, "Launcher.exe")))
                    return GAME.Episode2;
                return GAME.NonSteam;
            }
            return GAME.Unknown;
        }

        public static bool LaunchGame(bool launchOriginals = false)
        {
            var game = GetCurrentGame();
            if (game == GAME.Episode1)
            {
                if (File.Exists("main.conf"))
                    Process.Start(launchOriginals ? "Sonic_vis.orig.exe" : "Sonic_vis.exe");
                else
                    Process.Start("SonicLauncher.orig.exe");
            }
            else if (game == GAME.Episode2)
                Process.Start(launchOriginals ? "Sonic.orig.exe" : "Sonic.exe");
            else
                return false;
            return true;
        }

        public static bool LaunchConfig()
        {
            var game = GetCurrentGame();
            if (game == GAME.Episode1)
                Process.Start("SonicLauncher.orig.exe");
            else if (game == GAME.Episode2)
                Process.Start("Launcher.orig.exe");
            else
                return false;
            return true;
        }

        public static bool LaunchModManager()
        {
            if (File.Exists("Sonic4ModManager.exe"))
                Process.Start("Sonic4ModManager.exe");
            else
                return false;
            return true;
        }

        public static bool LaunchCsbEditor(string args="")
        {
            if (File.Exists("CsbEditor.exe"))
                Process.Start("CsbEditor.exe", args).WaitForExit();
            else
                return false;
            return true;
        }

        public static bool LaunchAMBPatcher(string args="")
        {
            if (File.Exists("AMBPatcher.exe"))
                Process.Start("AMBPatcher.exe", args).WaitForExit();
            else
                return false;
            return true;
        }
    }
}