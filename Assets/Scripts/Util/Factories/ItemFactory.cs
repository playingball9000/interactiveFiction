
public static class ItemFactory
{
    public static ItemBase CreateItem(string referenceName, string adjective, string description)
    {
        return new ItemBase
        {
            referenceName = referenceName,
            adjective = adjective,
            internalCode = "item_" + adjective.Replace(" ", "_") + referenceName.Replace(" ", "_"),
            description = description
        };
    }
}
