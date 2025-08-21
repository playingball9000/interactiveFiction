public static class CardExtensions
{
    public static Card Create(string title, CardCode code, float baseTime)
    {
        return new Card(title, code, baseTime);
    }

    public static Card WithToolTip(this Card card, string toolTip)
    {
        card.tooltipDesc = toolTip;
        return card;
    }
}