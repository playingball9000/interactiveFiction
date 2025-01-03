using System.Collections.Generic;

public class Player
{
    public string playerName { get; set; }
    public string description { get; set; }
    public Room currentLocation { get; set; }
    public List<string> Inventory { get; private set; }

    public Player(string name, string description, Room currentLocation)
    {
        this.playerName = name;
        this.description = description;
        this.currentLocation = currentLocation;
        Inventory = new List<string>();
    }


    // Method to add an item to the inventory
    public void AddToInventory(string item)
    {
        Inventory.Add(item);
    }

    // Method to remove an item from the inventory
    public void RemoveFromInventory(string item)
    {
        if (Inventory.Contains(item))
        {
            Inventory.Remove(item);
        }
    }
}
