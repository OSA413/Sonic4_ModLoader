def get_test(test_name="", AMBPATCHER="", MAIN_AMB=""):
    tests = {
        "create": [[AMBPATCHER, "create", MAIN_AMB]],
    }
    
    if test_name == "":
        return tests
    return tests[test_name]
