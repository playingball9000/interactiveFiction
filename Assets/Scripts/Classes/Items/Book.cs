[System.Serializable]
public class Book : IItem
{
    public string referenceName { get; set; }
    public string description { get; set; }

    public string GetDescription()
    {
        return description;
    }
}
