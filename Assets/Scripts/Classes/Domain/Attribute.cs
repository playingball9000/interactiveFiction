public enum AttributeType
{
    Ability,
    Buff,
    Stat
}

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
