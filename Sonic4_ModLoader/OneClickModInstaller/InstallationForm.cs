using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Principal;

namespace OneClickModInstaller
{
    public partial class InstallationForm : Form
    {
        public InstallationForm(string[] args)
        {
            if (WhereAmI() != "dunno")
            {
                InitializeComponent();

                lGameName.Text = "Sonic 4: " + WhereAmI();

                if (IsRunAsAdmin())
                {
                    label3.Text = "";
                }

                if (args.Length == 1)
                {
                    if (args[0] == "--install")
                    {
                        RegistryInstall();
                    }
                    else if (args[0] == "--uninstall")
                    {
                        RegistryUninstall();
                    }
                    else if (args[0] == "--fix")
                    {
                        RegistryFixPath();
                    }
                }

                UpdateWindow();
            }
            else
            {
                WrongWorkingDirectory wwd = new WrongWorkingDirectory();
                wwd.ShowDialog();
                Application.Exit();
                Environment.Exit(0);
            }
        }

        static string WhereAmI()
        {
            string where = "dunno";

            if (File.Exists("Sonic_vis.exe") && File.Exists("SonicLauncher.exe"))
            {
                where = "Episode 1";
            }
            else if (File.Exists("Sonic.exe") && File.Exists("Launcher.exe"))
            {
                where = "Episode 2";
            }

            return where;
        }

        static string GetGame()
        {
            string game = WhereAmI();

            switch (WhereAmI())
            {
                case "Episode 1":
                    game = "ep1";
                    break;
                case "Episode 2":
                    game = "ep2";
                    break;
            }

            return game;
        }

        static int GetInstallationStatus()
        {
            /* status description
             * 0    = Not installed
             * 1    = Properly installed
             * -1   = Improperly installed (something is not installed)
             * 2    = Another installation present (different path in registry)
             */
            
            int status = 0;
            
            string game = GetGame();

            if (game != "dunno")
            {
                string root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;
                
                if ((string) Registry.GetValue(root_key, "", null) == "URL:Sonic 4 Mod Loader's 1-Click Installer protocol")
                { if ((string) Registry.GetValue(root_key, "URL Protocol", null) == "")
                    { if ((string) Registry.GetValue(root_key + "\\DefaultIcon","",null) == "OneClickModInstaller.exe")
                        { if ((string) Registry.GetValue(root_key + "\\Shell\\Open\\Command","",null) == "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\"")
                            {
                                status = 1;
                            } else { status = 2; }
                        } else {status = -1;}
                    } else {status = -1;}
                }
            }
            
            return status;
        }

        static void RegistryFixPath()
        {
            if (IsRunAsAdmin())
            {
                string game = GetGame();

                if (game != "dunno")
                {
                    string root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;

                    Registry.SetValue(root_key + "\\Shell\\Open\\Command", "", "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\"");
                }
            }
            else
            {
                RestartAsAdmin("--fix");
            }
        }
        
        static void RegistryInstall()
        {
            if (IsRunAsAdmin())
            {
                string game = GetGame();

                if (game != "dunno")
                {
                    string root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;

                    Registry.SetValue(root_key, "", "URL:Sonic 4 Mod Loader's 1-Click Installer protocol");
                    Registry.SetValue(root_key, "URL Protocol", "");
                    Registry.SetValue(root_key + "\\DefaultIcon", "", "OneClickModInstaller.exe");
                    Registry.SetValue(root_key + "\\Shell\\Open\\Command", "", "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\"");
                }
            }
            else
            {
                RestartAsAdmin("--install");
            }
        }

        static void RegistryUninstall()
        {
            if (IsRunAsAdmin())
            {
                string game = GetGame();

                if (game != "dunno")
                {
                    string root_key = "sonic4mm" + game;
                    
                    if (Registry.ClassesRoot.OpenSubKey(root_key) != null)
                    { Registry.ClassesRoot.DeleteSubKeyTree(root_key); }
                }
            }
            else
            {
                RestartAsAdmin("--uninstall");
            }
        }

        public static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void UpdateWindow()
        {
            int status = GetInstallationStatus();
            
            if (status == 0)
            {
                lInstallationStatus.Text = "Not installed";
                bInstall.Enabled = true;
                bInstall.Text = "Install";
                bUninstall.Enabled = false;
            }
            else if (status == 1)
            {
                lInstallationStatus.Text = "Installed";
                bInstall.Enabled = false;
                bInstall.Text = "Install";
                bUninstall.Enabled = true;
            }
            else if (status == 2)
            {
                lInstallationStatus.Text = "Another installation present";
                bInstall.Enabled = true;
                bInstall.Text = "Fix registry path";
                bUninstall.Enabled = true;
            }
            else if (status == -1)
            {
                lInstallationStatus.Text = "Requires reinstallation";
                bInstall.Enabled = true;
                bInstall.Text = "Install";
                bUninstall.Enabled = true;
            }
        }

        static void RestartAsAdmin(string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = AppDomain.CurrentDomain.FriendlyName,
                Arguments = args,
                Verb = "runas"
            };
            
            Process.Start(startInfo);
            Application.Exit();
        }

        private void bInstall_Click(object sender, EventArgs e)
        {
            if (GetInstallationStatus() == 2)
            {
                RegistryFixPath();
            }
            else
            {
                RegistryInstall();
            }
            UpdateWindow();
        }

        private void bUninstall_Click(object sender, EventArgs e)
        {
            RegistryUninstall();
            UpdateWindow();
        }
    }
}
