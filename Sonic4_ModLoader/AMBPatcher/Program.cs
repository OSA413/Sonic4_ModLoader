using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

using Common.IniReader;
using Common.ValueUpdater;
using Common.Launcher;
using AMB;

namespace AMBPatcher
{
    class Program
    {
        public static bool GenerateLog;
        public static bool SHACheck;
        public static int  SHAType;
        
        public static class Log
        {
            public static void Write(string Message)
            {
                if (!GenerateLog) return;
                File.AppendAllText("AMBPatcher.log", Message + Environment.NewLine);
            }
            
            public static void Reset()
            {
                if (!GenerateLog) return;
                File.WriteAllText("AMBPatcher.log", "");
            }
        }

        public static class Settings
        {
            public static void Load()
            {
                ProgressBar.Enabled = true;
                GenerateLog = false;
                SHACheck    = true;
                SHAType     = 1;

                var cfg = IniReader.Read("AMBPatcher.cfg");
                if (!cfg.ContainsKey(IniReader.DEFAULT_SECTION)) return;

                ValueUpdater.UpdateIfKeyPresent(cfg, "ProgressBar", ref ProgressBar.Enabled);
                ValueUpdater.UpdateIfKeyPresent(cfg, "GenerateLog", ref GenerateLog);
                ValueUpdater.UpdateIfKeyPresent(cfg, "SHACheck", ref SHACheck);
                ValueUpdater.UpdateIfKeyPresent(cfg, "SHAType", ref SHAType);
            }
        }

        public static class ProgressBar
        {
            public static bool Enabled;

            public static void PrintProgress(int i, int max_i, string title)
            {
                if (!ProgressBar.Enabled ||
                    Console.WindowWidth <= 0)
                    return;

                int bar_len = 50;
                ProgressBar.MoveCursorUp();
                
                int cut = 0;
                if (title.Length > Console.WindowWidth - 1)
                    cut =  title.Length - Console.WindowWidth + 1;

                //What it is doing
                ProgressBar.ClearLine();
                if (i == max_i) Console.WriteLine("Done!");
                else            Console.WriteLine(title.Substring(cut));
                
                //Percentage
                ProgressBar.ClearLine();
                Console.WriteLine("[" + new string('#', bar_len * i / max_i)
                                    + new string(' ', bar_len - bar_len * i / max_i)
                                    + "] (" + (i * 100 / max_i).ToString() + "%)");
            }

            public static void ClearLine()
            {
                if (Console.WindowWidth <= 0) return;
                Console.CursorLeft = 0;
                Console.Write(new string(' ', Console.WindowWidth-1));
                Console.CursorLeft = 0;
            }

            public static void MoveCursorUp(int i = 2)
            {
                if (!ProgressBar.Enabled) return;
                Console.CursorTop -= Math.Min(i, Console.CursorTop);
            }

            public static void MoveCursorDown(int i = 2)
            {
                if (!ProgressBar.Enabled) return;
                Console.CursorTop += i;
            }

            public static void PrintFiller()
            {
                if (!ProgressBar.Enabled) return;
                Console.WriteLine("Doing absolutely nothing!"
                                + "\nProgress bar goes here"
                                + "\nSub-task!"
                                + "\nsub%");
            }
        }

        public static SHA1CryptoServiceProvider SHA1csp = new SHA1CryptoServiceProvider();
        public static SHA256CryptoServiceProvider SHA256csp = new SHA256CryptoServiceProvider();
        public static SHA384CryptoServiceProvider SHA384csp = new SHA384CryptoServiceProvider();
        public static SHA512CryptoServiceProvider SHA512csp = new SHA512CryptoServiceProvider();

        static string Sha(byte[] file)
        {
            byte[] hash;

            switch (SHAType)
            {
                case 512: hash = SHA512csp.ComputeHash(file); break;
                case 384: hash = SHA384csp.ComputeHash(file); break;
                case 256: hash = SHA256csp.ComputeHash(file); break;
                default:  hash = SHA1csp.ComputeHash(file); break;
            }

            return BitConverter.ToString(hash).Replace("-","");
        }
        
        static void ShaRemove(string file_name)
        {
            string orig_file_sha_root = Path.Combine("mods_sha", file_name);

            if (Directory.Exists(orig_file_sha_root))
            {
                var sha_files = Directory.GetFiles(orig_file_sha_root, "*", SearchOption.AllDirectories);

                foreach (string file in sha_files)
                    File.Delete(file);
            }
        }

