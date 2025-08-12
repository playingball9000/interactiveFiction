using UnityEngine;

public abstract class TickBase : MonoBehaviour
{
    protected float totalValue;
    protected float currentValue;
    protected int tickInterval;
    protected int tickCounter;

    protected virtual void OnEnable()
    {
        TickSystem.OnTick += HandleTick;
    }

    protected virtual void OnDisable()
    {
        TickSystem.OnTick -= HandleTick;
    }

    protected abstract void HandleTick();
}
