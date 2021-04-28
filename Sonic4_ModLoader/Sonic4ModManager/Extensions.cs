using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Sonic4ModManager
{
    public static class Extensions
    {
        //Formats Rich Text Box
        //https://github.com/OSA413/Sonic4_ModLoader/blob/master/docs/Mod%20structure.md#description-formating
        public static void Format(this RichTextBox rtb)
        {
            rtb.ReadOnly = false;
            rtb.Text = rtb.Text.Replace("\\n", "\n"); //Newline character
            rtb.Text = rtb.Text.Replace("\\t", "\t"); //Tab character
            rtb.Text = rtb.Text.Replace("\n* ", "\n • "); //Bullet character at the biginning of a line

            //Description from mod.ini
            foreach (string i in new string[] { "b", "i", "u", "strike" })
            {
                while (rtb.Text.Contains("[" + i + "]"))
                {
                    //Getting the list of all [i] and [\i]
                    int ind = 0;

                    List<int> end_lst = new List<int> { };
                    while (rtb.Text.Substring(ind).Contains("[\\" + i + "]"))
                    {
                        end_lst.Add(rtb.Text.Substring(ind).IndexOf("[\\" + i + "]") + ind);
                        ind = end_lst[end_lst.Count - 1] + 1;
                    }

                    int start_ind = rtb.Text.IndexOf("[" + i + "]");

                    //Formating the original text
                    if (end_lst.Count == 0)
                        end_lst.Add(rtb.Text.Length);
                    foreach (int j in end_lst)
                    {
                        if (j > start_ind)
                        {
                            for (int k = 0; k < j - start_ind; k++)
                            {
                                rtb.Select(start_ind + k, 1);

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
                            rtb.Select(start_ind, 2 + i.Length);
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
                int ind_l = rtb.Text.Contains("[l]") ? rtb.Text.IndexOf("[l]") : rtb.Text.Length;
                int ind_c = rtb.Text.Contains("[c]") ? rtb.Text.IndexOf("[c]") : rtb.Text.Length;
                int ind_r = rtb.Text.Contains("[r]") ? rtb.Text.IndexOf("[r]") : rtb.Text.Length;

                //Nearest tag
                int ind = Math.Min(Math.Min(ind_l, ind_c), ind_r);
                string tag = rtb.Text[ind + 1].ToString();

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

        //Moves a line with [index] in ListView to [insert_to] index
        public static void MoveItem(this ListView lv, int index, int insert_to)
        {
            var item = lv.Items[index];
            lv.Items.RemoveAt(index);
            if (insert_to >= lv.Items.Count)
                lv.Items.Add(item);
            else
                lv.Items.Insert(insert_to, item);
        }
    }
}
