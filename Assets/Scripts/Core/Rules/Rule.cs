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
    private readonly List<Criterion> criteria = new();

    private Action action;

    public string ruleName { get; set; }

    public static Rule Create(string name = "")
    {
        return new Rule { ruleName = name };
    }

    public Rule AddCriteria(Criterion criterion)
    {
        this.criteria.Add(criterion);
        return this;
    }

    public Rule SetAction(Action action)
    {
        this.action = action;
        return this;
    }

    public bool Evaluate(IEnumerable<Fact> facts)
    {
        if (criteria.All(c => c.Evaluate(facts)))
        {
            action?.Invoke();
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        string criteriaLogString = $"<b><color=orange>[Criteria]</color></b> {criteria.Count} total:\n";

        for (int i = 0; i < criteria.Count; i++)
        {
            criteriaLogString += $"  • {criteria[i]}\n";
        }
        return $"<b><color=#FFD700>[Rule]</color></b>\n" +
               $"  • Rule Name: <b>{ruleName}</b>\n" +
               $"  • <b>{criteriaLogString}</b>\n";
    }
}
