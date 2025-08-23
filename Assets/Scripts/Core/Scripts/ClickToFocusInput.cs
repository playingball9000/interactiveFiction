using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ClickToFocusInput : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] private TMP_InputField UI_playerInputBox;

    void Awake()
    {
        // Has to match game object name
        GameObject inputFieldObject = GameObject.Find("PlayerInputBox");
        UI_playerInputBox = inputFieldObject.GetComponent<TMP_InputField>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UI_playerInputBox.Select();
        UI_playerInputBox.ActivateInputField();
    }
}
