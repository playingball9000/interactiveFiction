using System.Collections.Generic;

[System.Serializable]
public class Scenery : IExaminable
{
    public string displayName { get; set; }
    public string description { get; set; }
    public string adjective { get; set; } = "";
    public List<string> aliases = new List<string>();

    public override string ToString()
    {
        string toString =
            $"<b><color=#FFBF00>[Scenery]</color></b>\n" +
            $"  • Name: <b>{displayName}</b>\n" +
            $"  • Adjective: {adjective}\n" +
            $"  • Description: {description}\n" +
            $"  • Aliases: {aliases}\n";

        return toString;
    }
}
