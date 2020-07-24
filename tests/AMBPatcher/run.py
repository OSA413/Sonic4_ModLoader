import os
import sys
import time

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

def run_test(test_name):
    test_sequence = instructions.get_test(test_name=test_name,
        AMBPATCHER = PATHS[AMBPATCHER], MAIN_AMB = "sandbox/"+MAIN_AMB)

    for seq in test_sequence:
        if MONO: seq.insert(0, MONO)
        subprocess.check_output(seq)

def check_files(test_name, REBUILD_SHA=False):
    for file in files_to_check.get_files(test_name, MAIN_AMB = MAIN_AMB):
        sha = sha256("sandbox/"+file)
        dir = "hashes/" + test_name
        sha_file = dir + "/" + file
        
        if (REBUILD_SHA):
            if not os.path.exists(dir):
                os.makedirs(dir)
            with open(sha_file, "w") as f: f.write(sha)

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
    rebuild_paths()
    EXIT_CODE = 0

    for test in instructions.get_test().keys():
        clear_sandbox()
        print(" " * 4 + test + " " * (20 - len(test)), end="")
        
        t_start = time.time()
        run_test(test)
        check = check_files(test, REBUILD_SHA=REBUILD_SHA)
        
        print_time = "("+str(round(time.time() - t_start, 4))+"s)"

        if check == 0:
            print(TEST_SUCCESS + " " + print_time)
        elif check == 1:
            EXIT_CODE = 1
            print(TEST_FAIL)
        elif check == 2:
            EXIT_CODE = 2
            print("? No SHA file")

    clear_sandbox()

    sys.exit(EXIT_CODE)
