using System;
using System.Linq;

using static FactExtensions;
using static RuleConcepts;
using static RuleKey;

public static class RuleExtensions
{
    public static Rule When(this Rule rule, Criterion criterion)
    {
        return rule.AddCriteria(criterion);
    }

    public static Rule And(this Rule rule, Criterion criterion)
    {
        return rule.AddCriteria(criterion);
    }

    public static Rule Do(this Rule rule, Action action)
    {
        return rule.SetAction(action);
    }

    public static Rule WhenAny(this Rule rule, params Criterion[] conditions)
    {
        return rule.AddCriteria(Criterion.Create("", facts => conditions.Any(condition => condition.Evaluate(facts))));
    }

    public static Rule WhenAll(this Rule rule, params Criterion[] conditions)
    {
        foreach (var condition in conditions)
        {
            rule = rule.And(condition);
        }
        return rule;
    }

    public static Rule CardIsComplete(this Rule rule, string cardCode)
    {
        return rule.WhenAll(
            FactExists(Concept, OnCardComplete),
            FactExists(CardCompleted, cardCode)
        );
    }
}
