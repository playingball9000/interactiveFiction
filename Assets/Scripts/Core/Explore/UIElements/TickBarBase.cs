using UnityEngine;
using UnityEngine.UI;

public abstract class TickBarBase : MonoBehaviour
{
    [SerializeField] protected Image progressImage;
    protected float currentValue;
    protected float targetFill;
    protected float fillSpeed = 2f;

    protected virtual void OnEnable()
    {
        TickSystem.OnTick += HandleTick;
        ResetProgress();
    }

    protected virtual void OnDisable()
    {
        TickSystem.OnTick -= HandleTick;
    }

    protected virtual void Update()
    {
        // This makes the bar progress smooth rather than jerky. It does screw up the progress so it never quite gets full or empty.
        progressImage.fillAmount = Mathf.Lerp(progressImage.fillAmount, targetFill, fillSpeed * Time.deltaTime);
    }

    protected abstract void HandleTick();
    public abstract void ResetProgress();
    public abstract void AddValue(float value);
}
