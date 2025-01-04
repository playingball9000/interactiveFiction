public class Shoes : IClothing
{
    public string referenceName { get; set; }
    public string description { get; set; }
    public string color { get; set; }

    public Shoes(string shoeName, string description, string color)
    {
        this.referenceName = shoeName;
        this.description = description;
        this.color = color;
    }

    public string GetDescription()
    {
        return this.description;
    }
}