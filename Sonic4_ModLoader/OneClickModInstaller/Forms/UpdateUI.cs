using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Diagnostics;

using Common.MyIO;
using Common.URL;
using Common.Launcher;

namespace OneClickModInstaller
{
    public partial class UltimateWinForm:Form
    {
        public static class UpdateUI {
            static UltimateWinForm form;
            public static void AttachForm(UltimateWinForm form) => UpdateUI.form = form;

            public static void Initial() {
                UltimateWinForm.Installation.Status = "Idle";
                UltimateWinForm.Installation.Local      =
                UltimateWinForm.Installation.FromDir    =
                UltimateWinForm.Installation.FromArgs   = false;

                
                form.lType.Text =
                form.lModID.Text =
                form.lDownloadType.Text =
                form.lDownloadID.Text = null;
            }

            public static void ModInfo() {
                form.lType.Text =
                form.lModID.Text =
                form.lDownloadType.Text =
                form.lDownloadID.Text = null;
                UltimateWinForm.Installation.Status = "Idle";
                UltimateWinForm.Installation.Local      =
                UltimateWinForm.Installation.FromDir    =
                UltimateWinForm.Installation.FromArgs   = false;
            }

            public static void GlobalGameStatus()
            {
                var statuses = hiWrapper.GetAllInstallationStatus();

                foreach (var key in statuses.Keys)
                {
                    Label lIOStatus;
                    Label lIOPath;
                    Button bIOUninstall;
                    Button bIOVisit;

                    switch (key)
                    {
                        case GAME.Episode1:
                            lIOStatus = form.lIOEp1Stat;
                            lIOPath = form.lIOEp1Path;
                            bIOUninstall = form.bIOEp1Uninstall;
                            bIOVisit = form.bIOEp1Visit;
                            break;
                        case GAME.Episode2:
                            lIOStatus = form.lIOEp2Stat;
                            lIOPath = form.lIOEp2Path;
                            bIOUninstall = form.bIOEp2Uninstall;
                            bIOVisit = form.bIOEp2Visit;
                            break;
                        default: continue;
                    }

                    if (statuses[key].Status == InstallationStatus.NotInstalled)//?
                    {
                        lIOStatus.Text = "Installed";
                        lIOPath.Text = ("Path: " + statuses[key].Location);//.Replace(' ', '\u2007');
                        bIOUninstall.Enabled =
                        bIOVisit.Enabled = true;

                        if (hiWrapper.RequiresAdmin()) form.bUninstall.Image = null;
                    }
                    else
                    {
                        lIOStatus.Text = "Not installed";
                        lIOPath.Text = "";
                        bIOUninstall.Enabled =
                        bIOVisit.Enabled = false;
                    }
                }
            }

            public static void CurrentGame()
            {
                var statuses = hiWrapper.GetAllInstallationStatus();
                var currentGame = Launcher.GetCurrentGame();

                if (hiWrapper.IsAdmin())
                {
                    form.lInstallAdmin.Text = "";
                    form.bInstall.Image =
                    form.bUninstall.Image = null;
                }

                if (currentGame == GAME.Unknown)
                {
                    form.lGameName.Text = "Not found";
                    form.lInstallationStatus.Text = "None";
                    form.bInstall.Enabled =
                    form.bUninstall.Enabled = false;
                }
                else
                {
                    form.lGameName.Text = "Sonic 4: " + Launcher.GetFullGame(Launcher.GetCurrentGame());

                    form.bInstall.Enabled =
                    form.bUninstall.Enabled = true;

                    switch (statuses[currentGame].Status)
                    {
                        case InstallationStatus.NotInstalled:
                            form.lInstallationStatus.Text = "Not installed";
                            form.bInstall.Text = "Install";
                            form.bUninstall.Enabled = false;
                            break;
                        case InstallationStatus.Installed:
                            form.lInstallationStatus.Text = "Installed";
                            form.bInstall.Enabled = false;
                            form.bInstall.Text = "Install";
                            break;
                        case InstallationStatus.AnotherInstallationPresent:
                            form.lInstallationStatus.Text = "Another installation present";
                            form.bInstall.Text = "Fix registry path";
                            break;
                        case InstallationStatus.ImproperlyInstalled:
                            form.lInstallationStatus.Text = "Requires reinstallation";
                            form.bInstall.Text = "Install";
                            break;
                    }
                }
            }

            public static void ModInstallation()
            {
                form.bModPath.Enabled =
                form.bModInstall.Enabled = false;
                form.bModInstall.Text = "Install";
                form.tbModURL.ReadOnly = Installation.FromArgs;
                form.progressBar.Style = ProgressBarStyle.Blocks;

                switch (Installation.Status)
                {
                    case "Idle":
                    case "Cancelled":
                        form.bModPath.Enabled = !Installation.FromArgs;
                        form.bModInstall.Enabled = true;
                        break;
                    case "Waiting for path":
                        form.tbModURL.ReadOnly =
                        form.bModInstall.Enabled = true;
                        form.bModInstall.Text = "Continue installation";
                        break;
                    case "Mod is complicated":
                        form.bModPath.Enabled = !Installation.FromArgs;
                        break;
                    case "Installed":
                        if (!Installation.FromArgs)
                        {
                            form.bModPath.Enabled =
                            form.bModInstall.Enabled = true;
                        }
                        else if (Installation.Local)
                            form.bModInstall.Enabled = true;
                        break;
                    case "Server error":
                        form.bModInstall.Text = "Retry";
                        form.bModInstall.Enabled = true;
                        form.tbModURL.ReadOnly = Installation.FromArgs;
                        form.bModPath.Enabled = !Installation.FromArgs;
                        break;
                }

                if (!Installation.FromArgs)
                {
                    if (form.bModInstall.Enabled)
                        form.tbModURL_TextChanged(null, null);
                }
            }

            public static void Settings() {
                form.cbUseLocal7zip.Checked           = OneClickModInstaller.Settings.UseLocal7zip;
                form.chSaveDownloadedArchives.Checked = OneClickModInstaller.Settings.SaveDownloadedArchives;
                form.cbExitLaunchManager.Checked      = OneClickModInstaller.Settings.ExitLaunchManager;
                form.tbPath7z.Text                    = OneClickModInstaller.Settings.Paths["7-Zip"];
                form.tbDownloadedArchiveLocation.Text = OneClickModInstaller.Settings.Paths["DownloadedArhives"];
            }
        }
    }
}