"""
What it does: creates a "mods.ini" file with random mods enabled.

How to use this:
1. Place this file to the "/mods" directory.
2. Run this file.

Result: "mod.ini" should be created with random folder names in it.
This is for AMBPatcher and Mod Manager.
"""
import os, random

a = os.listdir(os.path.dirname(__file__))
a = [x for x in a if os.path.isdir(x)]

random.shuffle(a)
a = a[:random.randint(0,len(a))]

with open("mods.ini","w") as f:
    f.write("\n".join(a))
