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
    public Dictionary<string, IExaminable> examinableItems { get; private set; }

    public void populateExaminableItems(string item)
    {
        npcs.ForEach(npc => examinableItems.Add(npc.referenceName, npc));
    }

    public List<string> GetRoomInteractionDescriptions()
    {
        List<string> interactionDescriptionsInRoom = new List<string>();

        List<string> npcNames = npcs.Select(npc => npc.referenceName).ToList();

        if (npcNames?.Any() == true)
        {
            interactionDescriptionsInRoom.Add("The following people are here: " + string.Join(", ", npcNames) + "\n");
        }

        foreach (Exit exit in exits)
        {
            interactionDescriptionsInRoom.Add(exit.exitDescription);
        }
        return interactionDescriptionsInRoom;
    }
}
