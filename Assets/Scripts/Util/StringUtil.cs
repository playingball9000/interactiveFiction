using System.Collections.Generic;

public static class StringUtil
{
    public static string GetStringFromList(List<string> list)
    {
        return "[" + string.Join(", ", list) + "]";

    }
}
