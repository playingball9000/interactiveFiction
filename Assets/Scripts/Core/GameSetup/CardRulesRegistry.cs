using System.Collections.Generic;
using System.Linq;

using static FactExtensions;
using static RuleConstants;

public static class CardRulesRegistry
{
    public static Dictionary<string, Rule> cardRulesDict = new();
    private static bool initialized;

    public static void Initialize()
    {
        if (initialized) return;
        initialized = true;

        // Rules are tightly coupled to Card constructor for implicit locking
        Register("card4", Rule.Create().WhenAll(
            FactIsEqual(KEY_CONCEPT, CONCEPT_ON_CARD_COMPLETE),
            FactIsEqual(KEY_CARD_COMPLETED, "card3")
        ));
    }

    private static void Register(string cardInternalCode, Rule rule)
    {
        cardRulesDict[cardInternalCode] = rule;
    }

    public static Rule GetRule(string cardInternalCode) => cardRulesDict.TryGetValue(cardInternalCode, out var rule) ? rule : null;
    public static List<Rule> GetAllRules() => cardRulesDict.Values.ToList();
}
