# Mod structure
---------------------------

## File placement

Everything starts with a mod folder. In this example it'll be called `My Cool Mod`.

You'll have to place all files there keeping the original game file structure.

If you changed something in a file, place the **modified** files in a folder:
* `.AMB` - Name the folder as the file.
* `.CSB` - Name the folder as the file but without the extension.

Here's an example of mod structure:
```
/mods/My Cool Mod/mod.ini
/mods/My Cool Mod/description.txt
/mods/My Cool Mod/G_COM/CPIT/CPIT_MAIN.AMB/G_FIX.AMB/USER_FONT00.DDS
/mods/My Cool Mod/SOUND/SONICDL_SNG01/Synth/materials/SNG_EMERALD_AIF.aax/Intro.adx
```

Note: AMBPatcher can do recursive patching, so there's no need to manually patch sub-archives.

## mod.ini structure

`mod.ini` file is not necessary, but recommended if you want to share your mod.

Place `mod.ini` into the root directory of your mod.
`/mods/My Cool Mod/mod.ini`

Here's an example of mod.ini structure:
```
Name=My Cool Mod
Authors=Author0, Author1, and Author2
Version=0.1.0.0-rc3
Description=This is my [b][i]cooool[\i][\b] mod!
```

Missing parameters:
* If `Name` is missed, folder name will be used instead.
* If `Description` is missed, `No description.` will be shown.
* Any other missing parameter will be replaced with `???`

Don't want to write the whole description in the `mod.ini` file? **Make a link to that file!**

Simply write
```
Description=file=description.txt
```
where `description.txt` is relative path (from `/mods/My Cool Mod/`) to your description file (only `.txt` files are supported).

### Description formating

You can make your description look better with formatting.

`\n` will be formatted into a newline character.
`\t` will be formatted into a tab character.

Mod Manager supports those tags:

* `[b]Bold[\b]` - **Bold**
* `[i]Italic[\i]` - *Italic*
* `[u]Underlined[\u]` - Underlined (Markdown doesn't support underlined text, sorry)
* `[strike]Stricken out[\strike]` - ~~Stricken out~~

If you add only the first tag (e.g. [u] or [i]), the format will be applied to the rest of the **text**. You also can mix them (but don't overdo!).

You can align the lines of the text:

* `[l]` - Left (default)
* `[c]` - Center
* `[r]` - Right

Add only one of those tags per line. Of course you can put as many tags as you want anywhere, but only the last tag/alignment on a line will be applied.

Also URL links to web-sites can be highlighted (they are **clickable!**):
* example.com - Doesn't work
* www.example.com - OK
* https://example.com - OK
* https://www.example.com - OK


## Examples

You can get some examples of mod structure from here: https://github.com/OSA413/Sonic4_ModLoader_examples
