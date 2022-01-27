import os
import sys
import time
import glob
import shutil

cwd = os.path.dirname(sys.argv[0])
if cwd != "":
    os.chdir(cwd)
del cwd

sys.path.append(os.path.abspath(".."))
from commons import *
import instructions
import files_to_check

MAIN_AMB = "test.amb"
MONO = mono_or_wine()

def run_test(test_name, NO_CRASH=False):
    test_sequence = instructions.get_test(test_name=test_name,
        AMBPATCHER = PATHS[AMBPATCHER], MAIN_AMB = "sandbox/"+MAIN_AMB)

    test_time = time.time()

    for seq in test_sequence:
        if type(seq) == str:
            if seq == "#TIME":
                test_time = time.time()
            elif seq == "#COPYMODS":
                copy_dir_recursively("mods/mods", "sandbox/mods")
                copy_dir_recursively("mods/textures", "sandbox/textures")
            elif seq == "#EDGE_EXTRACTED_PREP":
                copy_dir_recursively("files", "sandbox/files.amb_extracted/abc/def")
            elif seq.startswith("#CWD:") and len(seq) > 5:
                os.chdir(seq[5:])
            elif seq.startswith("#MODSINI:") and len(seq) >= 9:
                with open("mods/mods.ini", "w") as f:
                    f.write("\n".join([x for x in seq[9:]]))
            else:
                run_test(seq, NO_CRASH)
        else:
            if MONO: seq.insert(0, MONO)
            if NO_CRASH:
                try:
                    subprocess.check_output(seq)
                except:
                    sb = "sandbox/"
                    if not os.path.isdir("sandbox"): sb = ""
                    with open(sb + "fail", "w") as f: pass
            else:
                subprocess.check_output(seq)

    return time.time() - test_time

def check_files(test_name, REBUILD_SHA=False):
    if os.path.isfile("sandbox/fail"):
        return 1

    lst = files_to_check.get_files(test_name, MAIN_AMB = MAIN_AMB)
    if type(lst) == str:
        return check_files(lst, REBUILD_SHA = REBUILD_SHA)
    files = []

    for i in lst:
        if i == "#ALL":
            files += [x[8:] for x in glob.glob("sandbox/**/*", recursive=True) if os.path.isfile(x)]
        elif i.startswith("#EXCEPT:"):
            filtr = i[8:]
            if filtr[-1] == "*":
                files = [x for x in files if not x.startswith(filtr[:-1])]
            else:
                files = [x for x in files if x != filtr]
        else:
            files.append(i)

    expected_files = [x[len("hashes/" + test_name) + 1:] for x in glob.glob("hashes/" + test_name + "/**/*", recursive=True) if os.path.isfile(x)]
    actual_set = set(files)

    for file in expected_files:
        if file not in actual_set:
            return 2

    for file in files:
        sha = sha256("sandbox/"+file)
        sha_file = "hashes/" + test_name + "/" + file
        dir = os.path.dirname(sha_file)
        
        if (REBUILD_SHA):
            if not os.path.exists(dir):
                os.makedirs(dir)

            if sha != None:
                with open(sha_file, "w") as f:
                    f.write(sha)

        else:
            if os.path.isfile(sha_file):
                with open(sha_file, "r") as f:
                    expected_sha = f.read()
                
                if sha != expected_sha:
                    return 1
            else:
                return 2
    return 0
    
def clear_sandbox():
    clear_dir("sandbox")

if __name__ == "__main__":
    REBUILD_SHA = len(sys.argv) > 1 and sys.argv[1] == "--rebuild-sha"
    NO_CRASH = len(sys.argv) > 1 and sys.argv[1] == "--no-crash"
    rebuild_paths()
    EXIT_CODE = 0

    for test in [x for x in instructions.get_test().keys() if x[0] != "."]:
        clear_sandbox()
        print(" " * 4 + test + " " * (20 - len(test)), end="")
        
        test_time = run_test(test, NO_CRASH)
        check = check_files(test, REBUILD_SHA=REBUILD_SHA)

        if check == 0:
            try:
                print(TEST_SUCCESS + " (" + str(round(test_time, 4)) + "s)")
            except:
                print("SUCCESS (" + str(round(test_time, 4)) + "s)")
        elif check == 1:
            EXIT_CODE = 1
            try:
                print(TEST_FAIL)
            except:
                print("FAIL")
        elif check == 2:
            EXIT_CODE = 2
            print("? No SHA file")

        #this is for testing
        if test in []:
            input("Press Enter to continue")

    clear_sandbox()

    sys.exit(EXIT_CODE)
