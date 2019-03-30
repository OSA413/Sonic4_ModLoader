# How to compile and/or run this program on Linux.

1. Install [Mono](https://www.mono-project.com/download/stable/#download-lin)

2. Use `msbuild /path/to/solution.sln /p:Configuration=(Release|Debug)`

*Or run `bash build_linux.sh` in the root directory of the repository (don't forget to `cd` to that directory before using).*

3. To place all of the files into one directory, run `bash pack_linux.sh`. The files will go to `/dist/Sonic4ModLoader/` directory.

4. To run the executables, use `mono /path/to/program.exe`. Using Wine is not recommended.

You will also probably need the CsbEditor from SonicAudioTools. You can compile it the same way as this program, but I haven't tested it yet.

*Pro tip: You can run Windows' .Net executables on Linux with Mono as well as compile them on Linux for Windows with `msbuild`*
