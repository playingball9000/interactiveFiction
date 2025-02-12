using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Inventory : IStorage
{
    public List<IItem> contents { get; set; } = new List<IItem>();

    public List<IItem> GetContents()
    {
        return contents;
    }

    public string ContentsToString()
    {
        if (contents.Any())
        {
            List<string> itemNames = contents.Select(item => item.GetDisplayName()).ToList();
            return StringUtil.CreateBulletedListString("=== Inventory ===", itemNames);
        }
        else
        {
            return "Your inventory is empty";
        }
    }

    public bool isAccessible()
    {
        return true;
    }

    public void RemoveItem(IItem item)
    {
        contents.Remove(item);
    }
}
