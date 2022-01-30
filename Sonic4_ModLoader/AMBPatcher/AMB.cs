/*
 * Copyright (c) 2018-2022 Oleg "OSA413" Sokolov
 * Licensed under the MIT License
 * https://github.com/OSA413/Sonic4_ModLoader/blob/master/LICENSE
 */

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;


public class AMB
{
    public enum Version
    {
        PC = 0x20,
        Mobile = 0x28
    }

    private byte[] source;
    public string AmbPath;
    public bool SameEndianness = true;
    public List<BinaryObject> Objects = new List<BinaryObject>();
    public bool hasNames = true;
    public Version version = Version.PC;
    public int Length { get => PredictPointers().name + Objects.Count * (hasNames ? 0x20 : 0) ;}

    public bool IsSourceAMB(int ptr=0)
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

    public Version GetVersion(byte[] binary = null, int ptr=0)
    {
        if (binary == null)
            binary = source;

        Version result;

        if (Enum.TryParse<Version>(BitConverter.ToInt32(binary, ptr + 0x4).ToString(), out result))
            return result;
        
        Array.Reverse(binary, ptr + 0x4, 4);
        Enum.TryParse<Version>(BitConverter.ToInt32(binary, ptr + 0x4).ToString(), out result);
        Array.Reverse(binary, ptr + 0x4, 4);

        return result;
    }

