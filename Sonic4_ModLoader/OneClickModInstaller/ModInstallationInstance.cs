using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;

using Common.MyIO;
using Common.Launcher;

namespace OneClickModInstaller
{
    public enum ModInstallationStatus
    {
        Beginning,
        Downloading,
        ServerError,
        Downloaded,
        Extracting,
        Extracted,
        Scanning,
        Scanned,
        Installing,
        Installed,
        Cancelled,
        ModIsComplicated
    }

    public class ModInstallationInstance
    {
        public string Link;
        public string LastMod;
        public (string root, ModType Platform)[] ModRoots;
        private string currentPath;

        public Downloader Downloader = new ();
        public ModInstaller Installer;

        public bool FromArgs => Args != null;
        public readonly ModArgs Args;
        public bool Locked = false;

        public ModInstallationStatus Status;// { get; private set; }

        public ModInstallationInstance(ModArgs args = null)
        {
            Status = ModInstallationStatus.Beginning;
            if (args == null) return;
            Args = args;
            Link = Args.Path;
            Locked = true;
        }

        public static (bool Correct, bool FromArchive, bool FromDir, Downloader.ServerHost Host) GetInformationFromLink(string url)
        {
            if (File.Exists(url))
                return (Correct: true, FromArchive: true, FromDir: false, Host: Downloader.ServerHost.Unknown);
            else if (Directory.Exists(url))
                return (Correct: true, FromArchive: false, FromDir: true, Host: Downloader.ServerHost.Unknown);
            else if (url.StartsWith("https://"))
                return (Correct: true, FromArchive: false, FromDir: false, Host: Downloader.GetServerHost(url));
            return (Correct: false, FromArchive: false, FromDir: false, Host: 0);
        }

        public void Prepare(string link)
        {
            var info = GetInformationFromLink(link);
            if (!info.Correct) return;
            Locked = true;
            Link = link;

            Status = ModInstallationStatus.Downloading;
            if (info.FromArchive)
                Status = ModInstallationStatus.Downloaded;
            else if (info.FromDir)
                Status = ModInstallationStatus.Extracted;
        }

        public void Download()
        {

        }

        public void ExtractAndFindRoots()
        {
            if (!mod.FromDir)
            {
                statusBar.Text = "Extracting downloaded archive...";

                mod.ArchiveDir = mod.ArchiveName + "_extracted";

                if (File.Exists(mod.ArchiveName))
                {
                    if (Directory.Exists(mod.ArchiveName + "_extracted"))
                        MyDirectory.DeleteRecursively(mod.ArchiveName + "_extracted");

                    if (Settings.UseLocal7zip)
                        ModArchive.Extract(mod.ArchiveName, Settings.Paths["7-Zip"]);
                    else
                        ModArchive.Extract(mod.ArchiveName);
                }
            }

            var cont = -1;

            if (!Directory.Exists(mod.ArchiveDir))
            {
                statusBar.Text = "Couldn't extract archive";
                mod.Status = ModInstallationStatus.Downloaded;
                if (!(File.Exists("7z.exe") || (File.Exists(Settings.Paths["7-Zip"]) && Settings.UseLocal7zip)))
                    statusBar.Text += " (7-Zip not found)";
            }
            else
            {
                statusBar.Text = "Checking extracted files...";
                cont = ModArchive.CheckFiles(mod.ArchiveDir);
            }

            if (cont == 0)
            {
                MyDirectory.DeleteRecursively(mod.ArchiveDir);
                statusBar.Text = "Installation was cancelled";
                mod.Status = ModInstallationStatus.Cancelled;
            }
            else if (cont == 1)
            {
                statusBar.Text = "Finding root directories...";

                var FoundRootDirs = ModArchive.FindRoot(mod.ArchiveDir);

                if (FoundRootDirs.Item2 != ModType.Unknown)
                {
                    mod.ModRoots = FoundRootDirs.Item1;
                    mod.Platform = FoundRootDirs.Item2;
                }

                mod.Status = ModInstallationStatus.Scanned;

                tcMain.Invoke(new MethodInvoker(delegate
                {
                    ContinueInstallation();
                }));
            }
        }

