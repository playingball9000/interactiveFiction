using System;
using TMPro;

public class TickTimer : TickBase
{
    private TimerCode timerCode;
    private Action onComplete;
    public bool timerCanRun = false;

    public TextMeshProUGUI timerNumbers;

    public void Initialize(TimerCode code, float seconds, Action callback)
    {
        totalValue = seconds;
        currentValue = seconds;
        timerCode = code;
        onComplete = callback;
        timerCanRun = true;
        tickCounter = 0;

        if (timerNumbers != null)
        {
            timerNumbers.text = FormatTime(currentValue);
        }
    }

    protected override void HandleTick()
    {
        if (!ExploreControl.IsTimeRunning || !timerCanRun)
            return;

        tickCounter++;

        if (tickCounter >= tickInterval)
        {
            tickCounter = 0;
            currentValue = currentValue - 0.1f;
            if (timerNumbers != null)
            {
                timerNumbers.text = FormatTime(currentValue);
            }
            if (currentValue <= 0)
            {
                onComplete.Invoke();
                Stop();
            }
        }
    }

    public void Stop()
    {
        // Need to put here because when timer hits 0, it needs to manage itself in the timer dict
        TimerManager.Instance.RemoveTimer(timerCode);
        Destroy(gameObject);
    }

    public static string FormatTime(float timeSeconds)
    {
        int hours = (int)(timeSeconds / 3600);
        int minutes = (int)((timeSeconds % 3600) / 60);
        float seconds = timeSeconds % 60f;

        if (hours > 0)
        {
            // Hours, minutes, and seconds
            return $"{hours}:{minutes:00}:{seconds:00.0}";
        }
        else if (minutes > 0)
        {
            // Minutes and seconds
            return $"{minutes}:{seconds:00.0}";
        }
        else
        {
            // Just seconds
            return $"{seconds:0.0}";
        }
    }
}
