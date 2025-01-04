using System.Collections.Generic;
using System.Linq;

public class NPC : IExaminable
{
    public string referenceName { get; set; }
    public string description { get; set; }
    public Room currentLocation;
    public List<IClothing> clothes = new List<IClothing>();

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
}