        //Rename
        public void ContinueInstallation()
        {
            if (mod.Platform == ModType.ModLoader)
            {
                File.Delete(mod.ArchiveName);
                var result = MessageBox.Show("One-Click Mod Installer detected a Mod Loader distributive. Do you want to replace the current version of Mod Loader with the downloaded one?"
                                    , "Mod Loader update"
                                    , MessageBoxButtons.YesNo
                                    , MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Process.Start("Sonic4ModManager", "--upgrade \"" + mod.ArchiveDir + "\"");
                    Application.Exit();
                }
                else
                {
                    mod.Status = ModInstallationStatus.Cancelled;
                    statusBar.Text = "Installation was cancelled";
                }
            }
            else if (mod.Platform == ModType.Unknown)
            {
                //Status description
                /*  1 - start (open SelectRoots window)
                    *  2 - continue (choose destination directory)
                    *  0 - ok (user selected destination directory)
                    *  -1 - break (user cancelled SelectRoots window)
                    */
                var status = 1;
                var sr = new SelectRoots(mod.ArchiveDir);

                while (status > 0)
                {
                    if (status == 1)
                    {
                        status = -1;
                        if (sr.ShowDialog() == DialogResult.Yes)
                        {
                            status = 2;
                            mod.ModRoots = sr.output.ToArray();
                            if (mod.ModRoots.Length == 0)
                                status = -1;
                        }
                    }
                    else if (status == 2)
                    {
                        status = 1;
                        var path = MyDirectory.Select("test", "dir");
                        if (path != null)
                        {
                            mod.CustomPath = path;
                            status = 0;
                        }
                    }
                }

                if (status == -1)
                {
                    mod.Status = ModInstallationStatus.Cancelled;
                    statusBar.Text = "Installation was cancelled";
                    return;
                }
            }
            else if (mod.Platform == ModType.Mixed)
            {
                //TODO: think up a better explanation
                MessageBox.Show("Looks like this thing is complicated, try to install it manually. bye"
                                , "Couldn't install the mod"
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Exclamation);
                mod.Status = ModInstallationStatus.ModIsComplicated;
                return;
            }
            else
            {
                if (mod.ModRoots.Length > 1)
                {
                    var tmm_form = new TooManyMods(mod.ModRoots, mod.Platform);
                    tmm_form.ShowDialog();

                    mod.ModRoots = tmm_form.mods;
                }

                if (Settings.Paths.ContainsKey(mod.Platform.ToString()))
                {
                    if (Settings.Paths[mod.Platform.ToString()] == "")
                    {
                        /*tcMain.SelectTab(tabSettings);

                        MessageBox.Show("Looks like the mod you installed requires a special path to be installed to.\nPlease, specify "
                                        + mod.Platform + "\nand then press the \"Continue Installation\" button in the \"Download\" tab."
                                        , "Please specify a path"
                                        , MessageBoxButtons.OK
                                        , MessageBoxIcon.Information);
                        mod.Status = "Waiting for path";
                        UpdateUI.ModInstallation();*/
                        MessageBox.Show("That thing REALLY happened");
                    }
                }
            }

            if (mod.ModRoots.Length > 0)
                mod.LastMod = Path.GetFileName(mod.ModRoots[0].root);

            if (mod.Status == ModInstallationStatus.Scanned)
                FinishInstallation();
        }

        public void InstallFromModRoots()
        {
            var lastMod = ModRoots.First();
            foreach (var mod in ModRoots)
            {
                string dest = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "mods", Path.GetFileName(mod.root));

                if (Directory.Exists(dest) && mod.Platform == ModType.PC)
                    MyDirectory.DeleteRecursively(dest);

                if (mod.Platform != ModType.Unknown)
                {
                    MyDirectory.CopyAll(mod.root, dest);
                    lastMod = mod;
                }
            }
            Status = ModInstallationStatus.Installed;

            if (Settings.ExitLaunchManager)
                if (lastMod.Platform == ModType.PC)
                    if (Launcher.LaunchModManager("\"" + Path.GetFileName(lastMod.root) + "\""))            
                        Environment.Exit(0);
        }
    }
}
