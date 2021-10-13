using System;
using System.Collections.Generic;

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

public interface IHandlerInstaller<T>
{
    //public Platform platform {get; private set;}
    Platform SupportedPlatform();
    bool RequiresAdmin();
    bool IsAdmin();
    void RestartAsAdmin(string args);
    void Install(T game);
    void Uninstall(T game);
    void FixPath(T game);
    (InstallationStatus Status, string Location) GetInstallationStatus(T game);
}

public class HandlerInstallerWrapper : IHandlerInstaller<GAME?>
{
    private IHandlerInstaller<string> baseHandlerInstaller;
    public Platform SupportedPlatform() => baseHandlerInstaller.SupportedPlatform();
    public bool RequiresAdmin() => baseHandlerInstaller.RequiresAdmin();
    public bool IsAdmin() => baseHandlerInstaller.IsAdmin();
    public void RestartAsAdmin(string args) => baseHandlerInstaller.RestartAsAdmin(args);

    public HandlerInstallerWrapper()
    {
        switch (Environment.OSVersion.Platform)
        {
            case PlatformID.Win32NT: baseHandlerInstaller = new HandlerInstallerWindows(); break;
            case PlatformID.Unix: baseHandlerInstaller = new HandlerInstallerLinux(); break;
        }
    }

    private void HandleIt(Action<string> f, GAME? game, string args, bool noAdmin = false)
    {
        if (game == null) game = Launcher.GetCurrentGame();
        if (game == GAME.Unknown) return;
        var shrt = Launcher.GetShortGame(game);

        if (noAdmin || RequiresAdmin() && !IsAdmin())
            RestartAsAdmin(args + " " + shrt);
        else
            f(shrt);
    }

    public void Install(GAME? game = null) =>
        HandleIt(baseHandlerInstaller.Install, game, "--install");

    public void Uninstall(GAME? game = null)
    {
        if (GetInstallationStatus(game).Item1 != InstallationStatus.NotInstalled)
            HandleIt(baseHandlerInstaller.Uninstall, game, "--uninstall");
    }

    public void FixPath(GAME? game = null) =>
        HandleIt(baseHandlerInstaller.FixPath, game, "--fix");

    public (InstallationStatus Status, string Location) GetInstallationStatus(GAME? game = null)
    {
        if (game == null) game = Launcher.GetCurrentGame();
        if (game == GAME.Unknown) return (InstallationStatus.NotInstalled, null);
        var shrt = Launcher.GetShortGame(game);

        return baseHandlerInstaller.GetInstallationStatus(shrt);
    }

    public Dictionary<GAME, (InstallationStatus Status, string Location)> GetAllInstallationStatus()
    {
        var statuses = new Dictionary<GAME, (InstallationStatus, string)>();
        foreach (GAME game in Enum.GetValues(typeof(GAME)))
            statuses.Add(game, GetInstallationStatus(game));
        return statuses;
    }
}