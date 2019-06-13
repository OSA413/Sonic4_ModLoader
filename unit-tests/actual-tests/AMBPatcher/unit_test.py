import hashlib
import os
import sys
import subprocess
import glob
import shutil
import time

#Entering the sandbox
os.chdir(os.path.join(os.path.dirname(sys.argv[0]), "sandbox"))

#Copying the textures of brown bricks
shutil.copyfile("../brown_brick.dds",  "brown_brick.dds")
shutil.copyfile("../brown_bricks.dds", "brown_bricks.dds")

#Defining some constants
ORIG_FILE_NAME = "CPIT_MAIN.AMB"

#SHA-256
#TODO
HASH_LIST = {
    "ep1": {
        "original_file":            "2c53ec1661bae68f1897f3b917d3546b4c1f6cf01c4fc9a97d020075b5ac8c03",
        "extracted_dir":            "f8f34eba2f5aca37a86446b513cdf5e9f3455bdf8a7376c10d22a7dc37e49366",
        "orig_endianness_swapped":  "cbf8d5a08b574a327e78a3e165105e15370ad274c7a26b8f703829ba39bd0582",
        "added_small":              "1b9ab25052045b2d5f45838496d4a1c2324d5f4d15223bb7fd0348ea07a6aeff",
        "added_big":                "d525d12a7542f6dfd73bdc789c9b4e00edd1ad03b182632a12ab61e72a49ee5d",
        "patched_by_small":         "6057bed60505b9dd53b5a8109680bfd528b50e3c2e37347fea4ec8695738d8e1",
        "patched_by_big":           "dbb8b2fcc303501520c19e5221b93a72e9804f63f74f76627df8e99e8776b963",
        "added_recurs_small":       "",
        "added_recurs_big":         "",
        "patched_recurs_small":     "",
        "patched_recurs_big":       "",
        "modloader_simulated":      ""
    },
    "ep2": {
        "original_file":            "1dd32285b0157f4dd96e4b9efcffd8f3403662a1fde9736392723248c5a77fde",
        "extracted_dir":            "475e73817779ee02ff66c5d83a316cdac6ac492a1c15fe0d508590c4ad0de99a",
        "orig_endianness_swapped":  "9c2dd9458354613be3f87a30eb26f5d5f17603745760cb918406a06612a1d011",
        "added_small":              "b7f7aebd5ac29b21798885cb110a053b54e6e26cba4824cbc7779f417cb472d7",
        "added_big":                "9c84b90ab16f589fdb39ef73bec4ae1e7dea123e75be2805878c8bec5dc1812c",
        "patched_by_small":         "30deb0fd2685ed53baa45323670351e933db1f04ff425e5ce4fb38be4ffdd35a",
        "patched_by_big":           "c58afb81db725f72510ea65dd43fc0b99795b4dd1aed214728799ed7ef225e54",
        "added_recurs_small":       "",
        "added_recurs_big":         "",
        "patched_recurs_small":     "",
        "patched_recurs_big":       "",
        "modloader_simulated":      ""
    }
}

def sha256(x):
    if type(x) == str:
        if os.path.isfile(x):
            with open(x, "rb") as f:
                x = f.read()

    return hashlib.sha256(x).hexdigest()
    
def dir_sha(path):
    all_shas = []
    
    walking_down_the_dir = [os.path.abspath(x) for x in glob.glob("./" + path + "/**", recursive = True)]
    
    for i in walking_down_the_dir:
        all_shas.append(i)
        if os.path.isfile(i):
            all_shas.append(sha256(i))
    
    return sha256("".join(all_shas).encode('utf-8'))
    
def mono_or_wine():
    if os.name == "nt": return ""
    answer = ""
    
    try:
        #1/0
        subprocess.check_output(["mono", "--version"])
        answer = "mono"
    except:
        try:
            subprocess.check_output(["wine", "--version"])
            answer = "wine"
        except: pass
    
    return answer

def get_episode():
    main_file_sha = ""
    episode = "None"
    
    if os.path.isfile(ORIG_FILE_NAME):
        main_file_sha = sha256(ORIG_FILE_NAME)
        
    for i in HASH_LIST:
        if main_file_sha == HASH_LIST[i]["original_file"]:
            episode = i
            break

    return episode
    
EPISODE = get_episode()

