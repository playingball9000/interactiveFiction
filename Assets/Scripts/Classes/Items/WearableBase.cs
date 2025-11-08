using System.Collections.Generic;
[System.Serializable]
public class WearableBase : ItemBase, IWearable
{
    public bool isWearable { get; set; } = true;
    public List<EquipmentSlot> slotsTaken { get; set; } = new();
    public ClothingLayer layer { get; set; }
    public List<Attribute> attributes { get; set; } = new();

    public string GetDescription()
    {
        string title = $"{this.GetDisplayName()}\n[Slot: {StringUtil.CreateCommaSeparatedString(slotsTaken)} | Layer: {layer}]";
        string attributesText = attributes.Count > 0 ? "Attributes:\n" : "";
        attributesText += StringUtil.CreateCommaSeparatedString(attributes);
        string descriptionText = $"{description}";
        return $"{title}\n\n{attributesText}\n\n{descriptionText}";
    }

    public override string ToString()
    {
        return this.GetDisplayName();
    }
}
