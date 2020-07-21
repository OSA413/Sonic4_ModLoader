using System.IO;
using System.Collections.Generic;

public static class IniReader
{
    public static Dictionary<string, string> Read(string path)
    {
        var result = new Dictionary<string, string>();
        if (!File.Exists(path)) return result;

        var text = File.ReadAllLines(path);
        foreach (var line in text)
        {
            if (line[0] == '[' || line[0] == ';' || line[0] == '#') continue;
            var ind = line.IndexOf("=");
            if (ind == -1) continue;
            result.Add(line.Substring(0, ind), line.Substring(ind + 1));
        }
        return result;
    }
}