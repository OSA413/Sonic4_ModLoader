using System.IO;
using System.Collections.Generic;

public class Mod
{
    public string Path { get; private set; }
    public string Name { get { return name; } }
    public string Authors { get { return authors; } }
    public string Version { get { return version; } }
    public string Description { get { return description; } }

    private string name;
    private string authors;
    private string version;
    private string description;

    public Mod(string path)
    {
        Path = path;
        name = System.IO.Path.GetFileName(path);
        authors = "???";
        version = "???";
        description = "No description.";
        ReadIni(System.IO.Path.Combine(path, "mod.ini"));
        ReadDescription();
    }

    private void ReadIni(string iniPath)
    {
        var ini = IniReader.Read(iniPath);

        Dictionary<string, string> infoSection = null;
        if (ini.Keys.Count == 1 && ini.ContainsKey(IniReader.DEFAULT_SECTION))
            infoSection = ini[IniReader.DEFAULT_SECTION];
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
            description = File.ReadAllText(description.Substring(5));
    }

    private void UpdateIfKeyPresent<T, V>(Dictionary<T, V> d, T key, ref V value)
    {
        if (d.ContainsKey(key))
            value = d[key];
    }
}