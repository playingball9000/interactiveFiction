using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Player
{
    public string playerName { get; set; }
    public string description { get; set; }
    public Room currentLocation { get; set; }
    public Inventory inventory = new();
    List<IClothing> clothes { get; set; }

    public Memory playerMemory { get; set; } = new Memory();
    public Dictionary<string, Relationship> Relationships { get; set; } = new Dictionary<string, Relationship>();

    public void AddToInventory(IItem item)
    {
        inventory.AddItem(item);
    }

    public void RemoveFromInventory(IItem item)
    {
        if (inventory.ContainsItem(item))
        {
            inventory.RemoveItem(item);
        }
    }

    public string GetInventoryString()
    {
        return inventory.ContentsToString();
    }

    public List<IItem> GetInventory()
    {
        return inventory.contents;
    }

    public List<Fact> GetPlayerFacts()
    {
        List<Fact> playerFacts = new();
        playerFacts.AddRange(inventory.contents.Select(item => new Fact { key = RuleConstants.KEY_IN_INVENTORY, value = item.referenceName }).ToList());
        playerFacts.AddRange(playerMemory.GetMemoryFacts());
        return playerFacts;
    }


    public string GetRelationshipsToString()
    {
        var formattedStrings = Relationships
            .Select(kvp => $"{kvp.Key}: {kvp.Value.points}")
            .ToList();
        return "=== Relationship Status ===\n - " + string.Join("\n - ", formattedStrings);
    }

    public override string ToString()
    {
        string toString = $@"
            playerName: {playerName}
            description: {description}
            currentLocation: {currentLocation.roomName}
            {GetInventoryString()}
";
        return toString;
    }
}
