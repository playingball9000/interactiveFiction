[System.Serializable]
public class ItemBase : IItem
{
    public string displayName { get; set; }
    public string internalCode { get; set; }
    public string description { get; set; }
    public string adjective { get; set; } = "";
    public bool isGettable { get; set; } = true;

    // Backing field is needed otherwise C# auto props will get into an infinite loop
    private string pickUpNarrationBackingField;
    public string pickUpNarration { get => GetPickUpNarration(); set => pickUpNarrationBackingField = value; }

    public string GetPickUpNarration()
    {
        return pickUpNarrationBackingField ?? $"you pick up the {GetDisplayName()}";
    }
    public string GetDisplayName()
    {
        return ((IExaminable)this).GetDisplayName();
    }

    public override string ToString()
    {
        return GetDisplayName();
    }
}
