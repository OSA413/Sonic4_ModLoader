using System;
using System.Collections.Generic;

namespace Common.ValueUpdater
{
    public static class ValueUpdater
    {
        public static void UpdateIfKeyPresent<T, V>(Dictionary<T, V> d, T key, ref V value)
        {
            if (d == null) return;
            if (d.ContainsKey(key))
                value = d[key];
        }

        public static void UpdateIfKeyPresent<T, V>(Dictionary<T, V> d, T key, ref string value)
        {
            if (d == null) return;
            if (d.ContainsKey(key))
                value = d[key].ToString();
        }

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

        public static void UpdateIfKeyPresent<T, V>(Dictionary<T, V> d, T key, Dictionary<T, V> target_dict)
        {
            if (d == null) return;
            if (d.ContainsKey(key))
                target_dict[key] = d[key];
        }
    }
}