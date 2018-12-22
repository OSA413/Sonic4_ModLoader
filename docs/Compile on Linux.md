# How to compile this program on Linux.
***wip***

1. Install [Mono](https://www.mono-project.com/download/stable/#download-lin)

2. Use `msbuild /path/to/program.csproj /p:Configuration=(Release|Debug)` on every project file.
*Or run `build_linux_(release|debug).sh` in the root directory of the repository (don't forget to `cd` to that directory before using).*

3. To place all of the files in one directory, run `pack_linux.sh`. The files will go to `/dist/Sonic4ModLoader/` directory.

4. To run the executables, use `mono /path/to/program.exe`. Using Wine is not recommended.

You will also probably need the CsbEditor from SonicAudioTools. You can compile it the same way as this program, but I haven't tested it yet.

**At this moment I'm going to complete the Windows version before Linux optimizations. So Linux builds are more unstable than Windows'. I also could run neither Episode 1 nor 2 with Wine/Proton.**
