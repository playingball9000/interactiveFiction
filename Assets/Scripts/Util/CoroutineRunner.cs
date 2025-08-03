using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject runner = new GameObject("CoroutineRunner");
                _instance = runner.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(runner);
            }
            return _instance;
        }
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}

public static class CommonCoroutines
{
    public static IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
