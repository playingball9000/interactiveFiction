using System.Collections.Generic;
using System.Linq;

//TODO: Figure out what to do here... for gifts, rules? strategy pattern?
[System.Serializable]
public class Kate : NPC
{
    public string dialogueFile { get; set; }
    public string internalCode { get; set; }
    public Room currentLocation { get; set; }
    public List<IWearable> clothes { get; set; } = new List<IWearable>();
    public string referenceName { get; set; }
    public string description { get; set; }
    public string adjective { get; set; } = "";

    public Memory memory { get; set; } = new();

    public string GetDescription()
    {
        string fullDescription = $@"{description}";

        if (clothes.Any())
        {
            string clothingListString = string.Join(", ", clothes.Select(clothingItem => $"{clothingItem.referenceName}"));
            fullDescription = fullDescription + $"{referenceName} is wearing {clothingListString}";
        }

        return fullDescription;
    }

    public bool GetGiveReaction(IItem giftedItem)
    {
        if (giftedItem.internalCode == "item_old_book")
        {
            StoryTextHandler.invokeUpdateStoryDisplay("The woman frowns. 'Thank you, but no thanks.' She puts it on the ground.");
            currentLocation.AddItem(giftedItem);
            return true;
        }
        else if (giftedItem.internalCode == "item_chocolate_bar")
        {
            StoryTextHandler.invokeUpdateStoryDisplay("'Oh I love chocolate!' the woman exclaims. (+100)");
            WorldState.GetInstance().player.relationships[internalCode].points += 100;
            return true;
        }

        return false;
    }

    public string GetDisplayName()
    {
        return string.IsNullOrEmpty(adjective) ? referenceName : adjective + " " + referenceName;
    }

    public bool GetTickleReaction(string part)
    {
        return false;
    }
}
