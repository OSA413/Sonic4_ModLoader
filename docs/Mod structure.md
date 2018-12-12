# Mod structure
---------------------------

## File placement

//TODO

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

`\n` will be formated into a newline character.

Don't want to write the whole description in the `mod.ini` file? **Make a link to that file!**

Simply write
```
Description=file=description.txt
```
where `description.txt` is relative path (from `/mods/My Cool Mod/`) to your description file (only `.txt` files are supported).

### Description formating

You can make your description look better with formatting.

Mod Manager supports those tags:

* `[b]Bold[\b]` - **Bold**
* `[i]Italic[\i]` - *Italic*
* `[u]Underlined[\u]` - Underlined (Markdown doesn't support underlined text, sorry :confused:)
* `[strike]Stricken out[\strike]` - ~~Stricken out~~

If you add only the first tag (e.g. [u] or [i]), the format will be applied to the rest of the text. You also can mix them (but don't overdo!).

You can align the lines of the text:

* `[l]` - Left (default)
* `[c]` - Center
* `[r]` - Right

Place only one tag at the beginning of a line.

Also URL links to web-sites can be highlighted (they are **clickable!**):
* example.com - Doesn't work
* www.example.com - OK
* https://example.com - OK
* https://www.example.com - OK


## Examples

You can get some examples of mod structure from here: https://github.com/OSA413/Sonic4_ModLoader/tree/master/docs/Mod%20structure%20examples

Those files are archives that would be extracted to the `mods` folder if they were real mods.