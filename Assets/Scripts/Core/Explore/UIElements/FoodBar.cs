using UnityEngine;

public class FoodBar : TickBarBase
{
    private float changeAmount = -.5f;
    public bool isChanging = false;

    protected override void HandleTick()
    {
        if (!ExploreControl.IsTimeRunning || !isChanging)
            return;

        tickCounter++;

        if (tickCounter >= tickInterval)
        {
            tickCounter = 0;
            Change();

            if (currentValue <= 0)
            {
                EventManager.Raise(GameEvent.DieInArea);
            }
        }
    }

    private void Change()
    {
        currentValue = Mathf.Min(totalValue, currentValue + changeAmount);
        UpdateUIBarAndNumber();
        // Debug.Log("currentValue=" + currentValue);
    }

    public override void ResetProgress()
    {
        // Directly tied to player stats so any changes reflect immediately
        totalValue = PlayerContext.Get.stats.GetFinalStat(Stat.Food);
        currentValue = totalValue;
        isChanging = true;
        UpdateUIBarAndNumber();
        tickCounter = 0;
        tickInterval = 5;
    }


    public void UpdateTotal()
    {
        float difference = PlayerContext.Get.stats.GetFinalStat(Stat.Food) - totalValue;
        totalValue = totalValue + difference;
        // Do I always want current to change?
        currentValue = Mathf.Min(1f, currentValue + difference); // In case it's negative and goes down
        UpdateUIBarAndNumber();
    }

    public void Add(float amount)
    {
        currentValue = Mathf.Max(totalValue, currentValue + amount);
        UpdateUIBarAndNumber();
    }
}
