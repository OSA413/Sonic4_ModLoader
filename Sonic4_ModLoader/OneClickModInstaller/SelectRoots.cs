using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OneClickModInstaller
{
    public partial class SelectRoots:Form
    {
        public List<string>  output;
        public static string startPath;

        public SelectRoots(string dirName)
        {
            startPath = dirName;
            output = new List<string>();
            InitializeComponent();
            treeView1.PathSeparator = Path.DirectorySeparatorChar.ToString();
            treeView1.ImageList = ilIcons;

            var filesAndDirs = Directory.GetFileSystemEntries(dirName, "*", SearchOption.AllDirectories).ToList();
            filesAndDirs.Sort();
            int dirNum = 0;

            foreach (var dir in filesAndDirs)
                if (Directory.Exists(dir))
                {
                    filesAndDirs.Remove(dir);
                    filesAndDirs.Insert(dirNum++, dir);
                }

            foreach (var file in filesAndDirs)
            {
                var shortFileParts = file.Substring(dirName.Length + 1).Split(Path.DirectorySeparatorChar);
                var root = treeView1.Nodes;

                for (int i = 0; i < shortFileParts.Length; i++)
                {
                    var filePart = shortFileParts[i];

                    if (!root.ContainsKey(filePart))
                    {
                        var imageKey = "image-missing";
                        var extension = Path.GetExtension(filePart.ToUpper());

                        if (Directory.Exists(Path.Combine(dirName, Path.Combine(shortFileParts.Take(i+1).ToArray()))))
                            imageKey = "folder";
                        else if (extension.Length > 0)
                        {
                            switch (extension.Substring(1))
                            {
                                //https://github.com/OSA413/Sonic4_Tools/blob/master/docs/File%20description.md

                                //Images
                                case "PNG": case "DDS": case "GVR": case "PVR": case "JPG": case "GIF": case "BMP": case "TGA":
                                    imageKey = "image-x-generic"; break;

                                //Archives and containers
                                case "7Z": case "ZIP": case "RAR": case "AMB": case "CPK": case "ACB": case "CSB":
                                    imageKey = "package-x-generic"; break;

                                //Sound and music
                                case "WAV": case "MP3": case "FLAC": case "ADX":
                                    imageKey = "audio-x-generic"; break;

                                //Executables
                                case "EXE": case "DLL":
                                    imageKey = "application-x-executable"; break;

                                //Scripts
                                case "BAT": case "COM": case "PY": case "SH": case "CT": case "CFG": case "INI":
                                    imageKey = "text-x-script"; break;

                                //Text
                                case "TXT": case "RTF":
                                    imageKey = "text-x-generic"; break;
                            }
                        }

                        root.Add(filePart, filePart);
                        root[filePart].ImageKey = root[filePart].SelectedImageKey = imageKey;
                    }

                    root = root[filePart].Nodes;
                }
            }
        }

        private void bContinue_Click(object sender, EventArgs e)
        {
            output.Sort();
            output.Reverse();

            foreach (var path in output)
            {
                var pathParts = path.Split(Path.DirectorySeparatorChar);
                for (int i = 1; i < pathParts.Length - 1; i++)
                if (output.Contains(Path.Combine(pathParts.Take(i).ToArray())))
                {
                    output.Remove(path);
                    break;
                }
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var tn = e.Node;            
            if (tn.Checked)
                output.Add(tn.FullPath);
            else
                output.Remove(tn.FullPath);
        }

        private void bCancel_Click(object sender, EventArgs e) => output.Clear();
    }
}
