using UnityEngine;

public class FoodBar : TickBarBase
{
    private int tickInterval = 5;
    private float drainAmount = 1f;

    private int tickCounter = 0;

    protected override void HandleTick()
    {
        if (!ExploreControl.IsTimeRunning)
            return;

        tickCounter++;

        if (tickCounter >= tickInterval)
        {
            tickCounter = 0;
            Drain();

            if (currentValue <= 0)
            {
                EventManager.Raise(GameEvent.OnDieInArea);

                // Disabling so update() isn't called all the time
                //TODO: I should really have a manager class that enables/ disables all the bars, and maybe checks levels
                this.enabled = false;
            }
        }
    }

    private void Drain()
    {
        currentValue = Mathf.Min(totalValue, currentValue - drainAmount);
        targetFill = Mathf.Clamp01(currentValue / totalValue);
        // Debug.Log("currentValue=" + currentValue);
    }

    public override void ResetProgress()
    {
        totalValue = 10f;
        currentValue = totalValue;
        // Both need to be 1f for draining bar
        targetFill = 1f;
        progressImage.fillAmount = 1f;


        tickCounter = 0;
        //TODO: I should really have a manager class that enables/ disables all the bars
        this.enabled = true;
    }

    public override void AddValue(float value)
    {
        currentValue = Mathf.Min(totalValue, currentValue + value);
        targetFill = Mathf.Clamp01(currentValue / totalValue);
    }
}
