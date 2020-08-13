def get_files(test_name, MAIN_AMB=""):
    tests = {
        "swap_endianness_x2": "add",
        "add_to_swapped": "swap_endianness",
        "extract": ["#ALL", "#EXCEPT:"+MAIN_AMB],
        "extract_swapped": "extract",
        "add_from_dir": "add",

        "ml_empty": ["#ALL", "#EXCEPT:mods"],
        "ml_single": ["#ALL", "#EXCEPT:mods"]
    }

    if test_name in tests:
        return tests[test_name]
    else:
        return ["#ALL"]
        