def run_test(test_name, orig_file=ORIG_FILE_NAME, mono_or_wine=mono_or_wine(), EPISODE=EPISODE):
    test_result = "✖ Failed"
    
    test_types = {
        "extract":              ["AMBPatcher.exe", "extract", orig_file],
        "swap_endianness":      ["AMBPatcher.exe", "swap_endianness", orig_file],
        "add_small":            ["AMBPatcher.exe", "add", orig_file, "brown_brick.dds"],
        "add_big":              ["AMBPatcher.exe", "add", orig_file, "brown_bricks.dds"],
        "patch_by_small":       ["AMBPatcher.exe", "patch", orig_file, "G_FIX.AMA"],
        "patch_by_big":         ["AMBPatcher.exe", "patch", orig_file, "G_FIX.AMA"],
        "modloader_simulation": ["AMBPatcher.exe"],
        "modloader_recovery":   ["AMBPatcher.exe", "recover"]
    }
    
    exe_command = test_types[test_name]
    
    if os.name == "posix":
        exe_command.insert(0, mono_or_wine)
    
    subprocess.check_output(exe_command)
    
    
    #Don't Repeat Yourself
    real_sha = sha256(orig_file)
    expecting_sha = ""
    
    if test_name == "extract":
        real_sha = dir_sha(orig_file + "_extracted")
        expecting_sha = HASH_LIST[EPISODE]["extracted_dir"]
        
    elif test_name == "swap_endianness":
        expecting_sha = HASH_LIST[EPISODE]["orig_endianness_swapped"]

        if real_sha == expecting_sha:
            subprocess.check_output(exe_command)
            
            real_sha = sha256(orig_file)
            expecting_sha = HASH_LIST[EPISODE]["original_file"]

    elif test_name.startswith("add"):
        if "rec" in test_name:
            pass
        else:
            if "small" in test_name:
                expecting_sha = HASH_LIST[EPISODE]["added_small"]
            elif "big" in test_name:
                expecting_sha = HASH_LIST[EPISODE]["added_big"]
            
    elif test_name.startswith("patch_by"):
        if "rec" in test_name:
            pass
        else:
            if "small" in test_name:
                expecting_sha = HASH_LIST[EPISODE]["patched_by_small"]
            elif "big" in test_name:
                expecting_sha = HASH_LIST[EPISODE]["patched_by_big"]
                
    if real_sha == expecting_sha:
            test_result = "✔ OK"

    #print(real_sha)                

    return test_result

def before_test_setup(test_name):
    if test_name.startswith("patch_by"):
        if test_name.endswith("small"):
            shutil.copyfile("brown_brick.dds",  "G_FIX.AMA")
        elif test_name.endswith("big"):
            shutil.copyfile("brown_bricks.dds", "G_FIX.AMA")

def after_test_cleanup(test_name):
    if test_name == "extract":
        shutil.rmtree(ORIG_FILE_NAME+"_extracted")
        
    elif test_name == "swap_endianness":
        #it will recover original file in case if it fails
        shutil.copyfile(ORIG_FILE_NAME+".bkp", ORIG_FILE_NAME)
        
    elif test_name.startswith("add"):
        shutil.copyfile(ORIG_FILE_NAME+".bkp", ORIG_FILE_NAME)
        
    elif test_name.startswith("patch_by"):
        os.remove("G_FIX.AMA")
        shutil.copyfile(ORIG_FILE_NAME+".bkp", ORIG_FILE_NAME)
        
    elif test_name == "end":
        os.remove("CPIT_MAIN.AMB.bkp")
        os.remove("brown_brick.dds")
        os.remove("brown_bricks.dds")

if __name__ == "__main__":
    print("Detected " + EPISODE)
    
    if EPISODE == "None":
        print("SHA-256 of main file:")
        if os.path.isfile(ORIG_FILE_NAME):
            print(sha256(ORIG_FILE_NAME))
        else:
            print("File not found")
        print("Exiting...")
        exit()
    
    print("Creating a backup of main file")
    shutil.copyfile(ORIG_FILE_NAME, ORIG_FILE_NAME+".bkp")
    
    test_list = ["extract", "swap_endianness", "add_small", "add_big",
                    "patch_by_small", "patch_by_big"]
    summary = []
    
    print()

    for i in test_list:
        print("Running \"" + i + "\"...", end=" "*(32-len(i)))

        before_test_setup(i)

        t_start = time.time()
        summary.append(run_test(i))

        print(summary[-1], end=" ")
        print("("+str(round(time.time() - t_start, 4))+"s)")

        after_test_cleanup(i)
        
    after_test_cleanup("end")

    print("\nSummary:")
    print(str(summary.count("✔ OK")) + " out of " + str(len(summary))
          + " tests were successful (" + str(100*summary.count("OK")//len(summary))+"%)")
    if summary.count("Failed"):
        print("The following tests are failed:")
        for i in range(len(summary)):
            if summary[i] == "Failed":
                print("\t"+test_list[i])
