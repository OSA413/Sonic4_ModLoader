using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Common.MyIO
{
    public static class MyDirectory
    {
        public static void OpenInFileManager(string path)
        {
            if (File.Exists(path))
                path = Path.GetDirectoryName(path);

            var fileManager = "";
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:fileManager = "explorer"; break;
                case PlatformID.Unix:   fileManager = "xdg-open"; break;
                case PlatformID.MacOSX: fileManager = "open"; break;
            }
            Process.Start(fileManager, path);
        }
        public static void CopyAll(string source, string destination)
        {
            Directory.CreateDirectory(destination);

            foreach (var file in Directory.GetFiles(source))
            {
                File.Copy(file, Path.Combine(destination, Path.GetFileName(file)), true);
                File.SetAttributes(file, FileAttributes.Normal);
            }

            foreach (var dir in Directory.GetDirectories(source))
            {
                var dir_name = Path.GetFileName(dir);
                Directory.CreateDirectory(Path.Combine(destination, dir_name));
                MyDirectory.CopyAll(Path.Combine(source, dir_name), Path.Combine(destination, dir_name));
            }
        }

        public static void DeleteRecursively(string dir)
        {
            foreach (var dirr in Directory.GetDirectories(dir))
                MyDirectory.DeleteRecursively(dirr);

            foreach (var file in Directory.GetFiles(dir))
                MyFile.DeleteAnyway(file);

            //Yes, directories also can be readonly
            File.SetAttributes(dir, FileAttributes.Normal);
            //If Explorer locks the folder, it will throw "UnauthorizedAccess" instead of "Directory not empty"
            Directory.Delete(dir, true);
        }

        public static string Select(string path_to, string type)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.CheckPathExists = true;
                ofd.Title = "Select path to " + path_to;

                switch (type)
                {
                    case "dir":
                        ofd.CheckFileExists =
                        ofd.ValidateNames   = false;
                        ofd.FileName = "[DIRECTORY]";
                        break;
                    case "file":
                        ofd.Filter = "All files (*.*)|*.*";
                        break;
                    case "7z":
                        ofd.Filter = "7z.exe|7z.exe|All files (*.*)|*.*";
                        break;
                    case "arc/dir":
                        ofd.Filter = "Supported archives|*.7z;*.zip;*.rar|All files (*.*)|*.*";
                        ofd.Title += " (Enter - to select current directory)";
                        ofd.CheckFileExists =
                        ofd.ValidateNames   = false;
                        break;
                }
                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    switch (type)
                    {
                        case "dir":
                            return Path.GetDirectoryName(ofd.FileName);
                        case "arc/dir":
                            if (ofd.FileName == "-" || !File.Exists(ofd.FileName))
                                return Path.GetDirectoryName(ofd.FileName);
                            return ofd.FileName;
                        default:
                            return ofd.FileName;
                    }
                }
            }
            return null;
        }
    }

    public static class MyFile
    {
        public static void DeleteAnyway(string file)
        {
            //Program crashes if it tries to delete a read-only file
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }
    }
}