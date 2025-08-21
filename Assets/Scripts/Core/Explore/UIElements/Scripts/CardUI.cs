using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class CardUI : MonoBehaviour, ITooltip
{
    public TMP_Text titleText;
    public Button startButton;
    public Card cardRef;
    public float elapsedTime;

    public void Init(Card card, System.Action<CardUI> callback)
    {
        cardRef = card;
        titleText.text = card.title;
        startButton.onClick.AddListener(() => callback?.Invoke(this));
        elapsedTime = 0f;
    }

    public string GetTooltipText()
    {
        StringBuilder sb = new StringBuilder();

        // Header
        sb.AppendLine($"<b>{cardRef.title}</b>");
        sb.AppendLine($"{cardRef.lifecycle.GetTooltip()}");
        sb.AppendLine($"{cardRef.tooltipDesc}");
        sb.AppendLine($"{cardRef.currentTimeToComplete:F2} / {cardRef.baseTimeToComplete:F2}");
        sb.AppendLine($"Time Left: {cardRef.currentTimeToComplete - elapsedTime:F2}");

        return sb.ToString();
    }

    public override string ToString()
    {
        return $"<b><color=#FFD700>[CardUI]</color></b>\n" +
               $"  • titleText.text: <b>{titleText.text}</b>\n";
    }
}
