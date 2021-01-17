using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace AMB
{
    public class AMB_new
    {
        private byte[] source;
        private string ambPath;
        public bool SameEndianness = true;
        public List<BinaryObject> Objects = new List<BinaryObject>();
        //TODO: use uint
        public int Length { get => PredictPointers().name + Objects.Count * 0x20;}

        private bool IsSourceAMB(int ptr=0)
        {
            return source.Length - ptr >= 0x20
                && source[ptr + 0] == '#'
                && source[ptr + 1] == 'A'
                && source[ptr + 2] == 'M'
                && source[ptr + 3] == 'B';
        }

        public bool IsLittleEndian(byte[] binary = null)
        {
            if (binary == null)
                binary = source;
            bool FileIsLittleEndian = BitConverter.IsLittleEndian;
            if (BitConverter.ToInt32(binary, 4) > 0xFFFF)
                FileIsLittleEndian = !FileIsLittleEndian;
            return FileIsLittleEndian;
        }

        public void SwapEndianness(byte[] binary = null, int ptr=0)
        {
            if (binary == null)
                binary = source;
            var swapHeader = IsLittleEndian(binary) == BitConverter.IsLittleEndian;

            if (!swapHeader)
            {
                Array.Reverse(binary, ptr + 0x4, 4);
                Array.Reverse(binary, ptr + 0x10, 4);
                Array.Reverse(binary, ptr + 0x14, 4);
                Array.Reverse(binary, ptr + 0x18, 4);
                Array.Reverse(binary, ptr + 0x1C, 4);
            }

            var objNum = BitConverter.ToInt32(binary, ptr + 0x10);
            var listPointer = BitConverter.ToInt32(binary, ptr + 0x14);

            for (int i = 0; i < objNum; i++)
            {
                Array.Reverse(binary, listPointer + 0x10 * i, 4);
                Array.Reverse(binary, listPointer + 0x10 * i + 4, 4);
            }

            if (swapHeader)
            {
                Array.Reverse(binary, ptr + 0x4, 4);
                Array.Reverse(binary, ptr + 0x10, 4);
                Array.Reverse(binary, ptr + 0x14, 4);
                Array.Reverse(binary, ptr + 0x18, 4);
                Array.Reverse(binary, ptr + 0x1C, 4);
            }
        }

        private (int list, int data, int name) PredictPointers()
        {
            int data = 0x20 + 0x10 * Objects.Count;
            var ptr = data + Objects.Sum(x => x.LengthNice);
            return (0x20, data, ptr);
        }


        private StringBuilder sb = new StringBuilder();
        private string ReadString(byte[] source, int pointer)
        {
            sb.Clear();
            while (pointer < source.Length && source[pointer] != 0x00)
                sb.Append((char)source[pointer++]);

            var str = sb.ToString();
            sb.Clear();
            return str;
        }

        public AMB_new() {}

        public AMB_new(string fileName) : this(File.ReadAllBytes(fileName)) { ambPath = fileName; }

        public AMB_new(byte[] source, int sourcePtr=0)
        {
            this.source = source;
            if (!IsSourceAMB()) return;

            SameEndianness = BitConverter.IsLittleEndian == IsLittleEndian();

            if (!SameEndianness)
                SwapEndianness();

            var objNum = BitConverter.ToInt32(source, sourcePtr + 0x10);
            var listPtr = BitConverter.ToInt32(source, sourcePtr + 0x14) + sourcePtr;
            var dataPtr = BitConverter.ToInt32(source, sourcePtr + 0x18) + sourcePtr;
            var namePtr = BitConverter.ToInt32(source, sourcePtr + 0x1C) + sourcePtr;

            for (int i = 0; i < objNum; i++)
            {
                var objPtr = BitConverter.ToInt32(source, listPtr + 0x10 * i) + sourcePtr;
                if (objPtr == 0) continue;
                var objLen = BitConverter.ToInt32(source, listPtr + 0x10 * i + 4);
                var newObj = new BinaryObject(source, objPtr, objLen);
                newObj.Name = MakeNameSafe(ReadString(source, namePtr + 0x20 * i));
                if (IsSourceAMB(objPtr))
                    newObj.Amb = new AMB_new(source, objPtr);
                Objects.Add(newObj);
            }

            if (!SameEndianness)
                SwapEndianness();
        }

        public void Write(string filePath, bool swapEndianness = false)
        {
            if (Path.GetDirectoryName(filePath) != "")
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllBytes(filePath, Write(swapEndianness));
        }

        public byte[] Write(bool swapEndianness = false)
        {
            var result = new byte[Length];
            var pointers = PredictPointers();

            Array.Copy(Encoding.ASCII.GetBytes("#AMB"), 0, result, 0, 4);
            Array.Copy(BitConverter.GetBytes(0x1304), 0, result, 0x04, 4); //Endianness identifier (at least for AMBPatcher)
            Array.Copy(BitConverter.GetBytes(Objects.Count), 0, result, 0x10, 4);
            Array.Copy(BitConverter.GetBytes(pointers.list), 0, result, 0x14, 4);
            Array.Copy(BitConverter.GetBytes(pointers.data), 0, result, 0x18, 4);
            Array.Copy(BitConverter.GetBytes(pointers.name), 0, result, 0x1C, 4);

            foreach (var o in Objects)
            {
                Array.Copy(BitConverter.GetBytes(pointers.data), 0, result, pointers.list, 4);
                Array.Copy(BitConverter.GetBytes(o.LengthNice), 0, result, pointers.list + 4, 4);

                Array.Copy(o.Source, o.Pointer, result, pointers.data, o.Length);
                Array.Copy(Encoding.ASCII.GetBytes(o.Name), 0, result, pointers.name, o.Name.Length);

                pointers.list += 0x10;
                pointers.data += o.LengthNice;
                pointers.name += 0x20;
            }

            if (swapEndianness == SameEndianness)
                SwapEndianness(result);

            return result;
        }

        public string GetRelativeName(string MainFileName, string objectName)
        {
            //Turning "C:\1\2\3" into {"C:","1","2","3"}
            var modPathParts = objectName.Replace('/', '\\').Split('\\');
            var origFileName = Path.GetFileName(MainFileName);

            //Trying to find where the original file name starts in the mod file name.
            int index = Array.IndexOf(modPathParts, origFileName);
                    
            string InternalName;
            //If it's inside, return the part after original file ends
            if (index != -1)
                InternalName = String.Join("\\", modPathParts.Skip(index + 1).ToArray());
            //Else use file name
            else
                InternalName = modPathParts.Last();

            //This may occur when main file and added file have the same name
            if (InternalName == "")
                InternalName = modPathParts.Last();

            return InternalName;
        }

        public void Add(string filePath, string newName = null)
        {
            if (newName != null)
            {
                var index = Objects.FindIndex(x=>x.Name == newName);
                if (index >= 0)
                {
                    Replace(filePath, newName);
                    return;
                }
            }
            var newObj = new BinaryObject(filePath);
            newObj.Name = GetRelativeName(ambPath, filePath);
            if (newName != null)
                newObj.Name = newName;
            Objects.Add(newObj);
        }

        public AMB_new FindObject(string MainFileName, string objectName)
        {
            string InternalName = GetRelativeName(MainFileName, objectName);

            int InternalIndex = Objects.FindIndex(x => x.Name == InternalName);
            if (InternalIndex != -1)
                return Objects[InternalIndex].Amb;

            var ParentIndex = Objects.FindIndex(x => x.Name == InternalName.Split('\\').First());
            if (ParentIndex != -1)
                return Objects[ParentIndex].Amb.FindObject(Objects[ParentIndex].Name, objectName);

            return null;
        }

        //TODO this won't work for nested files
        public void Replace(BinaryObject bo, int targetIndex) => Objects[targetIndex] = bo;
        public void Replace(BinaryObject bo, string targetName)
        {
            Replace(bo, Objects.FindIndex(x => x.Name == targetName));
        }
        public void Replace(string filePath, string targetIndex) => Replace(new BinaryObject(filePath), targetIndex);

        public void Extract(string output = null)
        {
            if (output == null) output = ambPath + "_extracted";
            Directory.CreateDirectory(output);

            foreach (var o in Objects)
                File.WriteAllBytes(Path.Combine(output, o.Name), o.Source.Skip(o.Pointer).Take(o.Length).ToArray());
        }

        public static string MakeNameSafe(string rawName)
        {
            //removing ".\" in the names (Windows can't create "." folders)
            //sometimes they can have several ".\" in the names
            //Turns out there's a double dot directory in file names
            //And double backslash in file names
            int safeIndex = 0;
            while (rawName[safeIndex] == '.' || rawName[safeIndex] == '\\' || rawName[safeIndex] == '/')
                safeIndex++;

            if (safeIndex == 0)
                return rawName;
            return rawName.Substring(safeIndex);
        }

        public void Remove(int index) => Objects.RemoveAt(index);
        public void Remove(string objectName) => Remove(Objects.FindIndex(x=>x.Name == objectName));
    }

    public class BinaryObject
    {
        public string Name;
        public bool isAMB { get => Amb != null; }
        public AMB_new Amb;
        public AMB_new ParentAMB;

        public byte[] Source {get; private set;}
        //TODO: use uint
        public int Pointer {get; private set;}
        int length;
        public int Length {get => isAMB ? Amb.Length : length;}
        //This makes length of the mod file to be % 16 = 0 with no empty space at the end
        public int LengthNice {get => Length + (16 - Length % 16) % 16;}

        public BinaryObject(byte[] source, int pointer, int length)
        {
            Source = source;
            Pointer = pointer;
            this.length = length;
        }

        public BinaryObject(byte[] source)
        {
            Source = source;
            this.length = Source.Length;
        }

        public BinaryObject(string filePath)
        {
            Source = File.ReadAllBytes(filePath);
            this.length = Source.Length;
        }
    }
}