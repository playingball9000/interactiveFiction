using System.Collections.Generic;

[System.Serializable]
public class Scenery : IExaminable
{
    public string displayName { get; set; }
    public string description { get; set; }
    public string adjective { get; set; } = "";
    public List<string> aliases = new List<string>();
}
