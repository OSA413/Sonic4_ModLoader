using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Security.Cryptography;

namespace AMBPatcher
{
    class Program
    {
        public static bool GenerateLog;
        public static bool ProgressBar;
        public static bool SHACheck;
        public static int  SHAType;
        
        public class Log
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

        public class Settings
        {
            public static void Load()
            {
                ProgressBar = true;
                GenerateLog = false;
                SHACheck    = true;
                SHAType     = 1;

                if (File.Exists("AMBPatcher.cfg"))
                {
                    string[] cfg_file = File.ReadAllLines("AMBPatcher.cfg");
                    
                    foreach (string line in cfg_file)
                    {
                        if (!line.Contains("=")) continue;
                        string key   = line.Substring(0, line.IndexOf("="));
                        string value = line.Substring(line.IndexOf("=") + 1);

                        if (key == "ProgressBar")
                            ProgressBar = Convert.ToBoolean(Convert.ToInt32(value));
                        
                        else if (key == "GenerateLog")
                            GenerateLog = Convert.ToBoolean(Convert.ToInt32(value));
                        
                        else if (key == "SHACheck")
                            SHACheck = Convert.ToBoolean(Convert.ToInt32(value));
                        
                        else if (key == "SHAType")
                            SHAType = Convert.ToInt32(value);
                    }
                }
            }
        }

        static void ConsoleProgressBar(int i, int max_i, string title, int bar_len)
        {
            //To prevent crashes out of nowhere when progress bar is turned off
            if (!ProgressBar) return;

            Console.CursorTop -= 2;
            
            int cut = 0;
            if (title.Length > Console.WindowWidth - 1)
                cut =  title.Length - Console.WindowWidth + 1;

            //What it is doing
            Console.Write(String.Concat(Enumerable.Repeat(" ", Console.WindowWidth-1)));
            Console.CursorLeft = 0;
            if (i == max_i) Console.WriteLine("Done!");
            else            Console.WriteLine(title.Substring(cut));
            
            //Percentage
            Console.Write(String.Concat(Enumerable.Repeat(" ", Console.WindowWidth-1)));
            Console.CursorLeft = 0;
            Console.WriteLine("[" + String.Concat(Enumerable.Repeat("#", bar_len * i / max_i)) +
                                String.Concat(Enumerable.Repeat(" ", bar_len - bar_len * i / max_i)) + "] (" + (i * 100 / max_i).ToString() + "%)");
        }

        static string Sha(byte[] file)
        {
            byte[] hash;
            string str_hash = "";

            switch (SHAType)
            {
                case 512: hash = new SHA512CryptoServiceProvider().ComputeHash(file); break;
                case 384: hash = new SHA384CryptoServiceProvider().ComputeHash(file); break;
                case 256: hash = new SHA256CryptoServiceProvider().ComputeHash(file); break;
                default:  hash = new SHA1CryptoServiceProvider().ComputeHash(file); break;
            }
            
            foreach (byte b in hash)
                str_hash += b.ToString("X");

            return str_hash;
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
            ///////////////////////////////
            //Things needed for patching //
            ///////////////////////////////

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
                
                return (InternalName: InternalName, InternalIndex: InternalIndex, ParentName: ParentName, ParentIndex: ParentIndex);
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

                int files_counter = 0;

                if (!AMB.IsAMB(raw_file))
                    return result;

                if (BitConverter.IsLittleEndian != AMB.IsLittleEndian(raw_file))
                    raw_file = AMB.SwapEndianness(raw_file);

                files_counter = BitConverter.ToInt32(raw_file, 0x10);
                int list_pointer = BitConverter.ToInt32(raw_file, 0x14);

                //Some AMB files have no file inside of them
                if (files_counter > 0)
                {
                    //This is actually used to identify extra zero bytes in messed AMBs
                    int DataPointer = BitConverter.ToInt32(raw_file, 0x18);

                    //This is the pointer to where the names of the files start
                    int name_pointer;

                    int has_dummy_bytes = 0;
                    if (DataPointer == 0)
                    {
                        //Well, this means that we have "empty" integers in the places where pointers should be.
                        //We need to skip them
                        //Only iOS version of Episode 1 is caught at having this "defect".
                        has_dummy_bytes = 4;
                        DataPointer = BitConverter.ToInt32(raw_file, 0x18 + has_dummy_bytes);
                    }

                    name_pointer = BitConverter.ToInt32(raw_file, 0x1C + has_dummy_bytes);

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

                    //Getting the raw files names (with 0x00)
                    int filenames_offset = raw_file.Length - name_pointer;
                    byte[] files_names_bytes = new byte[filenames_offset];
                    Array.Copy(raw_file, name_pointer, files_names_bytes, 0, filenames_offset);

                    //Encoding characters from HEX to ASCII characters
                    string files_names_str = Encoding.ASCII.GetString(files_names_bytes);
                    string[] files_names_raw = files_names_str.Split('\x00');

                    //Some AMB files have no names of their files
                    if (name_pointer != 0)
                    {
                        //Adding only names that aren't empty
                        for (int i = 0; i < files_names_raw.Length; i++)
                        {
                            if (files_names_raw[i] != "")
                            {
                                files_names.Add(files_names_raw[i]);
                            }
                        }
                    }
                    else
                    {
                        //If there're no names, setting file names to their index
                        for (int i = 0; i < files_counter; i++)
                            files_names.Add(i.ToString("D3"));
                    }

                    //removing ".\" in the names (Windows can't create "." folders)
                    //sometimes they can have several ".\" in the names
                    for (int i = 0; i < files_names.Count; i++)
                    {
                        //Turns out there's a double dot directory in file names
                        //And double backslash in file names
                        while (files_names[i][0] == '.' || files_names[i][0] == '\\' || files_names[i][0] == '/' )
                            files_names[i] = files_names[i].Substring(1);
                    }
                }

                for (int i = 0; i < files_counter; i++)
                    result.Add((Name: files_names[i], Pointer: files_pointers[i], Length: files_lens[i]));

                return result;
            }
            
