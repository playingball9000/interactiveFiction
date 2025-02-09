using System.Collections.Generic;
public class ActionWordSynonyms
{
    public static readonly Dictionary<string, string> SynonymsDict = new()
    {
        { "n", ExitDirection.North.ToString() },
        { "north", ExitDirection.North.ToString() },
        { "nw", ExitDirection.Northwest.ToString() },
        { "northwest", ExitDirection.Northwest.ToString() },
        { "ne", ExitDirection.Northeast.ToString() },
        { "northeast", ExitDirection.Northeast.ToString() },
        { "south", ExitDirection.South.ToString() },
        { "sw", ExitDirection.Southwest.ToString() },
        { "southwest", ExitDirection.Southwest.ToString() },
        { "se", ExitDirection.Southeast.ToString() },
        { "southeast", ExitDirection.Southeast.ToString() },
        { "s", ExitDirection.South.ToString() },
        { "east", ExitDirection.East.ToString() },
        { "e", ExitDirection.East.ToString() },
        { "west", ExitDirection.West.ToString() },
        { "w", ExitDirection.West.ToString() },
        { "enter", ExitDirection.Enter.ToString() },
        { "exit", ExitDirection.Enter.ToString() },
        { "leave", ExitDirection.Enter.ToString() },

        { "talk", ActionConstants.ACTION_TALK },

        { "take", ActionConstants.ACTION_GET },
        { "grab", ActionConstants.ACTION_GET },
        { "get", ActionConstants.ACTION_GET },
        { "collect", ActionConstants.ACTION_GET },
        { "acquire", ActionConstants.ACTION_GET },
        { "pick", ActionConstants.ACTION_GET },
        { "sieze", ActionConstants.ACTION_GET },
        { "stash", ActionConstants.ACTION_GET },

        { "i", ActionConstants.ACTION_INVENTORY },
        { "inventory", ActionConstants.ACTION_INVENTORY },

        { "drop", ActionConstants.ACTION_DROP },
        { "discard", ActionConstants.ACTION_DROP },
        { "unload", ActionConstants.ACTION_DROP },
        { "unpack", ActionConstants.ACTION_DROP },

        { "give", ActionConstants.ACTION_GIVE },
        { "offer", ActionConstants.ACTION_GIVE },
        { "present", ActionConstants.ACTION_GIVE },
        { "grant", ActionConstants.ACTION_GIVE },
        { "deliver", ActionConstants.ACTION_GIVE },

        { "equip", ActionConstants.ACTION_EQUIP },
        { "wear", ActionConstants.ACTION_EQUIP },
        { "don", ActionConstants.ACTION_EQUIP },

        { "unequip", ActionConstants.ACTION_UNEQUIP },
        { "remove", ActionConstants.ACTION_UNEQUIP },

        { "open", ActionConstants.ACTION_OPEN },

        { "close", ActionConstants.ACTION_CLOSE },
        { "shut", ActionConstants.ACTION_CLOSE },

        { "put", ActionConstants.ACTION_PUT },
        { "insert",  ActionConstants.ACTION_PUT },
        { "place",  ActionConstants.ACTION_PUT },

        { "look", ActionConstants.ACTION_LOOK },
        { "l", ActionConstants.ACTION_LOOK },

        { "examine", ActionConstants.ACTION_EXAMINE },
        { "x", ActionConstants.ACTION_EXAMINE },
        { "inspect", ActionConstants.ACTION_EXAMINE },

        { "savegame", ActionConstants.ACTION_SAVEGAME },
        { "save", ActionConstants.ACTION_SAVEGAME },

        { "loadgame", ActionConstants.ACTION_LOADGAME },
        { "load", ActionConstants.ACTION_LOADGAME },
    };
}
