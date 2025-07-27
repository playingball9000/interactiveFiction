using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public static class Log
{
    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    public static void Debug(object message, bool condition = true)
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

    public static void LogList<T>(IEnumerable<T> objects, string header = "", bool condition = true)
    {
        if (condition)
        {
            if (objects == null || !objects.Any())
            {
                UnityEngine.Debug.Log("The list is empty or null.");
                return;
            }

            UnityEngine.Debug.Log(header + string.Join(", ", objects));
        }
    }
    public static void GameObject(GameObject obj)
    {
        if (obj == null)
        {
            UnityEngine.Debug.LogWarning("GameObject is null.");
            return;
        }

        string log = $"GameObject: {obj.name}\n" +
                     $"Tag: {obj.tag}\n" +
                     $"Layer: {LayerMask.LayerToName(obj.layer)} ({obj.layer})\n" +
                     $"Active In Hierarchy: {obj.activeInHierarchy}\n" +
                     $"Active Self: {obj.activeSelf}\n" +
                     $"Position: {obj.transform.position}\n" +
                     $"Rotation: {obj.transform.rotation.eulerAngles}\n" +
                     $"Scale: {obj.transform.localScale}\n" +
                     $"Components:\n";

        Component[] components = obj.GetComponents<Component>();
        foreach (var component in components)
        {
            log += $" - {component.GetType().Name}\n";
        }

        UnityEngine.Debug.Log(log, obj); // Passing `obj` links the log to the object in the editor.
    }

}
