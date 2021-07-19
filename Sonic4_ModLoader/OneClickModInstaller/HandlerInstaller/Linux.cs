//TODO

using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

public class HandlerInstallerLinux : IHandlerInstaller<string>
{
    public Platform SupportedPlatform() => Platform.Linux;
    public bool RequiresAdmin() => false;
    public bool IsAdmin() => false;
    public void RestartAsAdmin(string game) {}

    public void Install(string game)
    {
        //Not tested, redo
        var desktopFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.local/share/applications/sonic4mm" + game +".desktop";

        File.WriteAllText(desktopFile
                        , "[Desktop Entry]"
                        + "\nType=Application"
                        + "\nExec=mono \"" + Application.ExecutablePath + "\" %U"
                        + "\nStartupNotify=true"
                        + "\nTerminal=false"
                        + "\nMimeType=x-scheme-handler/sonic4mm" + game
                        + "\nName=One-Click Mod Installer"
                        + "\nComment=OSA413's One-Click Mod Installer");

        Process.Start("xdg-mime", "default sonic4mm" + game +".desktop x-scheme-handler/sonic4mm" + game +".desktop").WaitForExit();
    }

    public void Uninstall(string game)
    {

    }

    public void FixPath(string game)
    {

    }

    public InstallationStatus GetInstallationStatus(string game)
    {
        var status = InstallationStatus.NotInstalled;
        var desktop_file = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.local/share/applications/sonic4mm" + game +".desktop";
        var process = Process.Start(new ProcessStartInfo
        {
            FileName = "xdg-mime",
            Arguments = "query default x-scheme-handler/sonic4mm" + game,
            RedirectStandardOutput = true,
            UseShellExecute = false
        });

        var output = process.StandardOutput.ReadToEnd();

        status = InstallationStatus.ImproperlyInstalled;
        if (output == "sonic4mm" + game + ".desktop\n")
            if (File.Exists(desktop_file))
                foreach (string line in File.ReadAllLines(desktop_file))
                    if (line.StartsWith("Exec="))
                    {
                        status = InstallationStatus.AnotherInstallationPresent;
                        if (line == "Exec=mono \"" + Application.ExecutablePath + "\" %U")
                            status = InstallationStatus.Installed;
                        break;
                    }

        return status;
    }

    public string GetInstallationLocation(string game)
    {
        var desktopFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.local/share/applications/sonic4mm" + game +".desktop";
                    
        if (File.Exists(desktopFile))
            foreach (string line in File.ReadAllLines(desktopFile))
                if (line.StartsWith("Exec="))
                    return line.Substring(12, line.Length - 16);
        return null;
    }
}