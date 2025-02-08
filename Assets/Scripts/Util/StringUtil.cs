using System;
using System.Collections.Generic;

public static class StringUtil
{
    public static string CreateCommaSeparatedString<T>(IEnumerable<T> list)
    {
        return CreateDelimSeparatedString(list, ", ");
    }

    public static string CreateOrSeparatedString<T>(IEnumerable<T> list)
    {
        return CreateDelimSeparatedString(list, " or ");
    }


    public static string CreateBulletedListString<T>(string title, IEnumerable<T> list)
    {
        string bulletedList = $"{title}\n - {string.Join("\n - ", list)}";
        return bulletedList;
    }

    public static string CreateDelimSeparatedString<T>(IEnumerable<T> list, string delimiter)
    {
        return string.Join(delimiter, list);
    }

    public static bool EqualsIgnoreCase(string one, string two)
    {
        return one.Equals(two, StringComparison.OrdinalIgnoreCase);
    }
}
