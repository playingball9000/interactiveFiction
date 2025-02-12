/*
Ability - like immune to something or flying. Value Doesn't apply here.
Buff - like elemental resistance
Stat - like Strength + 10
*/
public enum AttributeType
{
    Ability,
    Buff,
    Stat
}

[System.Serializable]
public class Attribute
{
    public string displayName { get; set; }
    public string internalCode { get; set; }
    public AttributeType type { get; set; }
    public int? value { get; set; }

    public override string ToString()
    {
        return value != null ? $"{displayName} : {value}" : displayName;
    }
}
