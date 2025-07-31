using System.Collections.Generic;
using System.Linq;

public static class CardRulesRegistry
{
    public static Dictionary<string, Rule> cardRulesDict = new()
    {
        { "card4", Rule.Create().CardIsComplete("card3")}
    };

    public static Rule GetRule(string cardInternalCode) => cardRulesDict.TryGetValue(cardInternalCode, out var rule) ? rule : null;
}
