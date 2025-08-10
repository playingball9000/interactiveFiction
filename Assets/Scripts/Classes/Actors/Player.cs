using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Player : IActor
{
    public string playerName { get; set; }
    public string description { get; set; }

    public PlayerStats stats { get; } = new PlayerStats();

    public ILocation currentLocation;

    public Room currentRoom => currentLocation as Room;
    public Area currentArea => currentLocation as Area;

    public Inventory inventory = new();
    public List<IWearable> equipment { get; set; } = new();
    public int money { get; set; }

    public Memory playerMemory { get; set; } = new Memory();
    public Dictionary<string, Relationship> relationships { get; set; } = new Dictionary<string, Relationship>();

    public List<Fact> GetPlayerFacts()
    {
        List<Fact> playerFacts = new() {
            new Fact { key = RuleConstants.KEY_CURRENT_ROOM, value = currentRoom?.internalCode ?? "None"},
            new Fact { key = RuleConstants.KEY_CURRENT_AREA, value = currentArea?.internalCode ?? "None"}
            };

        playerFacts.AddRange(inventory.contents.Select(item => new Fact { key = RuleConstants.KEY_IN_INVENTORY, value = item.internalCode }).ToList());
        playerFacts.AddRange(playerMemory.GetMemoryFacts());
        playerFacts.AddRange(relationships.Select(kvp => new Fact { key = "relationship_" + kvp.Key, value = kvp.Value.points }).ToList());
        playerFacts.AddRange(equipment.Select(e => new Fact { key = RuleConstants.KEY_PLAYER_EQUIPMENT, value = e.internalCode }).ToList());
        playerFacts.AddRange(GetActiveAbilities().Select(a => new Fact { key = RuleConstants.KEY_PLAYER_ABILITIES, value = a.internalCode }).ToList());
        return playerFacts;
    }

    public string GetRelationshipsToString()
    {
        var relationStrings = relationships
            .Select(kvp => $"{kvp.Key}: {kvp.Value.points}")
            .ToList();
        return StringUtil.CreateBulletedListString("=== Relationship Status ===", relationStrings);
    }

    public string GetEquipmentToString()
    {
        var equipmentList = Enum.GetValues(typeof(EquipmentSlot)).Cast<EquipmentSlot>()
            .Select(slot =>
            {
                var gear = equipment.FirstOrDefault(e => e.slotsTaken.Contains(slot));
                string gearString = gear != null ? $"{gear.GetDisplayName()} ({StringUtil.CreateCommaSeparatedString(gear.attributes)})" : "empty";
                string fullGearString = $"[{slot}] {gearString}";
                return fullGearString;
            });
        return StringUtil.CreateBulletedListString("=== Equipment ===", equipmentList);
    }

    public List<Attribute> GetActiveAbilities()
    {
        return equipment.SelectMany(e => e.attributes).Where(e => e.type == AttributeType.Ability).ToList();
    }

    public List<IItem> GetInventory()
    {
        return inventory.contents;
    }

    public void AddToInventory(IItem item)
    {
        inventory.contents.Add(item);
    }

    public void AddToInventory(List<IItem> items)
    {
        inventory.contents.AddRange(items);
    }

    public void RemoveFromInventory(IItem item)
    {
        inventory.contents.Remove(item);
    }

    public void RemoveFromInventory(List<IItem> itemsToRemove)
    {
        inventory.contents.RemoveAll(item => itemsToRemove.Contains(item));
    }

    public string GetInventoryString()
    {
        return inventory.ContentsToString();
    }

    public override string ToString()
    {
        string toString =
            $"<b><color=#8B4513>[Player]</color></b>\n" +
            $"  • Location: {currentLocation.displayName}\n" +
            $"  • Inventory: {GetInventoryString()}\n" +
            $"  • Stats: {stats}\n";

        return toString;
    }
}
