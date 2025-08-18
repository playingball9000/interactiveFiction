using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Grace : NPC
{
    public string dialogueFile { get; set; }
    public string internalCode { get; set; }
    public ILocation currentLocation { get; set; }
    public List<IWearable> clothes { get; set; } = new List<IWearable>();
    public string displayName { get; set; }
    public string description { get; set; }
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
}
