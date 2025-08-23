using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class DialogueChoiceUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int choiceNumber;
    private TextMeshProUGUI label;
    private Color defaultColor;
    public Color hoverColor;

    public static event Action<int> OnChoiceClicked;

    void Awake()
    {
        label = GetComponentInChildren<TextMeshProUGUI>();

        ColorUtility.TryParseHtmlString("#ADD8E6", out hoverColor);
        ColorUtility.TryParseHtmlString("#FFD700", out defaultColor);
    }

    public void Init(int number, string text)
    {
        choiceNumber = number;
        label.text = $"{choiceNumber}. {text}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        label.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        label.color = defaultColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnChoiceClicked?.Invoke(choiceNumber);
    }
}
