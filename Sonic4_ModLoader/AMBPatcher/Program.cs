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
        /* Two variable to store log
         * The log won't help you if your game is crashing, but it can tell you if you've named a file 
         * in the wrong way (e.g. haven't removed "_extracted" in the directory name).
         */
        public static bool GenerateLog { set; get; }
        public static bool ProgressBar { set; get; }
        public static bool SHACheck { get; set; }
        public static int SHAType { get; set; }
        
        public class Log
        {
            public static void Write(string Message)
            {
                if (!GenerateLog) { return; }
                
                File.AppendAllText("AMBPatcher.log", Message + Environment.NewLine);
            }
            
            public static void Reset()
            {
                if (!GenerateLog) { return; }
                File.WriteAllText("AMBPatcher.log", "");
            }
        }

        static void ConsoleProgressBar(int i, int max_i, string title, int bar_len)
        {
            //To prevent crashes out of nowhere when progress bar is turned off
            if (!ProgressBar) { return; }

            Console.CursorTop -= 2;
            
            //What it is doing
            Console.Write(String.Concat(Enumerable.Repeat(" ", Console.WindowWidth-1)));
            Console.CursorLeft = 0;
            if (i == max_i) {Console.WriteLine("Done!");}
            else            {Console.WriteLine(title);}
            
            //Percentage
            Console.Write(String.Concat(Enumerable.Repeat(" ", Console.WindowWidth-1)));
            Console.CursorLeft = 0;
            Console.WriteLine("[" + String.Concat(Enumerable.Repeat("#", bar_len * i / max_i)) +
                                String.Concat(Enumerable.Repeat(" ", bar_len - bar_len * i / max_i)) + "] (" + (i * 100 / max_i).ToString() + "%)");
        }

        static void Load_Settings()
        {
            ProgressBar = true;
            GenerateLog = false;
            SHACheck = true;
            SHAType = 1;

            if (File.Exists("AMBPatcher.cfg"))
            {
                string[] cfg_file = File.ReadAllLines("AMBPatcher.cfg");
                for (int j = 0; j < cfg_file.Length; j++)
                {
                    if (cfg_file[j].StartsWith("ProgressBar="))
                    {
                        ProgressBar = Convert.ToBoolean(Convert.ToInt32(String.Join("=", cfg_file[j].Split('=').Skip(1))));
                    }
                    else if (cfg_file[j].StartsWith("GenerateLog="))
                    {
                        GenerateLog = Convert.ToBoolean(Convert.ToInt32(String.Join("=", cfg_file[j].Split('=').Skip(1))));
                    }
                    else if (cfg_file[j].StartsWith("SHACheck="))
                    {
                        SHACheck = Convert.ToBoolean(Convert.ToInt32(String.Join("=", cfg_file[j].Split('=').Skip(1))));
                    }
                    else if (cfg_file[j].StartsWith("SHAType="))
                    {
                        SHAType = Convert.ToInt32(String.Join("=", cfg_file[j].Split('=').Skip(1)));
                    }
                }
            }
        }

        static string Sha(byte[] file)
        {
            byte[] hash;
            string str_hash = "";

            if (SHAType == 512)
            { hash = new SHA512CryptoServiceProvider().ComputeHash(file); }
            else if (SHAType == 384)
            { hash = new SHA384CryptoServiceProvider().ComputeHash(file); }
            else if (SHAType == 256)
            { hash = new SHA256CryptoServiceProvider().ComputeHash(file); }
            else
            { hash = new SHA1CryptoServiceProvider().ComputeHash(file); }
            
            foreach (byte b in hash) { str_hash += b.ToString("X"); }

            return str_hash;
        }
        
        static void ShaRemove(string file_name)
        {
            string orig_file_sha_root = Path.Combine("mods_sha", file_name);

            if (Directory.Exists(orig_file_sha_root))
            {
                var sha1_files = Directory.GetFiles(orig_file_sha_root, "*", SearchOption.AllDirectories);

                foreach (string file in sha1_files)
                {
                    File.Delete(file);
                }
            }
        }

        static bool ShaChanged(string file_name, List<string> mod_files, List<string> mod_paths)
        {
            if (!SHACheck) { return true; }

            bool files_changed = false;
            
            List<string> sha_list = new List<string> { };

            string orig_file_sha_root = Path.Combine("mods_sha", file_name);

            if (Directory.Exists(orig_file_sha_root))
            {
                sha_list = new List<string>(Directory.GetFiles(orig_file_sha_root, "*.txt", SearchOption.AllDirectories));
            }

            //Checking SHA1s
            for (int i = 0; i < mod_files.Count; i++)
            {
                if (files_changed) { break; }

                string mod_file_full = Path.Combine("mods", mod_paths[i], mod_files[i] );
                string mod_file_sha = Path.Combine("mods_sha", mod_files[i] + ".txt");

                if (sha_list.Contains(mod_file_sha))
                {
                    sha_list.Remove(mod_file_sha);
                }
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
                else { files_changed = true; }
            }

            //Checking if there're removed files
            //And removing those SHA1s
            if (sha_list.Count > 0)
            {
                files_changed = true;

                foreach (string file in sha_list)
                {
                    File.Delete(file);
                }
            }

            return files_changed;
        }

        static void ShaWrite(string relative_mod_file_path, string full_mod_file_path)
        {
            string sha_file = Path.Combine("mods_sha", relative_mod_file_path + ".txt");
            string sha_dir = Path.GetDirectoryName(sha_file);

            if (!Directory.Exists(sha_dir)) { Directory.CreateDirectory(sha_dir); }
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

            internal static Tuple<string, int, string, int> GetInternalThings(byte[] raw_file, string OriginalFileName, string ModFileName)
            {
                /*
                 * Item1 is InternalName (like from AMB.Read)
                 * Item2 is InternalIndex (index of file name in AMB.Read, if present)
                 * Item4 is ParentName (AMB.Read()[ParentIndex].Item1, if present)
                 * Item3 is ParentIndex (index of parent file in AMB.Read, if present)
                 */

                var files = AMB.Read(raw_file);

                /////////////////
                //Internal Name//
                /////////////////

                string InternalName;
                int InternalIndex = -1;
                
                //Turning "C:\1\2\3" into {"C:","1","2","3"}
                string[] mod_file_parts = ModFileName.Split(Path.DirectorySeparatorChar);
                
                string orig_file_last = Path.GetFileName(OriginalFileName);

                //Trying to find where the original file name starts in the mod file name.
                int index = Array.IndexOf(mod_file_parts, orig_file_last);
                
                //If it's inside, return the part after original file ends
                if (index != -1)
                {
                    InternalName = String.Join("\\", mod_file_parts.Skip(index + 1).ToArray());
                }
                //Else use file name
                else
                {
                    InternalName = mod_file_parts.Last();
                }

                //Find internal index
                for (int i = 0; i < files.Count; i++)
                {
                    if (InternalName == files[i].Item1)
                    {
                        InternalIndex = i;
                    }
                }
                
                int ParentIndex = -1;
                string ParentName = "";

                for (int i = 0; i < files.Count; i++)
                {
                    if ((InternalName + "\\").StartsWith(files[i].Item1 + "\\"))
                    {
                        ParentIndex = i;
                        ParentName = files[i].Item1;
                    }
                }
                
                return Tuple.Create(InternalName, InternalIndex, ParentName, ParentIndex);
            }

            ////////
            //Read//
            ////////
            
            public static List<Tuple<string, int, int>> Read(byte[] raw_file)
            {
                /* returns a list of:
                 * list[0].Item1 = Name of a file
                 * list[0].Item2 = Pointer to the file
                 * list[0].Item3 = Length of the file
                 */

                List<int> files_pointers = new List<int>();
                List<int> files_lens = new List<int>();
                List<string> files_names = new List<string>();
                int files_counter = 0;

                //Identifing that the file is an AMB file
                if (raw_file[0] == 0x23 &&  //#
                    raw_file[1] == 0x41 &&  //A
                    raw_file[2] == 0x4D &&  //M
                    raw_file[3] == 0x42)    //B
                {
                    files_counter = BitConverter.ToInt32(raw_file, 0x10);
                    int list_pointer = BitConverter.ToInt32(raw_file, 0x14);

                    //Some AMB files have no file inside of it
                    if (files_counter > 0)
                    {
                        //This is actually used to identify extra zero bytes in messed AMBs
                        int DataPointer = BitConverter.ToInt32(raw_file, 0x18);

                        //This is the pointer to where the names of the files start
                        int name_pointer;

                        if (DataPointer == 0)
                        {
                            //Well, this means that we have "empty" integers in the places where pointers should be.
                            //We need to skip them

                            name_pointer = BitConverter.ToInt32(raw_file, 0x20);

                            for (int i = 0; i < files_counter; i++)
                            {
                                int point = list_pointer + i * 0x14;
                                
                                if (BitConverter.ToInt32(raw_file, point) != 0)
                                {
                                    files_pointers.Add(BitConverter.ToInt32(raw_file, point));
                                    files_lens.Add(BitConverter.ToInt32(raw_file, point + 8));
                                }
                            }
                        }
                        else
                        {
                            name_pointer = BitConverter.ToInt32(raw_file, 0x1C);

                            for (int i = 0; i < files_counter; i++)
                            {
                                int point = list_pointer + i * 0x10;

                                //Adding pointers and lengths of files into corresponding lists
                                if (BitConverter.ToInt32(raw_file, point) != 0)
                                {
                                    files_pointers.Add(BitConverter.ToInt32(raw_file, point));
                                    files_lens.Add(BitConverter.ToInt32(raw_file, point + 4));
                                }
                            }
                        }

                        //Actual number of files inside may differ from the number given in the header
                        files_counter = files_pointers.Count;

                        //Getting the raw files names (with 0x00)
                        int filenames_offset = raw_file.Length - name_pointer;
                        byte[] files_names_bytes = new byte[filenames_offset];
                        Array.Copy(raw_file, name_pointer, files_names_bytes, 0, filenames_offset);

                        //Encoding charactes from HEX to ASCII characters
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
                                    for (int j = 0; j < files_names_raw[i].Length / 32 + 1; j++)
                                    {
                                        files_names.Add(files_names_raw[i].Substring(32 * j, Math.Min(32, files_names_raw[i].Length - 32 * j)));
                                    }
                                }
                            }
                        }
                        else
                        {
                            //If there're no names, setting file names as their order number (i)
                            for (int i = 0; i < files_counter; i++)
                            {
                                files_names.Add(i.ToString());
                            }
                        }

                        //removing ".\" in the names (Windows can't create "." folders)
                        //sometimes they can have several ".\" in the names
                        bool starts_with_dot;
                        do
                        {
                            starts_with_dot = false;
                            for (int i = 0; i < files_names.Count; i++)
                            {
                                if (files_names[i].StartsWith(".\\"))
                                {
                                    files_names[i] = files_names[i].Substring(2);
                                    if (files_names[i].StartsWith(".\\"))
                                    {
                                        starts_with_dot = true;
                                    }
                                }
                            }
                        }
                        while (starts_with_dot);
                    }
                }

                var result = new List<Tuple<string, int, int>>();
                for (int i = 0; i < files_counter; i++)
                {
                    result.Add(Tuple.Create(files_names[i], files_pointers[i], files_lens[i]));
                }
                return result;
            }
            
            public static List<Tuple<string, int, int>> Read(string file_name)
            {
                return AMB.Read(File.ReadAllBytes(file_name));
            }

            ///////////
            //Extract//
            ///////////

            public static void Extract(string file_name, string output)
            {
                byte[] raw_file = File.ReadAllBytes(file_name);

                //Creating folder if it doesn't exist
                if (!Directory.Exists(output))
                {
                    Directory.CreateDirectory(output);
                }
                
                if (BitConverter.IsLittleEndian != AMB.IsLittleEndian(raw_file))
                {
                    raw_file = AMB.SwapEndianness(raw_file);
                }

                var files = AMB.Read(raw_file);

                for (int i = 0; i < files.Count; i++)
                {
                    string output_file = Path.Combine(output, files[i].Item1);

                    //Copying raw file from the archive into a byte array.
                    byte[] file_bytes = new byte[files[i].Item3];
                    Array.Copy(raw_file, files[i].Item2, file_bytes, 0, files[i].Item3);

                    if (!Directory.Exists(Path.GetDirectoryName(output_file)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(output_file));
                    }
                    //And writing that byte array into a file
                    File.WriteAllBytes(output_file, file_bytes);
                }
            }

            /////////
            //Patch//
            /////////
            
            public static byte[] Patch(byte[] raw_file, string OrigFileName, string ModFileName, string ModFilePath)
            {
                //Why do I need the original file name? To patch files that are inside of an AMB file that is inside of an AMB file that is inside of ...
                var files = AMB.Read(raw_file);

                var InternalThings = AMB.GetInternalThings(raw_file, OrigFileName, ModFileName);
                
                string InternalName  = InternalThings.Item1;
                int    InternalIndex = InternalThings.Item2;
                string ParentName    = InternalThings.Item3;
                int    ParentIndex   = InternalThings.Item4;

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
                        byte[] parent_raw = new byte[files[ParentIndex].Item3];
                        Array.Copy(raw_file, files[ParentIndex].Item2, parent_raw, 0, files[ParentIndex].Item3);

                        //This returns pathed parent file
                        raw_mod_file = AMB.Patch(parent_raw,
                                                 ParentName,
                                                 InternalName,
                                                 ModFilePath);

                        //Now we have a parent file, so we need to replace it.
                        InternalName  = ParentName;
                        InternalIndex = ParentIndex;
                    }

                    //If the mod file's length is smaller or equal to the original one
                    //The original file will be replaced
                    if (raw_mod_file.Length <= files[InternalIndex].Item3)
                    {
                        Log.Write("AMB.Patch: " + ModFileName + " 's length is OK, replacing...");
                        for (int i = 0; i < files[InternalIndex].Item3; i++)
                        {
                            if (raw_mod_file.Length - 1 >= i)
                            {
                                raw_file[files[InternalIndex].Item2 + i] = raw_mod_file[i];
                            }
                            else
                            {
                                raw_file[files[InternalIndex].Item2 + i] = 0;
                            }
                        }
                    }
                    //Else the AMB file will be expanded
                    else
                    {
                        Log.Write("AMB.Patch: " + ModFileName + " 's length is bigger than the original length, expanding...");
                        //Splitting the AMB file into three parts
                        //Before the file data
                        byte[] part_one = new byte[files[InternalIndex].Item2];
                        Array.Copy(raw_file, 0, part_one, 0, files[InternalIndex].Item2);

                        //After the file data
                        byte[] part_thr;

                        int name_pointer = BitConverter.ToInt32(part_one, 0x1C);

                        int len_dif;
                        //If this is the last file, start copying from names
                        if (InternalIndex + 1 == files.Count)
                        {
                            len_dif = name_pointer - files[InternalIndex].Item2;
                            int tmp_len = raw_file.Length - name_pointer;
                            part_thr = new byte[tmp_len];
                            Array.Copy(raw_file, name_pointer, part_thr, 0, tmp_len);
                        }
                        else
                        {
                            len_dif = files[InternalIndex + 1].Item2 - files[InternalIndex].Item2;
                            int tmp_len = raw_file.Length - files[InternalIndex + 1].Item2;
                            part_thr = new byte[tmp_len];
                            Array.Copy(raw_file, files[InternalIndex + 1].Item2, part_thr, 0, tmp_len);
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
                        raw_file = new byte[part_one.Length + part_two.Length + part_thr.Length];
                        
                        Buffer.BlockCopy(part_one, 0, raw_file, 0, part_one.Length);
                        Buffer.BlockCopy(part_two, 0, raw_file, part_one.Length, part_two.Length);
                        Buffer.BlockCopy(part_thr, 0, raw_file, part_one.Length + part_two.Length, part_thr.Length);
                    }
                }
                else
                {
                    Log.Write("AMB.Patch: " + ModFileName + " is not in " + OrigFileName + ", trying to add!");
                    raw_file = AMB.Add(raw_file, OrigFileName, ModFileName, ModFilePath);
                }

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
                //Okay, I've got a great plan:
                //Add empty file and patch it.

                var InternalThings = AMB.GetInternalThings(raw_file, OrigFileName, ModFileName);

                string InternalName  = InternalThings.Item1;
                int    InternalIndex = InternalThings.Item2;
                string ParentName    = InternalThings.Item3;
                int    ParentIndex   = InternalThings.Item4;
                
                if (InternalIndex != -1 || ParentIndex != -1)
                {
                    return AMB.Patch(raw_file, OrigFileName, ModFileName, ModFilePath);
                }
                
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
                empty_file_enumeration[0x5] =
                empty_file_enumeration[0x6] =
                empty_file_enumeration[0x7] = 0x00;

                //Blank space
                empty_file_enumeration[0x8] =
                empty_file_enumeration[0x9] =
                empty_file_enumeration[0xA] =
                empty_file_enumeration[0xB] =
                empty_file_enumeration[0xC] =
                empty_file_enumeration[0xD] =
                empty_file_enumeration[0xE] =
                empty_file_enumeration[0xF] = 0x00;

                byte[] mod_file_name_bytes = new byte[0x20];

                for (int i = 0; i < InternalName.Length; i++)
                {
                    if (i >= mod_file_name_bytes.Length) { break; }
                    mod_file_name_bytes[i] = (byte)InternalName[i];
                }
                
                enumeration_part = enumeration_part.Concat(empty_file_enumeration).ToArray();
                data_part        = data_part.Concat(new byte[0x10]).ToArray();
                name_part        = name_part.Concat(mod_file_name_bytes).ToArray();

                raw_file = enumeration_part.Concat(
                                  data_part.Concat(
                                  name_part)).ToArray();
                
                return AMB.Patch(raw_file, OrigFileName, ModFileName, ModFilePath);
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
                {
                    FileIsLittleEndian = !FileIsLittleEndian;
                }

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
                {
                    Array.Reverse(raw_file, pointer, 4);
                }

                return raw_file;
            }

            public static void SwapEndianness(string FileName)
            {
                File.WriteAllBytes(FileName, SwapEndianness(File.ReadAllBytes(FileName)));
            }
        }

        static void PatchAll(string file_name, List<string> mod_files, List<string> mod_paths)
        {
            if (File.Exists(file_name))
            {
                if (file_name.ToUpper().EndsWith(".AMB"))
                {
                    if (ShaChanged(file_name, mod_files, mod_paths))
                    {
                        Recover(file_name);
                        if (file_name.EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                        {
                            Recover(file_name.Substring(0, file_name.Length - 4) + ".CPK");
                        }
                        Log.Write("PatchAll: file " + file_name + " was restored.");

                        for (int i = 0; i < mod_files.Count; i++)
                        {
                            string mod_file_full = Path.Combine("mods", mod_paths[i], mod_files[i]);

                            ConsoleProgressBar(i, mod_files.Count, mod_file_full, 64);

                            if (file_name == mod_files[i])
                            {
                                Log.Write("PatchAll: replacing " + file_name + " with " + mod_file_full);
                                File.Copy(mod_file_full, file_name, true);
                            }
                            else
                            {
                                AMB.Patch(file_name, mod_file_full);
                            }

                            ShaWrite(mod_files[i], mod_file_full);
                        }
                    }
                    else { Log.Write("PatchAll: file " + file_name + " wasn't changed."); }
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

                        Log.Write("PatchAll: asking CsbEditor to unpack " + file_name);
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
                            Log.Write("PatchAll: copying " + mod_file + " to " + mod_files[i]);

                            File.Copy(mod_file, mod_files[i], true);

                            ShaWrite(mod_files[i], mod_file);
                        }

                        Log.Write("PatchAll: asking CsbEditor to repack " + file_name);
                        ConsoleProgressBar(99, 100, "Asking CsbEditor to repack " + file_name, 64);
                        startInfo.Arguments = file_name.Substring(0, file_name.Length - 4);
                        Process.Start(startInfo).WaitForExit();
                    }
                }
            }
            else { Log.Write("PatchAll: " + file_name + " file not found"); }
        }

        static void Backup(string file_name)
        {
            if (!File.Exists(file_name + ".bkp"))
            {
                File.Copy(file_name, file_name + ".bkp");
            }
        }

        static void Recover(string file_name)
        {
            if (File.Exists(file_name + ".bkp"))
            {
                File.Copy(file_name + ".bkp", file_name, true);
            }
        }

        static List<Tuple<string, List<string>, List<string>>> GetModFiles()
        {
            /* returns a list of:
             * list[0].Item1 = The original file path
             * list[0].Item2 = List of files that will be modified
             * list[0].Item3 = List of the mod directories of the mod files of Item2
             */

            var result = new List<Tuple<string, List<string>, List<string>>>();

            //Reading the mods.ini file
            if (File.Exists("mods/mods.ini"))
            {
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
                    if (Directory.Exists("mods\\" + ini_mods[i]))
                    {
                        string[] filenames = Directory.GetFiles("mods\\" + ini_mods[i], "*", SearchOption.AllDirectories);

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
                                    original_file = possible_orig_file;
                                    break;
                                }
                                else if (File.Exists(possible_orig_file + ".CSB"))
                                {
                                    original_file = possible_orig_file + ".CSB";
                                    break;
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
                {
                    result.Add(Tuple.Create(orig_files[i], mod_files[i], mod_dirs[i]));
                }
            }
            else { Log.Write("GetModFiles: \"mods/mods.ini\" file not found"); }

            return result;
        }

        static void ShowHelpMessage()
        {
            Console.WriteLine("AMB Patcher by OSA413");
            Console.WriteLine("Usage:");
            Console.WriteLine("\tAMBPatcher.exe - Patch all files used by enabled mods.");
            Console.WriteLine("\tAMBPatcher.exe [AMB] and");
            Console.WriteLine("\tAMBPatcher.exe extract [AMB] - Extract all files from [AMB] to \"[AMB]_extracted\" directory.");
            Console.WriteLine("\tAMBPatcher.exe read [AMB file] - Prints content of [AMB]");
            Console.WriteLine("\tAMBPatcher.exe extract [AMB] [dir] - Extract all files from [AMB] to [dir] directory.");
            Console.WriteLine("\tAMBPatcher.exe patch [AMB] [file] - Patch [AMB] by [file] if [file] is in [AMB].");
            Console.WriteLine("\tAMBPatcher.exe [AMB] [dir] and");
            Console.WriteLine("\tAMBPatcher.exe patch [AMB] [dir] - Patch [AMB] by all files in [dir] if those files are in [AMB].");
            Console.WriteLine("\tAMBPatcher.exe recover - Recover original files that were changed.");
            Console.WriteLine("\tAMBPatcher.exe add [AMB] [file] - Add [file] to [AMB].");
            Console.WriteLine("\tAMBPatcher.exe add [AMB] [file] [name] - Add [file] to [AMB] with internal name of [name].");
            Console.WriteLine("\tAMBPatcher.exe endianness [AMB] - Print endianness of [AMB].");
            Console.WriteLine("\tAMBPatcher.exe swap_endianness [AMB] - Swaps endianness of pointers and lengths of [AMB].");
            Console.WriteLine("\tAMBPatcher.exe -h and");
            Console.WriteLine("\tAMBPatcher.exe --help - Show this message.");
        }

        static void Main(string[] args)
        {
            Load_Settings();
            Log.Reset();

            if (args.Length == 0)
            {
                Log.Write("Getting list of enabled mods...");
                var test = GetModFiles();

                if (GenerateLog)
                {
                    Log.Write("====================");
                    Log.Write("File list:");
                    for (int i = 0; i < test.Count; i++)
                    {
                        Log.Write("\n" + test[i].Item1);
                        for (int j = 0; j < test[i].Item2.Count; j++)
                        {
                            Log.Write("\t" + test[i].Item2[j] + "\t" + test[i].Item3[j]);
                        }
                    }
                    Log.Write("====================");
                }

                List<string> mods_prev = new List<string> { };
                List<string> modified_files = new List<string> { };

                if (File.Exists(@"mods\mods_prev"))
                {
                    mods_prev = File.ReadAllLines(@"mods\mods_prev").ToList<string>();
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
                    modified_files.Add(test[i].Item1);
                    
                    if (ProgressBar)
                    {
                        ConsoleProgressBar(i, test.Count, "Modifying \"" + test[i].Item1 + "\"...", 64);
                        Console.CursorTop += 2;
                    }

                    Backup(test[i].Item1);
                    //Some CSB files may have CPK archive
                    if (File.Exists(test[i].Item1.Substring(0, test[i].Item1.Length - 4) + ".CPK"))
                    {
                        Backup(test[i].Item1.Substring(0, test[i].Item1.Length - 4) + ".CPK");
                    }

                    PatchAll(test[i].Item1, test[i].Item2, test[i].Item3);
                    mods_prev.Remove(test[i].Item1);

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
                    {
                        Recover(mods_prev[i].Substring(0, mods_prev[i].Length - 4) + ".CPK");
                    }
                    
                    if (mods_prev[i].EndsWith(".CSB", StringComparison.OrdinalIgnoreCase))
                    { ShaRemove(mods_prev[i].Substring(0, mods_prev[i].Length - 4)); }
                    else
                    { ShaRemove(mods_prev[i]); }
                    
                    
                }
                Log.Write("Restored");

                Log.Write("\nSaving list of modified files...");
                if (Directory.Exists("mods"))
                {
                    File.WriteAllText(@"mods\mods_prev", string.Join("\n", modified_files.ToArray()));
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
                    if (File.Exists(@"mods\mods_prev"))
                    {
                        Console.WriteLine("\n");
                        string[] mods_prev = File.ReadAllLines(@"mods\mods_prev");

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
                        File.Delete(@"mods\mods_prev");
                        ConsoleProgressBar(1, 1, "", 64);
                    }
                }
                else
                {
                    if (File.Exists(args[0]))
                    {
                        if (!Directory.Exists(args[0] + "_extracted"))
                        {
                            Directory.CreateDirectory(args[0] + "_extracted");
                        }
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
                        if (!Directory.Exists(args[1] + "_extracted"))
                        {
                            Directory.CreateDirectory(args[1] + "_extracted");
                        }
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
                            Console.WriteLine("\nFile name:    " + data[i].Item1);
                            Console.WriteLine("File pointer: " + data[i].Item2 + "\t(0x" + data[i].Item2.ToString("X") + ")");
                            Console.WriteLine("File length:  " + data[i].Item3 + "\t(0x" + data[i].Item3.ToString("X") + ")");
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
                else { ShowHelpMessage(); }
            }

            else if (args.Length == 3)
            {
                if (args[0] == "extract")
                {
                    if (File.Exists(args[1]))
                    {
                        if (!Directory.Exists(args[2]))
                        {
                            Directory.CreateDirectory(args[2]);
                        }
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
}
