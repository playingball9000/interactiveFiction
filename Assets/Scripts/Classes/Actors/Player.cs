using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Player
{
    public string playerName { get; set; }
    public string description { get; set; }
    public Room currentLocation { get; set; }
    public List<IItem> inventory { get; private set; }

    public Player(string name, string description, Room currentLocation)
    {
        this.playerName = name;
        this.description = description;
        this.currentLocation = currentLocation;
        inventory = new List<IItem>();
    }


    public void AddToInventory(IItem item)
    {
        DisplayTextHandler.invokeUpdateTextDisplay("You pick up " + item.referenceName);

        inventory.Add(item);
    }

    public void RemoveFromInventory(IItem item)
    {
        if (inventory.Contains(item))
        {
            DisplayTextHandler.invokeUpdateTextDisplay("You drop " + item.referenceName);
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
