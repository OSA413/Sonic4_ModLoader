def get_test(test_name="", AMBPATCHER="", MAIN_AMB=""):
    tests = {
        "create": [[AMBPATCHER, "create", MAIN_AMB]],
        "add": ["create", [AMBPATCHER, "add", MAIN_AMB, "files/1"],
                [AMBPATCHER, "add", MAIN_AMB, "files/2"],
                [AMBPATCHER, "add", MAIN_AMB, "files/3"]],
        "swap_endianness": ["add", [AMBPATCHER, "swap_endianness", MAIN_AMB]],
        "swap_endianness_x2": ["swap_endianness", [AMBPATCHER, "swap_endianness", MAIN_AMB]],
        "add_to_swapped": [[AMBPATCHER, "create", MAIN_AMB],
                            [AMBPATCHER, "swap_endianness", MAIN_AMB],
                            [AMBPATCHER, "add", MAIN_AMB, "files/1"],
                            [AMBPATCHER, "add", MAIN_AMB, "files/2"],
                            [AMBPATCHER, "add", MAIN_AMB, "files/3"]],
        "extract": ["add", [AMBPATCHER, "extract", MAIN_AMB]],
        "extract_swapped": ["add_to_swapped", [AMBPATCHER, "extract", MAIN_AMB]]
    }
    
    if test_name == "":
        return tests
    return tests[test_name]
