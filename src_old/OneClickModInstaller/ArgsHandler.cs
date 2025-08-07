using System;
using System.IO;
using Common.Launcher;

namespace OneClickModInstaller {
    
    public static class ArgsHandler {
        public static ModArgs ModArgs {get; private set;}
        public static void Handle(string[] args) {
            if (args.Length > 0)
            {
                GAME? game = null;
                if (args.Length > 1) game = Launcher.GetGameFromShort(args[1]);

                switch (args[0])
                {
                    case "--install":   Program.hiWrapper.Install(game);   Environment.Exit(0); break;
                    case "--uninstall": Program.hiWrapper.Uninstall(game); Environment.Exit(0); break;
                    case "--fix":       Program.hiWrapper.FixPath(game);   Environment.Exit(0); break;
                }

                var arg = args[^1];

                if (File.Exists(arg)
                    || Directory.Exists(arg)
                    || arg.StartsWith("https://")
                    || arg.StartsWith("http://"))
                    ModArgs = new ModArgs(arg);

                if (arg.StartsWith("sonic4mmep1:") || arg.StartsWith("sonic4mmep2:"))
                {
                    //sonic4mmepx:url,mod_type,mod_id
                    var modArgs = arg[12..].Split(',');
                    string path = null;
                    string type = null;
                    var id = 0;
                    if (modArgs.Length > 0) path = modArgs[0];
                    if (modArgs.Length > 1) type = modArgs[1];
                    if (modArgs.Length > 2) id = Convert.ToInt32(modArgs[2]);
                    ModArgs = new ModArgs(path, type, id);
                }
            }
        }
    }
}