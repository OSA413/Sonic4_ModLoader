def get_files(test_name, MAIN_AMB=""):
    tests = {
        "create": [MAIN_AMB],
    }

    if test_name in tests:
        return tests[test_name]
    else:
        return []
