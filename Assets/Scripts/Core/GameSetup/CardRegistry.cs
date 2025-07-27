using System.Collections.Generic;
using System.Linq;

public static class CardRegistry
{
    public static Dictionary<string, Card> cardDict = new();
    private static bool initialized;

    public static void Initialize()
    {
        if (initialized) return;
        initialized = true;

        Register(new Card("Find the key", 1f, "card1"));
        Register(new Card("Unlock the door", 2f, "card2"));
        Register(new Card("Defeat the guard", 3f, "card3"));
        Register(new Card("Rescue Princess", 2f, "card4"));
    }

    private static void Register(Card card)
    {
        cardDict[card.internalCode] = card;
    }

    public static Card GetCard(string internalCode) => cardDict[internalCode];
    public static List<Card> GetAllCards() => cardDict.Values.ToList();
}
