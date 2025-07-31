using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Sonic4ModManager
{
    public static class Extensions
    {
        public static bool IsEscaped(string text, int index)
        {
            if (index > 0 && text[index - 1] == '\\')
                return !IsEscaped(text, index - 1);
            return false;
        }

        public static bool Contains(this string s, string other, int index)
        {
            if (s.Length - index < other.Length) return false;
            for (int i = 0; i < other.Length; i++)
                if (s[index + i] != other[i])
                    return false;
            return true;
        }

        //https://github.com/OSA413/Sonic4_ModLoader/blob/main/docs/Mod%20structure.md#description-formating
        public static void Format(this RichTextBox rtb)
        {
            var markers = new List<String>();
            var markersFormatting = new [] {"b", "i", "u", "strike"};
            var markersAlignment = new [] {"l", "c", "r"};
            foreach (var marker in markersFormatting.Union(markersAlignment))
            {
                markers.Add("[" + marker + "]");
                if (markersFormatting.Contains(marker))
                    markers.Add("[\\" + marker + "]");
            }
            var specialChars = new Dictionary<string, string> {{"\\n", "\n"}, {"\\t", "\t"}, {"\n* ", "\n • "}};
            markers.AddRange(specialChars.Keys);

            rtb.ReadOnly = false;
            if (rtb.Text.StartsWith("* ")) rtb.Text = " • " + rtb.Text.Substring(2);

            var tokens = new List<(int index, string token)>();
            for (int i = 0; i < rtb.TextLength;)
            {
                var t = rtb.Text;
                var prevTokensCount = tokens.Count;
                foreach (var marker in markers)
                    if (t.Contains(marker, i) && (marker[0] != '\\' || marker[0] == '\\' && IsEscaped(t, i + 1)))
                    {
                        tokens.Add((i, marker));
                        break;
                    }
                
                if (tokens.Count != prevTokensCount)
                    i += tokens[tokens.Count - 1].token.Length;
                else
                    i++;
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i].token.Substring(1, tokens[i].token.Length - 2);
                if (markersFormatting.Contains(token))
                {
                    var endToken = "[\\" + tokens[i].token.Substring(1);
                    var endIndex = tokens.FindIndex(i + 1, x => x.token == endToken);
                    if (endIndex == -1) endIndex = rtb.TextLength; 

                    rtb.Select(tokens[i].index, tokens[endIndex].index + tokens[endIndex].token.Length - tokens[i].index);

                    FontStyle newStyle = FontStyle.Regular;
                    switch (token)
                    {
                        case "b": newStyle = FontStyle.Bold; break;
                        case "i": newStyle = FontStyle.Italic; break;
                        case "u": newStyle = FontStyle.Underline; break;
                        case "strike": newStyle = FontStyle.Strikeout; break;
                    }

                    rtb.SelectionFont = new Font(rtb.SelectionFont, newStyle | rtb.SelectionFont.Style);
                }

                else if (markersAlignment.Contains(token))
                {
                    rtb.Select(tokens[i].index, 3);

                    switch (token)
                    {
                        case "c": rtb.SelectionAlignment = HorizontalAlignment.Center; break;
                        case "r": rtb.SelectionAlignment = HorizontalAlignment.Right; break;
                        default: rtb.SelectionAlignment = HorizontalAlignment.Left; break;
                    }
                }
            }

            var offset = 0;
            foreach (var token in tokens)
            {
                var replacement = "";
                rtb.Select(token.index - offset, token.token.Length);
                if (specialChars.ContainsKey(token.token))
                    replacement = specialChars[token.token];
                rtb.SelectedText = replacement;
                offset += token.token.Length - replacement.Length;
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
