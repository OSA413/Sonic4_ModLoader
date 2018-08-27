﻿using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace AMBPatcher
{
    class Program
    {
        static List<Tuple<string, int, int>> AMB_Read(string filename)
        {
            List<int> files_pointers = new List<int>();
            List<int> files_lens = new List<int>();
            List<string> files_names = new List<string>();
            int files_counter = 0;

            byte[] raw_file = File.ReadAllBytes(filename);
            //Identifing that the file is an AMB file
            if (raw_file[0] == 0x23 &&  //#
                raw_file[1] == 0x41 &&  //A
                raw_file[2] == 0x4D &&  //M
                raw_file[3] == 0x42)    //B
            {
                files_counter = raw_file[0x10] + raw_file[0x11] * 0x100;

                if (files_counter > 0)
                {
                    for (int i = 0; i < files_counter; i++)
                    {
                        int point = 0x20 + i * 0x10;
                        if (raw_file[point + 0xb] == 0xff)
                        {
                            files_pointers.Add(raw_file[point] + (raw_file[point + 1] * 0x100) + (raw_file[point + 2] * 0x10000));
                            files_lens.Add(raw_file[point + 4] + (raw_file[point + 5] * 0x100) + (raw_file[point + 6] * 0x10000));
                        }
                        else
                        {
                            //continue; //is actually useless here
                        }
                    }

                    //Actual number of files inside may differ from the number given in the header
                    files_counter = files_pointers.Count;

                    int filenames_index = raw_file[0x1C] + raw_file[0x1D] * 0x100 + raw_file[0x1E] * 0x10000 + raw_file[0x1F] * 0x1000000;

                    int filenames_offset = raw_file.Length - filenames_index;
                    Byte[] files_names_bytes = new Byte[filenames_offset];
                    Array.Copy(raw_file, filenames_index, files_names_bytes, 0, filenames_offset);

                    string files_names_str = Encoding.ASCII.GetString(files_names_bytes);
                    string[] files_names_raw = files_names_str.Split('\x00');

                    //Some AMB files have no names of their files
                    if (filenames_index != 0)
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

        static void AMB_Extract(string filename, string output)
        {
            var files = AMB_Read(filename);

            byte[] raw_file = File.ReadAllBytes(filename);

            //Creating folder if it doesn't exist
            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }

            for (int i = 0; i < files.Count; i++)
            {
                string file_name = output + Path.DirectorySeparatorChar + files[i].Item1;

                Byte[] file_bytes = new Byte[files[i].Item3];
                Array.Copy(raw_file, files[i].Item2, file_bytes, 0, files[i].Item3);
                
                if (!Directory.Exists(Path.GetDirectoryName(file_name)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(file_name));
                }
                File.WriteAllBytes(file_name, file_bytes);
            }
        }
        
        static void AMB_Patch(string file_name, string mod_file)
        {
            var files = AMB_Read(file_name);

            byte[] raw_file = File.ReadAllBytes(file_name);

            int index = -1;

            string[] mod_file_parts = mod_file.Split(Path.DirectorySeparatorChar);

            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].Item1 == mod_file_parts[mod_file_parts.Length - 1])
                {
                    index = i;
                    break; //TODO: find a better way of finding a variable in list of tuples.
                }
                else if (files[i].Item1 == String.Join("\\",mod_file_parts.Skip(mod_file_parts.Length - 2)))
                {
                    index = i;
                    break; //TODO stays the same
                }
            }

            if (index != -1)
            {
                byte[] raw_mod_file = File.ReadAllBytes(mod_file);

                if (raw_mod_file.Length <= files[index].Item3) //TODO: make pointers and lengths shifting aka bigger files
                {
                    for (int i = 0; i < files[index].Item3; i++)
                    {
                        if (raw_mod_file.Length - 1 >= i)
                        {
                            raw_file[files[index].Item2 + i] = raw_mod_file[i];
                        }
                        else
                        {
                            raw_file[files[index].Item2 + i] = 0;
                        }
                    }
                }
                File.WriteAllBytes(file_name, raw_file);
            }
        }

        static void PatchAll(string file_name, List<string> mod_files, List<string> mod_paths)
        {
            if (File.Exists(file_name))
            {
                Backup(file_name);
                if (file_name.ToUpper().EndsWith(".AMB"))
                {
                    for (int i = 0; i < mod_files.Count; i++)
                    {
                        string mod_file_full = String.Join(Path.DirectorySeparatorChar.ToString(), new string[] { "mods", mod_paths[i], mod_files[i] });

                        if (file_name == mod_files[i])
                        {
                            File.Copy(mod_file_full, file_name, true);
                        }
                        else
                        {
                            AMB_Patch(file_name, mod_file_full);
                        }
                    }
                }
                else if (file_name.ToUpper().EndsWith(".CSB"))
                {
                    if (File.Exists(file_name.Substring(0, file_name.Length - 4) + ".CPK"))
                    {
                        Backup(file_name.Substring(0, file_name.Length - 4) + ".CPK");
                    }
                    //Needs CSB Editor (from SonicAudioTools) to work
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "CsbEditor.exe";
                    startInfo.Arguments = file_name;
                    Process.Start(startInfo).WaitForExit();

                    for (int i = 0; i < mod_files.Count; i++)
                    {
                        File.Copy("mods" + Path.DirectorySeparatorChar + mod_paths[i] + Path.DirectorySeparatorChar + mod_files[i], mod_files[i], true);
                    }
                    
                    startInfo.Arguments = file_name.Substring(0, file_name.Length - 4);
                    Process.Start(startInfo).WaitForExit();
                }
            }
        }

        static void Backup(string file_name)
        {
            if (!File.Exists(file_name + ".bkp"))
            {
                File.Copy(file_name, file_name + ".bkp");
            }
        }

        static void Restore(string file_name)
        {
            if (File.Exists(file_name + ".bkp"))
            {
                File.Copy(file_name + ".bkp", file_name, true);
            }
        }

        static List<Tuple<string, List<string>, List<string>>> GetModFiles()
        {
            var result = new List<Tuple<string, List<string>, List<string>>>();

            if (File.Exists("mods/mods.ini"))
            {
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
                            string[] filename_parts = filenames[j].Split(Path.DirectorySeparatorChar);
                            string original_file = "";
                            
                            for (int k = 0; k < filename_parts.Length - 2; k++)
                            {
                                string possible_orig_file = String.Join(Path.DirectorySeparatorChar.ToString(), filename_parts.Skip(2).Take(k+1));
                                
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
                            
                            string mod_file = String.Join(Path.DirectorySeparatorChar.ToString(), filename_parts.Skip(2));
                            
                            string mod_path = filename_parts[1];
                           
                            if (!orig_files.Contains(original_file))
                            {
                                orig_files.Add(original_file);
                            }

                            if (mod_files.Count != j + 1)
                            {
                                mod_files.Add(new List<string> { });
                                mod_dirs.Add(new List<string> { });
                            }
                            
                            int ind = orig_files.IndexOf(original_file);
                            

                            if (!mod_files[ind].Contains(mod_file))
                            {
                                mod_files[ind].Add(mod_file);
                                mod_dirs[ind].Add(mod_path);
                            }
                            else
                            {
                                mod_dirs[ind][mod_files[ind].IndexOf(mod_file)] = mod_path;
                            }
                        }
                    }
                }

                //Into a Tuple into a List
                for (int i = 0; i < orig_files.Count; i++)
                {
                    result.Add(Tuple.Create(orig_files[i], mod_files[i], mod_dirs[i]));
                }
            }
            return result;
        }

        static void ShowHelpMessage()
        {
            Console.WriteLine("AMB Patcher by OSA413");
            Console.WriteLine("Usage:");
            Console.WriteLine("\tAMBPatcher.exe - Patch all files used by enabled mods.");
            Console.WriteLine("\tAMBPatcher.exe [AMB file] and");
            Console.WriteLine("\tAMBPatcher.exe extract [AMB file] - Extract all files from [AMB file] to \"[AMB file]_extracted\" directory.");
            Console.WriteLine("\tAMBPatcher.exe extract [AMB file] [dest dir] - Extract all files from [AMB file] to [dest dir] directory.");
            Console.WriteLine("\tAMBPatcher.exe patch [AMB file] [another file] - Patch [AMB file] by [another file] if [another file] is in [AMB file].");
            Console.WriteLine("\tAMBPatcher.exe -h and");
            Console.WriteLine("\tAMBPatcher.exe --help - Show this message.");
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var test = GetModFiles();

                /*
                for (int i = 0; i < test.Count; i++)
                {
                    Console.WriteLine(test[i].Item1);
                    for (int j = 0; j < test[i].Item2.Count; j++)
                    {
                        Console.Write("\t"+test[i].Item2[j]);
                        Console.WriteLine("\t"+test[i].Item3[j]);
                    }
                }
                Console.Read();
                */
                
                List<string> mods_prev = new List<string> { };
                List<string> modified_files = new List<string> { };

                if (File.Exists(@"mods\mods_prev"))
                {
                    mods_prev = File.ReadAllLines(@"mods\mods_prev").ToList<string>();
                }

                for (int i = 0; i < test.Count; i++)
                {
                    modified_files.Add(test[i].Item1);
                    Restore(test[i].Item1);
                    if (File.Exists(test[i].Item1.Substring(0, test[i].Item1.Length - 4) + ".CPK"))
                    {
                        Restore(test[i].Item1.Substring(0, test[i].Item1.Length - 4) + ".CPK");
                    }
                    PatchAll(test[i].Item1, test[i].Item2, test[i].Item3);
                    mods_prev.Remove(test[i].Item1);
                }

                for (int i = 0; i < mods_prev.Count; i++)
                {
                    Restore(mods_prev[i]);
                    if (File.Exists(mods_prev[i].Substring(0, mods_prev[i].Length - 4) + ".CPK"))
                    {
                        Restore(mods_prev[i].Substring(0, mods_prev[i].Length - 4) + ".CPK");
                    }
                }

                File.WriteAllText(@"mods\mods_prev", string.Join("\n", modified_files.ToArray()));
            }

            else if (args.Length == 1)
            {
                if (args[0] == "-h" || args[0] == "--help")
                {
                    ShowHelpMessage();
                }
                else
                {
                    if (File.Exists(args[0]))
                    {
                        if (!Directory.Exists(args[0] + "_extracted"))
                        {
                            Directory.CreateDirectory(args[0] + "_extracted");
                        }
                        AMB_Extract(args[0], args[0] + "_extracted");
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
                        AMB_Extract(args[1], args[1] + "_extracted");
                    }
                    else { ShowHelpMessage(); }
                }
                else if (args[0] == "read")
                {
                    if (File.Exists(args[1]))
                    {
                        var data = AMB_Read(args[1]);

                        for (int i = 0; i < data.Count; i++)
                        {
                            Console.WriteLine("\nFile name:    " + data[i].Item1);
                            Console.WriteLine("File pointer: " + data[i].Item2 + " (0x" + data[i].Item2.ToString("X") + ")");
                            Console.WriteLine("File length:  " + data[i].Item3 + " (0x" + data[i].Item3.ToString("X") + ")");
                        }
                    }
                    else { ShowHelpMessage(); }
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
                        AMB_Extract(args[1], args[2]);
                    }
                    else { ShowHelpMessage(); }
                }
                else if (args[0] == "patch")
                {
                    if (File.Exists(args[1]) && File.Exists(args[2]))
                    {
                        AMB_Patch(args[1], args[2]);
                    }
                    else { ShowHelpMessage(); }
                }
                else { ShowHelpMessage(); }
            }
            else { ShowHelpMessage(); }
        }
    }
}
