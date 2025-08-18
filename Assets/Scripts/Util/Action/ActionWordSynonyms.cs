using System.Collections.Generic;
public class ActionWordSynonyms
{
    public static readonly Dictionary<string, PlayerAction> SynonymsDict = new()
    {
        { "n", PlayerAction.North },
        { "north", PlayerAction.North },
        { "nw", PlayerAction.NorthWest },
        { "northwest", PlayerAction.NorthWest },
        { "ne", PlayerAction.NorthEast },
        { "northeast", PlayerAction.NorthEast },
        { "south", PlayerAction.South },
        { "sw", PlayerAction.SouthWest },
        { "southwest", PlayerAction.SouthWest },
        { "se", PlayerAction.SouthEast },
        { "southeast", PlayerAction.SouthEast },
        { "s", PlayerAction.South },
        { "east", PlayerAction.East },
        { "e", PlayerAction.East },
        { "west", PlayerAction.West },
        { "w", PlayerAction.West },
        { "enter", PlayerAction.Enter },
        { "exit", PlayerAction.Enter },
        { "leave", PlayerAction.Enter },

        { "unlock", PlayerAction.Unlock },

        { "tickle", PlayerAction.Tickle },

        { "insult", PlayerAction.Insult },

        { "battle", PlayerAction.Battle },
        { "fight", PlayerAction.Battle },
        { "challenge", PlayerAction.Battle },
        { "aggress", PlayerAction.Battle },

        { "talk", PlayerAction.Talk },

        { "take", PlayerAction.Get },
        { "grab", PlayerAction.Get },
        { "get", PlayerAction.Get },
        { "collect", PlayerAction.Get },
        { "acquire", PlayerAction.Get },
        { "pick", PlayerAction.Get },
        { "sieze", PlayerAction.Get },
        { "stash", PlayerAction.Get },

        { "i", PlayerAction.Inventory },
        { "inventory", PlayerAction.Inventory },

        { "drop", PlayerAction.Drop },
        { "discard", PlayerAction.Drop },
        { "unload", PlayerAction.Drop },
        { "unpack", PlayerAction.Drop },

        { "give", PlayerAction.Give },
        { "offer", PlayerAction.Give },
        { "present", PlayerAction.Give },
        { "grant", PlayerAction.Give },
        { "deliver", PlayerAction.Give },

        { "equip", PlayerAction.Equip },
        { "wear", PlayerAction.Equip },
        { "don", PlayerAction.Equip },

        { "unequip", PlayerAction.Unequip },
        { "remove", PlayerAction.Unequip },

        { "open", PlayerAction.Open },

        { "close", PlayerAction.Close },
        { "shut", PlayerAction.Close },

        { "put", PlayerAction.Put },
        { "insert",  PlayerAction.Put },
        { "place",  PlayerAction.Put },

        { "look", PlayerAction.Look },
        { "l", PlayerAction.Look },

        { "examine", PlayerAction.Examine },
        { "x", PlayerAction.Examine },
        { "inspect", PlayerAction.Examine },

        { "savegame", PlayerAction.SaveGame },
        { "save", PlayerAction.SaveGame },

        { "loadgame", PlayerAction.LoadGame },
        { "load", PlayerAction.LoadGame },
    };

    public static PlayerAction Get(string name)
    {
        return SynonymsDict.TryGetValue(name, out var action) ? action : PlayerAction.Unknown;
    }
}
