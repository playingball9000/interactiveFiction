using System;
using System.Collections.Generic;
using System.Linq;

public static class RoomIncidentRegistry
{
    private static Dictionary<string, string> roomIncidentDict = new();

    public static void Register(string code, string flavor)
    {
        roomIncidentDict.Add(code, flavor);
    }

    public static string GetFlavorText(string code)
    {
        return roomIncidentDict.TryGetValue(code, out var flavor) ? flavor : null;
    }

    public static List<string> GetAllFlavorText()
    {
        return roomIncidentDict.Values.ToList();
    }

    public static Dictionary<string, string> GetDict()
    {
        return roomIncidentDict;
    }

    public static void Load(Dictionary<string, string> r)
    {
        roomIncidentDict = r;
    }

    public static void ClearRegistry()
    {
        roomIncidentDict.Clear();
    }
}
