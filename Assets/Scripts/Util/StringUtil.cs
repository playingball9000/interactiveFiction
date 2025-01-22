using System.Collections.Generic;

public static class StringUtil
{
    public static string CreateCommaSeparatedString(List<string> list)
    {
        return string.Join(", ", list);
    }
}
