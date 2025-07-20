using System.Collections.Generic;
using UnityEngine.EventSystems;

// For multiple rules, use the rule engine, for single rule, can use rule.Evaluate()
public static class RuleEngine
{
    private static List<Rule> rules = new();

    //TODO: probably a good idea to have different rulesets (concepts) for speed ie. move, action, etc.
    static RuleEngine()
    {
        rules.Add(new Rule()
            .AddCriteria(facts => Criterion.FactValueEquals(facts, RuleConstants.KEY_CONCEPT, RuleConstants.CONCEPT_ON_MOVE))
            .AddCriteria(facts => Criterion.FactExists(facts, RuleConstants.KEY_IN_INVENTORY, "item_old_book"))
            .AddCriteria(facts => Criterion.FactExists(facts, RuleConstants.KEY_IN_ROOM_NPC, "npc_kate"))
            .SetAction(facts =>
            {
                StoryTextHandler.invokeUpdateStoryDisplay(
                    TmpTextTagger.Color(@"As you leave, the woman calls after you, ""Hey! That's my book!""", UiConstants.TEXT_COLOR_NPC_TEXT),
                    UiConstants.EFFECT_TYPEWRITER);
            }));

        rules.Add(new Rule()
            .AddCriteria(facts => Criterion.FactValueEquals(facts, RuleConstants.KEY_CONCEPT, RuleConstants.CONCEPT_ON_MOVE))
            .AddCriteria(facts => Criterion.FactExists(facts, RuleConstants.KEY_ACTION_MOVED_TO_ROOM, RoomConstants.THE_ABYSS))
            .SetAction(facts =>
            {
                GameController.invokeShowExploreCanvas();
            }));

    }

    public static void AddRule(Rule rule)
    {
        rules.Add(rule);
    }

    /// <summary>
    /// Checks facts against internal rules and initiates action based on rule
    /// </summary>
    /// <param name="facts"></param>
    /// <returns>List of rules activated</returns>
    public static List<string> Execute(IEnumerable<Fact> facts)
    {
        // LoggingUtil.LogList(facts);

        List<string> executedRules = new();
        foreach (Rule rule in rules)
        {
            if (rule.Evaluate(facts))
            {
                executedRules.Add(rule.ruleName);
            }
        }
        return executedRules;
    }
}
