using System;
using Common.Launcher;

public enum Platform
{
    Windows,
    Linux
}

public enum InstallationStatus
{
    NotInstalled,
    Installed,
    ImproperlyInstalled,
    AnotherInstallationPresent
}

public interface IHandlerInstaller
{
    //public Platform platform {get; private set;}
    Platform SupportedPlatform();
    bool RequiresAdmin();
    bool IsAdmin();
    void Install(GAME? game = null);
    void Uninstall(GAME? game = null);
    void FixPath(GAME? game = null);
    InstallationStatus GetInstallationStatus(GAME? game = null);
    string GetInstallationLocation(GAME? game = null);
}

public class HandlerInstallerWrapper : IHandlerInstaller
{
    private IHandlerInstaller baseHandlerInstaller;
    public Platform SupportedPlatform() => baseHandlerInstaller.SupportedPlatform();
    public bool RequiresAdmin() => baseHandlerInstaller.RequiresAdmin();
    public bool IsAdmin() => baseHandlerInstaller.IsAdmin();

    public HandlerInstallerWrapper()
    {
        baseHandlerInstaller = new HandlerInstallerWindows();
    }

    public void Install(GAME? game = null)
    {

    }

    public void Uninstall(GAME? game = null)
    {

    }

    public void FixPath(GAME? game = null)
    {

    }

    public InstallationStatus GetInstallationStatus(GAME? game = null)
    {
        return InstallationStatus.NotInstalled;
    }

    public string GetInstallationLocation(GAME? game = null)
    {
        return null;
    }
}