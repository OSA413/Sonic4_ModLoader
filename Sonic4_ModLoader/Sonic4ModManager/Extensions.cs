using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Sonic4ModManager
{
    public static class Extensions
    {
        //https://github.com/OSA413/Sonic4_ModLoader/blob/master/docs/Mod%20structure.md#description-formating
        public static void Format(this RichTextBox rtb)
        {
            rtb.ReadOnly = false;
            rtb.Text = rtb.Text.Replace("\\n", "\n"); //Newline character
            rtb.Text = rtb.Text.Replace("\\t", "\t"); //Tab character
            rtb.Text = rtb.Text.Replace("\n* ", "\n • "); //Bullet character at the biginning of a line
            if (rtb.Text.StartsWith("* ")) rtb.Text = " • " + rtb.Text.Substring(2);

            foreach (var i in new [] { "b", "i", "u", "strike" })
            {
                while (rtb.Text.Contains("[" + i + "]"))
                {
                    //Getting the list of all [i] and [\i]
                    var ind = 0;

                    var endList = new List<int>();
                    while (rtb.Text.Substring(ind).Contains("[\\" + i + "]"))
                    {
                        endList.Add(rtb.Text.Substring(ind).IndexOf("[\\" + i + "]") + ind);
                        ind = endList[endList.Count - 1] + 1;
                    }

                    var startInd = rtb.Text.IndexOf("[" + i + "]");

                    //Formating the original text
                    if (endList.Count == 0)
                        endList.Add(rtb.Text.Length);
                    foreach (int j in endList)
                    {
                        if (j > startInd)
                        {
                            for (int k = 0; k < j - startInd; k++)
                            {
                                rtb.Select(startInd + k, 1);

                                FontStyle new_style = FontStyle.Regular;
                                switch (i)
                                {
                                    case "b": new_style = FontStyle.Bold; break;
                                    case "i": new_style = FontStyle.Italic; break;
                                    case "u": new_style = FontStyle.Underline; break;
                                    case "strike": new_style = FontStyle.Strikeout; break;
                                }

                                rtb.SelectionFont = new Font(rtb.SelectionFont, new_style | rtb.SelectionFont.Style);
                            }

                            rtb.Select(j, 3 + i.Length);
                            rtb.SelectedText = "";
                            rtb.Select(startInd, 2 + i.Length);
                            rtb.SelectedText = "";

                            break;
                        }
                    }
                }
            }

            //Text alignment
            while (rtb.Text.Contains("[l]") ||
                   rtb.Text.Contains("[c]") ||
                   rtb.Text.Contains("[r]"))
            {
                var lInd = rtb.Text.Contains("[l]") ? rtb.Text.IndexOf("[l]") : rtb.Text.Length;
                var cInd = rtb.Text.Contains("[c]") ? rtb.Text.IndexOf("[c]") : rtb.Text.Length;
                var rInd = rtb.Text.Contains("[r]") ? rtb.Text.IndexOf("[r]") : rtb.Text.Length;

                //Nearest tag
                var ind = Math.Min(Math.Min(lInd, cInd), rInd);
                var tag = rtb.Text[ind + 1].ToString();

                rtb.Select(ind, 3);

                switch (tag)
                {
                    case "c": rtb.SelectionAlignment = HorizontalAlignment.Center; break;
                    case "r": rtb.SelectionAlignment = HorizontalAlignment.Right; break;
                    default: rtb.SelectionAlignment = HorizontalAlignment.Left; break;
                }
                rtb.SelectedText = "";
            }
            rtb.ReadOnly = true;
        }

        public static void MoveItem(this ListView lv, int index, int insertTo)
        {
            if (index == insertTo) return;
            var item = lv.Items[index];
            lv.Items.RemoveAt(index);
            if (insertTo >= lv.Items.Count)
                lv.Items.Add(item);
            else
                lv.Items.Insert(insertTo, item);
        }
    }
}
