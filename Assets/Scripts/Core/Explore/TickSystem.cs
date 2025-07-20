using UnityEngine;
using System;

public class TickSystem : MonoBehaviour
{
    public static event Action OnTick;

    public float tickInterval = 0.1f;

    private float tickTimer;
    private int tickCount;

    private void Awake()
    {
        tickTimer = tickInterval;
        tickCount = 0;
    }

    private void Update()
    {
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0f)
        {
            tickTimer += tickInterval;
            tickCount++;
            OnTick?.Invoke();
            // Debug.Log($"Tick #{tickCount}");
        }
    }

    public int GetTickCount()
    {
        return tickCount;
    }
}
