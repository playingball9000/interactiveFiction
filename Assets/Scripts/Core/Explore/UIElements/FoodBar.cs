using UnityEngine;

public class FoodBar : TickBarBase
{
    [SerializeField] private int tickInterval = 5;
    [SerializeField] private float fillAmount = 0.1f;
    protected float totalValue = 10f;

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
                // TODO: Really I should send out some sort of event here
                GameController.invokeShowMainCanvas();

                // Disabling so update() isn't called all the time
                //TODO: I should really have a manager class that enables/ disables all the bars
                this.enabled = false;
            }
        }
    }

    private void Drain()
    {
        currentValue = Mathf.Min(totalValue, currentValue - fillAmount);
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