        static bool ShaChanged(string file_name, List<string> mod_files, List<string> mod_paths)
        {
            if (!SHACheck) return true;

            bool files_changed = false;
            
            List<string> sha_list = new List<string> { };

            string orig_file_sha_root = Path.Combine("mods_sha", file_name);

            if (Directory.Exists(orig_file_sha_root))
                sha_list = new List<string>(Directory.GetFiles(orig_file_sha_root, "*.txt", SearchOption.AllDirectories));

            //Checking SHA1s
            for (int i = 0; i < mod_files.Count; i++)
            {
                if (files_changed) { break; }

                string mod_file_full    = Path.Combine("mods", mod_paths[i], mod_files[i] );
                string mod_file_sha     = Path.Combine("mods_sha", mod_files[i] + ".txt");

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

        static void ShaWrite(string relative_mod_file_path, string full_mod_file_path)
        {
            string sha_file = Path.Combine("mods_sha", relative_mod_file_path + ".txt");
            string sha_dir = Path.GetDirectoryName(sha_file);

            Directory.CreateDirectory(sha_dir);
            File.WriteAllText(sha_file, Sha(File.ReadAllBytes(full_mod_file_path)));
        }

        //////////////////
        //Main functions//
        //////////////////
        
        public class AMB
        {
            ////////////////////////////////////////////////////
            //Some internal functions that help do some things//
            ////////////////////////////////////////////////////

            public class Internal
            {
                public static (string InternalName, int InternalIndex, string ParentName, int ParentIndex) GetInternalThings(byte[] raw_file, string OriginalFileName, string ModFileName)
                {
                    /*
                    * InternalName (like from AMB.Read)
                    * InternalIndex is index of file name in AMB.Read, if present
                    * ParentName is AMB.Read()[ParentIndex].Name, if present
                    * ParentIndex is index of parent file in AMB.Read, if present
                    */

                    var files = AMB.Read(raw_file);

                    /////////////////
                    //Internal Name//
                    /////////////////

                    string InternalName;
                    int InternalIndex = -1;
                    
                    //Turning "C:\1\2\3" into {"C:","1","2","3"}
                    string[] mod_file_parts = ModFileName.Replace('/', '\\').Split('\\');
                    
                    string orig_file_last = Path.GetFileName(OriginalFileName);

                    //Trying to find where the original file name starts in the mod file name.
                    int index = Array.IndexOf(mod_file_parts, orig_file_last);
                    
                    //If it's inside, return the part after original file ends
                    if (index != -1)
                        InternalName = String.Join("\\", mod_file_parts.Skip(index + 1).ToArray());
                    //Else use file name
                    else
                        InternalName = mod_file_parts.Last();

                    //This may occur when main file and added file have the same name
                    if (InternalName == "")
                        InternalName = mod_file_parts.Last();

                    //Find internal index
                    for (int i = 0; i < files.Count; i++)
                    {
                        if (InternalName == files[i].Name)
                        { InternalIndex = i; break; }
                    }
                    
                    int ParentIndex = -1;
                    string ParentName = "";

                    for (int i = 0; i < files.Count; i++)
                    {
                        if ((InternalName + "\\").StartsWith(files[i].Name + "\\"))
                        {
                            ParentIndex = i;
                            ParentName = files[i].Name;
                            break;
                        }
                    }
                    
                    return (InternalName:  InternalName,
                            InternalIndex: InternalIndex,
                            ParentName:    ParentName,
                            ParentIndex:   ParentIndex);
                }

                public static string MakeNameSafe(string rawName)
                {
                    //removing ".\" in the names (Windows can't create "." folders)
                    //sometimes they can have several ".\" in the names
                    //Turns out there's a double dot directory in file names
                    //And double backslash in file names
                    int safeIndex = 0;
                    while (rawName[safeIndex] == '.' || rawName[safeIndex] == '\\' || rawName[safeIndex] == '/' )
                        safeIndex++;

                    if (safeIndex == 0)
                        return rawName;
                    return rawName.Substring(safeIndex);
                }
            }

            ////////
            //Read//
            ////////
            
            public static List<(string Name, int Pointer, int Length)> Read(byte[] raw_file)
            {
                List<int>       files_pointers  = new List<int>();
                List<int>       files_lens      = new List<int>();
                List<string>    files_names     = new List<string>();
                var result = new List<(string Name, int Pointer, int Length)>();

                if (!AMB.IsAMB(raw_file))
                    return result;

                if (BitConverter.IsLittleEndian != AMB.IsLittleEndian(raw_file))
                    raw_file = AMB.SwapEndianness(raw_file);

                int files_counter = BitConverter.ToInt32(raw_file, 0x10);

                //Some AMB files have no file inside of them
                if (files_counter == 0)
                    return result;

                int list_pointer  = BitConverter.ToInt32(raw_file, 0x14);
                //This is actually used to identify extra zero bytes in messed AMBs
                int DataPointer = BitConverter.ToInt32(raw_file, 0x18);

                int has_dummy_bytes = 0;
                if (DataPointer == 0)
                {
                    //Well, this means that we have "empty" integers in the places where pointers should be.
                    //We need to skip them
                    //Only iOS version of Episode 1 is caught at having this "defect".
                    has_dummy_bytes = 4;
                    DataPointer = BitConverter.ToInt32(raw_file, 0x18 + has_dummy_bytes);
                }

                for (int i = 0; i < files_counter; i++)
                {
                    int point = list_pointer + i * (0x10 + has_dummy_bytes);

                    //Sometimes the lines where pointer and lenght should be are empty
                    if (BitConverter.ToInt32(raw_file, point) != 0)
                    {
                        files_pointers.Add(BitConverter.ToInt32(raw_file, point));
                        files_lens.Add(BitConverter.ToInt32(raw_file, point + 4 + has_dummy_bytes));
                    }
                }

                //Actual number of files inside may differ from the number given in the header
                files_counter = files_pointers.Count;
                int name_pointer = BitConverter.ToInt32(raw_file, 0x1C + has_dummy_bytes);

                //Some AMB files have no names of their files
                if (name_pointer != 0)
                {
                    //Encoding characters from HEX to ASCII characters
                    string files_names_raw = Encoding.ASCII.GetString(raw_file, name_pointer, raw_file.Length - name_pointer);
                    string[] files_names_str = files_names_raw.Split(new char[] {'\x00'}, StringSplitOptions.RemoveEmptyEntries);
                    files_names.AddRange(files_names_str);

                    //Cleaning up the names
                    for (int i = 0; i < files_names.Count; i++)
                        files_names[i] = AMB.Internal.MakeNameSafe(files_names[i]);
                }
                else
                {
                    //If there're no names, setting file names to their indexes
                    for (int i = 0; i < files_counter; i++)
                        files_names.Add(i.ToString("D8"));
                }

                for (int i = 0; i < files_counter; i++)
                    result.Add((Name:       files_names[i],
                                Pointer:    files_pointers[i],
                                Length:     files_lens[i]));

                return result;
            }
            
            public static List<(string Name, int Pointer, int Length)> Read(string file_name)
            {
                return AMB.Read(File.ReadAllBytes(file_name));
            }

            //This method is used for Windows Phone version AMB files
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

            ///////////
            //Extract//
            ///////////

            public static void Extract(string file_name, string output)
            {
                byte[] raw_file = File.ReadAllBytes(file_name);
                if (!AMB.IsAMB(raw_file)) return;

                Directory.CreateDirectory(output);
                
                if (BitConverter.IsLittleEndian != AMB.IsLittleEndian(raw_file))
                    raw_file = AMB.SwapEndianness(raw_file);

                var files = AMB.Read(raw_file);

                for (int i = 0; i < files.Count; i++)
                {
                    string output_file = Path.Combine(output, files[i].Name.Replace('\\', Path.DirectorySeparatorChar));

                    //Copying raw file from the archive into a byte array.
                    byte[] file_bytes = new byte[files[i].Length];
                    Array.Copy(raw_file, files[i].Pointer, file_bytes, 0, files[i].Length);

                    Directory.CreateDirectory(Path.GetDirectoryName(output_file));

                    //And writing that byte array into a file
                    File.WriteAllBytes(output_file, file_bytes);
                }
            }

            public static void ExtractWP(string fileName, string outputDirectory)
            {
                byte[] raw_file = File.ReadAllBytes(fileName);

                Directory.CreateDirectory(outputDirectory);

                var files = AMB.ReadWP(raw_file);

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

            ///////////////
            //Extract all//
            ///////////////

            public static void ExtractAll(string path)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        AMB.Extract(path, path + "_e");
                    }
                    catch
                    {
                        Console.WriteLine("Error extracting " + path);
                    }

                    if (Directory.Exists(path + "_e"))
                    {
                        File.Delete(path);
                        Directory.Move(path + "_e", path);
                        AMB.ExtractAll(path);
                    }
                }
                else if (Directory.Exists(path))
                {
                    foreach (string sub_path in Directory.GetFileSystemEntries(path))
                        AMB.ExtractAll(sub_path);
                }
            }

            /////////
            //Patch//
            /////////
            
            public static byte[] Patch(byte[] raw_file, string OrigFileName, string ModFileName, string ModFilePath)
            {
                if (!AMB.IsAMB(raw_file)) return raw_file;

                //This will make patching work with different endianness
                bool swap_endianness_back = BitConverter.IsLittleEndian != AMB.IsLittleEndian(raw_file);
                if (swap_endianness_back)
                    raw_file = AMB.SwapEndianness(raw_file);

                //Why do I need the original file name? To patch files that are inside of an AMB file that is inside of an AMB file that is inside of ...
                var files = AMB.Read(raw_file);

                var it = AMB.Internal.GetInternalThings(raw_file, OrigFileName, ModFileName);
                
                //If mod file is in the original file or it has a parent file.
                if (it.InternalIndex != -1 || it.ParentIndex != -1)
                {
                    byte[] raw_mod_file;

                    //In case if the file is in another file, we need to get patched(!) parent file and patch original file.
                    //Else we can simply get mod file and patch original one.
                    
                    //If the file file is in the original file (not in a parent file)
                    if (it.InternalIndex != -1)
                    {
                        raw_mod_file = File.ReadAllBytes(ModFilePath);
                    }
                    //Else we get parent file and patch it.
                    //This is the place where recursive patching begins
                    else
                    {
                        //These two lines "extract" parent file (TODO: add a feature to extract only one file)
                        byte[] parent_raw = new byte[files[it.ParentIndex].Length];
                        Array.Copy(raw_file, files[it.ParentIndex].Pointer, parent_raw,
                        0, files[it.ParentIndex].Length);

                        //This returns patched parent file
                        raw_mod_file = AMB.Patch(parent_raw,
                                                 it.ParentName,
                                                 it.InternalName,
                                                 ModFilePath);

                        //Now we have a parent file, so we need to replace it.
                        it.InternalName  = it.ParentName;
                        it.InternalIndex = it.ParentIndex;
                    }

                    Log.Write("AMB.Patch: replacing original file with " + ModFileName);

                    int namePointer = BitConverter.ToInt32(raw_file, 0x1C);
                    //This makes length of the mod file to be % 16 = 0
                    int rawModLen = raw_mod_file.Length + (16 - raw_mod_file.Length % 16) % 16;

                    int finalPartPointer;
                    //If this is the last file, start copying from names
                    if (it.InternalIndex == files.Count - 1)
                        finalPartPointer = namePointer;
                    else
                        finalPartPointer = files[it.InternalIndex + 1].Pointer;

                    //The length difference between the mod file and the original one.
                    int len_dif = rawModLen - finalPartPointer + files[it.InternalIndex].Pointer;
                    
                    byte[] patchedFile = new byte[raw_file.Length + len_dif];
                    
                    //Copying beginning of the file
                    Array.Copy(raw_file, 0, patchedFile, 0, files[it.InternalIndex].Pointer);

                    //Copying mod file
                    Array.Copy(raw_mod_file, 0, patchedFile, files[it.InternalIndex].Pointer, raw_mod_file.Length);

                    //Copyting final part
                    Array.Copy(raw_file, finalPartPointer, patchedFile, finalPartPointer + len_dif, raw_file.Length - finalPartPointer);

                    //Changing Name Pointer
                    Array.Copy(BitConverter.GetBytes(namePointer + len_dif), 0, patchedFile, 0x1C, 4);

                    //Changing File Length
                    Array.Copy(BitConverter.GetBytes(rawModLen), 0, patchedFile, 0x24 + it.InternalIndex * 0x10, 4);

                    //Changing File Pointers
                    for (int i = it.InternalIndex + 1; i < files.Count; i++)
                    {
                        int tmp_pointer = 0x20 + i * 0x10;
                        Array.Copy(BitConverter.GetBytes(BitConverter.ToInt32(patchedFile, tmp_pointer) + len_dif),
                                    0, patchedFile, tmp_pointer, 4);
                    }
                   
                    raw_file = patchedFile;
                }
                else
                {
                    Log.Write("AMB.Patch: " + ModFileName + " is not in " + OrigFileName + ", trying to add!");
                    raw_file = AMB.Add(raw_file, OrigFileName, ModFileName, ModFilePath);
                }

                if (swap_endianness_back)
                    raw_file = AMB.SwapEndianness(raw_file);

                return raw_file;
            }
            
            public static void Patch(string file_name, string mod_file)
            {
                byte[] raw_file = AMB.Patch(File.ReadAllBytes(file_name),
                                            file_name,
                                            mod_file,
                                            mod_file);
                File.WriteAllBytes(file_name, raw_file);
            }

            ///////
            //Add//
            ///////

            public static byte[] Add(byte[] raw_file, string OrigFileName, string ModFileName, string ModFilePath)
            {
                if (!AMB.IsAMB(raw_file)) return raw_file;
                //Okay, I've got a great plan:
                //Add empty file and patch it.
                //2019 note: lol this is not very effective

                //This will make addition work with different endianness
                bool swap_endianness_back = BitConverter.IsLittleEndian != AMB.IsLittleEndian(raw_file);
                if (swap_endianness_back)
                    raw_file = AMB.SwapEndianness(raw_file);

                //Some empty files from S4E1 are "broken", it's better to use a new empty file.
                if (AMB.Read(raw_file).Count == 0)
                   raw_file = AMB.Create();

                var it = AMB.Internal.GetInternalThings(raw_file, OrigFileName, ModFileName);

                if (it.InternalIndex != -1 || it.ParentIndex != -1)
                    return AMB.Patch(raw_file, OrigFileName, ModFileName, ModFilePath);
                
                int file_number  = BitConverter.ToInt32(raw_file, 0x10);
                int enum_pointer = BitConverter.ToInt32(raw_file, 0x14);
                int data_pointer = BitConverter.ToInt32(raw_file, 0x18);
                int name_pointer = BitConverter.ToInt32(raw_file, 0x1C);
                
                byte[] enumeration_part = new byte[enum_pointer + 0x10 * file_number];
                byte[] data_part        = new byte[name_pointer - enumeration_part.Length];
                byte[] name_part        = new byte[raw_file.Length - name_pointer];

                Array.Copy(raw_file, 0, enumeration_part, 0, enumeration_part.Length);
                Array.Copy(raw_file, enumeration_part.Length, data_part, 0, data_part.Length);
                Array.Copy(raw_file, name_pointer, name_part, 0, name_part.Length);

                //////////
                //Header//
                //////////
                
                //Increasing file counter by 1
                file_number += 1;
                Array.Copy(BitConverter.GetBytes(file_number), 0, enumeration_part, 0x10, 4);

                //Shifting data pointer by 0x10
                data_pointer += 0x10;
                Array.Copy(BitConverter.GetBytes(data_pointer), 0, enumeration_part, 0x18, 4);

                //Shifting name pointer by 0x20
                name_pointer += 0x20;
                Array.Copy(BitConverter.GetBytes(name_pointer), 0, enumeration_part, 0x1C, 4);

                ///////////////
                //Enumeration//
                ///////////////
                
                //Shifting other file pointers by 0x10
                for (int i = 0; i < file_number - 1; i++)
                {
                    int tmp_pointer = enum_pointer + 0x10 * i;

                    Array.Copy(BitConverter.GetBytes(BitConverter.ToInt32(enumeration_part, tmp_pointer) + 0x10),
                                                    0, enumeration_part, tmp_pointer, 4);
                }

                //Injecting empty file pointer and length
                byte[] empty_file_enumeration = new byte[0x10];
                name_pointer -= 0x10;
                Array.Copy(BitConverter.GetBytes(name_pointer), 0, empty_file_enumeration, 0x00, 4);

                name_pointer += 0x10;

                //Length
                empty_file_enumeration[0x4] = 0x10;

                byte[] mod_file_name_bytes = new byte[0x20];

                for (int i = 0; i < it.InternalName.Length; i++)
                {
                    if (i >= mod_file_name_bytes.Length) break;
                    mod_file_name_bytes[i] = (byte)it.InternalName[i];
                }
                
                raw_file = enumeration_part.Join(empty_file_enumeration,
                                    data_part, new byte[0x10],
                                    name_part, mod_file_name_bytes);
                
                raw_file = AMB.Patch(raw_file, OrigFileName, ModFileName, ModFilePath);

                if (swap_endianness_back)
                    raw_file = AMB.SwapEndianness(raw_file);

                return raw_file;
            }

            public static void Add(string file_name, string mod_file, string ModFileName)
            {
                byte[] raw_file = AMB.Add(File.ReadAllBytes(file_name), file_name, ModFileName, mod_file);
                File.WriteAllBytes(file_name, raw_file);
            }

            //////////////////
            //Get endianness//
            //////////////////
            
            public static bool IsLittleEndian(byte[] raw_file)
            {
                bool FileIsLittleEndian = BitConverter.IsLittleEndian;
                if (BitConverter.ToInt32(raw_file, 4) > 0xFFFF)
                    FileIsLittleEndian = !FileIsLittleEndian;

                return FileIsLittleEndian;
            }

            ///////////////////
            //Swap endianness//
            ///////////////////

            public static byte[] SwapEndianness(byte[] raw_file)
            {
                bool FileIsLittleEndian = IsLittleEndian(raw_file);

                List<int> PointerList = new List<int> { 0x4, //Endianness
                                                       0x10,
                                                       0x14,
                                                       0x18,
                                                       0x1C };

                byte[] FileCounter_raw = new byte[4];
                byte[] ListPointer_raw = new byte[4];

                Array.Copy(raw_file, 0x10, FileCounter_raw, 0, 4);
                Array.Copy(raw_file, 0x14, ListPointer_raw, 0, 4);

                if (FileIsLittleEndian != BitConverter.IsLittleEndian)
                {
                    Array.Reverse(FileCounter_raw);
                    Array.Reverse(ListPointer_raw);
                }

                int FileCounter = BitConverter.ToInt32(FileCounter_raw, 0);
                int ListPointer = BitConverter.ToInt32(ListPointer_raw, 0);

                for (int i = 0; i < FileCounter; i++)
                {
                    PointerList.Add(ListPointer + 0x10 * i);
                    PointerList.Add(ListPointer + 0x10 * i + 4);
                }
                
                foreach (int pointer in PointerList)
                    Array.Reverse(raw_file, pointer, 4);

                return raw_file;
            }

            ////////////////////////
            //Delete file from AMB//
            ////////////////////////

            public static byte[] Delete(byte[] raw_file, string file_name_to_delete)
            {
                //This will make deletion work with different endianness
                bool swap_endianness_back = BitConverter.IsLittleEndian != AMB.IsLittleEndian(raw_file);
                if (swap_endianness_back)
                    raw_file = AMB.SwapEndianness(raw_file);

                var files = AMB.Read(raw_file);

                int pointer_of_file_to_delete = -1;
                int len_of_file_to_delete = -1;

                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].Name == file_name_to_delete)
                    {
                        pointer_of_file_to_delete = files[i].Pointer;
                        len_of_file_to_delete = files[i].Length;
                    }
                }

