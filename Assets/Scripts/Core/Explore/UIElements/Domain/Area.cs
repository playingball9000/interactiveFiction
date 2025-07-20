using System.Collections.Generic;

[System.Serializable]
public class Area
{
    public string areaName;
    public List<Card> cards = new List<Card>();

    public Area(string areaName)
    {
        this.areaName = areaName;
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
    }
}
