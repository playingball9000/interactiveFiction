
public interface ICardLifecycle
{
    string GetTooltip();
    void OnComplete(Card card);
    void OnReset(Card card);
}

[System.Serializable]
public class OnceLifecycle : ICardLifecycle
{

    public string GetTooltip()
    {
        return "Progress persists";
    }

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
    public string GetTooltip()
    {
        return "Repeatable";
    }

    public void OnComplete(Card card)
    {
        card.completionCount++;
    }

    public void OnReset(Card card)
    {
        card.isLocked = CardRuleRegistry.Get(card.internalCode) != null;
    }
}

[System.Serializable]
public class LimitedRepeatLifecycle : ICardLifecycle
{
    public int maxRepeats;
    public int completeCountThisRun;

    public string GetTooltip()
    {
        return $"Completed: {completeCountThisRun} / {maxRepeats}";
    }

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
        card.isLocked = CardRuleRegistry.Get(card.internalCode) != null;
    }
}

[System.Serializable]
public class RegularLifecycle : ICardLifecycle
{
    public string GetTooltip()
    {
        return "";
    }

    public void OnComplete(Card card)
    {
        card.completionCount++;
        card.isComplete = true;
    }

    public void OnReset(Card card)
    {
        card.isComplete = false;
        card.isLocked = CardRuleRegistry.Get(card.internalCode) != null;
    }
}

//TODO: Would be cool to have a cooldown card that reactivates after a set time