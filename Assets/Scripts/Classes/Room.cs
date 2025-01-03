using System.Collections.Generic;
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
}
