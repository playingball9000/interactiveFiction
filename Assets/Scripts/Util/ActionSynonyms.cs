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
        { "take", ActionConstants.ACTION_GET },
        { "grab", ActionConstants.ACTION_GET },
        { "get", ActionConstants.ACTION_GET },
        { "collect", ActionConstants.ACTION_GET },
        { "acquire", ActionConstants.ACTION_GET },
        { "pick", ActionConstants.ACTION_GET },
        { "store", ActionConstants.ACTION_GET },
        { "sieze", ActionConstants.ACTION_GET },
        { "stash", ActionConstants.ACTION_GET },
        { "i", ActionConstants.ACTION_INVENTORY },
        { "inventory", ActionConstants.ACTION_INVENTORY },
        { "drop", ActionConstants.ACTION_DROP },
        { "discard", ActionConstants.ACTION_DROP },
        { "unload", ActionConstants.ACTION_DROP },
        { "unpack", ActionConstants.ACTION_DROP },
        { "put", "put" },
        { "place", "put" },
        { "look", ActionConstants.ACTION_LOOK },
        { "l", ActionConstants.ACTION_LOOK },
        { "examine", ActionConstants.ACTION_EXAMINE },
        { "x", ActionConstants.ACTION_EXAMINE },
        { "inspect", ActionConstants.ACTION_EXAMINE },
    };
}
