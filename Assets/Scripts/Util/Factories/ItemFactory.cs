
public static class ItemFactory
{
    //TODO: When I get a game item organizer class, this will add it there too
    public static ItemBase CreateItem(string name, string adjective, string description)
    {
        return new ItemBase
        {
            displayName = name,
            adjective = adjective,
            internalCode = "item_" + adjective.Replace(" ", "_") + "_" + name.Replace(" ", "_"),
            description = description
        };
    }

    public static TickleToolBase CreateTickleItem(string name, string adjective, string description)
    {
        return new TickleToolBase
        {
            displayName = name,
            adjective = adjective,
            internalCode = "item_" + adjective.Replace(" ", "_") + "_" + name.Replace(" ", "_"),
            description = description
        };
    }
}
