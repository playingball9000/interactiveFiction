using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class CardRunRegistry
{
    private static Dictionary<string, Action> runDict = new()
    {
        { "card3", () => { TimerManager.Instance.CreateUiTimer(TimerCode.UITest, 1.2f, () =>{
            Debug.Log("Timer2 DONE");
        }); } },
    };

    public static Action Get(string name)
    {
        return runDict.TryGetValue(name, out var code) ? code : null;
    }
}


