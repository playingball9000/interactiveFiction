using UnityEngine;

public class UiUtilMb : MonoBehaviour
{
    private static UiUtilMb _instance;

    public static UiUtilMb Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject runner = new GameObject("UiUtilMb");
                _instance = runner.AddComponent<UiUtilMb>();
                DontDestroyOnLoad(runner);
            }
            return _instance;
        }
    }

    public void DestroyChildrenInContainer(Transform container)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }
}