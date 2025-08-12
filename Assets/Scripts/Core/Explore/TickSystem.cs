using UnityEngine;
using System;

// TickSystem is its own event because separate from other events (timekeeping vs game events)
public class TickSystem : MonoBehaviour
{
    public static event Action OnTick;

    private void FixedUpdate()
    {
        // This method called every .02 seconds - ie 50/sec
        OnTick?.Invoke();
    }
}
