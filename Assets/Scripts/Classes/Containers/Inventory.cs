using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Inventory : IStorage
{
    public List<IItem> contents { get; set; } = new List<IItem>();

    public void AddItem(IItem item)
    {
        contents.Add(item);
    }

    public bool ContainsItem(IItem item)
    {
        return contents.Contains(item);
    }

    public List<IItem> GetContents()
    {
        return contents;
    }

    public string ContentsToString()
    {
        if (contents.Any() == true)
        {
            List<string> itemNames = contents.Select(item => item.GetDisplayName()).ToList();
            return "=== Inventory ===\n - " + string.Join("\n - ", itemNames);
        }
        else
        {
            return "Your inventory is empty";
        }
    }

    public void RemoveItem(IItem item)
    {
        if (contents.Contains(item))
        {
            contents.Remove(item);
        }
    }

    public bool isAccessible()
    {
        return true;
    }
}
