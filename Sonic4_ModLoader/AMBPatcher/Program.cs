using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Common.Mods;
using Common.Launcher;

namespace AMBPatcher
{
    class Program
    {
        public class AMB_old
        {
            //This method is used for Windows Phone version AMB files
            //TODO: use it later in AMB.cs
            public static List<(string Name, int Pointer, int Length)> ReadWP(byte[] rawFile)
            {
                List<int> filePointers = new List<int>();
                List<int> fileLengths = new List<int>();
                List<string> fileNames = new List<string>();
                var result = new List<(string Name, int Pointer, int Length)>();

                int fileNumber = BitConverter.ToInt32(rawFile, 0x00);

                var sb = new StringBuilder();

                var ptr = 0xD;
                for (int currentFile = 0; currentFile < fileNumber; ptr++)
                {
                    for (; rawFile[ptr] >= 0x20 && rawFile[ptr] < 0x7F; ptr++)
                        sb.Append((char)rawFile[ptr]);
                    if (sb.Length > 0)
                        fileNames.Add(sb.ToString());
                    sb.Clear();
                    if (fileNames.Count > currentFile)
                        currentFile++;
                }

                for (int i = 0; i < fileNumber; i++)
                {
                    filePointers.Add(BitConverter.ToInt32(rawFile, ptr + 4 * i));
                    fileLengths.Add(BitConverter.ToInt32(rawFile, ptr + 4 * fileNumber + i * 4));
                }

                for (int i = 0; i < fileNumber; i++)
                    result.Add((Name: fileNames[i],
                                Pointer: filePointers[i],
                                Length: fileLengths[i]));

                return result;
            }

            public static void ExtractWP(string fileName, string outputDirectory)
            {
                byte[] raw_file = File.ReadAllBytes(fileName);

                Directory.CreateDirectory(outputDirectory);

                var files = AMB_old.ReadWP(raw_file);

                for (int i = 0; i < files.Count; i++)
                {
                    string outputFile = Path.Combine(outputDirectory, files[i].Name.Replace('\\', Path.DirectorySeparatorChar));

                    //Copying raw file from the archive into a byte array.
                    byte[] fileBytes = new byte[files[i].Length];
                    Array.Copy(raw_file, files[i].Pointer, fileBytes, 0, files[i].Length);

                    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));

