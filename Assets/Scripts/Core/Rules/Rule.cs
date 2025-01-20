using System;
using System.Collections.Generic;
using System.Linq;

public class Rule
{
    private readonly List<Func<IEnumerable<Fact>, bool>> conditions = new List<Func<IEnumerable<Fact>, bool>>();

    private Action<IEnumerable<Fact>> action;

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

    public void Evaluate(IEnumerable<Fact> facts)
    {
        if (conditions.All(condition => condition(facts)))
        {
            action?.Invoke(facts);
        }
    }
}
