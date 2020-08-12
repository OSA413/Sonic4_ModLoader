import hashlib
import os
import glob
import subprocess
from paths import *

TEST_SUCCESS = "✔ OK"
TEST_FAIL = "✖ Failed"

def sha256(x):
    if type(x) == str:
        if os.path.isfile(x):
            with open(x, "rb") as f:
                raw = f.read()

            return hashlib.sha256(raw).hexdigest()
    return None

def clear_dir(d):
    files = os.listdir(d)
    for i in files:
        i = d + "/" + i 
        if os.path.isdir(i):
            clear_dir(i)
            os.rmdir(i)
        elif os.path.isfile(i):
            if i.endswith(".gitignore"): continue
            os.remove(i)

def mono_or_wine():
    answer = ""
    if os.name == "nt": return answer

    try:
        subprocess.check_output(["mono", "--version"])
        answer = "mono"
    except:
        try:
            subprocess.check_output(["wine", "--version"])
            answer = "wine"
        except:
            pass

    return answer

def make_path_abs(p):
    return os.path.abspath(PATHS[DIST]) + "/" + str(p)

def rebuild_paths():
    PATHS[AMBPATCHER] = make_path_abs(PATHS[AMBPATCHER])
