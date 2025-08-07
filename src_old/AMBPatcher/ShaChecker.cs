using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace AMBPatcher
{
    public static class ShaChecker
    {
        public static HashAlgorithm SHAcsp = SHA1.Create();
        public static string Sha(byte[] file)
        {
            var hash = SHAcsp.ComputeHash(file);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public static void ShaRemove(string file_name)
        {
            string orig_file_sha_root = Path.Combine("mods_sha", file_name);

            if (Directory.Exists(orig_file_sha_root))
            {
                var sha_files = Directory.GetFiles(orig_file_sha_root, "*", SearchOption.AllDirectories);

                foreach (string file in sha_files)
                    File.Delete(file);
            }
        }

        public static bool ShaChanged(string file_name, List<string> mod_files, List<string> mod_paths)
        {
            if (!Settings.SHACheck) return true;

            bool files_changed = false;

            List<string> sha_list = new() { };

            string orig_file_sha_root = Path.Combine("mods_sha", file_name);

            if (Directory.Exists(orig_file_sha_root))
                sha_list = new List<string>(Directory.GetFiles(orig_file_sha_root, "*.txt", SearchOption.AllDirectories));

            //Checking SHA1s
            for (int i = 0; i < mod_files.Count; i++)
            {
                if (files_changed) { break; }

                string mod_file_full = Path.Combine("mods", mod_paths[i], mod_files[i]);
                string mod_file_sha = Path.Combine("mods_sha", mod_files[i] + ".txt");

                if (sha_list.Contains(mod_file_sha))
                    sha_list.Remove(mod_file_sha);
                else
                {
                    files_changed = true;
                    break;
                }

                if (File.Exists(mod_file_sha))
                {
                    string sha_tmp = Sha(File.ReadAllBytes(mod_file_full));
                    if (sha_tmp != File.ReadAllText(mod_file_sha)) { files_changed = true; }
                }
                else files_changed = true;
            }

            //Checking if there're removed files
            //And removing those SHAs
            if (sha_list.Count > 0)
            {
                files_changed = true;

                foreach (string file in sha_list)
                    File.Delete(file);
            }

            return files_changed;
        }

        public static void ShaWrite(string relative_mod_file_path, string full_mod_file_path)
        {
            string sha_file = Path.Combine("mods_sha", relative_mod_file_path + ".txt");
            string sha_dir = Path.GetDirectoryName(sha_file);

            Directory.CreateDirectory(sha_dir);
            File.WriteAllText(sha_file, Sha(File.ReadAllBytes(full_mod_file_path)));
        }

    }
}
