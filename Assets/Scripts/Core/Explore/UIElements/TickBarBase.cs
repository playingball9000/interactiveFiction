using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class TickBarBase : TickBase
{
    [SerializeField] protected Image progressImage;
    [SerializeField] public TextMeshProUGUI numbers;

    protected virtual void UpdateBarAndNumber()
    {
        progressImage.fillAmount = Mathf.Clamp01(currentValue / totalValue);
        numbers.text = $"{Mathf.Round(currentValue)} / {totalValue}";
    }

    public abstract void ResetProgress();
}
