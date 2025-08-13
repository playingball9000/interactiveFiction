using System.Collections.Generic;

[System.Serializable]
public class Area : ILocation
{
    public string displayName { get; set; }
    public List<Card> cards = new List<Card>();
    public LocationCode internalCode { get; set; }

    public Area(string displayName, LocationCode internalCode)
    {
        this.displayName = displayName;
        this.internalCode = internalCode;
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
    }
    public override string ToString()
    {
        return $"<b><color=#1E90FF>[Area]</color></b>\n" +
               $"  • Name: <b>{displayName}</b>\n" +
               $"  • Code: {internalCode}\n" +
               $"  • Cards: {cards?.Count ?? 0}";
    }
}
