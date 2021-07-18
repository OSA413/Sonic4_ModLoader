using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;
using System.Security.Principal;
using Common.Launcher;

public class HandlerInstallerWindows : IHandlerInstaller
{
    private static bool? isAdmin;

    public bool IsCurrentUserAdmin()
    {
        if (isAdmin == null)
        {
            var id = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(id);
            isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        return (bool)isAdmin;
    }

    public void RunAsAdmin(string args)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = AppDomain.CurrentDomain.FriendlyName,
            Arguments = args,
            Verb = "runas"
        };
        
        Process.Start(startInfo).WaitForExit();
    }

    public Platform SupportedPlatform() => Platform.Windows;
    public bool RequiresAdmin() => true;
    public bool IsAdmin()
    {
        return false;
    }

    public void Install(GAME? game = null)
    {
        if (game == null) game = Launcher.GetCurrentGame();
        if (game == GAME.Unknown) return;
        var shrt = Launcher.GetShortGame(game);
        
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            if (IsCurrentUserAdmin())
            {
                var root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + shrt;

                Registry.SetValue(root_key, "", "URL:OSA413's One-Click Mod Installer protocol");
                Registry.SetValue(root_key, "URL Protocol", "");
                Registry.SetValue(root_key + "\\DefaultIcon", "", "OneClickModInstaller.exe");
                Registry.SetValue(root_key + "\\Shell\\Open\\Command", "", "\"" + Assembly.GetEntryAssembly().Location + "\" \"%1\"");
            }
            else
                RunAsAdmin("--install " + shrt);
        }
    }

    public void Uninstall(GAME? game = null)
    {
        if (game == null) game = Launcher.GetCurrentGame();
        if (game == GAME.Unknown) return;
        var shrt = Launcher.GetShortGame(game);

        if (GetInstallationStatus(game) == InstallationStatus.NotInstalled)
            return;
        
        if (IsCurrentUserAdmin())
        {
            var root_key = "sonic4mm" + shrt;
            if (Registry.ClassesRoot.OpenSubKey(root_key) != null)
                Registry.ClassesRoot.DeleteSubKeyTree(root_key);
        }
        else
            RunAsAdmin("--uninstall " + shrt);
    }

    public void FixPath(GAME? game = null)
    {
        if (game == null) game = Launcher.GetCurrentGame();
        if (game == GAME.Unknown) return;
        var shrt = Launcher.GetShortGame(game);

        if (IsCurrentUserAdmin())
            Registry.SetValue("HKEY_CLASSES_ROOT\\sonic4mm" + shrt + "\\Shell\\Open\\Command",
                "", "\"" + Assembly.GetEntryAssembly().Location + "\" \"%1\"");
        else
            RunAsAdmin("--fix " + shrt);
    }

    public InstallationStatus GetInstallationStatus(GAME? game = null)
    {
        var status = InstallationStatus.NotInstalled;

        if (game == null) game = Launcher.GetCurrentGame();
        if (game == GAME.Unknown) return status;
        var shrt = Launcher.GetShortGame(game);

        var root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + shrt;

        if ((string)Registry.GetValue(root_key, "", null) == "URL:OSA413's One-Click Mod Installer protocol")
            if ((string)Registry.GetValue(root_key, "URL Protocol", null) == "")
                if ((string)Registry.GetValue(root_key + "\\DefaultIcon", "", null) == "OneClickModInstaller.exe")
                    if ((string)Registry.GetValue(root_key + "\\Shell\\Open\\Command", "", null) == "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\"")
                        status = InstallationStatus.Installed;
                    else status = InstallationStatus.AnotherInstallationPresent;
                else status = InstallationStatus.ImproperlyInstalled;
            else status = InstallationStatus.ImproperlyInstalled;

        return status;
    }

    public string GetInstallationLocation(GAME? game = null)
    {
        return null;
    }
}