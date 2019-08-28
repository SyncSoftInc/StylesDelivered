using System;
using System.Text.RegularExpressions;

namespace SyncSoft.StylesDelivered
{
    public static class Utils
    {
        private const string _space = " ";

        public static string RemoveExtraSpaces(string str)
        {
            if (str.IsMissing()) return str;
            return Regex.Replace(str, @"\s{2,}", _space);
        }

        public static string RemoveSpecialCharacters(string str)
        {
            if (str.IsMissing()) return str;
            return Regex.Replace(str, @"[^\w\s\-]+", string.Empty);
        }

        public static string FormatAddress(string str)
        {
            if (str.IsMissing()) return string.Empty;

            str = str.Trim();
            str = RemoveSpecialCharacters(str);
            str = RemoveExtraSpaces(str);
            return str;
        }
    }
}
