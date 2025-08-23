using UnityEngine;

public class StatBar : TickBarBase
{
    [SerializeField] private Stat statType;
    [SerializeField] private float changeAmount;
    public bool isChanging;

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
                EventManager.Raise(GameEvent.DieInArea);
        }
    }

    private void Change()
    {
        currentValue = Mathf.Min(totalValue, currentValue + changeAmount);
        UpdateUIBarAndNumber();
    }

    public override void ResetProgress()
    {
        totalValue = PlayerContext.Get.stats.GetFinalStat(statType);
        currentValue = totalValue;
        UpdateUIBarAndNumber();
        tickCounter = 0;
    }

    public void UpdateTotalFromStats()
    {
        float difference = PlayerContext.Get.stats.GetFinalStat(statType) - totalValue;
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
