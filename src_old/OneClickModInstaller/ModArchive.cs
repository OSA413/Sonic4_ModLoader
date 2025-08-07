using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using Common.MyIO;

namespace OneClickModInstaller
{
    public enum ModType {
        PC,
        ModLoader,
        Mixed,
        Unknown
    }

    public static class ModArchive
    {
        public static void Extract(string file, string path_to_7z = "7z.exe")
        {
            if (!File.Exists(path_to_7z))
            {
                if (File.Exists("7z.exe"))
                    path_to_7z = "7z.exe";
                else if (File.Exists(@"C:\Program Files\7-Zip\7z.exe"))
                    path_to_7z = @"C:\Program Files\7-Zip\7z.exe";
                else
                    return;
            }

            ProcessStartInfo startInfo = new()

            {
                FileName = path_to_7z,
                Arguments = "x \"" + file + "\" -o\"" + file + "_extracted" + "\""
            };
            Process.Start(startInfo).WaitForExit();
        }

        public static bool CheckFiles(string dir_name)
        {
            var good_formats = "TXT,INI,DDS,TXB,AMA,AME,ZNO,ZNM,ZNV,DC,EV,RG,MD,MP,AT,DF,DI,PSH,VSH,LTS,XNM,MFS,SSS,GPB,MSG,AYK,ADX,AMB,CPK,CSB,PNG,CT,TGA".Split(',');

            var all_files = Directory.GetFiles(dir_name, "*", SearchOption.AllDirectories);
            var suspicious_files = new List<string>();

            foreach (var file in all_files)
            {
                var file_short = file.Substring(dir_name.Length + 1);

                if (int.TryParse(Path.GetFileName(file_short), out int n) && file_short.Contains(Path.Combine("DEMO", "WORLDMAP", "WORLDMAP.AMB")))
                    continue;
 
                var extension_len = Path.GetExtension(file_short).Length;
                if (extension_len != 0) extension_len = 1;

                if (good_formats.Contains(Path.GetExtension(file_short).Substring(extension_len), StringComparer.OrdinalIgnoreCase))
                    continue;

                suspicious_files.Add(file_short);
            }


            var continuee = true;
            if (suspicious_files.Count != 0)
            {
                continuee = false;
                var SuspiciousDialog = new Suspicious(suspicious_files.ToArray());

                var result = SuspiciousDialog.ShowDialog();

                //Continue
                if (result != DialogResult.Cancel)
                    continuee = true;

                if (result == DialogResult.Yes)
                    foreach (var file in suspicious_files)
                        MyFile.DeleteAnyway(Path.Combine(dir_name, file));
            }
            return continuee;
        }

        public static (string, ModType)[] FindRoot(string dir_name)
        {
            var platform = ModType.Unknown;
            var mod_roots = new List<string>();

            var platforms = new [] { ModType.PC, ModType.ModLoader };
            var game_folders_array = new [] { "CUTSCENE,DEMO,G_COM,G_SS,G_EP1COM,G_EP1ZONE2,G_EP1ZONE3,G_EP1ZONE4,G_ZONE1,G_ZONE2,G_ZONE3,G_ZONE4,G_ZONEF,MSG,NNSTDSHADER,SOUND"
                                                        //, "WSNE8P,WSNP8P,WSNJ8P"
                                                        , "Sonic4ModLoader"};

            for (int i = 0; i < platforms.Length; i++)
            {
                var game_folders = game_folders_array[i].Split(',');

                foreach (var folder in game_folders)
                {
                    foreach (var mod_folder in Directory.GetDirectories(dir_name, folder, SearchOption.AllDirectories))
                    {
                        var tmp_root = Path.GetDirectoryName(mod_folder);
                        if (!mod_roots.Contains(tmp_root))
                            mod_roots.Add(tmp_root);
                    }
                }

                if (mod_roots.Count > 0)
                {
                    platform = platforms[i];
                    break;
                }
            }

            return mod_roots.Select(x => (x, platform)).ToArray();
        }
    }
}