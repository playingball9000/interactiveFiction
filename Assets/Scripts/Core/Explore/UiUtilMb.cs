using UnityEngine;
using UnityEngine.UI;

public class UiUtilMb : MonoBehaviour
{
    private static UiUtilMb _instance;

    public static UiUtilMb Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject util = new GameObject("UiUtilMb");
                _instance = util.AddComponent<UiUtilMb>();
                DontDestroyOnLoad(util);
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

    public void ScrollToBottom(ScrollRect scrollRect)
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}