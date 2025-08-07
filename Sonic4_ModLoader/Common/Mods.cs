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
        public string Name { get { return name; } }
        public string Authors { get { return authors; } }
        public string Version { get { return version; } }
        public string Description { get { return description; } }

        private string name;
        private string authors = "???";
        private string version = "???";
        private string description = "No description.";

        public bool Enabled = false;
        public int Priority;

        public Mod(string path)
        {
            name = System.IO.Path.GetFileName(path);
            Path = name;
            ReadIni(System.IO.Path.Combine(path, "mod.ini"));
            ReadDescription();
        }

        private void ReadIni(string iniPath)
        {
            var ini = Ini.IniReader.Read(iniPath);
            if (ini.Count == 0) return;

            UpdateIfKeyPresent(ini, "Name", ref name);
            UpdateIfKeyPresent(ini, "Authors", ref authors);
            UpdateIfKeyPresent(ini, "Version", ref version);
            UpdateIfKeyPresent(ini, "Description", ref description);
        }

        private void ReadDescription()
        {
            if (description.StartsWith("file="))
            {
                var descriptionPath = System.IO.Path.Combine("mods", Path, description.Substring(5));
                if (File.Exists(descriptionPath))
                    //todo: wrap try-catch
                    if (descriptionPath.EndsWith("txt", StringComparison.OrdinalIgnoreCase))
                        description = File.ReadAllText(descriptionPath);
                    else description = "Error: unsupported format of \"" + descriptionPath + "\" file.";
                else description = "Error: \"" + descriptionPath + "\" file not found.";
            }
        }

        private void UpdateIfKeyPresent<T, V>(Dictionary<T, V> d, T key, ref V value)
        {
            if (d.ContainsKey(key))
                value = d[key];
        }
    }
}