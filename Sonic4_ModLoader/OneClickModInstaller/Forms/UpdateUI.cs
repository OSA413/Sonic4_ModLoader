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