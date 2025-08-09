using UnityEngine;
using System;

public class TickSystem : MonoBehaviour
{
    public static event Action OnTick;

    private void FixedUpdate()
    {
        // This method called every .02 seconds - ie 50/sec
        OnTick?.Invoke();
    }
}
