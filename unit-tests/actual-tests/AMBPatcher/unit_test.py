import hashlib
import os
import sys
import subprocess
import glob
import shutil

#Changing working directory to the sandbox
os.chdir(os.path.join(os.path.dirname(sys.argv[0]),"sandbox"))

#Defining some constants
ORIG_FILE_NAME = "CPIT_MAIN.AMB"

#SHA-256
#TODO
HASH_LIST = {
    "ep1": {
        "original_file":            "2c53ec1661bae68f1897f3b917d3546b4c1f6cf01c4fc9a97d020075b5ac8c03",
        "extracted_dir":            "f8f34eba2f5aca37a86446b513cdf5e9f3455bdf8a7376c10d22a7dc37e49366",
        "add":                      "",
        "orig_endianness_swapped":  ""
    },
    "ep2": {
        "original_file":            "1dd32285b0157f4dd96e4b9efcffd8f3403662a1fde9736392723248c5a77fde",
        "extracted_dir":            "475e73817779ee02ff66c5d83a316cdac6ac492a1c15fe0d508590c4ad0de99a"
    }
}

def sha256(x):
    return hashlib.sha256(x).hexdigest()
    
def dir_sha(path):
    all_shas = []
    
    walking_down_the_dir = [os.path.abspath(x) for x in glob.glob("./" + path + "/**", recursive = True)]
    
    for i in walking_down_the_dir:
        all_shas.append(i)
        if os.path.isfile(i):
            with open(i, "rb") as f:
                all_shas.append(sha256(f.read()))
    
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
    episode = None
    
    if os.path.isfile(ORIG_FILE_NAME):
        with open(ORIG_FILE_NAME, "rb") as f:
            main_file_sha = sha256(f.read())
        
    for i in HASH_LIST:
        if main_file_sha == HASH_LIST[i]["original_file"]:
            episode = i
            break

    return episode
    
EPISODE = get_episode()

def run_test(test_name, orig_file=ORIG_FILE_NAME, mono_or_wine=mono_or_wine(), EPISODE=EPISODE):
    test_result = "Failed"
    
    test_types = {
        "extract": ["AMBPatcher.exe", "extract", orig_file],
        "add": ["AMBPatcher.exe", "add", orig_file, ""]#mod_file]
    }
    
    exe_command = test_types[test_name]
    
    if os.name == "posix":
        exe_command.insert(0, mono_or_wine)
    
    console_output = subprocess.check_output(exe_command)
    
    if test_name == "extract":
        real_sha = dir_sha(orig_file + "_extracted")
        expecting_sha = HASH_LIST[EPISODE]["extracted_dir"]
        
        if real_sha == expecting_sha:
            test_result = "OK"
    
    return test_result

if __name__ == "__main__":
    print("Detected " + EPISODE)
    print("Creating a backup of main file")
    shutil.copyfile(ORIG_FILE_NAME, ORIG_FILE_NAME+".bkp")
    
    test_list = ["extract"]
    summary = []
    
    print()
    for i in test_list:
        print("Running \"" + i + "\"...")
        summary.append(run_test(i))
        print(summary[-1])
        
    print("\nSummary:")
    print(str(summary.count("OK")) + " out of " + str(len(summary))
          + " tests were successful (" + str(100*summary.count("OK")//len(summary))+"%)")
    if summary.count("Failed"):
        print("The following tests are failed:")
        for i in range(len(summary)):
            if summary[i] == "Failed":
                print("\t"+test_list[i])
