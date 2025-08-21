using System.Collections.Generic;
using System.Linq;

public static class CardRegistry
{
    public static Dictionary<CardCode, Card> cards = new();

    public static void Register(Card card)
    {
        cards[card.internalCode] = card;
    }

    public static Card GetCard(CardCode internalCode) => cards[internalCode];
    public static List<Card> GetAllCards() => cards.Values.ToList();

    public static Dictionary<CardCode, Card> GetDict()
    {
        return cards;
    }

    public static void Load(Dictionary<CardCode, Card> c)
    {
        cards = c;
    }
}
