using System.Collections.Generic;
public class ActionSynonyms
{
    public static readonly Dictionary<string, string> SynonymsDict = new Dictionary<string, string>
    {
        { "n", ExitDirection.north.ToString() },
        { "north", ExitDirection.north.ToString() },
        { "south", ExitDirection.south.ToString() },
        { "s", ExitDirection.south.ToString() },
        { "east", ExitDirection.east.ToString() },
        { "e", ExitDirection.east.ToString() },
        { "west", ExitDirection.west.ToString() },
        { "w", ExitDirection.west.ToString() },
        { "talk", ActionConstants.ACTION_TALK },
        { "take", "take" },
        { "grab", "take" },
        { "get", "take" },
        { "drop", "drop" },
        { "discard", "drop" },
        { "put", "put" },
        { "place", "put" },
        { "look", ActionConstants.ACTION_LOOK },
        { "l", ActionConstants.ACTION_LOOK },
        { "examine", ActionConstants.ACTION_EXAMINE },
        { "x", ActionConstants.ACTION_EXAMINE },
        { "inspect", ActionConstants.ACTION_EXAMINE },
    };
}
