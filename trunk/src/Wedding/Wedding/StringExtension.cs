using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public static class StringExtension
{
    public static string ToTitleCase(this string str)
    {
        if (String.IsNullOrEmpty(str))
            return str;

        var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
        return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
    }
}
