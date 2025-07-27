using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardQueueUIEntry : MonoBehaviour
{
    public TMP_Text titleText;
    public Slider progressBar;
    public Button cancelButton;


    [HideInInspector] public CardUI originalCardUI;

    [HideInInspector] public Card cardData;
    [HideInInspector] public System.Action<CardQueueUIEntry> onCancelClicked;

    public void Init(CardUI cardUI, System.Action<CardQueueUIEntry> cancelCallback)
    {
        cardData = cardUI.cardReference;
        originalCardUI = cardUI;
        titleText.text = cardUI.cardReference.title;
        onCancelClicked = cancelCallback;
        progressBar.value = 0f;

        cancelButton.onClick.AddListener(() => onCancelClicked?.Invoke(this));
    }

    public void SetProgress(float t)
    {
        progressBar.value = Mathf.Clamp01(t);
    }

    public void MarkComplete()
    {
        titleText.text += " Complete";
        progressBar.value = 1f;
        cancelButton.gameObject.SetActive(false);
    }

    public override string ToString()
    {
        return $"<b><color=#FFD700>[CardQueueUIEntry]</color></b>\n" +
               $"  • Title: <b>{titleText.text}</b>\n";
    }

}
