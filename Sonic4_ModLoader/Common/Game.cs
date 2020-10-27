using System.IO;

namespace Common.Game
{
    public enum GAME
    {
        Unknown,
        Episode1,
        Episode2
    }

    public static class Game
    {
        public static GAME GetGame()
        {
            if (File.Exists("Sonic_vis.exe") && File.Exists("SonicLauncher.exe"))
                return GAME.Episode1;
            else if (File.Exists("Sonic.exe") && File.Exists("Launcher.exe"))
                return GAME.Episode2;

            return GAME.Unknown;
        }
    }
}