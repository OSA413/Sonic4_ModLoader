# How to compile this program on Windows.

0. Download and install Visual Studio (C#) or [MSBuild](https://docs.microsoft.com/visualstudio/msbuild/msbuild)

1. Download source code of this Mod Loader: https://github.com/OSA413/Sonic4_ModLoader
Build the solution.

2. To get necessary dependencies, follow instructions in the [`dependencies/readme.md`](/dependencies/readme.md) file.

3. Run the `build_pack_test.sh` file in the root directory.¹

4. You will get an archive and a folder named `Sonic4ModLoader` with the program.

That's all.

¹ - to run this `sh` script, you need to install Linux on Windows through [WSL](https://aka.ms/wslinstall), or find a bash for Windows if you only need to pack the files.
