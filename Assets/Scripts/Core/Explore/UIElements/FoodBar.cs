using UnityEngine;

public class FoodBar : TickBarBase
{
    private int tickInterval = 5;
    private float drainAmount = 40f;
    protected float totalValue = 100f;

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

            if (progressImage.fillAmount <= .001)
            {
                GameEvents.RaiseDieInArea();

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
    }

    public override void ResetProgress()
    {
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
