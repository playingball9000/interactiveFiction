using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardQueueUIEntry : MonoBehaviour
{
    public TMP_Text titleText;
    public Slider progressBar;
    public Button cancelButton;

    [HideInInspector] public CardUI cardUIRef;
    [HideInInspector] public System.Action<CardQueueUIEntry> onCancelClicked;

    public void Init(CardUI cardUI, System.Action<CardQueueUIEntry> cancelCallback)
    {
        cardUIRef = cardUI;
        titleText.text = cardUI.cardRef.title;
        onCancelClicked = cancelCallback;
        progressBar.value = cardUI.elapsedTime / cardUI.cardRef.currentTimeToComplete;

        cancelButton.onClick.AddListener(() => onCancelClicked?.Invoke(this));
    }

    public void SetProgress(float t)
    {
        progressBar.value = Mathf.Clamp01(t);
    }

    public override string ToString()
    {
        return $"<b><color=#FFD700>[CardQueueUIEntry]</color></b>\n" +
               $"  • Title: <b>{titleText.text}</b>\n";
    }

}
