using System.Collections.Generic;

[System.Serializable]
public class ItemBase : IItem, IAliasable
{
    public string displayName { get; set; }
    public string internalCode { get; set; }
    public string description { get; set; }
    public string adjective { get; set; } = "";
    public bool isGettable { get; set; } = true;
    public List<string> aliases { get; set; } = new();

    // Backing field is needed otherwise C# auto props will get into an infinite loop
    private string pickUpNarrationBackingField;
    public string pickUpNarration { get => GetPickUpNarration(); set => pickUpNarrationBackingField = value; }

    public string GetPickUpNarration()
    {
        return pickUpNarrationBackingField ?? $"you pick up the {this.GetDisplayName()}";
    }

    public override string ToString()
    {
        return this.GetDisplayName();
    }
}
