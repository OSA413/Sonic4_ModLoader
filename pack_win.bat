::7-Zip folder goes here
SET sevenZ="dependencies\7-Zip"

::Current working directory
set cwd=%~dp0

::Absolute path to 7-zip
set sevenZ=%cwd%%sevenZ%

cd /d %cwd%

::Creating empty "dist" directory
rmdir /s /q dist
::mkdir "dist"
mkdir "dist/Sonic4ModLoader"

::Copying required files

SET dist="dist\Sonic4ModLoader\"

::What's new
COPY "Mod Loader - Whats new.txt"	%dist%"Mod Loader - Whats new.txt"

::Sonic4ModLoader
::License
COPY "LICENSE"				%dist%"LICENSE-Sonic4_ModLoader"
COPY "docs\files"			%dist%"LICENSE-Sonic4_ModLoader_files"
::EXEs
COPY "Sonic4_ModLoader\AMBPatcher\bin\Release\AMBPatcher.exe"			%dist%"AMBPatcher.exe"
COPY "Sonic4_ModLoader\ManagerLauncher\bin\Release\ManagerLauncher.exe" 	%dist%"ManagerLauncher.exe"
COPY "Sonic4_ModLoader\PatchLauncher\bin\Release\PatchLauncher.exe"		%dist%"PatchLauncher.exe"
COPY "Sonic4_ModLoader\Sonic4ModManager\bin\Release\Sonic4ModManager.exe"	%dist%"Sonic4ModManager.exe"
::READMEs
COPY "docs\README.rtf"		%dist%"README.rtf"
COPY "docs\README.txt"		%dist%"README.txt"
COPY "docs\README-tldr.txt"	%dist%"README-tldr.txt"

::SonicAudioTools
::License
COPY "dependencies\SonicAudioTools\LICENSE"		%dist%"LICENSE-SonicAudioTools"
COPY "dependencies\SonicAudioTools\files"		%dist%"LICENSE-SonicAudioTools_files"
::EXEs
COPY "dependencies\SonicAudioTools\SonicAudioLib.dll"	%dist%"SonicAudioLib.dll"
COPY "dependencies\SonicAudioTools\CsbEditor.exe"	%dist%"CsbEditor.exe"

::7-Zip
::License
::COPY "dependencies\7-Zip\License.txt"	%dist%"LICENSE-7-Zip"
::COPY "dependencies\7-Zip\files"	%dist%"LICENSE-7-Zip_files"
::EXEs
::COPY "dependencies\7-Zip\7z.exe"	%dist%"7z.exe"
::COPY "dependencies\7-Zip\7z.dll"	%dist%"7z.dll"

CD dist

::Archiving
%SevenZ%\7z a "Sonic4ModLoader.7z" * -mx=9
