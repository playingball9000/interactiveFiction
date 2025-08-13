using System.Collections.Generic;
using System.Linq;

public static class CardRulesRegistry
{
    public static Dictionary<CardCode, Rule> cardRulesDict = new()
    {
        { CardCode.card4, Rule.Create().CardIsComplete(CardCode.card3)}
    };

    public static Rule GetRule(CardCode cardInternalCode) => cardRulesDict.TryGetValue(cardInternalCode, out var rule) ? rule : null;
}
