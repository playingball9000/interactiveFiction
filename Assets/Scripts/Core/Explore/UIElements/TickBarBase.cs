using UnityEngine;
using UnityEngine.UI;

public abstract class TickBarBase : MonoBehaviour
{
    [SerializeField] protected Image progressImage;
    protected float currentValue;
    protected float targetFill;
    protected float fillSpeed = .5f;

    protected virtual void OnEnable()
    {
        TickSystem.OnTick += HandleTick;
        GameEvents.OnEnterArea += ResetProgress;
        ResetProgress();
    }

    protected virtual void OnDisable()
    {
        TickSystem.OnTick -= HandleTick;
    }

    protected virtual void Update()
    {
        progressImage.fillAmount = Mathf.MoveTowards(progressImage.fillAmount, targetFill, fillSpeed * Time.deltaTime);
    }

    protected abstract void HandleTick();
    public abstract void ResetProgress();
    public abstract void AddValue(float value);
}
