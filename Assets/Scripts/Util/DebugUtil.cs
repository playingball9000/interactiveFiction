using System;
using System.Diagnostics;

public static class DebugUtil
{
    public static void printPlayer()
    {
        Log.Debug($@" DEBUGUTIL - {System.Reflection.MethodBase.GetCurrentMethod().Name}
            {PlayerContext.Get}
            {PlayerContext.Get.currentRoom}
            ");

    }

    /// <summary>
    /// Usage
    /// DebugUtil.MeasureExecutionTime(() =>
    ///     {
    ///         for (int i = 0; i < 1000000; i++) { }
    ///     }, "OPERATION_NAME");
    /// </summary>
    /// <param name="action">Code to be measured</param>
    /// <param name="description">Name of code for logs</param>
    public static void MeasureExecutionTime(Action action, string description = "Operation")
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        Log.Debug($"{description} completed in {stopwatch.ElapsedMilliseconds} ms");
    }
}
