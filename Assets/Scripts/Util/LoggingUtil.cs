using System.Diagnostics;
using UnityEngine;

public static class LoggingUtil
{
    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Log(object message, bool condition = true)
    {
        if (condition)
        {
            UnityEngine.Debug.Log(message);
        }
    }

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Log(object message, Object context, bool condition = true)
    {
        if (condition)
        {
            UnityEngine.Debug.Log(message, context);
        }
    }
}
