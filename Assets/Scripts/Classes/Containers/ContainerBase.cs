using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ContainerBase : IStorage, IItem, IOpenable, IExaminable
{
    public string referenceName { get; set; }
    public string description { get; set; }
    public string adjective { get; set; } = "";
    public string internalCode { get; set; }
    public bool isOpen { get; set; }
    public bool isGettable { get; set; }
    // This is used for isGettable = false to display message
    public string pickUpNarration { get; set; }

    public List<IItem> contents { get; set; }

    public void AddItem(IItem item)
    {
        contents.Add(item);
    }

    public bool ContainsItem(IItem item)
    {
        return contents.Contains(item);
    }

    public string GetDescription()
    {
        string fullDescription = description;

        if (isOpen)
        {
            string itemNames = ContentsToString();
            fullDescription += $"\nIt is open. Contents: {itemNames}.\n";
        }
        else
        {
            fullDescription += "\nIt is closed.";
        }

        return fullDescription;
    }

    public List<IItem> GetContents()
    {
        return contents;
    }

    public string ContentsToString()
    {
        List<string> itemList = contents.Select(c => c.GetDisplayName()).ToList();
        return contents.Any() ? StringUtil.CreateCommaSeparatedString(itemList) : "empty";
    }

    public void RemoveItem(IItem item)
    {
        if (contents.Contains(item))
        {
            contents.Remove(item);
        }
    }

    public string GetDisplayName()
    {
        return string.IsNullOrEmpty(adjective) ? referenceName : adjective + " " + referenceName;
    }

    public bool isAccessible()
    {
        return isOpen;
    }

    public override string ToString()
    {
        string toString = $@"
            GetDisplayName: {GetDisplayName()}
            isAccessible: {isAccessible()}
            {ContentsToString()}
";
        return toString;
    }
}
