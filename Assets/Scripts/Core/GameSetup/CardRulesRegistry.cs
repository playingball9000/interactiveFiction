using System.Collections.Generic;

public static class CardRuleRegistry
{
    public static Dictionary<CardCode, Rule> cardRulesDict = new()
    {
        { CardCode.card4, Rule.Create().CardIsComplete(CardCode.card3)}
    };

    public static Rule Get(CardCode cardInternalCode) => cardRulesDict.TryGetValue(cardInternalCode, out var rule) ? rule : null;
}
