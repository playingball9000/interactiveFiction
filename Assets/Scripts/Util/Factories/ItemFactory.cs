
public static class ItemFactory
{
    //TODO: When I get a game item organizer class, this will add it there too
    public static ItemBase CreateItem(string referenceName, string adjective, string description)
    {
        return new ItemBase
        {
            referenceName = referenceName,
            adjective = adjective,
            internalCode = "item_" + adjective.Replace(" ", "_") + "_" + referenceName.Replace(" ", "_"),
            description = description
        };
    }
}
