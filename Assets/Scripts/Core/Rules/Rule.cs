using System;
using System.Collections.Generic;
using System.Linq;

public class Rule
{
    private readonly List<Func<IEnumerable<Fact>, bool>> conditions = new();

    private Action<IEnumerable<Fact>> action;

    public string ruleName { get; set; }

    public Rule AddCondition(Func<IEnumerable<Fact>, bool> condition)
    {
        conditions.Add(condition);
        return this;
    }

    public Rule SetAction(Action<IEnumerable<Fact>> action)
    {
        this.action = action;
        return this;
    }

    public bool Evaluate(IEnumerable<Fact> facts)
    {
        if (conditions.All(condition => condition(facts)))
        {
            action?.Invoke(facts);
            return true;
        }
        return false;
    }
}
