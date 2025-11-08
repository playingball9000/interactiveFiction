using System.Collections.Generic;
public class BodyPartsSynonyms
{
    public static readonly Dictionary<string, BodyPart> SynonymsDict = new()
    {
        { "feet", BodyPart.Feet },
        { "toes", BodyPart.Feet },
        { "arches", BodyPart.Feet },
        { "soles", BodyPart.Feet },
        { "knee", BodyPart.Knees },
        { "knees", BodyPart.Knees },
        { "kneecaps", BodyPart.Knees },
        { "thighs", BodyPart.Thighs },
        { "sides", BodyPart.Sides },
        { "waist", BodyPart.Sides },
        { "torso", BodyPart.Sides },
        { "stomach", BodyPart.Stomach },
        { "belly", BodyPart.Stomach },
        { "tummy", BodyPart.Stomach },
        { "midsection", BodyPart.Stomach },
        { "ribs", BodyPart.Ribs },
        { "ribcage", BodyPart.Ribs },
        { "underarm", BodyPart.Underarms },
        { "underarms", BodyPart.Underarms },
        { "armpit", BodyPart.Underarms },
        { "armpits", BodyPart.Underarms },
        { "pits", BodyPart.Underarms },
        { "neck", BodyPart.Neck },
        { "nape", BodyPart.Neck },
        { "throat", BodyPart.Neck },
    };

    public static BodyPart Get(string name)
    {
        return SynonymsDict.TryGetValue(name, out var part) ? part : BodyPart.Unknown;
    }
}
