# AMB File Structure

. | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | A | B | C | D | E | F
-- | - | - | - | - | - | - | - | - | - | - | - | - | - | - | - | -
0x00 | 0x23 | 0x41 | 0x4D | 0x42 | 0x20 | 0x00 | 0x00 | 0x00 | 0x00 | 0x00 | 0x04 | 0x00 | 0x00 | 0x00 | 0x00 | 0x00
0x10 | [FN] | [FN] | [FN] | [FN] | [LP] | [LP] | [LP] | [LP] | [DP] | [DP] | [DP] | [DP] | [NP] | [NP] | [NP] | [NP]
[LP] | [FP] | [FP] | [FP] | [FP] | [FL] | [FL] | [FL] | [FL] | 0xFF | 0xFF | 0xFF | 0xFF | 0x00 | 0x00 | 0x00 | 0x00

//TODO: Add more information and try to understand different AMB structure (e.g. \EP 1\G_COM\PLY\SON_MTN.AMB)

## Header

[FN] (File Number) - Number of files in the archive [backwards] (may differ from the actual number of files)

[LP] (List Pointer) - Pointer to where enumeration of the files pointers and lengths starts [backwards]

[DP] (Data Pointer) - Pointer to where enumeration of the files data starts [backwards]

[NP] (Name Pointer) - Pointer to where enumeration of the files names starts [backwards]

## Data

[FP] (File Pointer) - Pointer to where file data starts [backwards]

[FL] (File Length) - Pointer to the length of the file [backwards]
