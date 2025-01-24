using System;
using System.Collections.Generic;
using System.Linq;

/**
Rules are made up of criteria.
A criterion is basically a conditional around a fact.
A fact is a flat data structure (k/v pair) representing info about the game state.
*/
public class Rule
{
    private readonly List<Func<IEnumerable<Fact>, bool>> criteria = new();

    private Action<IEnumerable<Fact>> action;

    public string ruleName { get; set; }

    public Rule AddCriteria(Func<IEnumerable<Fact>, bool> criterion)
    {
        this.criteria.Add(criterion);
        return this;
    }

    public Rule SetAction(Action<IEnumerable<Fact>> action)
    {
        this.action = action;
        return this;
    }

    public bool Evaluate(IEnumerable<Fact> facts)
    {
        if (criteria.All(condition => condition(facts)))
        {
            action?.Invoke(facts);
            return true;
        }
        return false;
    }
}
