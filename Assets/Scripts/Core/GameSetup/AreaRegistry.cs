using System.Collections.Generic;
using System.Linq;

public static class AreaRegistry
{
    public static Dictionary<string, Area> Areas = new();
    private static bool initialized = false;

    public static void Initialize()
    {
        if (initialized) return;
        initialized = true;

        var abyss = new Area("Abyss", "abyss_area");
        abyss.AddCard(CardRegistry.GetCard("card1"));
        abyss.AddCard(CardRegistry.GetCard("card2"));
        abyss.AddCard(CardRegistry.GetCard("card3"));
        abyss.AddCard(CardRegistry.GetCard("card4"));

        Areas[abyss.internalCode] = abyss;
    }

    public static Area GetArea(string internalCode) => Areas[internalCode];
    public static List<Area> GetAllAreas() => Areas.Values.ToList();

    public static new string ToString()
    {
        return $@"
        AreaRegistry:
        - {GetAllAreas()}
        ";
    }
}
