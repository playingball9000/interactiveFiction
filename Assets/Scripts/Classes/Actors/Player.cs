using System.Collections.Generic;

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
}
