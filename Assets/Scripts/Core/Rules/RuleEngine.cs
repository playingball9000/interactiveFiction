using System.Collections.Generic;
using System.Linq;

public static class RuleEngine
{
    private static List<Rule> rules = new List<Rule>();

    //TODO: probably a good idea to have different rulesets (concepts) for speed ie. move, action, etc.
    static RuleEngine()
    {
        rules.Add(new Rule()
            .AddCondition(facts => Criteria.FactValueEquals(facts, "concept", "OnMove"))
            .AddCondition(facts => Criteria.FactValueEquals(facts, "in_inventory", "book"))
            .AddCondition(facts => Criteria.FactValueEquals(facts, "in_room_npc", "Woman"))
            .SetAction(facts =>
            {
                StoryTextHandler.invokeUpdateStoryDisplay(@"As you leave, the woman calls after you, ""Hey! That's my book!""");
            }));

    }

    public static void AddRule(Rule rule)
    {
        rules.Add(rule);
    }

    public static void Execute(IEnumerable<Fact> facts)
    {
        // LoggingUtil.LogList(facts.ToList());
        foreach (var rule in rules)
        {
            rule.Evaluate(facts);
        }
    }
}
