using UnityEngine;
using UnityEngine.UI;

public abstract class TickBarBase : MonoBehaviour
{
    [SerializeField] protected Image progressImage;
    protected float totalValue;
    protected float currentValue;
    protected float targetFill;
    protected float fillSpeed = 1f;

    protected virtual void OnEnable()
    {
        TickSystem.OnTick += HandleTick;
        EventManager.Subscribe(GameEvent.EnterArea, ResetProgress);
    }

    protected virtual void OnDisable()
    {
        TickSystem.OnTick -= HandleTick;
        EventManager.Unsubscribe(GameEvent.EnterArea, ResetProgress);

    }

    protected virtual void Update()
    {
        progressImage.fillAmount = Mathf.MoveTowards(progressImage.fillAmount, targetFill, fillSpeed * Time.deltaTime);
    }

    protected virtual void UpdateFillAmount()
    {
        progressImage.fillAmount = Mathf.Clamp01(currentValue / totalValue);
        targetFill = progressImage.fillAmount;
    }

    protected abstract void HandleTick();
    public abstract void ResetProgress();
}
