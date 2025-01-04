using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Adventure/Room")]
public class Room : ScriptableObject
{
    [TextArea(15, 20)]
    public string description;
    public string roomName;

    public List<Exit> exits;
    public List<NPC> npcs = new List<NPC>();
    public List<IItem> roomItems = new List<IItem>();

    public Dictionary<string, IExaminable> examinableItems { get; private set; }

    public void populateExaminableItems(string item)
    {
        npcs.ForEach(npc => examinableItems.Add(npc.referenceName, npc));
    }

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
}
