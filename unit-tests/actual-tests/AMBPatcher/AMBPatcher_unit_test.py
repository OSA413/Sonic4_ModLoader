import hashlib, os, sys, subprocess

cwd = os.path.dirname(sys.argv[0])

def test_extract():
    output = subprocess.check_output(["AMBPatcher.exe", "extract", os.path.join(cwd, "tmp", "CPIT_MAIN.AMB")])

test_extract()
