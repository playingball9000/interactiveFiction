[System.Serializable]
public class Scenery : IExaminable
{
    public string referenceName { get; set; }
    public string description { get; set; }
    public string adjective { get; set; } = "";


    public string GetDisplayName()
    {
        return string.IsNullOrEmpty(adjective) ? referenceName : adjective + " " + referenceName;
    }

    public string GetDescription()
    {
        return description;
    }
}
