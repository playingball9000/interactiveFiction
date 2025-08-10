
using UnityEngine;
using System;
using System.Collections.Generic;

public enum GameEvent
{
    EnterArea,
    DieInArea,

    StatsChanged
}

public class EventManager : MonoBehaviour
{

    private Dictionary<GameEvent, Action> eventTable = new Dictionary<GameEvent, Action>();
    private Dictionary<GameEvent, int> lastFiredFrame = new Dictionary<GameEvent, int>();

    private static EventManager _instance; // private backing field

    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var em = new GameObject("EventManager");
                _instance = em.AddComponent<EventManager>();
                // Will throw warning about object not being cleaned up but I want this to persist so that's fine
                DontDestroyOnLoad(em);
            }
            return _instance;
        }
    }

    public static void Subscribe(GameEvent evt, Action listener)
        => Instance.SubscribeInternal(evt, listener);

    public static void Unsubscribe(GameEvent evt, Action listener)
        => Instance.UnsubscribeInternal(evt, listener);

    public static void Raise(GameEvent evt)
        => Instance.RaiseInternal(evt);


    public void SubscribeInternal(GameEvent eventId, Action listener)
    {
        if (!eventTable.ContainsKey(eventId))
        {
            eventTable[eventId] = delegate { };
        }

        eventTable[eventId] += listener;
    }

    public void UnsubscribeInternal(GameEvent eventId, Action listener)
    {
        if (eventTable.ContainsKey(eventId))
        {
            eventTable[eventId] -= listener;
        }
    }

    public void RaiseInternal(GameEvent eventId)
    {
        // Debug.Log("Raised event: " + eventId);
        int currentFrame = Time.frameCount;

        if (lastFiredFrame.TryGetValue(eventId, out int lastFrame) && lastFrame == currentFrame)
            return;

        lastFiredFrame[eventId] = currentFrame;

        if (eventTable.ContainsKey(eventId))
        {
            eventTable[eventId]?.Invoke();
        }
    }
}
