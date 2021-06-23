using System.IO;

namespace Common.MyIO
{
    public static class MyDirectory
    {
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