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
                EventManager.Raise(GameEvent.DieInArea);
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
        totalValue = PlayerContext.Get.stats.GetFinalStat(Stat.Food);
        currentValue = totalValue;
        // Both need to be 1f for draining bar
        UpdateFillAmount();
        tickCounter = 0;
    }


    public void UpdateBar()
    {
        float difference = PlayerContext.Get.stats.GetFinalStat(Stat.Food) - totalValue;
        totalValue = totalValue + difference;
        currentValue = Mathf.Min(1f, currentValue + difference); // In case it's negative and goes down
        UpdateFillAmount();
    }
}
