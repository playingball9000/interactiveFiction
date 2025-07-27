using System.Collections.Generic;

[System.Serializable]
public class Area : ILocation
{
    public string displayName { get; set; }
    public List<Card> cards = new List<Card>();
    public string internalCode { get; set; }

    public Area(string displayName, string internalCode)
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
        return $@"
        displayName: {displayName}
        internalCode: {internalCode}
        ";
    }
}
