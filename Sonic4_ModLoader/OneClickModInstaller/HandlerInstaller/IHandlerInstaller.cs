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
    InstallationStatus GetInstallationStatus(T game);
    string GetInstallationLocation(T game);
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
        baseHandlerInstaller = new HandlerInstallerWindows();
    }

    private void HandleIt(Action<string> f, GAME? game, string args)
    {
        if (game == null) game = Launcher.GetCurrentGame();
        if (game == GAME.Unknown || game == GAME.NonSteam) return;
        var shrt = Launcher.GetShortGame(game);

        if (RequiresAdmin() && !IsAdmin())
            RestartAsAdmin(args + " " + shrt);
        else
            f(shrt);
    }

    public void Install(GAME? game = null) =>
        HandleIt(baseHandlerInstaller.Install, game, "--install");

    public void Uninstall(GAME? game = null)
    {
        if (GetInstallationStatus(game) != InstallationStatus.NotInstalled)
            HandleIt(baseHandlerInstaller.Uninstall, game, "--uninstall");
    }

    public void FixPath(GAME? game = null) =>
        HandleIt(baseHandlerInstaller.FixPath, game, "--fix");

    public InstallationStatus GetInstallationStatus(GAME? game = null)
    {
        return InstallationStatus.NotInstalled;
    }

    public string GetInstallationLocation(GAME? game = null)
    {
        return null;
    }
}