using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Common.Mods
{
    public static class Mods
    {
        public static List<Mod> GetMods()
        {
            var result = new List<Mod>();

            if (Directory.Exists("mods"))
                foreach (var modDir in Directory.GetDirectories("mods"))
                    result.Add(new Mod(modDir));

            if (File.Exists("mods/mods.ini"))
            {
                var modsIni = File.ReadAllLines("mods/mods.ini");

                foreach (var mod in result)
                {
                    var priority = Array.IndexOf(modsIni, mod.Path);
                    if (priority != -1)
                    {
                        mod.Enabled = true;
                        mod.Priority = priority;
                    }
                }
            }

            return result.OrderByDescending(x => x.Enabled).ThenBy(x => x.Priority).ToList();
        }
    }

    public class Mod
    {
        public string Path { get; private set; }

        public bool Enabled = false;
        public int Priority;

        public Mod(string path)
        {
            Path = System.IO.Path.GetFileName(path);
        }
    }
}