using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public TMP_Text titleText;
    public Button startButton;
    public Slider progressBar;
    public Card cardReference;

    public void Init(Card card, System.Action<CardUI> callback)
    {
        cardReference = card;
        titleText.text = card.title;
        startButton.onClick.AddListener(() => callback?.Invoke(this));
    }
}
