using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace OneClickModInstaller
{
    public partial class SelectRoots:Form
    {
        public SelectRoots(string dir_name)
        {
            InitializeComponent();
            treeView1.PathSeparator = Path.PathSeparator.ToString();
            treeView1.ImageList = ilIcons;

            foreach (string file in Directory.GetFileSystemEntries(dir_name, "*", SearchOption.AllDirectories))
            {
                string[] short_file_parts = file.Substring(dir_name.Length + 1).Split(Path.DirectorySeparatorChar);
                TreeNodeCollection test = treeView1.Nodes;
                
                for (int i = 0; i < short_file_parts.Length; i++)
                {
                    string file_part = short_file_parts[i];

                    if (!test.ContainsKey(file_part))
                    {
                        string imageKey = "image-missing";
                        if (Directory.Exists(Path.Combine(dir_name, Path.Combine(short_file_parts.Take(i+1).ToArray()))))
                        { imageKey = "folder"; }
                        else
                        {
                            switch (Path.GetExtension(file_part.ToUpper()).Substring(1))
                            {
                                //https://github.com/OSA413/Sonic4_Tools/blob/master/docs/File%20description.md
                                
                                //Images
                                case "PNG":
                                case "DDS":
                                case "GVR":
                                case "PVR":
                                case "JPG":
                                case "GIF":
                                case "BMP":
                                    imageKey = "image-x-generic"; break;

                                //Archives and containers
                                case "7Z":
                                case "ZIP":
                                case "RAR":
                                case "AMB":
                                case "CPK":
                                case "ACB":
                                case "CSB":
                                    imageKey = "package-x-generic"; break;

                                //Sound and music
                                case "WAV":
                                case "MP3":
                                case "FLAC":
                                case "ADX":
                                    imageKey = "audio-x-generic"; break;

                                //Executables
                                case "EXE":
                                case "DLL":
                                    imageKey = "application-x-executable"; break;

                                //Scripts
                                case "BAT":
                                case "COM":
                                case "PY":
                                case "SH":
                                case "CT":
                                case "CFG":
                                case "INI":
                                    imageKey = "text-x-script"; break;

                                //Text
                                case "TXT":
                                    imageKey = "text-x-generic"; break;
                            }
                        }

                        test.Add(file_part, file_part);
                        test[file_part].ImageKey         =
                        test[file_part].SelectedImageKey = imageKey;
                    }

                    test = test[file_part].Nodes;
                }
            }
        }
    }
}
