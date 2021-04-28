def get_test(test_name="", AMBPATCHER="", MAIN_AMB=""):
    tests = {
        ".add:files/1": [[AMBPATCHER, "add", MAIN_AMB, "files/1"]],
        ".add:files/2": [[AMBPATCHER, "add", MAIN_AMB, "files/2"]],
        ".add:files/3": [[AMBPATCHER, "add", MAIN_AMB, "files/3"]],
        ".swap_endianness": [[AMBPATCHER, "swap_endianness", MAIN_AMB]],
        ".extract": [[AMBPATCHER, "extract", MAIN_AMB]],
        ".add_itself": [[AMBPATCHER, "add", MAIN_AMB, MAIN_AMB]],
        ".extract_all": [[AMBPATCHER, "extract_all", MAIN_AMB+"/../"]],
        ".recover": [[AMBPATCHER, "recover"]],

        "create": [[AMBPATCHER, "create", MAIN_AMB]],
        "add": ["create", ".add:files/1", ".add:files/2", "#TIME", ".add:files/3"],
        "delete": ["add", "#TIME", [AMBPATCHER, "delete", MAIN_AMB, "2"]],
        "swap_endianness": ["add", "#TIME",".swap_endianness"],
        "swap_endianness_x2": ["swap_endianness", "#TIME", ".swap_endianness"],
        "add_to_swapped": ["create", ".swap_endianness",
                            ".add:files/1", ".add:files/2","#TIME", ".add:files/3"],
        "extract": ["add","#TIME", ".extract"],
        "extract_swapped": ["add_to_swapped","#TIME", ".extract"],
        "add_from_dir": ["create", "#TIME", [AMBPATCHER, "add", MAIN_AMB, "files"]],
        "add_as": ["create", "#TIME", [AMBPATCHER, "add", MAIN_AMB, "files/1", "2"]],
        "create_nested": ["add", ".add_itself", ".add_itself", "#TIME", ".add_itself"],
        "add_into_nested": ["create_nested", "#TIME", [AMBPATCHER, "add", MAIN_AMB, "files/1", "test.amb/test.amb/3"]],
        "extract_nested": ["create_nested", "#TIME", ".extract"],
        "extract_all": ["create_nested", "#TIME", ".extract_all"],

        #This test covers cases when patching an original file by a folder that was
        #generated after extraction and internal files have directories.
        "edge_extracted": [[AMBPATCHER, "create", "sandbox/files.amb"],
                            [AMBPATCHER, "add", "sandbox/files.amb", "files/2", "abc/def/1"],
                            [AMBPATCHER, "add", "sandbox/files.amb", "files/3", "abc/def/2"],
                            [AMBPATCHER, "add", "sandbox/files.amb", "files/1", "abc/def/3"],
                            "#EDGE_EXTRACTED_PREP",
                            [AMBPATCHER, "sandbox/files.amb", "sandbox/files.amb_extracted"]],

        ".ml": [[AMBPATCHER]],
        ".ml_start": ["#COPYMODS", "#CWD:sandbox"],
        ".ml_end": ["#CWD:.."],

        "ml_empty": [".ml_start", "#TIME", "#MODSINI:", ".ml", ".ml_end"],
        "ml_single": [".ml_start", "#TIME", "#MODSINI:2", ".ml", ".ml_end"],
        "ml_multiple": [".ml_start", "#TIME", "#MODSINI:3421", ".ml", ".ml_end"],
        "ml_inversed": [".ml_start", "#TIME", "#MODSINI:1243", ".ml", ".ml_end"],
        "ml_recover": ["ml_multiple", "#CWD:sandbox", "#TIME", ".recover", ".ml_end"],
        "ml_changed_0": [".ml_start", "#TIME", "#MODSINI:1", ".ml", "#MODSINI:3", ".ml", ".ml_end"],
        "ml_changed_1": [".ml_start", "#TIME", "#MODSINI:2", ".ml", "#MODSINI:431", ".ml", ".ml_end"],
        "ml_changed_2": [".ml_start", "#TIME", "#MODSINI:4", ".ml", "#MODSINI:3214", ".ml", ".ml_end"],
        "ml_changed_3": [".ml_start", "#TIME", "#MODSINI:13", ".ml", "#MODSINI:24", ".ml", ".ml_end"],
        "ml_changed_4": [".ml_start", "#TIME", "#MODSINI:42", ".ml", "#MODSINI:1234", ".ml", ".ml_end"],
        "ml_changed_5": [".ml_start", "#TIME", "#MODSINI:423", ".ml", "#MODSINI:1", ".ml", ".ml_end"],
        "ml_changed_6": [".ml_start", "#TIME", "#MODSINI:321", ".ml", "#MODSINI:4132", ".ml", ".ml_end"],
    }
    
    if test_name == "":
        return tests
    return tests[test_name]