                if (pointer_of_file_to_delete != -1)
                {
                    //Remember that the actual number of files inside may be different
                    int prev_file_number = BitConverter.ToInt32(raw_file, 0x10);
                    //Changing File Number
                    Array.Copy(BitConverter.GetBytes(prev_file_number - 1)
                                , 0, raw_file, 0x10, 4);

                    int list_pointer = BitConverter.ToInt32(raw_file, 0x14);

                    int prev_data_pointer = BitConverter.ToInt32(raw_file, 0x18);
                    //Changing Data Pointer
                    Array.Copy(BitConverter.GetBytes(prev_data_pointer - 0x10)
                                , 0, raw_file, 0x18, 4);

                    int prev_name_pointer = BitConverter.ToInt32(raw_file, 0x1C);
                    //Changing Name Pointer
                    Array.Copy(BitConverter.GetBytes(prev_name_pointer - 0x10 - len_of_file_to_delete)
                                , 0, raw_file, 0x1C, 4);

                    int enum_pointer_of_file_to_delete = -1;
                    int file_index = -1;

                    //decreasing file data pointers by the length of the file to delete
                    for (int i = 0; i < prev_file_number; i++)
                    {
                        int file_pointer = BitConverter.ToInt32(raw_file, list_pointer + 0x10 * i);

                        if (file_pointer != 0)
                        {
                            int tmp = len_of_file_to_delete;
                            if (enum_pointer_of_file_to_delete == -1) {tmp = 0;}

                            Array.Copy(BitConverter.GetBytes(file_pointer - tmp - 0x10)
                                        , 0, raw_file, list_pointer + 0x10 * i, 4);
                        }

                        if (file_pointer == pointer_of_file_to_delete)
                        {
                            enum_pointer_of_file_to_delete = list_pointer + 0x10 * i;
                            file_index = i;
                        }
                    }

                    //Copying the part before the pointer in the list
                    byte[] before_enum = new byte[enum_pointer_of_file_to_delete];
                    Array.Copy(raw_file, 0, before_enum, 0, before_enum.Length);

                    //Copyting the part after the pointer in the list and before the file data
                    byte[] before_data = new byte[pointer_of_file_to_delete - (enum_pointer_of_file_to_delete + 0x10)];
                    Array.Copy(raw_file, enum_pointer_of_file_to_delete + 0x10
                                , before_data, 0, before_data.Length);

                    //Copyting the part after file data and before the file name
                    byte[] before_name = new byte[prev_name_pointer + file_index * 0x20 - (pointer_of_file_to_delete + len_of_file_to_delete)];
                    Array.Copy(raw_file, pointer_of_file_to_delete + len_of_file_to_delete
                                , before_name, 0, before_name.Length);

                    //Copyting the part after file data and before the file name
                    byte[] after_name = new byte[raw_file.Length - (prev_name_pointer + (file_index + 1) * 0x20)];
                    Array.Copy(raw_file, prev_name_pointer + (file_index + 1) * 0x20
                                , after_name, 0, after_name.Length);

                    //Joining all together
                    raw_file = before_enum.Join(before_data, before_name, after_name);
                }

