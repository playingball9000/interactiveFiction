using System.Collections.Generic;
using System.Linq;

public static class CardRegistry
{
    public static Dictionary<CardCode, Card> cards = new();
    private static bool initialized;

    public static void Initialize()
    {
        if (initialized) return;
        initialized = true;

        Register(new Card("Find the key", 1f, CardCode.card1));
        Register(new Card("Unlock the door", 2f, CardCode.card2));
        Register(new Card("Defeat the guard", 3f, CardCode.card3));
        Register(new Card("Rescue Princess", 2f, CardCode.card4));
    }

    private static void Register(Card card)
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
