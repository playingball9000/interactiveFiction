public static class SimpleNPCExtensions
{
    public static SimpleNPC AddCurrentLocation(this SimpleNPC simpleNPC, LocationCode location)
    {
        simpleNPC.currentLocation = location;
        RoomRegistry.GetRoom(location);
        return simpleNPC;
    }

    public static SimpleNPC AddBasicDialogue(this SimpleNPC simpleNPC, string dialogue)
    {
        simpleNPC.BasicDialogue = dialogue;
        return simpleNPC;
    }

    public static SimpleNPC AddDisplayName(this SimpleNPC simpleNPC, string name)
    {
        simpleNPC.displayName = name;
        return simpleNPC;
    }

    public static SimpleNPC AddDescription(this SimpleNPC simpleNPC, string desc)
    {
        simpleNPC.description = desc;
        return simpleNPC;
    }

    public static SimpleNPC AddAdjective(this SimpleNPC simpleNPC, string adj)
    {
        simpleNPC.adjective = adj;
        return simpleNPC;
    }
}
