def get_files(test_name, MAIN_AMB=""):
    tests = {
        "swap_endianness_x2": "add",
        "add_to_swapped": "swap_endianness",
        "extract": [MAIN_AMB + "_extracted/1",
                        MAIN_AMB + "_extracted/2",
                        MAIN_AMB + "_extracted/3"],
        "extract_swapped": "extract",
        "add_from_dir": "add",

        ".ml": ["#ALL", "#EXCEPT:mods"],
        "ml_empty": ".ml",
        "ml_single": ".ml"
    }

    if test_name in tests:
        return tests[test_name]
    else:
        return ["#ALL"]
        