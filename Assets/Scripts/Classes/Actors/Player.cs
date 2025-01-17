using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Player
{
    public string playerName { get; set; }
    public string description { get; set; }
    public Room currentLocation { get; set; }
    public List<IItem> inventory = new List<IItem>();
    List<IClothing> clothes { get; set; }

    public void AddToInventory(IItem item)
    {
        inventory.Add(item);
    }

    public void RemoveFromInventory(IItem item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
        }
    }

    public string GetInventoryString()
    {
        if (inventory.Any() == true)
        {
            List<string> itemNames = inventory.Select(item => item.referenceName).ToList();
            return "Inventory:\n - " + string.Join("\n - ", itemNames);
        }
        else
        {
            return "";
        }
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
