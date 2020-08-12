def get_test(test_name="", AMBPATCHER="", MAIN_AMB=""):
    tests = {
        ".add:files/1": [[AMBPATCHER, "add", MAIN_AMB, "files/1"]],
        ".add:files/2": [[AMBPATCHER, "add", MAIN_AMB, "files/2"]],
        ".add:files/3": [[AMBPATCHER, "add", MAIN_AMB, "files/3"]],
        ".swap_endianness": [[AMBPATCHER, "swap_endianness", MAIN_AMB]],
        ".extract": [[AMBPATCHER, "extract", MAIN_AMB]],

        "create": [[AMBPATCHER, "create", MAIN_AMB]],
        "add": ["create", ".add:files/1", ".add:files/2", "#TIME", ".add:files/3"],
        "swap_endianness": ["add", "#TIME",".swap_endianness"],
        "swap_endianness_x2": ["swap_endianness", "#TIME", ".swap_endianness"],
        "add_to_swapped": ["create", ".swap_endianness",
                            ".add:files/1", ".add:files/2","#TIME", ".add:files/3"],
        "extract": ["add","#TIME", ".extract"],
        "extract_swapped": ["add_to_swapped","#TIME", ".extract"],
        "add_from_dir": ["create", "#TIME", [AMBPATCHER, "patch", MAIN_AMB, "files"]]
    }
    
    if test_name == "":
        return tests
    return tests[test_name]