            public static List<(string Name, int Pointer, int Length)> Read(string file_name)
            {
                return AMB.Read(File.ReadAllBytes(file_name));
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

                var InternalThings = AMB.GetInternalThings(raw_file, OrigFileName, ModFileName);
                
                string InternalName  = InternalThings.InternalName;
                int    InternalIndex = InternalThings.InternalIndex;
                string ParentName    = InternalThings.ParentName;
                int    ParentIndex   = InternalThings.ParentIndex;

                //If mod file is in the original file or it has a parent file.
                if (InternalIndex != -1 || ParentIndex != -1)
                {
                    byte[] raw_mod_file;


                    //In case if the file is in another file, we need to get patched(!) parent file and patch original file.
                    //Else we can simply get mod file and patch original one.
                    
                    //If the file file is in the original file (not in a parent file)
                    if (InternalIndex != -1)
                    {
                        raw_mod_file = File.ReadAllBytes(ModFilePath);
                    }
                    //Else we get parent file and patch it.
                    //This is the place where recursive patching begins
                    else
                    {
                        //These two lines "extract" parent file (TODO: add a feature to extract only one file)
                        byte[] parent_raw = new byte[files[ParentIndex].Length];
                        Array.Copy(raw_file, files[ParentIndex].Pointer, parent_raw, 0, files[ParentIndex].Length);

                        //This returns patched parent file
                        raw_mod_file = AMB.Patch(parent_raw,
                                                 ParentName,
                                                 InternalName,
                                                 ModFilePath);

                        //Now we have a parent file, so we need to replace it.
                        InternalName  = ParentName;
                        InternalIndex = ParentIndex;
                    }

                    Log.Write("AMB.Patch: replacing original file with " + ModFileName);
                    //Splitting the AMB file into three parts
                    //Before the file data
                    byte[] part_one = new byte[files[InternalIndex].Pointer];
                    Array.Copy(raw_file, 0, part_one, 0, files[InternalIndex].Pointer);

                    //After the file data
                    byte[] part_thr;

                    int name_pointer = BitConverter.ToInt32(part_one, 0x1C);

                    int len_dif;
                    //If this is the last file, start copying from names
                    if (InternalIndex + 1 == files.Count)
                    {
                        len_dif = name_pointer - files[InternalIndex].Pointer;
                        int tmp_len = raw_file.Length - name_pointer;
                        part_thr = new byte[tmp_len];
                        Array.Copy(raw_file, name_pointer, part_thr, 0, tmp_len);
                    }
                    else
                    {
                        len_dif = files[InternalIndex + 1].Pointer - files[InternalIndex].Pointer;
                        int tmp_len = raw_file.Length - files[InternalIndex + 1].Pointer;
                        part_thr = new byte[tmp_len];
                        Array.Copy(raw_file, files[InternalIndex + 1].Pointer, part_thr, 0, tmp_len);
                    }

                    //The mod file
                    int part_two_len;

                    //This simply adds 0x00 at the the of the file
                    part_two_len = raw_mod_file.Length + (16 - raw_mod_file.Length % 16) % 16;

                    byte[] part_two = new byte[part_two_len];
                    Array.Copy(raw_mod_file, 0, part_two, 0, raw_mod_file.Length);

                    //The difference between the length of the mod file and the original one.
                    //Or "how much bytes the program has added"
                    len_dif = part_two_len - len_dif;

                    //Changing Name Pointer
                    name_pointer += len_dif;
                    Array.Copy(BitConverter.GetBytes(name_pointer), 0, part_one, 0x1C, 4);

                    //Changing File Length
                    Array.Copy(BitConverter.GetBytes(raw_mod_file.Length), 0, part_one, 0x24 + InternalIndex * 0x10, 4);
                        
                    //Changing Files Pointers
                    for (int i = InternalIndex + 1; i < files.Count; i++)
                    {
                        //Reading the original pointer
                        int tmp_pointer = BitConverter.ToInt32(part_one, 0x20 + i * 0x10);

                        //Writing the new pointer
                        tmp_pointer += len_dif;

                        Array.Copy(BitConverter.GetBytes(tmp_pointer), 0, part_one, 0x20 + i * 0x10, 4);
                    }

                    //Combining the parts into one file                        
                    raw_file = part_one.Join(part_two.Join(part_thr));
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

                //This will make addition work with different endianness
                bool swap_endianness_back = BitConverter.IsLittleEndian != AMB.IsLittleEndian(raw_file);
                if (swap_endianness_back)
                    raw_file = AMB.SwapEndianness(raw_file);

                //Some empty files from S4E1 are "broken", it's better to use a new empty file.
                if (AMB.Read(raw_file).Count == 0)
                   raw_file = AMB.Create();

                var InternalThings = AMB.GetInternalThings(raw_file, OrigFileName, ModFileName);

                string InternalName  = InternalThings.InternalName;
                int    InternalIndex = InternalThings.InternalIndex;
                string ParentName    = InternalThings.ParentName;
                int    ParentIndex   = InternalThings.ParentIndex;
                
                if (InternalIndex != -1 || ParentIndex != -1)
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
                    int file_pointer = BitConverter.ToInt32(enumeration_part, enum_pointer + 0x10 * i);

                    file_pointer += 0x10;
                    Array.Copy(BitConverter.GetBytes(file_pointer), 0, enumeration_part, enum_pointer + 0x10 * i, 4);
                }

                //Injecting empty file pointer and length
                byte[] empty_file_enumeration = new byte[0x10];
                name_pointer -= 0x10;
                Array.Copy(BitConverter.GetBytes(name_pointer), 0, empty_file_enumeration, 0x00, 4);

                name_pointer += 0x10;

                //Length
                empty_file_enumeration[0x4] = 0x10;

                byte[] mod_file_name_bytes = new byte[0x20];

                for (int i = 0; i < InternalName.Length; i++)
                {
                    if (i >= mod_file_name_bytes.Length) { break; }
                    mod_file_name_bytes[i] = (byte)InternalName[i];
                }
                
                enumeration_part = enumeration_part.Join(empty_file_enumeration);
                data_part        = data_part.Join(new byte[0x10]);
                name_part        = name_part.Join(mod_file_name_bytes);

                
                raw_file = enumeration_part.Join(
                                  data_part.Join(
                                  name_part));
                
                raw_file = AMB.Patch(raw_file, OrigFileName, ModFileName, ModFilePath);

                if (swap_endianness_back)
                    raw_file = AMB.SwapEndianness(raw_file);

                return raw_file;
            }

