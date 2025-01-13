using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Player
{
    public string playerName { get; set; }
    public string description { get; set; }
    public Room currentLocation { get; set; }
    public List<IItem> inventory = new List<IItem>();

    public void AddToInventory(IItem item)
    {
        StoryTextHandler.invokeUpdateTextDisplay("You pick up " + item.referenceName);

        inventory.Add(item);
    }

    public void RemoveFromInventory(IItem item)
    {
        if (inventory.Contains(item))
        {
            StoryTextHandler.invokeUpdateTextDisplay("You drop " + item.referenceName);
            inventory.Remove(item);
        }
    }

    public string GetInventoryString()
    {
        if (inventory.Any() == true)
        {
            List<string> itemNames = inventory.Select(item => item.referenceName).ToList();
            return "Inventory:\n - " + string.Join(" - ", itemNames) + "\n";
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
