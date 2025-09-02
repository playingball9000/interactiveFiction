using System;
using System.Collections.Generic;
using System.Linq;

public static class RoomContextRegistry
{
    private static Dictionary<LocationCode, Dictionary<string, Action>> roomContextActions = new();

    public static void Register(LocationCode code, Dictionary<string, Action> contextActions)
    {
        roomContextActions[code] = contextActions;
    }

    public static Dictionary<string, Action> GetContextActions(LocationCode internalCode)
    {
        return roomContextActions.TryGetValue(internalCode, out var contextActions) ? contextActions : null;
    }

    public static List<Dictionary<string, Action>> GetAllContextActions()
    {
        return roomContextActions.Values.ToList();
    }

    public static Dictionary<LocationCode, Dictionary<string, Action>> GetDict()
    {
        return roomContextActions;
    }

    public static void Load(Dictionary<LocationCode, Dictionary<string, Action>> r)
    {
        roomContextActions = r;
    }

    public static void ClearRegistry()
    {
        roomContextActions.Clear();
    }
}
