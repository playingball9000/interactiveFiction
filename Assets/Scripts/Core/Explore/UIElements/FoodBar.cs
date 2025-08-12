using UnityEngine;

public class FoodBar : TickBarBase
{
    private float drainAmount = .1f;
    public bool canDrain = false;

    protected override void HandleTick()
    {
        if (!ExploreControl.IsTimeRunning || !canDrain)
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
        UpdateBarAndNumber();
        // Debug.Log("currentValue=" + currentValue);
    }

    public override void ResetProgress()
    {
        // Directly tied to player stats so any changes reflect immediately
        totalValue = PlayerContext.Get.stats.GetFinalStat(Stat.Food);
        currentValue = totalValue;
        canDrain = true;
        UpdateBarAndNumber();
        tickCounter = 0;
        tickInterval = 5;
    }


    public void UpdateTotal()
    {
        float difference = PlayerContext.Get.stats.GetFinalStat(Stat.Food) - totalValue;
        totalValue = totalValue + difference;
        // Do I always want current to change?
        currentValue = Mathf.Min(1f, currentValue + difference); // In case it's negative and goes down
        UpdateBarAndNumber();
    }

    public void Add(float amount)
    {
        currentValue = Mathf.Max(totalValue, currentValue + amount);
        UpdateBarAndNumber();
    }
}
