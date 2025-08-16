using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class CardUI : MonoBehaviour, IToolTip
{
    public TMP_Text titleText;
    public Button startButton;
    public Card cardReference;

    public void Init(Card card, System.Action<CardUI> callback)
    {
        cardReference = card;
        titleText.text = card.title;
        startButton.onClick.AddListener(() => callback?.Invoke(this));
    }

    public string GetToolTipText()
    {
        StringBuilder sb = new StringBuilder();

        // Header
        sb.AppendLine($"<b>{cardReference.title}</b>");
        sb.AppendLine($"{cardReference.toolTipDesc}");
        sb.AppendLine($"{cardReference.GetCurrentTimeToComplete():F2} / {cardReference.baseTimeToComplete:F2}");

        return sb.ToString();
    }

    public override string ToString()
    {
        return $"<b><color=#FFD700>[CardUI]</color></b>\n" +
               $"  • titleText.text: <b>{titleText.text}</b>\n";
    }
}