                if (swap_endianness_back)
                    raw_file = AMB.SwapEndianness(raw_file);

                return raw_file;
            }

            public static void Delete(string file_name, string file_name_to_delete)
            {
                File.WriteAllBytes(file_name
                                    , AMB.Delete(File.ReadAllBytes(file_name), file_name_to_delete));
            }

            /////////////////////////
            //Create empty AMB file//
            /////////////////////////

            public static byte[] Create()
            {
                byte[] raw_file = new byte[0x20];

                //The header
                Array.Copy(Encoding.ASCII.GetBytes("#AMB"), 0, raw_file, 0, 4);

                //Endianness identifier (at least for AMBPatcher)
                Array.Copy(BitConverter.GetBytes(0x1304), 0, raw_file, 0x04, 4);

                byte[] bytes0x20 = BitConverter.GetBytes(0x20);
                //List pointer
                Array.Copy(bytes0x20, 0, raw_file, 0x14, 4);
                //Data pointer
                Array.Copy(bytes0x20, 0, raw_file, 0x18, 4);
                //Name pointer
                Array.Copy(bytes0x20, 0, raw_file, 0x1C, 4);

                return raw_file;
            }

            ////////////////////////////////////
            //Check if file is AMB (by header)//
            ////////////////////////////////////

            public static bool IsAMB(byte[] raw_file)
            {
                return raw_file.Length >= 4
                && raw_file[0] == '#'
                && raw_file[1] == 'A'
                && raw_file[2] == 'M'
                && raw_file[3] == 'B';
            }
                                
        }

        static void PatchAll(string file_name, List<string> mod_files, List<string> mod_paths)
        {
            Log.Write("> " + file_name);
            if (File.Exists(file_name))
            {
                if (file_name.EndsWith(".AMB", StringComparison.OrdinalIgnoreCase))
                {
                    if (ShaChanged(file_name, mod_files, mod_paths))
                    {
                        Recover(file_name);
                        Log.Write("PatchAll: file " + file_name + " was restored.");

                        for (int i = 0; i < mod_files.Count; i++)
                        {
                            string mod_file_full = Path.Combine("mods", mod_paths[i], mod_files[i]);
                            Log.Write(mod_file_full);
                            ProgressBar.PrintProgress(i, mod_files.Count, mod_file_full);

                            if (file_name == mod_files[i])
                            {
                                Log.Write("Replaced");
                                File.Copy(mod_file_full, file_name, true);
                            }
                            else
                            {
                                AMB.Patch(file_name, mod_file_full);
                            }

                            ShaWrite(mod_files[i], mod_file_full);
                        }
                    }
                    else Log.Write("Not changed");
                }
                else if (file_name.ToUpper().EndsWith(".CSB"))
                {
                    if (ShaChanged(file_name.Substring(0, file_name.Length - 4), mod_files, mod_paths))
                    {
                        Recover(file_name);
                        if (file_name.EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                            Recover(file_name.Substring(0, file_name.Length - 4) + ".CPK");

                        Log.Write("Asking CsbEditor to unpack");
                        ProgressBar.PrintProgress(0, 100, "Asking CsbEditor to unpack " + file_name);

                        //Needs CSB Editor (from SonicAudioTools) to work
                        //FIXME
                        if (!Launcher.LaunchCsbEditor(file_name))
                            throw new Exception("CsbEditor not found (PatchAll)");

                        for (int i = 0; i < mod_files.Count; i++)
                        {
                            string mod_file = Path.Combine("mods", mod_paths[i], mod_files[i]);

                            ProgressBar.PrintProgress(i, mod_files.Count, mod_file);
                            Log.Write(mod_file);
                            File.Copy(mod_file, mod_files[i], true);

                            ShaWrite(mod_files[i], mod_file);
                        }

                        Log.Write("Asking CsbEditor to repack");
                        ProgressBar.PrintProgress(99, 100, "Asking CsbEditor to repack " + file_name);
                        Launcher.LaunchCsbEditor(file_name.Substring(0, file_name.Length - 4));
                    }
                    else Log.Write("Not changed");
                }
            }
            else Log.Write("File not found");

            Log.Write("< " + file_name);
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
            {
                Log.Write("GetModFiles: \"mods/mods.ini\" file not found");
                return result;
            }

            //The mods.ini contains directory names of the enabled mods in reversed priority
            /*e.g.
             * Mod 3
             * Mod 2
             * Mod 1
             */
            string[] ini_mods = File.ReadAllLines("mods/mods.ini");

            List<string> orig_files = new List<string>();
            List<List<string>> mod_files = new List<List<string>>();
            List<List<string>> mod_dirs = new List<List<string>>();

            for (int i = 0; i < ini_mods.Length; i++)
            {
                if (Directory.Exists("mods/" + ini_mods[i]))
                {
                    string[] filenames = Directory.GetFiles(Path.Combine("mods",ini_mods[i]), "*", SearchOption.AllDirectories).OrderBy(x => x).ToArray();

                    for (int j = 0; j < filenames.Length; j++)
                    {
                        //Getting "folder/file" from "mods/mod/folder/file/mod_file"
                        string[] filename_parts = filenames[j].Split(Path.DirectorySeparatorChar);
                        string original_file = "";

                        for (int k = 0; k < filename_parts.Length - 2; k++)
                        {
                            string possible_orig_file = Path.Combine(filename_parts.Skip(2).Take(k + 1).ToArray());

                            if (File.Exists(possible_orig_file))
                            {
                                original_file = possible_orig_file; break;
                            }
                            else if (File.Exists(possible_orig_file + ".CSB"))
                            {
                                original_file = possible_orig_file + ".CSB"; break;
                            }
                        }

                        if (original_file == "") { continue; }

                        //Getting "folder/file/mod_file" from "mods/mod/folder/file/mod_file"
                        string mod_file = Path.Combine(filename_parts.Skip(2).ToArray());

                        //Getting "mod" from "mods/mod/folder/file/mod_file"
                        string mod_path = filename_parts[1];

                        //Adding all of this into lists
                        if (!orig_files.Contains(original_file))
                        {
                            orig_files.Add(original_file);
                            mod_files.Add(new List<string> { });
                            mod_dirs.Add(new List<string> { });
                        }

                        int ind = orig_files.IndexOf(original_file);

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
                            int mod_index = mod_files[ind].IndexOf(mod_file);
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
            + "\n\tAMBPatcher patch [AMB] [file] - Patch [AMB] by [file] if [file] is in [AMB]."
            + "\n\tAMBPatcher [AMB] [dir] and"
            + "\n\tAMBPatcher patch [AMB] [dir] - Patch [AMB] by all files in [dir]."
            + "\n\tAMBPatcher recover - Recover original files that were changed."
            + "\n\tAMBPatcher add [AMB] [file] - Add [file] to [AMB]."
            + "\n\tAMBPatcher add [AMB] [file] [name] - Add [file] to [AMB] as [name]."
            + "\n\tAMBPatcher endianness [AMB] - Print endianness of [AMB]."
            + "\n\tAMBPatcher swap_endianness [AMB] - Swaps endianness of [AMB]."
            + "\n\tAMBPatcher delete [AMB] [file] - Delete [file] from [AMB]."
            + "\n\tAMBPatcher create [name] - Creates an empty AMB file with [name]."
            + "\n\tAMBPatcher extract_all [path] - Extract all files from [path] (can be a file or directory) to be Mod Loader compatible (note: this removes original AMB files!)."
            + "\n\tAMBPatcher -h and"
            + "\n\tAMBPatcher --help - Show help message.";

            Console.WriteLine(text);
        }

        static void Main(string[] args)
        {
            Settings.Load();
            Log.Reset();

            if (args.Length == 0)
            {
                if (!File.Exists("mods/mods.ini"))
                {
                    ShowHelpMessage();
                    return;
                }

                Log.Write("Getting list of enabled mods...");
                var test = GetModFiles();

                if (GenerateLog)
                {
                    Log.Write("Content of mods.ini:");
                    Log.Write(File.ReadAllText("mods/mods.ini"));
                    Log.Write("====================");
                    Log.Write("File list:");
                    for (int i = 0; i < test.Count; i++)
                    {
                        Log.Write("\n" + test[i].OrigFile);
                        for (int j = 0; j < test[i].ModFiles.Count; j++)
                        {
                            Log.Write("\t" + test[i].ModFiles[j] + "\t" + test[i].ModName[j]);
                        }
                    }
                    Log.Write("====================");
                }

                List<string> mods_prev = new List<string> { };
                List<string> modified_files = new List<string> { };

                if (File.Exists("mods/mods_prev"))
                {
                    mods_prev = File.ReadAllLines("mods/mods_prev").ToList<string>();
                }

                Log.Write("Patching original files...");

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

                Log.Write("\nRestoring unchanged files:");
                for (int i = 0; i < mods_prev.Count; i++)
                {
                    Log.Write("Restoring " + mods_prev[i]);

                    Recover(mods_prev[i]);
                    //Some CSB files may have CPK archive
                    if (mods_prev[i].EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                        Recover(mods_prev[i].Substring(0, mods_prev[i].Length - 4) + ".CPK");
                    
                    if (mods_prev[i].EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                        ShaRemove(mods_prev[i].Substring(0, mods_prev[i].Length - 4));
                    else
                        ShaRemove(mods_prev[i]);
                    
                    
                }
                Log.Write("Restored");

                Log.Write("\nSaving list of modified files...");
                if (Directory.Exists("mods"))
                {
                    File.WriteAllText("mods/mods_prev", string.Join("\n", modified_files.ToArray()));
                    Log.Write("Saved");
                }
                else { Log.Write("But \"mods\" folder is not present!"); }

                Log.Write("\nFinished.");
            }

            else if (args.Length == 1)
            {
                if (args[0] == "-h" || args[0] == "--help")
                {
                    ShowHelpMessage();
                }
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
                                ShaRemove(file.Substring(0, file.Length - 4));
                            }
                            else
                            {
                                ShaRemove(file);
                            }
                        }
                        File.Delete("mods/mods_prev");
                        ProgressBar.PrintProgress(1, 1, "");
                    }
                }
                else
                {
                    if (File.Exists(args[0]))
                    {
                        Directory.CreateDirectory(args[0] + "_extracted");
                        AMB.Extract(args[0], args[0] + "_extracted");
                    }
                    else ShowHelpMessage();
                }
            }

            else if (args.Length == 2)
            {
                if (args[0] == "extract")
                {
                    if (File.Exists(args[1]))
                    {

                        Directory.CreateDirectory(args[1] + "_extracted");
                        AMB.Extract(args[1], args[1] + "_extracted");
                    }
                    else ShowHelpMessage();
                }
                else if (args[0] == "read")
                {
                    if (File.Exists(args[1]))
                    {
                        var data = AMB.Read(args[1]);

                        for (int i = 0; i < data.Count; i++)
                        {
                            Console.WriteLine("\nFile name:    " + data[i].Name);
                            Console.WriteLine("File pointer: " + data[i].Pointer + "\t(0x" + data[i].Pointer.ToString("X") + ")");
                            Console.WriteLine("File length:  " + data[i].Length  + "\t(0x" + data[i].Length.ToString("X") + ")");
                        }
                    }
                    else ShowHelpMessage();
                }
                else if (File.Exists(args[0]) && Directory.Exists(args[1]))
                {
                    var files = Directory.GetFiles(args[1], "*", SearchOption.AllDirectories);

                    foreach (string file in files)
                    {
                        Console.WriteLine("Patching by \"" + file + "\"...");
                        AMB.Patch(args[0], file);
                    }
                    Console.WriteLine("Done.");
                }
                else if (args[0] == "swap_endianness" && File.Exists(args[1]))
                {
                    var amb = new AMB_new(args[1]);
                    amb.Write(args[1], true);
                }
                else if (args[0] == "endianness" && File.Exists(args[1]))
                {
                    Console.WriteLine("This file's endianness is...");

                    var amb = new AMB_new(args[1]);
                    if (amb.IsLittleEndian())
                        Console.WriteLine("Little endian!");
                    else
                        Console.WriteLine("Big endian!");
                }
                else if (args[0] == "create")
                {
                    if (Path.GetDirectoryName(args[1]) != "")
                        Directory.CreateDirectory(Path.GetDirectoryName(args[1]));
                    var amb = new AMB_new();
                    amb.Write(args[1]);
                }
                else if (args[0] == "extract_all")
                {
                    AMB.ExtractAll(args[1]);
                }

                else if (args[0] == "extract_wp")
                {
                    if (File.Exists(args[1]))
                        AMB.ExtractWP(args[1], args[1] + "_extracted");
                    else
                        ShowHelpMessage();
                }

                else ShowHelpMessage();
            }

            else if (args.Length == 3)
            {
                if (args[0] == "extract")
                {
                    if (File.Exists(args[1]))
                    {
                        Directory.CreateDirectory(args[2]);
                        AMB.Extract(args[1], args[2]);
                    }
                    else ShowHelpMessage();
                }
                else if (args[0] == "add")
                {
                    if (File.Exists(args[1]) && File.Exists(args[2]))
                    {
                        var amb = new AMB_new(args[1]);
                        amb.Add(args[2]);
                        amb.Write(args[1]);
                    }
                    else if (File.Exists(args[1]) && Directory.Exists(args[2]))
                    {
                        var files = Directory.GetFiles(args[2], "*", SearchOption.AllDirectories);

                        foreach (string file in files.OrderBy(x => x))
                        {
                            Console.WriteLine("Patching by \""+file+"\"...");
                            AMB.Patch(args[1], file);
                        }
                        Console.WriteLine("Done.");
                    }
                    else ShowHelpMessage();
                }
                else if (args[0] == "delete" || args[0] == "remove")
                {
                    if (File.Exists(args[1]))
                    {
                        AMB.Delete(args[1], args[2]);
                    } else ShowHelpMessage();
                }
                else ShowHelpMessage();
            }
            else if (args.Length == 4)
            {
                if (args[0] == "add")
                {
                    if (File.Exists(args[1]) && File.Exists(args[2]))
                    {
                        AMB.Add(args[1], args[2], args[3]);
                    }
                    else ShowHelpMessage();
                }
                else ShowHelpMessage();
            }
            else ShowHelpMessage();
        }
    }

    public static class Extensions
    {
        //This thing joins two byte arrays just like Concat().ToArray() but faster (i think)
        public static byte[] Join(this byte[] first, params byte[][] others)
        {
            int totalLength = first.Length;
            foreach (byte[] arr in others)
                totalLength += arr.Length;

            byte[] output = new byte[totalLength];

            Array.Copy(first,  0, output, 0, first.Length);
            int index = first.Length;
            for (int i = 0; i < others.Length; i++)
            {
                Array.Copy(others[i], 0, output, index, others[i].Length);
                index += others[i].Length;
            }

            return output;
        }
    }

}
