
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;


public class TimerManager : MonoBehaviour
{
    public Transform container;
    public GameObject timerPrefab;

    private Dictionary<TimerCode, TickTimer> timers = new();

    private static TimerManager _instance; // private backing field

    public static TimerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var tm = new GameObject("TimerManager");
                _instance = tm.AddComponent<TimerManager>();
                DontDestroyOnLoad(tm);
            }
            return _instance;
        }
    }

    void Awake()
    {
        container = GameObject.Find("Log Canvas")?.transform;
        timerPrefab = (GameObject)Resources.Load("prefabs/Timer", typeof(GameObject));
    }

    public TickTimer CreateTimer(TimerCode code, float seconds, Action onComplete)
    {
        GameObject timerObj = new GameObject("TickTimer_" + code);
        TickTimer timer = timerObj.AddComponent<TickTimer>();
        timer.Initialize(code, seconds, onComplete);
        timers.Add(code, timer);
        return timer;
    }

    public TickTimer CreateUiTimer(TimerCode code, float seconds, Action onComplete)
    {
        GameObject timerObj = Instantiate(timerPrefab, container);
        TickTimer timer = timerObj.GetComponent<TickTimer>();
        timer.Initialize(code, seconds, onComplete);
        timers.Add(code, timer);
        return timer;
    }

    public void StopTimer(TimerCode code)
    {
        if (timers.TryGetValue(code, out TickTimer timer))
        {
            timer.Stop();
        }
    }

    public void RemoveTimer(TimerCode code)
    {
        timers.Remove(code);
    }

}
