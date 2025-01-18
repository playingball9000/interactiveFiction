[System.Serializable]
public class ItemBase : IItem
{
    public string referenceName { get; set; }
    public string description { get; set; }
    public bool isGettable { get; set; }
    public string pickUpNarration { get; set; }

    public string GetDescription()
    {
        return description;
    }
}
