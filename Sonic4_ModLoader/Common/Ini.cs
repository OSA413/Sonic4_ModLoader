using System.IO;
using System.Collections.Generic;

namespace Common.Ini
{
    public static class IniReader
    {
        public static readonly string DEFAULT_SECTION = "Default";
        public static Dictionary<string, Dictionary<string, string>> Read(string path)
        {
            var result = new Dictionary<string, Dictionary<string, string>>();
            if (!File.Exists(path)) return result;

            var text = File.ReadAllLines(path);
            string currentSection = null;

            foreach (var line in text)
            {
                if (line.Length == 0) continue;
                if (line[0] == ';' || line[0] == '#') continue;

                if (line[0] == '[' && line[line.Length - 1] == ']')
                {
                    currentSection = line.Substring(1, line.Length - 2);
                    result.Add(currentSection, new Dictionary<string, string>());
                }
                else
                {
                    var ind = line.IndexOf("=");
                    if (ind == -1) continue;

                    if (currentSection == null)
                    {
                        currentSection = DEFAULT_SECTION;
                        result.Add(currentSection, new Dictionary<string, string>());
                    }

                    result[currentSection].Add(line.Substring(0, ind), line.Substring(ind + 1));
                }
            }
            return result;
        }
    }
    public static class IniWriter
    {
        public static Dictionary<string, Dictionary<string, string>> CreateIni() =>
            new Dictionary<string, Dictionary<string, string>>();

        private static void WriteSection(List<string> result, Dictionary<string, string> section)
        {
            foreach (var v in section)
                result.Add(v.Key + "=" + v.Value);
        }

        public static void Write(Dictionary<string, Dictionary<string, string>> ini, string path)
        {
            var result = new List<string>();

            var onlyDefaultSection = ini.ContainsKey(IniReader.DEFAULT_SECTION) && ini.Keys.Count == 1;

            foreach (var section in ini)
            {
                if (!onlyDefaultSection)
                    result.Add("[" + section.Key ?? IniReader.DEFAULT_SECTION + "]");
                WriteSection(result, section.Value);
            }

            File.WriteAllText(path, string.Join("\n", result));
        }
    }
}