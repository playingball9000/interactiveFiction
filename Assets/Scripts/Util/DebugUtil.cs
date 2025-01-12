using System;
using System.Diagnostics;

public static class DebugUtil
{
    public static void printPlayer()
    {
        LoggingUtil.Log($@" DEBUGUTIL - {System.Reflection.MethodBase.GetCurrentMethod().Name}
            {WorldState.GetInstance().player.ToString()}
            {WorldState.GetInstance().player.currentLocation.ToString()}
            ");

    }


    /**
     * Usage
     * DebugUtil.MeasureExecutionTime(() =>
        {
            // Code to measure, example:
            for (int i = 0; i < 1000000; i++) { }
        }, "OPERATION_NAME");
     * 
     */
    public static void MeasureExecutionTime(Action action, string description = "Operation")
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        LoggingUtil.Log($"{description} completed in {stopwatch.ElapsedMilliseconds} ms");
    }
}
