using System;
using System.Collections.Generic;
using System.Linq;

namespace CISM_PJ.Helpers
{
    public static class ExtensionMethods
    {
        public static string TrimIfNotNull(this string value)
        {
            if (value != null)
            {
                return value.Trim();
            }
            return null;
        }

        public static Uri Append(this Uri uri, params string[] paths)
        {
            return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));
        }

        public static T KeyByValue<T, W>(this Dictionary<T, W> dict, W val)
        {
            T key = default;
            foreach (KeyValuePair<T, W> pair in dict)
            {
                if (EqualityComparer<W>.Default.Equals(pair.Value, val))
                {
                    key = pair.Key;
                    break;
                }
            }
            return key;
        }
    }
}