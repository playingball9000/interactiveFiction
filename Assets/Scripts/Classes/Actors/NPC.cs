using System.Collections.Generic;
using System.Linq;

public class NPC : IExaminable
{
    public string referenceName { get; set; }
    public string description;
    public Room currentLocation;
    public List<IClothing> clothes = new List<IClothing>();

    public string GetDescription()
    {
        string clothingListString = string.Join(", ", clothes.Select(clothingItem => $"{clothingItem.color} {clothingItem.referenceName}"));

        string fullDescription = $@"{this.description}";

        if (clothingListString.Length > 0)
        {
            fullDescription = fullDescription + $"{referenceName} is wearing {clothingListString}";
        }


        return fullDescription;
    }
}
