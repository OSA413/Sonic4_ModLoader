"""
What it does: creates a .bat file that extracts all AMB files.

How to use this:
1. Place this file, AMBPatcher.exe, and finder.py in the root directory of the game
2. Run this file. Wait for a while. When the program has finished its work, a number
    of .AMB files that are not extracted will be printed in the terminal. Press Enter.
3. Run extract_everything.bat .
4. Repeat 1-3 steps several times until you see "0" at the end of the step 2.

Result: no errors should appear, every AMB file should be extracted.
"""
import finder, os

a = finder.find("",".AMB",recursive = 1, case_sensitive = 0)

b = ""
c = 0

for i in a:
    if ((i.upper().endswith(".AMB") and os.path.isfile(i)) or i.split("\\")[-1].isdigit()):
        if not os.path.exists(i+"_extracted"):
            b += "AMBPatcher extract \""+i+"\"\n"
            c += 1

with open("extract_everything.bat","w") as f:
    f.write(b)

input(c)
