public class CardBuilder
{
    private readonly Card card;

    public CardBuilder(string title, CardCode code, float baseTime)
    {
        card = new Card(title, code, baseTime);
    }

    public CardBuilder WithTooltip(string tip)
    {
        card.tooltipDesc = tip;
        return this;
    }


    public CardBuilder WithCompletionLog(string log)
    {
        card.completionLog = log;
        return this;
    }

    // ---- Lifecycle strategies ----
    public CardBuilder AsOnce()
    {
        card.lifecycle = new OnceLifecycle();
        return this;
    }

    public CardBuilder AsRepeatable()
    {
        card.lifecycle = new RepeatableLifecycle();
        return this;
    }

    public CardBuilder AsLimitedRepeats(int maxRepeats)
    {
        card.lifecycle = new LimitedRepeatLifecycle { maxRepeats = maxRepeats };
        return this;
    }

    public Card Build()
    {
        if (card.lifecycle == null)
        {
            card.lifecycle = new RegularLifecycle();
        }
        CardRegistry.Register(card);
        return card;
    }
}
