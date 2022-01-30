import hashlib
import os
import glob
import subprocess
import shutil
try:
    #This is a fallback for shutil.copytree for WSL
    import distutils.dir_util
except:
    pass
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

def make_path_abs(p):
    return os.path.abspath(PATHS[DIST]) + "/" + str(p)

def rebuild_paths():
    PATHS[AMBPATCHER] = make_path_abs(PATHS[AMBPATCHER])

def copy_dir_recursively_shutil(src, dst):
    shutil.copytree(src, dst)

def copy_dir_recursively_distutils(src, dst):
    distutils.dir_util._path_created = {}
    distutils.dir_util.copy_tree(src, dst)

COPY_DIR_RECURSIVELY_FUNCTION = None
def copy_dir_recursively(src, dst):
    global COPY_DIR_RECURSIVELY_FUNCTION
    if COPY_DIR_RECURSIVELY_FUNCTION == None:
        try:
            #For some reason shutil.copytree may crash with Errno 13 on WSL when running script outside of home directory
            copy_dir_recursively_shutil(src, dst)
            COPY_DIR_RECURSIVELY_FUNCTION = copy_dir_recursively_shutil
        except:
            copy_dir_recursively_distutils(src, dst)
            COPY_DIR_RECURSIVELY_FUNCTION = copy_dir_recursively_distutils
    else:
        COPY_DIR_RECURSIVELY_FUNCTION(src, dst)
