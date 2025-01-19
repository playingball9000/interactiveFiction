using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Woman : NPC
{
    public string dialogueFile { get; set; }
    public Room currentLocation { get; set; }
    public List<IClothing> clothes { get; set; } = new List<IClothing>();
    public string referenceName { get; set; }
    public string description { get; set; }

    public string GetDescription()
    {

        string fullDescription = $@"{this.description}";

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
            StoryTextHandler.invokeUpdateStoryDisplay("Thank you, but no thanks.");
            this.currentLocation.AddItem(giftedItem);
            return true;
        }

        return false;
    }
}