    public void SwapEndianness(byte[] binary = null, int ptr=0)
    {
        if (binary == null)
            binary = source;
        var swapHeader = IsLittleEndian(binary) == BitConverter.IsLittleEndian;

        if (!swapHeader)
        {
            Array.Reverse(binary, ptr + 0x4, 4); //version
            Array.Reverse(binary, ptr + 0x10, 4); //file number
            Array.Reverse(binary, ptr + 0x14, 4); //enumeration pointer
            Array.Reverse(binary, ptr + 0x18, 4); //data pointer
            Array.Reverse(binary, ptr + 0x1C, 4); //name pointer
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
        return (0x20, data, hasNames ? ptr : 0);
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

    public AMB() {}

    public AMB(string fileName) : this(File.ReadAllBytes(fileName), 0, fileName) {}

    public AMB(byte[] source, int sourcePtr=0, string fileName=null)
    {
        this.source = source;
        AmbPath = fileName;
        if (!IsSourceAMB()) return;
        version = GetVersion();
        
        int shift = 0;
        if (version == Version.Mobile)
            shift = 0x4;

        SameEndianness = BitConverter.IsLittleEndian == IsLittleEndian();

        if (!SameEndianness)
            SwapEndianness();

        var objNum = BitConverter.ToInt32(source, sourcePtr + 0x10);
        var listPtr = BitConverter.ToInt32(source, sourcePtr + 0x14) + sourcePtr;
        //var dataPtr = BitConverter.ToInt32(source, sourcePtr + 0x18 + shift) + sourcePtr; //this may be not dataPtr for mobile
        var namePtr = BitConverter.ToInt32(source, sourcePtr + 0x1C + shift) + sourcePtr;
        if (namePtr == 0) hasNames = false;

        for (int i = 0; i < objNum; i++)
        {
            var objPtr = BitConverter.ToInt32(source, listPtr + (0x10 + shift) * i) + sourcePtr;
            if (objPtr == 0) continue;
            var objLen = BitConverter.ToInt32(source, listPtr + (0x10 + shift) * i + 4 + shift);
            var newObj = new BinaryObject(source, objPtr, objLen);
            newObj.RealName = hasNames ? ReadString(source, namePtr + 0x20 * i) : i.ToString();
            newObj.Name = MakeNameSafe(newObj.RealName);
            newObj.ParentAMB = this;
            if (IsSourceAMB(objPtr))
                newObj.Amb = new AMB(source, objPtr, AmbPath + "\\" + newObj.Name);
            Objects.Add(newObj);
        }

        if (!SameEndianness)
            SwapEndianness();
    }

    public void Save(string filePath = null, bool swapEndianness = false)
    {
        if (filePath == null) filePath = AmbPath;
        if (Path.GetDirectoryName(filePath) != "")
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        File.WriteAllBytes(filePath, Write(swapEndianness));
    }

    public byte[] Write(bool swapEndianness = false)
    {
        var result = new byte[Length];
        var pointers = PredictPointers();

        Array.Copy(Encoding.ASCII.GetBytes("#AMB"), 0, result, 0, 4);
        Array.Copy(BitConverter.GetBytes(Enum.IsDefined(typeof(Version), version) ? (int)version : 0x20), 0, result, 0x04, 4); //AMB file version
        Array.Copy(BitConverter.GetBytes(Objects.Count), 0, result, 0x10, 4);
        Array.Copy(BitConverter.GetBytes(pointers.list), 0, result, 0x14, 4);
        Array.Copy(BitConverter.GetBytes(pointers.data), 0, result, 0x18, 4);
        Array.Copy(BitConverter.GetBytes(pointers.name), 0, result, 0x1C, 4);

        foreach (var o in Objects)
        {
            Array.Copy(BitConverter.GetBytes(pointers.data), 0, result, pointers.list, 4);
            Array.Copy(BitConverter.GetBytes(o.LengthNice), 0, result, pointers.list + 4, 4);

            var oWrite = o.Write();
            Array.Copy(oWrite, 0, result, pointers.data, o.Length);
            if (hasNames)
            {
                Array.Copy(Encoding.ASCII.GetBytes(o.RealName), 0, result, pointers.name, o.RealName.Length);
                pointers.name += 0x20;
            }

            pointers.list += 0x10;
            pointers.data += o.LengthNice;
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
        var target = FindObject(newName?.Replace('/', '\\') ?? GetRelativeName(AmbPath, filePath.Replace("_extracted", "")));

        var newObj = new BinaryObject(filePath);
        newObj.Name = newObj.RealName = target.name;

        if (target.index == -1)
            target.amb.Objects.Add(newObj);
        else
            target.amb.Replace(newObj, target.index);
    }

    public (AMB amb, int index, string name) FindObject(string objectName)
    {
        var InternalName = objectName;

        var InternalIndex = Objects.FindIndex(x => x.Name == InternalName);
        if (InternalIndex != -1)
            return (this, InternalIndex, InternalName);

        var ParentIndex = Objects.FindIndex(x => x.Name == String.Join("\\", InternalName.Split('\\').Take(x.Name.Count(c => c == '\\') + 1)));

        if (ParentIndex != -1)
            return Objects[ParentIndex].Amb.FindObject(objectName.Substring(Objects[ParentIndex].Name.Length + 1));

        return (this, -1, InternalName);
    }

    public void Replace(BinaryObject bo, int targetIndex)
    {
        bo.RealName = Objects[targetIndex].RealName;
        Objects[targetIndex] = bo;
    }

    public void ExtractAll(string output = null) => Extract(output, true);

    public void Extract(string output = null, bool extractAll = false)
    {
        if (!IsSourceAMB()) return;
        if (output == null) output = AmbPath + "_extracted";
        Directory.CreateDirectory(output);

        foreach (var o in Objects)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(output, o.Name)));
            if (extractAll && o.isAMB)
                o.Amb.Extract(Path.Combine(output, o.Name), true);
            else
                o.Save(Path.Combine(output, o.Name));
        }
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

        return rawName.Substring(safeIndex);
    }

    public void Remove(int index) => Objects.RemoveAt(index);
    public void Remove(string objectName)
    {
        var target = FindObject(objectName);
        target.amb.Remove(target.index);
    }
}

public class BinaryObject
{
    public string Name;
    public string RealName;
    public bool isAMB { get => Amb != null; }
    public AMB Amb;
    public AMB ParentAMB;

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

    public byte[] Write()
    {
        if (isAMB)
            return Amb.Write();
        return Source.Skip(Pointer).Take(Length).ToArray();
    }

    public void Save(string path) => File.WriteAllBytes(path, Write());
}