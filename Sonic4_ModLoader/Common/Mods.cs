using System.IO;
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

            return result;
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

        public Mod(string path)
        {
            Path = path;
            name = System.IO.Path.GetFileName(path);
            ReadIni(System.IO.Path.Combine(path, "mod.ini"));
            ReadDescription();
        }

        private void ReadIni(string iniPath)
        {
            var ini = Common.Ini.IniReader.Read(iniPath);

            Dictionary<string, string> infoSection = null;
            if (ini.Keys.Count == 1 && ini.ContainsKey(Common.Ini.IniReader.DEFAULT_SECTION))
                infoSection = ini[Common.Ini.IniReader.DEFAULT_SECTION];
            else
                infoSection = ini["Info"];

            UpdateIfKeyPresent(infoSection, "Name", ref name);
            UpdateIfKeyPresent(infoSection, "Authors", ref authors);
            UpdateIfKeyPresent(infoSection, "Version", ref version);
            UpdateIfKeyPresent(infoSection, "Description", ref description);
        }

        private void ReadDescription()
        {
            if (description.StartsWith("file="))
            {
                var descriptionPath = description.Substring(5);
                if (File.Exists(descriptionPath))
                    //todo: wrap try-catch
                    if (descriptionPath.EndsWith("txt", System.StringComparison.OrdinalIgnoreCase))
                        description = File.ReadAllText(description.Substring(5));
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