                    //And writing that byte array into a file
                    File.WriteAllBytes(outputFile, fileBytes);
                }
            }
        }

        static void PatchAll(string file_name, List<string> mod_files, List<string> mod_paths)
        {
            if (File.Exists(file_name))
            {
                if (file_name.EndsWith(".AMB", StringComparison.OrdinalIgnoreCase))
                {
                    if (ShaChecker.ShaChanged(file_name, mod_files, mod_paths))
                    {
                        AMB amb;
                        if (file_name == mod_files.FirstOrDefault())
                            amb = new AMB(Path.Combine(mod_paths[0], mod_files[0]));
                        else if (File.Exists(file_name + ".bkp"))
                            amb = new AMB(file_name + ".bkp");
                        else
                            amb = new AMB(file_name);
                        amb.AmbPath = file_name;

                        for (int i = 0; i < mod_files.Count; i++)
                        {
                            var mod_file_full = Path.Combine("mods", mod_paths[i], mod_files[i]);
                            ProgressBar.PrintProgress(i, mod_files.Count, mod_file_full);
                            amb.Add(mod_file_full);
                            ShaChecker.ShaWrite(mod_files[i], mod_file_full);
                        }

                        amb.Save(file_name);
                    }
                }
                else if (file_name.ToUpper().EndsWith(".CSB"))
                {
                    if (ShaChecker.ShaChanged(file_name.Substring(0, file_name.Length - 4), mod_files, mod_paths))
                    {
                        Recover(file_name);
                        if (file_name.EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                            Recover(file_name.Substring(0, file_name.Length - 4) + ".CPK");

                        ProgressBar.PrintProgress(0, 100, "Asking CsbEditor to unpack " + file_name);

                        //Needs CSB Editor (from SonicAudioTools) to work
                        //FIXME
                        if (!Launcher.LaunchCsbEditor(file_name))
                            throw new Exception("CsbEditor not found (PatchAll)");

                        for (int i = 0; i < mod_files.Count; i++)
                        {
                            string mod_file = Path.Combine("mods", mod_paths[i], mod_files[i]);

                            ProgressBar.PrintProgress(i, mod_files.Count, mod_file);
                            File.Copy(mod_file, mod_files[i], true);

                            ShaChecker.ShaWrite(mod_files[i], mod_file);
                        }

                        ProgressBar.PrintProgress(99, 100, "Asking CsbEditor to repack " + file_name);
                        Launcher.LaunchCsbEditor(file_name.Substring(0, file_name.Length - 4));
                    }
                }
            }
        }

        static void Backup(string file_name)
        {
            if (!File.Exists(file_name + ".bkp"))
                File.Copy(file_name, file_name + ".bkp");
        }

        static void Recover(string file_name)
        {
            if (File.Exists(file_name + ".bkp"))
                File.Copy(file_name + ".bkp", file_name, true);
        }

        static List<(string OrigFile, List<string> ModFiles, List<string> ModName)> GetModFiles()
        {
            /* returns a list of:
             * list[0].OrigFile = Path to original file
             * list[0].ModFiles = List of mod files
             * list[0].ModName = List of mod names of ModFiles
             */

            var result = new List<(string OrigFile, List<string> ModFiles, List<string> ModName)>();

            //Reading the mods.ini file
            if (!File.Exists("mods/mods.ini"))
                return result;

            //The mods.ini contains directory names of the enabled mods in reversed priority
            /*e.g.
             * Mod 3
             * Mod 2
             * Mod 1
             */

            
            //TODO: Make it work with forward order, remove Reverse()
            var modsIni = Mods.GetMods().Where(x => x.Enabled).Select(x => x.Path).Reverse().ToList();

            List<string> orig_files = new List<string>();
            List<List<string>> mod_files = new List<List<string>>();
            List<List<string>> mod_dirs = new List<List<string>>();

            for (int i = 0; i < modsIni.Count; i++)
            {
                if (Directory.Exists("mods/" + modsIni[i]))
                {
                    var filenames = Directory.GetFiles(Path.Combine("mods",modsIni[i]), "*", SearchOption.AllDirectories).OrderBy(x => x).ToArray();

                    for (int j = 0; j < filenames.Length; j++)
                    {
                        //Getting "folder/file" from "mods/mod/folder/file/mod_file"
                        var filename_parts = filenames[j].Split(Path.DirectorySeparatorChar);
                        var original_file = "";

                        for (int k = 0; k < filename_parts.Length - 2; k++)
                        {
                            var possible_orig_file = Path.Combine(filename_parts.Skip(2).Take(k + 1).ToArray());

                            if (File.Exists(possible_orig_file))
                            {
                                original_file = possible_orig_file; break;
                            }
                            else if (File.Exists(possible_orig_file + ".CSB"))
                            {
                                original_file = possible_orig_file + ".CSB"; break;
                            }
                        }

                        if (original_file == "") continue;

                        //Getting "folder/file/mod_file" from "mods/mod/folder/file/mod_file"
                        var mod_file = Path.Combine(filename_parts.Skip(2).ToArray());

                        //Getting "mod" from "mods/mod/folder/file/mod_file"
                        var mod_path = filename_parts[1];

                        //Adding all of this into lists
                        if (!orig_files.Contains(original_file))
                        {
                            orig_files.Add(original_file);
                            mod_files.Add(new List<string> { });
                            mod_dirs.Add(new List<string> { });
                        }

                        var ind = orig_files.IndexOf(original_file);

                        if (mod_files[ind].Contains(mod_file))
                        {
                            /* Updating queue of the files that will be modified
                             * to correspond to the given mod priority.
                             * This is needed because the old single-depth no replacing
                             * method doesn't work correctly (theoretically) after some new features.
                             * Before it was not a queue at all.
                             * Thank you for your attention.
                             * 
                             * ~OSA413
                             */
                            var mod_index = mod_files[ind].IndexOf(mod_file);
                            mod_files[ind].RemoveAt(mod_index);
                            mod_dirs[ind].RemoveAt(mod_index);
                        }

                        mod_files[ind].Add(mod_file);
                        mod_dirs[ind].Add(mod_path);
                    }
                }
            }

            //Into a Tuple into a List
            for (int i = 0; i < orig_files.Count; i++)
                result.Add((OrigFile: orig_files[i], ModFiles: mod_files[i], ModName: mod_dirs[i]));
            
            return result;
        }

        static void ShowHelpMessage()
        {
            string text = "\t\tAMB Patcher by OSA413"
            + "\n\t\tReleased under the MIT License"
            + "\n\t\thttps://github.com/OSA413"
            + "\n"
            + "\nUsage:"
            + "\n\tAMBPatcher - If \"mods\" directory exists, patch all files used by enabled mods, else show help message."
            + "\n\tAMBPatcher [AMB] and"
            + "\n\tAMBPatcher extract [AMB] - Extract all files from [AMB] to \"[AMB]_extracted\" directory."
            + "\n\tAMBPatcher extract [AMB] [dir] - Extract all files from [AMB] to [dir]."
            + "\n\tAMBPatcher read [AMB file] - Prints content of [AMB]"
            + "\n\tAMBPatcher [AMB] [dir] and"
            + "\n\tAMBPatcher add [AMB] [dir] - Add all files from [dir] to [AMB]."
            + "\n\tAMBPatcher recover - Recover original files that were changed."
            + "\n\tAMBPatcher add [AMB] [file] - Add [file] to [AMB]."
            + "\n\tAMBPatcher add [AMB] [file] [name] - Add [file] to [AMB] as [name]."
            + "\n\tAMBPatcher endianness [AMB] - Print endianness of [AMB]."
            + "\n\tAMBPatcher swap_endianness [AMB] - Swaps endianness of [AMB]."
            + "\n\tAMBPatcher delete [AMB] [file] - Delete [file] from [AMB]."
            + "\n\tAMBPatcher create [name] - Creates an empty AMB file with [name]."
            + "\n\tAMBPatcher extract_all [path] - Extract all files from [path] (can be a file or directory) to be Mod Loader compatible."
            + "\n\tAMBPatcher -h and"
            + "\n\tAMBPatcher --help - Show help message.";

            Console.WriteLine(text);
        }

        static void Main(string[] args)
        {
            Settings.Load();

            if (args.Length == 0)
            {
                if (!File.Exists("mods/mods.ini"))
                {
                    ShowHelpMessage();
                    return;
                }

                var test = GetModFiles();
                List<string> mods_prev = new List<string> { };
                List<string> modified_files = new List<string> { };

                if (File.Exists("mods/mods_prev"))
                    mods_prev = File.ReadAllLines("mods/mods_prev").ToList<string>();

                ProgressBar.PrintFiller();
                ProgressBar.MoveCursorUp();

                for (int i = 0; i < test.Count; i++)
                {
                    modified_files.Add(test[i].OrigFile);
                    
                    ProgressBar.PrintProgress(i, test.Count, "Modifying \"" + test[i].OrigFile + "\"...");
                    ProgressBar.MoveCursorDown();

                    Backup(test[i].OrigFile);
                    //Some CSB files may have CPK archive
                    if (File.Exists(test[i].OrigFile.Substring(0, test[i].OrigFile.Length - 4) + ".CPK"))
                        Backup(test[i].OrigFile.Substring(0, test[i].OrigFile.Length - 4) + ".CPK");

                    PatchAll(test[i].OrigFile, test[i].ModFiles, test[i].ModName);
                    mods_prev.Remove(test[i].OrigFile);

                    ProgressBar.MoveCursorUp();
                }

                ProgressBar.PrintProgress(1, 1, "");
                ProgressBar.MoveCursorDown();
                ProgressBar.PrintProgress(1, 1, "");

                for (int i = 0; i < mods_prev.Count; i++)
                {
                    Recover(mods_prev[i]);
                    //Some CSB files may have CPK archive
                    if (mods_prev[i].EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                        Recover(mods_prev[i].Substring(0, mods_prev[i].Length - 4) + ".CPK");
                    
                    if (mods_prev[i].EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                        ShaChecker.ShaRemove(mods_prev[i].Substring(0, mods_prev[i].Length - 4));
                    else
                        ShaChecker.ShaRemove(mods_prev[i]);
                }

                if (Directory.Exists("mods"))
                    File.WriteAllText("mods/mods_prev", string.Join("\n", modified_files.ToArray()));
            }

            else if (args.Length == 1)
            {
                if (args[0] == "-h" || args[0] == "--help")
                    ShowHelpMessage();
                else if (args[0] == "recover")
                {
                    if (File.Exists("mods/mods_prev"))
                    {
                        string[] mods_prev = File.ReadAllLines("mods/mods_prev");

                        for (int i = 0; i < mods_prev.Length; i++)
                        {
                            string file = mods_prev[i];
                            
                            ProgressBar.PrintProgress(i, mods_prev.Length, "Recovering \""+ file +"\" file...");

                            Recover(file);
                            if (file.EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                            {
                                Recover(file.Substring(0, file.Length - 4) + ".CPK");
                                ShaChecker.ShaRemove(file.Substring(0, file.Length - 4));
                            }
                            else
                            {
                                ShaChecker.ShaRemove(file);
                            }
                        }
                        File.Delete("mods/mods_prev");
                        ProgressBar.PrintProgress(1, 1, "");
                    }
                }
                else
                {
                    if (File.Exists(args[0]))
                        new AMB(args[0]).Extract();
                    else if (Directory.Exists(args[0]))
                    {
                        AMB amb;
                        if (args[0].EndsWith("_extracted")
                            && (File.Exists(args[0].Substring(0, args[0].Length - 14) + ".AMB")
                            || File.Exists(args[0].Substring(0, args[0].Length - 14) + ".amb")))
                        {
                            var filePath = args[0].Substring(0, args[0].Length - 14) + ".AMB";
                            if (!File.Exists(filePath))
                                filePath = args[0].Substring(0, args[0].Length - 14) + ".amb";
                            amb = new AMB(filePath);
                        }
                        else
                            amb = new AMB();
                        amb.AmbPath = args[0];
                        var files = Directory.GetFiles(args[0], "*.*", SearchOption.AllDirectories).OrderBy(x => x);
                        foreach (var f in files)
                            amb.Add(f);
                        amb.Save(args[0] + ".AMB");
                    }
                    else ShowHelpMessage();
                }
            }

            else if (args.Length == 2)
            {
                if (args[0] == "extract")
                {
                    if (File.Exists(args[1]))
                        new AMB(args[1]).Extract();
                    else ShowHelpMessage();
                }
                else if (args[0] == "read")
                {
                    if (File.Exists(args[1]))
                    {
                        var amb = new AMB(args[1]);

                        foreach (var o in amb.Objects)
                        {
                            Console.WriteLine("\nFile name:    " + o.Name);
                            Console.WriteLine("File pointer: " + o.Pointer + "\t(0x" + o.Pointer.ToString("X") + ")");
                            Console.WriteLine("File length:  " + o.Length + "\t(0x" + o.Length.ToString("X") + ")");
                        }
                    }
                    else ShowHelpMessage();
                }
                else if (File.Exists(args[0]) && Directory.Exists(args[1]))
                {
                    var files = Directory.GetFiles(args[1], "*", SearchOption.AllDirectories);
                    if (files.Length == 0) return;

                    var amb = new AMB(args[0]);
                    foreach (string file in files)
                    {
                        Console.WriteLine("Patching by \"" + file + "\"...");
                        amb.Add(file);
                    }
                    amb.Save();
                    Console.WriteLine("Done.");
                }
                else if (args[0] == "swap_endianness" && File.Exists(args[1]))
                    new AMB(args[1]).Save(args[1], true);

                else if (args[0] == "endianness" && File.Exists(args[1]))
                {
                    if (new AMB(args[1]).IsLittleEndian())
                        Console.WriteLine("Little endian");
                    else
                        Console.WriteLine("Big endian");
                }

                else if (args[0] == "create")
                    new AMB().Save(args[1]);

                else if (args[0] == "extract_all")
                {
                    string[] files = { args[1] };
                    if (Directory.Exists(args[1]))
                        files = Directory.GetFiles(args[1], "*.*", SearchOption.AllDirectories);

                    foreach (var f in files)
                        new AMB(f).ExtractAll();
                }

                else if (args[0] == "extract_wp")
                {
                    if (File.Exists(args[1]))
                        AMB_old.ExtractWP(args[1], args[1] + "_extracted");
                    else
                        ShowHelpMessage();
                }

                else if (args[0] == "recreate" && File.Exists(args[1]))
                {
                    var amb = new AMB(args[1]);
                    amb.Save(args[1]+"_recreated");
                }

                else ShowHelpMessage();
            }

            else if (args.Length == 3)
            {
                if (args[0] == "find")
                {
                    var amb = new AMB(args[1]);
                    var a = amb.FindObject(args[2]);
                    foreach (var o in a.amb.Objects)
                        Console.WriteLine(o.Name);
                    Console.ReadLine();
                }

                if (args[0] == "extract")
                {
                    if (File.Exists(args[1]))
                        new AMB(args[1]).Extract(args[2]);

                    else ShowHelpMessage();
                }
                else if (args[0] == "add")
                {
                    if (File.Exists(args[1]) && File.Exists(args[2]))
                    {
                        var amb = new AMB(args[1]);
                        amb.Add(args[2]);
                        amb.Save();
                    }
                    else if (File.Exists(args[1]) && Directory.Exists(args[2]))
                    {
                        var files = Directory.GetFiles(args[2], "*", SearchOption.AllDirectories).OrderBy(x => x);

                        var amb = new AMB(args[1]);
                        foreach (var file in files)
                            amb.Add(file);
                        amb.Save();
                    }
                    else ShowHelpMessage();
                }
                else if (args[0] == "delete" || args[0] == "remove")
                {
                    if (File.Exists(args[1]))
                    {
                        var amb = new AMB(args[1]);
                        amb.Remove(args[2]);
                        amb.Save();
                    }else ShowHelpMessage();
                }
                else ShowHelpMessage();
            }
            else if (args.Length == 4)
            {
                if (args[0] == "add")
                {
                    if (File.Exists(args[1]) && File.Exists(args[2]))
                    {
                        var amb = new AMB(args[1]);
                        amb.Add(args[2], args[3]);
                        amb.Save();
                    }
                    else ShowHelpMessage();
                }
                else ShowHelpMessage();
            }
            else ShowHelpMessage();
        }
    }
}
