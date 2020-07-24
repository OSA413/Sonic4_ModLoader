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
                x = f.read()

    return hashlib.sha256(x).hexdigest()

def dir_sha(path):
    all_shas = []

    walking_down_the_dir = [x for x in glob.glob("./" + path + "/**", recursive = True)]
    walking_down_the_dir.sort()

    for i in walking_down_the_dir:
        all_shas.append(i.replace("\\", "/"))
        if os.path.isfile(i):
            all_shas.append(sha256(i))

    return sha256("".join(all_shas).encode('utf-8'))

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
