import shutil, os, sys

os.chdir(os.path.dirname(sys.argv[0]))

shutil.copyfile("../../../dist/Sonic4ModLoader/AMBPatcher.exe",  "sandbox/AMBPatcher.exe")
print("Updated from release!\n")
