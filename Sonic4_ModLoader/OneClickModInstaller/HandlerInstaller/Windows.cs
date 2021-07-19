using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;
using System.Security.Principal;

public class HandlerInstallerWindows : IHandlerInstaller<string>
{
    public Platform SupportedPlatform() => Platform.Windows;
    public bool RequiresAdmin() => true;
    public bool IsAdmin()
    {
        if (isAdmin == null)
        {
            var id = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(id);
            isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        return (bool)isAdmin;
    }
    private static bool? isAdmin;
    public void RestartAsAdmin(string args)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = AppDomain.CurrentDomain.FriendlyName,
            Arguments = args,
            Verb = "runas"
        };
        
        Process.Start(startInfo).WaitForExit();
    }

    public void Install(string game)
    {
        var root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;
        Registry.SetValue(root_key, "", "URL:OSA413's One-Click Mod Installer protocol");
        Registry.SetValue(root_key, "URL Protocol", "");
        Registry.SetValue(root_key + "\\DefaultIcon", "", "OneClickModInstaller.exe");
        Registry.SetValue(root_key + "\\Shell\\Open\\Command", "", "\"" + Assembly.GetEntryAssembly().Location + "\" \"%1\"");
    }

    public void Uninstall(string game)
    {
        var root_key = "sonic4mm" + game;
        if (Registry.ClassesRoot.OpenSubKey(root_key) != null)
            Registry.ClassesRoot.DeleteSubKeyTree(root_key);
    }

    public void FixPath(string game)
    {
        Registry.SetValue("HKEY_CLASSES_ROOT\\sonic4mm" + game + "\\Shell\\Open\\Command",
            "", "\"" + Assembly.GetEntryAssembly().Location + "\" \"%1\"");
    }

    public InstallationStatus GetInstallationStatus(string game)
    {
        var status = InstallationStatus.NotInstalled;
        var root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;
        if ((string)Registry.GetValue(root_key, "", null) == "URL:OSA413's One-Click Mod Installer protocol")
        {
            status = InstallationStatus.ImproperlyInstalled;
            if ((string)Registry.GetValue(root_key, "URL Protocol", null) == "")
                if ((string)Registry.GetValue(root_key + "\\DefaultIcon", "", null) == "OneClickModInstaller.exe")
                    if ((string)Registry.GetValue(root_key + "\\Shell\\Open\\Command", "", null) == "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\"")
                        status = InstallationStatus.Installed;
        }

        return status;
    }

    public string GetInstallationLocation(string game)
    {
        var root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;
        var location = (string)Registry.GetValue(root_key + "\\Shell\\Open\\Command", "", null);

        if (location != null)
            location = location.Substring(1, location.Length - 7);
        return location;
    }
}