import glob, os

a = glob.glob("./**/*.sh", recursive = True)
b = glob.glob("./**/files", recursive = True)
a += b

os.linesep = "\n"

for i in a:
    with open(i, "r") as f:
        b = f.readlines()
    with open(i, "w", newline="\n") as f:
        f.writelines(b)
