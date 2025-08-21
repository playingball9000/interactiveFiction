using System;
using System.Collections.Generic;
using UnityEngine;

public static class CardRunRegistry
{
    private static Dictionary<CardCode, Action> runDict = new()
    {
        { CardCode.card3, () => { TimerManager.Instance.CreateUiTimer(TimerCode.UITest, 1.2f, () =>{
            Debug.Log("Timer2 DONE");
        }); } },
    };

    public static Action Get(CardCode name)
    {
        return runDict.TryGetValue(name, out var code) ? code : null;
    }
}


