using UnityEngine;
using System;

public static class ExploreControl
{
    private static bool timeRunning = true;

    // public static event Action<bool> OnCanDrainChanged;

    public static bool IsTimeRunning
    {
        get => timeRunning;
        set => timeRunning = value;
    }

    public static void ToggleTimeRunning()
    {
        timeRunning = !timeRunning;
        // OnCanDrainChanged?.Invoke(canDrain);
    }
}
