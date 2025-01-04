using System.Collections.Generic;
using System.Linq;

public class NPC : IExaminable
{
    public string referenceName { get; set; }
    public string description;
    public Room currentLocation;
    public List<IClothing> clothes = new List<IClothing>();
    public List<BodyPart> bodyParts = new List<BodyPart>();

    public string GetDescription()
    {

        string fullDescription = $@"{this.description}" + "\n";

        if (clothes.Any() == true)
        {
            string clothingListString = string.Join(", ", clothes.Select(clothingItem => $"{clothingItem.color} {clothingItem.referenceName}"));
            fullDescription = fullDescription + $"{referenceName} is wearing {clothingListString}" + "\n";
        }


        return fullDescription;
    }
}
