
using UnityEngine;
using System;
using System.Collections.Generic;

// public static class GameEvents
// {
//     public static event Action OnEnterArea;
//     public static event Action OnDieInArea;
// }

public enum GameEvent
{
    OnEnterArea,
    OnDieInArea,
}

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private Dictionary<GameEvent, Action> eventTable = new Dictionary<GameEvent, Action>();
    private Dictionary<GameEvent, int> lastFiredFrame = new Dictionary<GameEvent, int>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
            eventTable[eventId] = delegate { };

        eventTable[eventId] += listener;
    }

    public void UnsubscribeInternal(GameEvent eventId, Action listener)
    {
        if (eventTable.ContainsKey(eventId))
            eventTable[eventId] -= listener;
    }

    public void RaiseInternal(GameEvent eventId)
    {
        int currentFrame = Time.frameCount;

        if (lastFiredFrame.TryGetValue(eventId, out int lastFrame) && lastFrame == currentFrame)
            return;

        lastFiredFrame[eventId] = currentFrame;

        if (eventTable.ContainsKey(eventId))
            eventTable[eventId]?.Invoke();
    }
}
