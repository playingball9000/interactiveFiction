
public interface ICardLifecycle
{
    void OnComplete(Card card);
    void OnReset(Card card);
}

[System.Serializable]
public class OnceLifecycle : ICardLifecycle
{
    public void OnComplete(Card card)
    {
        card.completionCount++;
        card.isComplete = true;
    }

    public void OnReset(Card card) { }
}

[System.Serializable]
public class RepeatableLifecycle : ICardLifecycle
{
    public void OnComplete(Card card)
    {
        card.completionCount++;
    }

    public void OnReset(Card card) { }
}

[System.Serializable]
public class LimitedRepeatLifecycle : ICardLifecycle
{
    public int maxRepeats;
    public int completeCountThisRun;

    public void OnComplete(Card card)
    {
        completeCountThisRun++;
        card.completionCount++;
        if (completeCountThisRun == maxRepeats)
        {
            card.isComplete = true;
        }
    }

    public void OnReset(Card card)
    {
        completeCountThisRun = 0;
        card.isComplete = false;
    }
}

[System.Serializable]
public class RegularLifecycle : ICardLifecycle
{
    public void OnComplete(Card card)
    {
        card.completionCount++;
        card.isComplete = true;
    }

    public void OnReset(Card card)
    {
        card.isComplete = false;
    }
}

//TODO: Would be cool to have a cooldown card that reactivates after a set time