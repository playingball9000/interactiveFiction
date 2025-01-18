using System.Collections.Generic;
using System.Linq;

//TODO: add a function to get all examinable stuff in the room

[System.Serializable]
public class Room
{
    public string description;
    public string roomName;

    public List<Exit> exits = new List<Exit>();
    public List<NPC> npcs = new List<NPC>();
    public List<IItem> roomItems = new List<IItem>();
    public List<IExaminable> roomScenery = new List<IExaminable>();

    public List<string> GetRoomInteractionDescriptions()
    {
        List<string> interactionDescriptionsInRoom = new List<string>();


        if (npcs?.Any() == true)
        {
            List<string> npcNames = npcs.Select(npc => npc.referenceName).ToList();
            interactionDescriptionsInRoom.Add("The following people are here: " + string.Join(", ", npcNames) + "\n");
        }

        if (roomItems?.Any() == true)
        {
            List<string> itemNames = roomItems.Select(item => item.referenceName).ToList();
            interactionDescriptionsInRoom.Add("Items in room: " + string.Join(", ", itemNames) + "\n");
        }

        foreach (Exit exit in exits)
        {
            interactionDescriptionsInRoom.Add(exit.exitDescription);
        }
        return interactionDescriptionsInRoom;
    }

    public List<IExaminable> GetExaminableThings()
    {
        List<IExaminable> examinableThings = npcs.ToList<IExaminable>();
        examinableThings.AddRange(roomItems.ToList<IExaminable>());
        examinableThings.AddRange(roomScenery);

        examinableThings.AddRange(roomItems.OfType<IContainer>()
                                    .Where(container => container.isOpen)
                                    .SelectMany(openContainer => openContainer.contents)
                                    .ToList());

        return examinableThings;
    }

    public void RemoveItem(IItem item)
    {
        if (roomItems.Contains(item))
        {
            roomItems.Remove(item);
        }
    }

    public void AddItem(IItem item)
    {
        roomItems.Add(item);
    }

    public override string ToString()
    {
        string npcNames = StringUtil.GetStringFromList(npcs.Select(npc => npc.referenceName).ToList());
        string itemNames = StringUtil.GetStringFromList(roomItems.Select(item => item.referenceName).ToList());
        string exitsPaths = StringUtil.GetStringFromList(exits.Select(ex => ex.ToString()).ToList());

        string toString = $@"
            roomName: {roomName}
            description: {description}
            npcNames: {npcNames}
            itemNames: {itemNames}
            exitsPaths: {exitsPaths}
        ";

        return toString;
    }
}
