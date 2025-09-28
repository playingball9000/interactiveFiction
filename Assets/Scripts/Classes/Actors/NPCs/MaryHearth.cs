using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class MaryHearth : ComplexNPC
{
    public string dialogueFile { get; set; } = "maryDialogue";
    public NpcCode internalCode { get; set; } = NpcCode.Mary_Hearth;
    public LocationCode currentLocation { get; set; }
    public List<IWearable> clothes { get; set; } = new();
    public string displayName { get; set; } = "Mary";
    public string description { get; set; } = "The dark-haired woman wears her signature battle apron over a coat. She could be engaging a throng of breasts or pancakes, both would be fitting.";
    public string adjective { get; set; } = "";

    public Memory memory { get; set; } = new();

    public string GetDescription()
    {
        string fullDescription = $@"{description}";

        if (clothes.Any())
        {
            string clothingListString = string.Join(", ", clothes.Select(clothingItem => $"{clothingItem.displayName}"));
            fullDescription = fullDescription + $"{displayName} is wearing {clothingListString}";
        }

        return fullDescription;
    }
    public bool GetGiveReaction(IItem giftedItem)
    {
        return false;
    }
    public bool GetTickleReaction(string part)
    {
        return false;
    }
    public bool GetInsultReaction()
    {
        return false;
    }
}