            public static void Add(string file_name, string mod_file, string ModFileName)
            {
                byte[] raw_file = AMB.Add(File.ReadAllBytes(file_name), file_name, mod_file, ModFileName);
                File.WriteAllBytes(file_name, raw_file);
            }

            public static void Add(string file_name, string mod_file)
            {
                byte[] raw_file = AMB.Add(File.ReadAllBytes(file_name), file_name, mod_file, mod_file);
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

            public static bool IsLittleEndian(string FileName)
            {
                return AMB.IsLittleEndian(File.ReadAllBytes(FileName));
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

            public static void SwapEndianness(string FileName)
            {
                File.WriteAllBytes(FileName, SwapEndianness(File.ReadAllBytes(FileName)));
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
                    raw_file = before_enum.Join(before_data.Join(before_name.Join(after_name)));
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
                raw_file[0x00] = (byte) '#';
                raw_file[0x01] = (byte) 'A';
                raw_file[0x02] = (byte) 'M';
                raw_file[0x03] = (byte) 'B';

                //Endianness identifier (at least for AMBPatcher)
                Array.Copy(BitConverter.GetBytes(0x1304), 0, raw_file, 0x04, 4);
                //List pointer
                Array.Copy(BitConverter.GetBytes(0x20), 0, raw_file, 0x14, 4);
                //Data pointer
                Array.Copy(BitConverter.GetBytes(0x20), 0, raw_file, 0x18, 4);
                //Name pointer
                Array.Copy(BitConverter.GetBytes(0x20), 0, raw_file, 0x1C, 4);

                return raw_file;
            }

            public static void Create(string file_name)
            {
                File.WriteAllBytes(file_name, AMB.Create());
            }

            ////////////////////////////////////
            //Check if file is AMB (by header)//
            ////////////////////////////////////

            public static bool IsAMB(byte[] raw_file)
            {
                if (raw_file.Length >= 4)
                {if (raw_file[0] == '#' &&
                     raw_file[1] == 'A' &&
                     raw_file[2] == 'M' &&
                     raw_file[3] == 'B')
                    {return true;}}

                return false;
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
                            ConsoleProgressBar(i, mod_files.Count, mod_file_full, 64);

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
                    else { Log.Write("Not changed"); }
                }
                else if (file_name.ToUpper().EndsWith(".CSB"))
                {
                    if (ShaChanged(file_name.Substring(0, file_name.Length - 4), mod_files, mod_paths))
                    {
                        Recover(file_name);
                        if (file_name.EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                        {
                            Recover(file_name.Substring(0, file_name.Length - 4) + ".CPK");
                        }

                        Log.Write("Asking CsbEditor to unpack");
                        ConsoleProgressBar(0, 100, "Asking CsbEditor to unpack " + file_name, 64);

                        //Needs CSB Editor (from SonicAudioTools) to work
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = "CsbEditor.exe";
                        startInfo.Arguments = file_name;
                        Process.Start(startInfo).WaitForExit();

                        for (int i = 0; i < mod_files.Count; i++)
                        {
                            string mod_file = Path.Combine("mods", mod_paths[i], mod_files[i]);

                            ConsoleProgressBar(i, mod_files.Count, mod_file, 64);
                            Log.Write(mod_file);
                            File.Copy(mod_file, mod_files[i], true);

                            ShaWrite(mod_files[i], mod_file);
                        }

                        Log.Write("Asking CsbEditor to repack");
                        ConsoleProgressBar(99, 100, "Asking CsbEditor to repack " + file_name, 64);
                        startInfo.Arguments = file_name.Substring(0, file_name.Length - 4);
                        Process.Start(startInfo).WaitForExit();
                    }
                    else { Log.Write("Not changed"); }
                }
            }
            else { Log.Write("File not found"); }

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
                    string[] filenames = Directory.GetFiles(Path.Combine("mods",ini_mods[i]), "*", SearchOption.AllDirectories);

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
            
            //Uncomment this line to get the help message as text file.
            //File.WriteAllText("HelpMessage.txt", text);
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

                if (ProgressBar)
                {
                    Console.WriteLine("Doing absolutely nothing!");
                    Console.WriteLine("Progress bar goes here");
                    Console.WriteLine("Sub-task!");
                    Console.WriteLine("sub%");
                    Console.CursorTop -= 2;
                }
                for (int i = 0; i < test.Count; i++)
                {
                    modified_files.Add(test[i].OrigFile);
                    
                    if (ProgressBar)
                    {
                        ConsoleProgressBar(i, test.Count, "Modifying \"" + test[i].OrigFile + "\"...", 64);
                        Console.CursorTop += 2;
                    }

                    Backup(test[i].OrigFile);
                    //Some CSB files may have CPK archive
                    if (File.Exists(test[i].OrigFile.Substring(0, test[i].OrigFile.Length - 4) + ".CPK"))
                        Backup(test[i].OrigFile.Substring(0, test[i].OrigFile.Length - 4) + ".CPK");

                    PatchAll(test[i].OrigFile, test[i].ModFiles, test[i].ModName);
                    mods_prev.Remove(test[i].OrigFile);

                    if (ProgressBar) { Console.CursorTop -= 2; }
                }

                if (ProgressBar)
                {
                    ConsoleProgressBar(1, 1, "", 64);
                    Console.CursorTop += 2;
                    ConsoleProgressBar(1, 1, "", 64);
                }

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
                        Console.WriteLine("\n");
                        string[] mods_prev = File.ReadAllLines("mods/mods_prev");

                        for (int i = 0; i < mods_prev.Length; i++)
                        {
                            string file = mods_prev[i];
                            
                            ConsoleProgressBar(i, mods_prev.Length, "Recovering \""+ file +"\" file...", 64);

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
                        ConsoleProgressBar(1, 1, "", 64);
                    }
                }
                else
                {
                    if (File.Exists(args[0]))
                    {
                        Directory.CreateDirectory(args[0] + "_extracted");
                        AMB.Extract(args[0], args[0] + "_extracted");
                    }
                    else { ShowHelpMessage(); }
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
                    else { ShowHelpMessage(); }
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
                    else { ShowHelpMessage(); }
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
                    AMB.SwapEndianness(args[1]);
                }
                else if (args[0] == "endianness" && File.Exists(args[1]))
                {
                    Console.WriteLine("This file's endianness is...");
                    if (AMB.IsLittleEndian(args[1]))
                    { Console.WriteLine("Little endian!"); }
                    else { Console.WriteLine("Big endian!"); }
                }
                else if (args[0] == "create")
                {
                    if (Path.GetDirectoryName(args[1]) != "")
                    { Directory.CreateDirectory(Path.GetDirectoryName(args[1])); }
                    AMB.Create(args[1]);
                }
                else if (args[0] == "extract_all")
                {
                    AMB.ExtractAll(args[1]);
                }
                else { ShowHelpMessage(); }
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
                    else { ShowHelpMessage(); }
                }
                else if (args[0] == "patch")
                {
                    if (File.Exists(args[1]) && File.Exists(args[2]))
                    {
                        AMB.Patch(args[1], args[2]);
                    }
                    else if (File.Exists(args[1]) && Directory.Exists(args[2]))
                    {
                        var files = Directory.GetFiles(args[2], "*", SearchOption.AllDirectories);

                        foreach (string file in files)
                        {
                            Console.WriteLine("Patching by \""+file+"\"...");
                            AMB.Patch(args[1], file);
                        }
                        Console.WriteLine("Done.");
                    }
                    else { ShowHelpMessage(); }
                }
                else if (args[0] == "add")
                {
                    if (File.Exists(args[1]) && File.Exists(args[2]))
                    {
                        AMB.Add(args[1], args[2]);
                    }
                    else { ShowHelpMessage(); }
                }
                else if (args[0] == "delete" || args[0] == "remove")
                {
                    if (File.Exists(args[1]))
                    {
                        AMB.Delete(args[1], args[2]);
                    } else { ShowHelpMessage(); }
                }
                else { ShowHelpMessage(); }
            }
            else if (args.Length == 4)
            {
                if (args[0] == "add")
                {
                    if (File.Exists(args[1]) && File.Exists(args[2]))
                    {
                        AMB.Add(args[1], args[2], args[3]);
                    }
                    else { ShowHelpMessage(); }
                }
                else { ShowHelpMessage(); }
            }
            else { ShowHelpMessage(); }
        }
    }

    public static class Extensions
    {
        //This thing joins two byte arrays just like Concat().ToArray() but faster (i think)
        public static byte[] Join(this byte[] first, byte[] second)
        {
            byte[] output = new byte[first.Length + second.Length];

            Array.Copy(first,  0, output, 0, first.Length);
            Array.Copy(second, 0, output, first.Length, second.Length);

            return output;
        }
    }

}
