using System.Collections.Generic;
[System.Serializable]
//TODO: Should this also extend ItemBase?
public class WearableBase : IWearable
{
    public string displayName { get; set; }
    public string internalCode { get; set; }
    public string description { get; set; }
    public string adjective { get; set; } = "";
    public bool isGettable { get; set; } = true;

    // Backing field is needed otherwise C# auto props will get into an infinite loop
    private string pickUpNarrationBackingField;
    public string pickUpNarration { get => GetPickUpNarration(); set => pickUpNarrationBackingField = value; }

    public bool isWearable { get; set; } = true;
    public List<EquipmentSlot> slotsTaken { get; set; } = new();
    public ClothingLayer layer { get; set; }
    public List<Attribute> attributes { get; set; } = new();

    public string GetPickUpNarration()
    {
        return pickUpNarrationBackingField ?? $"you pick up the {GetDisplayName()}";
    }

    public string GetDescription()
    {
        string title = $"{GetDisplayName()}\n[Slot: {StringUtil.CreateCommaSeparatedString(slotsTaken)} | Layer: {layer}]";
        string attributesText = attributes.Count > 0 ? "Attributes:\n" : "";
        attributesText += StringUtil.CreateCommaSeparatedString(attributes);
        string descriptionText = $"{description}";
        return $"{title}\n\n{attributesText}\n\n{descriptionText}";
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
