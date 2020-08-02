def get_test(test_name="", AMBPATCHER="", MAIN_AMB=""):
    tests = {
        ".add:files/1": [[AMBPATCHER, "add", MAIN_AMB, "files/1"]],
        ".add:files/2": [[AMBPATCHER, "add", MAIN_AMB, "files/2"]],
        ".add:files/3": [[AMBPATCHER, "add", MAIN_AMB, "files/3"]],
        ".swap_endianness": [[AMBPATCHER, "swap_endianness", MAIN_AMB]],
        ".extract": [[AMBPATCHER, "extract", MAIN_AMB]],
        
        "create": [[AMBPATCHER, "create", MAIN_AMB]],
        "add": ["create", ".add:files/1", ".add:files/2", ".add:files/3"],
        "swap_endianness": ["add", ".swap_endianness"],
        "swap_endianness_x2": ["swap_endianness", ".swap_endianness"],
        "add_to_swapped": ["create", ".swap_endianness",
                            ".add:files/1", ".add:files/2", ".add:files/3"],
        "extract": ["add", ".extract"],
        "extract_swapped": ["add_to_swapped", ".extract"]
    }
    
    if test_name == "":
        return tests
    return tests[test_name]
