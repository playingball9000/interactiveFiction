using System.Collections.Generic;
using System.Linq;

public static class CardUtil
{
    public static void InitialUnlockCards()
    {
        // Since no facts, just unlocks cards with no rule
        CardRegistry.GetAllCards().ForEach(c => c.isLocked = !CardUtil.IsCardUnlocked(c, new List<Fact>()));
    }

    public static List<Card> UnlockCardsPostComplete()
    {
        // If perf sucks, can create and update these outside this flow
        List<Card> allCards = CardRegistry.GetAllCards();
        List<Card> completedCards = new List<Card>();
        List<Card> lockedCards = new List<Card>();

        foreach (Card card in allCards)
        {
            if (card.isComplete)
            {
                completedCards.Add(card);
            }
            else if (card.isLocked)
            {
                lockedCards.Add(card);
            }
        }

        QueryRunner.RunCardCompleteFacts(completedCards, lockedCards);
        return lockedCards.Where(card => !card.isLocked).ToList();
    }

    public static bool IsCardUnlocked(Card card, List<Fact> facts)
    {
        Rule rule = CardRulesRegistry.GetRule(card.internalCode);
        return rule == null || rule.Evaluate(facts);
    }
}