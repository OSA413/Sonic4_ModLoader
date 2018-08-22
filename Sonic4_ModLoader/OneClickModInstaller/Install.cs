//TODO: write everything once

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace OneClickModInstaller
{
    public partial class Install : Form
    {
        public Install(string[] args)
        {
            if (WhereAmI() != "dunno")
            {
                if (args.Length == 0)
                {
                    InitializeComponent();
                    SetButtonShield(bInstall, true);
                    SetButtonShield(bUninstall, true);
                }
                else
                {

                }
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

        static int GetInstallationStatus()
        {
            /* status description
             * 0    = Not installed
             * 1    = Properly installed
             * -1   = Improperly installed (something is not installed)
             * 2    = Another installation present (different path in registry)
             */


            int status = 0;

            /*
             * https://stackoverflow.com/questions/4467458/reading-a-registry-key-in-c-sharp
            * https://msdn.microsoft.com/en-us/ie/aa767914(v=vs.94)
            * https://stackoverflow.com/questions/2021831/admin-rights-for-a-single-method
            */

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

            if (game != "dunno")
            {
                string root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;
                
                if (Registry.GetValue(root_key, "", null) != null)
                { if (Registry.GetValue(root_key, "URL Protocol", null) == "")
                    { if (Registry.GetValue(root_key + "\\DefaultIcon","",null) == "OneClickModInstaller.exe")
                        { if (Registry.GetValue(root_key + "\\Shell\\Open\\Command","",null) == "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\"")
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

            if (game != "dunno")
            {
                string root_key = "HKEY_CLASSES_ROOT\\sonic4mm" + game;

                Registry.SetValue(root_key + "\\Shell\\Open\\Command", "", "\"" + System.Reflection.Assembly.GetEntryAssembly().Location + "\" \"%1\"");
            }
        }

        static void RegistryInstall()
        {

        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        public static void SetButtonShield(Button btn, bool showShield)
        {
            btn.FlatStyle = FlatStyle.System;
            SendMessage(new HandleRef(btn, btn.Handle), 0x160C, IntPtr.Zero, showShield ? new IntPtr(1) : IntPtr.Zero);
        }

        private void bInstall_Click(object sender, EventArgs e)
        {

        }

        private void bUninstall_Click(object sender, EventArgs e)
        {

        }
    }
}
