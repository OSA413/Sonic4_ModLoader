def get_test(test_name="", AMBPATCHER="", MAIN_AMB=""):
    tests = {
        ".add:files/1": [[AMBPATCHER, "add", MAIN_AMB, "files/1"]],
        ".add:files/2": [[AMBPATCHER, "add", MAIN_AMB, "files/2"]],
        ".add:files/3": [[AMBPATCHER, "add", MAIN_AMB, "files/3"]],
        ".swap_endianness": [[AMBPATCHER, "swap_endianness", MAIN_AMB]],
        ".extract": [[AMBPATCHER, "extract", MAIN_AMB]],
        ".add_itself": [[AMBPATCHER, "add", MAIN_AMB, MAIN_AMB]],
        ".extract_all": [[AMBPATCHER, "extract_all", MAIN_AMB+"/../"]],

        "create": [[AMBPATCHER, "create", MAIN_AMB]],
        "add": ["create", ".add:files/1", ".add:files/2", "#TIME", ".add:files/3"],
        "swap_endianness": ["add", "#TIME",".swap_endianness"],
        "swap_endianness_x2": ["swap_endianness", "#TIME", ".swap_endianness"],
        "add_to_swapped": ["create", ".swap_endianness",
                            ".add:files/1", ".add:files/2","#TIME", ".add:files/3"],
        "extract": ["add","#TIME", ".extract"],
        "extract_swapped": ["add_to_swapped","#TIME", ".extract"],
        "add_from_dir": ["create", "#TIME", [AMBPATCHER, "patch", MAIN_AMB, "files"]],
        "add_as": ["create", "#TIME", [AMBPATCHER, "add", MAIN_AMB, "files/1", "2"]],
        "create_nested": ["add", ".add_itself", ".add_itself", "#TIME", ".add_itself"],
        "extract_nested": ["create_nested", "#TIME", ".extract"],
        "extract_all": ["create_nested", "#TIME", ".extract_all"]
    }
    
    if test_name == "":
        return tests
    return tests[test_name]
