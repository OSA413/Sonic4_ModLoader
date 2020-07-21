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

        UpdateIfKeyPresent(ini, "Name", ref name);
        UpdateIfKeyPresent(ini, "Authors", ref authors);
        UpdateIfKeyPresent(ini, "Version", ref version);
        UpdateIfKeyPresent(ini, "Description", ref description);
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