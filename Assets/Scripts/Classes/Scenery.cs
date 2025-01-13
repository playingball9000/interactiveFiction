[System.Serializable]
public class Scenery : IExaminable
{
    public string referenceName { get; set; }
    public string description { get; set; }

    public string GetDescription()
    {
        return description;
    }
}
