using System.Collections.Generic;
using System.Linq;

public static class AreaRegistry
{
    public static Dictionary<LocationCode, Area> areas = new();
    private static bool initialized = false;

    public static void Initialize()
    {
        if (initialized) return;
        initialized = true;

        var abyss = new Area("Abyss", LocationCode.Abyss_a);
        abyss.AddCard(CardRegistry.GetCard(CardCode.card1));
        abyss.AddCard(CardRegistry.GetCard(CardCode.card2));
        abyss.AddCard(CardRegistry.GetCard(CardCode.card3));
        abyss.AddCard(CardRegistry.GetCard(CardCode.card4));

        areas[abyss.internalCode] = abyss;
    }

    public static Area GetArea(LocationCode internalCode) => areas[internalCode];
    public static List<Area> GetAllAreas() => areas.Values.ToList();


    public static Dictionary<LocationCode, Area> GetDict()
    {
        return areas;
    }

    public static void Load(Dictionary<LocationCode, Area> a)
    {
        areas = a;
    }

    public static new string ToString()
    {
        return $@"
        AreaRegistry:
        - {GetAllAreas()}
        ";
    }
}
