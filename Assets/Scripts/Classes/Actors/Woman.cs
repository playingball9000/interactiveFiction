using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Woman : NPC
{
    public string dialogueFile { get; set; }
    public string internalCode { get; set; }
    public Room currentLocation { get; set; }
    public List<IClothing> clothes { get; set; } = new List<IClothing>();
    public string referenceName { get; set; }
    public string description { get; set; }
    public string adjective { get; set; } = "";

    public Memory memory { get; set; }

    public string GetDescription()
    {

        string fullDescription = $@"{description}";

        if (clothes.Any() == true)
        {
            string clothingListString = string.Join(", ", clothes.Select(clothingItem => $"{clothingItem.color} {clothingItem.referenceName}"));
            fullDescription = fullDescription + $"{referenceName} is wearing {clothingListString}";
        }

        return fullDescription;
    }

    public bool GetGiveReaction(IItem giftedItem)
    {
        if (giftedItem.referenceName == "book")
        {
            StoryTextHandler.invokeUpdateStoryDisplay("The woman frowns. 'Thank you, but no thanks.' and puts it on the ground.");
            currentLocation.AddItem(giftedItem);
            return true;
        }
        else if (giftedItem.referenceName == "bar")
        {
            StoryTextHandler.invokeUpdateStoryDisplay("'Oh I love chocolate!' the woman exclaims. (+100)");
            WorldState.GetInstance().player.relationships[referenceName].points += 100;
            return true;
        }

        return false;
    }

    public string GetDisplayName()
    {
        return string.IsNullOrEmpty(adjective) ? referenceName : adjective + " " + referenceName;
    }
}
