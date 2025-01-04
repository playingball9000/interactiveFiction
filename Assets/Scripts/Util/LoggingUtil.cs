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
        if (condition)
        {
            UnityEngine.Debug.Log(message);
        }
    }

    // Probably should just implement toString on everything and use that instead of fieldName
    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void LogList<T>(List<T> objects, string fieldName, bool condition = true)
    {
        if (condition)
        {
            if (objects == null || objects.Count == 0)
                return;

            PropertyInfo property = typeof(T).GetProperty(fieldName);
            if (property == null)
                throw new ArgumentException($"Field '{fieldName}' does not exist on type '{typeof(T).Name}'.");

            string typeName = typeof(T).Name;
            var fieldValues = objects.Select(obj => property.GetValue(obj)?.ToString()).ToList();
            string result = "[" + string.Join(", ", fieldValues) + "]";
            string log = $"Type: {typeName}, Field: {fieldName}, Values: {result}";
            UnityEngine.Debug.Log(log);
        }
    }

}
