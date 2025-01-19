[System.Serializable]
public class ItemBase : IItem
{
    public string referenceName { get; set; }
    public string description { get; set; }
    public bool isGettable { get; set; }

    // Backing field is needed otherwise C# auto props will get into an infinite loop
    private string pickUpNarrationBackingField;
    public string pickUpNarration { get => GetPickUpNarration(); set => pickUpNarrationBackingField = value; }

    public string GetPickUpNarration()
    {
        return pickUpNarrationBackingField ?? $"you pick up the {referenceName}";
    }

    public string GetDescription()
    {
        return description;
    }

    public override string ToString()
    {
        return referenceName;
    }
}
