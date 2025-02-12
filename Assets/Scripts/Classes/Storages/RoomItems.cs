using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class RoomItems : IStorage
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
            return "Items in room: " + TmpTextTagger.Color(StringUtil.CreateCommaSeparatedString(itemNames), UiConstants.TEXT_COLOR_STORY_ITEM);
        }
        else
        {
            return "";
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
