"""
What it does: creates a .bat file that extracts all AMB files.

How to use this:
1. Place this file, "AMBPatcher.exe", and "finder.py" in the root directory of the game
2. Run this file. Wait for a while. When the program has finished its work, a number
    of .AMB files that are not extracted will be printed in the terminal. Press Enter.
3. Run "extract_everything.bat".
4. Repeat 1-3 steps several times until you see "0" at the end of the step 2.

Result: no errors should appear, every AMB file should be extracted.
"""
import finder, os

a = finder.find("","",recursive = 1, case_sensitive = 0)

other_formats = "DDS,TXB,AMA,AME,ZNO,TXB,ZNM,ZNV,DC,EV,RG,MD,MP,AT,DF,DI,PSH,VSH,LTS,XNM,MFS,SSS,GPB,MSG,AYK".split(",")

for i in other_formats:
    a = [x for x in a if not x.endswith("."+i)]

a = [x for x in a if os.path.isfile(x)]
a = [x for x in a if not os.path.exists(x+"_extracted")]

b = ""
c = 0

for i in a:
    if i.upper().endswith(".AMB") or i.split("\\")[-1].isdigit():
        b += "AMBPatcher extract \""+i+"\"\n"
        c += 1

    else:
        pass#print(i)

with open("extract_everything.bat","w") as f:
    f.write(b)

input(c)
