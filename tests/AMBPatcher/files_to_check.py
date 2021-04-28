def get_files(test_name, MAIN_AMB=""):
    tests = {
        "swap_endianness_x2": "add",
        "add_to_swapped": "swap_endianness",
        "extract": ["#ALL", "#EXCEPT:"+MAIN_AMB],
        "extract_swapped": "extract",
        "add_from_dir": "add",

        "edge_extracted": ["#ALL", "#EXCEPT:files.amb_extracted*"],

        "ml_empty": ["#ALL", "#EXCEPT:mods*"],
        "ml_single": ["#ALL", "#EXCEPT:mods*"],
        "ml_multiple": ["#ALL", "#EXCEPT:mods*"],
        "ml_inversed": ["#ALL", "#EXCEPT:mods*"],
        "ml_recover": ["#ALL", "#EXCEPT:mods*"],
        "ml_changed_0": ["#ALL", "#EXCEPT:mods*"],
        "ml_changed_1": ["#ALL", "#EXCEPT:mods*"],
        "ml_changed_2": ["#ALL", "#EXCEPT:mods*"],
        "ml_changed_3": ["#ALL", "#EXCEPT:mods*"],
        "ml_changed_4": ["#ALL", "#EXCEPT:mods*"],
        "ml_changed_5": ["#ALL", "#EXCEPT:mods*"],
        "ml_changed_6": ["#ALL", "#EXCEPT:mods*"],
    }

    if test_name in tests:
        return tests[test_name]
    else:
        return ["#ALL"]
        