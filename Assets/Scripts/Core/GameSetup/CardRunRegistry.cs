using System;
using System.Collections.Generic;
using UnityEngine;

public static class CardRunRegistry
{
    private static Dictionary<string, Action> runDict = new()
    {
        { "card3", () => { Debug.Log("Gained 10 wood from chopping."); } },
    };

    public static Action Get(string name)
    {
        return runDict.TryGetValue(name, out var code) ? code : null;
    }
}


