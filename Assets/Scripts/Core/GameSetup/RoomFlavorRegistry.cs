using System;
using System.Collections.Generic;
using System.Linq;

public static class RoomFlavorRegistry
{
    private static Dictionary<string, string> roomFlavorText = new();

    public static void Register(string code, string flavor)
    {
        roomFlavorText[code] = flavor;
    }

    public static string GetFlavorText(string code)
    {
        return roomFlavorText.TryGetValue(code, out var flavor) ? flavor : null;
    }

    public static List<string> GetAllFlavorText()
    {
        return roomFlavorText.Values.ToList();
    }

    public static Dictionary<string, string> GetDict()
    {
        return roomFlavorText;
    }

    public static void Load(Dictionary<string, string> r)
    {
        roomFlavorText = r;
    }

    public static void ClearRegistry()
    {
        roomFlavorText.Clear();
    }
}
