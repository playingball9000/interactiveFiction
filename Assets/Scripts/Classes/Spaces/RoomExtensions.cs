public static class RoomExtensions
{

    // Room manages NPC location, but player manages its own
    public static Room AddNPC(this Room room, NPC npc)
    {
        npc.currentLocation = room;
        room.npcs.Add(npc);
        return room;
    }

    public static Room RemoveNPC(this Room room, NPC npc)
    {
        npc.currentLocation = null;
        room.npcs.Remove(npc);
        return room;
    }

    public static void RemoveItem(this Room room, IItem item)
    {
        room.roomItems.contents.Remove(item);
    }

    public static void AddItem(this Room room, IItem item)
    {
        room.roomItems.contents.Add(item);
    }

    public static Room AddScenery(this Room room, string name, string description)
    {
        room.roomScenery.Add(new Scenery
        {
            displayName = name,
            description = description
        });
        return room;
    }

    public static Room AddScenery(this Room room, string name, string adjective, string description)
    {
        room.roomScenery.Add(new Scenery
        {
            displayName = name,
            adjective = adjective,
            description = description
        });
        return room;
    }
}