using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

public static class LoggingUtil
{
    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Log(object message, bool condition = true)
    {
        if (message == null)
        {
            UnityEngine.Debug.Log("LoggingUtil: null input");

        }
        if (condition)
        {
            UnityEngine.Debug.Log(message);
        }
    }

    public static void LogList<T>(List<T> objects, string header = "", bool condition = true)
    {
        if (condition)
        {
            if (objects == null || objects.Count == 0)
            {
                UnityEngine.Debug.Log("The list is empty or null.");
                return;
            }

            UnityEngine.Debug.Log(header + string.Join(", ", objects));
        }
    }


}
