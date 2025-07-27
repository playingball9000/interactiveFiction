using System;
using System.Collections.Generic;
using System.Linq;

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
        // return rule.AddCriteria(Criterion.Create("", facts => conditions.All(condition => condition.Evaluate(facts))));
        // return rule.AddCriteria(facts => conditions.All(condition => condition(facts)));
    }
}
