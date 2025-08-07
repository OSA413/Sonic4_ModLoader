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

        public static string GetIconByExtension(string extension) => extension switch
        {
            //https://github.com/OSA413/Sonic4_Tools/blob/master/docs/File%20description.md
            "PNG" or "DDS" or "GVR" or "PVR" or "JPG" or "GIF" or "BMP" or "TGA" => "image-x-generic",
            "7Z" or "ZIP" or "RAR" or "AMB" or "CPK" or "ACB" or "CSB" => "package-x-generic",
            "WAV" or "MP3" or "FLAC" or "ADX" => "audio-x-generic",
            "EXE" or "DLL" => "application-x-executable",
            "BAT" or "COM" or "PY" or "SH" or "CT" or "CFG" or "INI" => "text-x-script",
            "TXT" or "RTF" => "text-x-generic",
            _ => "image-missing"
        };

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
                            imageKey = GetIconByExtension(extension.Substring(1));

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
