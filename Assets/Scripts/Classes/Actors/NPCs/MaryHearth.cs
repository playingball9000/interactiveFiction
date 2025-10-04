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
    public string description { get; set; } = "The dark-haired woman wears her signature battle apron over a coat. She could be engaging a throng of beasts or pancakes, both would be fitting. The weapon [Solid Cast] is strapped to her side.";
    public string adjective { get; set; } = "";

    public Memory memory { get; set; } = new();
    public List<IExaminable> examinables { get; set; } = new();

    public MaryHearth()
    {
        examinables.Add(new ItemBase()
        {
            displayName = "apron",
            adjective = "battle",
            description = "A worn apron equally splattered with cooking oil and stains from the ruins."

        });
        examinables.Add(new ItemBase()
        {
            displayName = "cast",
            adjective = "solid",
            description = "It's a frying pan. Upon closer inpsection, it's just a cast iron frying pan...",
            aliases = new List<string> { "weapon", }
        });
    }

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
