using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using Common.Launcher;

public class HandlerInstallerLinux : IHandlerInstaller
{
    public Platform SupportedPlatform() => Platform.Linux;
    public bool RequiresAdmin() => false;
    public bool IsAdmin() => false;

    public void Install(GAME? game = null)
    {
        if (game == null) game = Launcher.GetCurrentGame();
        if (game == GAME.Unknown) return;
        var shrt = Launcher.GetShortGame(game);

        //Not tested, redo
        if (Environment.OSVersion.Platform == PlatformID.Unix)
        {
            var desktopFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.local/share/applications/sonic4mm" + shrt +".desktop";

            File.WriteAllText(desktopFile
                            , "[Desktop Entry]"
                            + "\nType=Application"
                            + "\nExec=mono \"" + Application.ExecutablePath + "\" %U"
                            + "\nStartupNotify=true"
                            + "\nTerminal=false"
                            + "\nMimeType=x-scheme-handler/sonic4mm" + shrt
                            + "\nName=One-Click Mod Installer"
                            + "\nComment=OSA413's One-Click Mod Installer");

            Process.Start("xdg-mime", "default sonic4mm" + shrt +".desktop x-scheme-handler/sonic4mm" + shrt +".desktop").WaitForExit();
        }
    }

    public void Uninstall(GAME? game = null)
    {

    }

    public void FixPath(GAME? game = null)
    {

    }

    public InstallationStatus GetInstallationStatus(GAME? game = null)
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

    public string GetInstallationLocation(GAME? game = null)
    {
        return null;
    }
}