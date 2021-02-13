using System;

namespace AMBPatcher
{
    public static class ProgressBar
    {
        public static bool Enabled;

        public static void PrintProgress(int i, int max_i, string title)
        {
            if (!ProgressBar.Enabled ||
                Console.WindowWidth <= 0)
                return;

            int bar_len = 50;
            ProgressBar.MoveCursorUp();

            int cut = 0;
            if (title.Length > Console.WindowWidth - 1)
                cut = title.Length - Console.WindowWidth + 1;

            //What it is doing
            ProgressBar.ClearLine();
            if (i == max_i) Console.WriteLine("Done!");
            else Console.WriteLine(title.Substring(cut));

            //Percentage
            ProgressBar.ClearLine();
            Console.WriteLine("[" + new string('#', bar_len * i / max_i)
                                + new string(' ', bar_len - bar_len * i / max_i)
                                + "] (" + (i * 100 / max_i).ToString() + "%)");
        }

        public static void ClearLine()
        {
            if (Console.WindowWidth <= 0) return;
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.CursorLeft = 0;
        }

        public static void MoveCursorUp(int i = 2)
        {
            if (!ProgressBar.Enabled) return;
            Console.CursorTop -= Math.Min(i, Console.CursorTop);
        }

        public static void MoveCursorDown(int i = 2)
        {
            if (!ProgressBar.Enabled) return;
            Console.CursorTop += i;
        }

        public static void PrintFiller()
        {
            if (!ProgressBar.Enabled) return;
            Console.WriteLine("Doing absolutely nothing!"
                            + "\nProgress bar goes here"
                            + "\nSub-task!"
                            + "\nsub%");
        }
    }

}
