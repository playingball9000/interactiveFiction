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

    public static void LogList<T>(List<T> objects, bool condition = true)
    {
        if (condition)
        {
            if (objects == null || objects.Count == 0)
            {
                UnityEngine.Debug.Log("The list is empty or null.");
                return;
            }

            UnityEngine.Debug.Log(string.Join(", ", objects));
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
            string result = StringUtil.CreateCommaSeparatedString(fieldValues);
            string log = $"Type: {typeName}, Field: {fieldName}, Values: {result}";
            UnityEngine.Debug.Log(log);
        }
    }

}
