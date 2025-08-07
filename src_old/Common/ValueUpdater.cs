using System;
using System.Collections.Generic;

namespace Common.ValueUpdater
{
    public static class ValueUpdater
    {
        public static void UpdateIfKeyPresent<T, V>(Dictionary<T, V> d, T key, ref int value)
        {
            if (d == null) return;
            if (d.ContainsKey(key))
                value = Convert.ToInt32(d[key]);
        }

        public static void UpdateIfKeyPresent<T, V>(Dictionary<T, V> d, T key, ref bool value)
        {
            if (d == null) return;
            if (d.ContainsKey(key))
                value = d[key].ToString() == "0" ? false : true;
        }
    }
}