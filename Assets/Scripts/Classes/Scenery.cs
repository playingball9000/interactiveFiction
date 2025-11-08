using System.Collections.Generic;

[System.Serializable]
public class Scenery : IExaminable, IAliasable
{
    public string displayName { get; set; }
    public string description { get; set; }
    public string adjective { get; set; } = "";
    public List<string> aliases { get; set; } = new();
    public bool examined { get; set; } = false;

    public override string ToString()
    {
        string toString =
            $"<b><color=#FFBF00>[Scenery]</color></b>\n" +
            $"  • DisplayName: <b>{displayName}</b>\n" +
            $"  • Adjective: {adjective}\n" +
            $"  • Description: {description}\n" +
            $"  • Examined: {examined}\n" +
            $"  • Aliases: {aliases}\n";

        return toString;
    }
}
