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

        public static class Writer
        {

        }
    }
}