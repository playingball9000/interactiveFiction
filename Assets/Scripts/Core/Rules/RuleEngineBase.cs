using System.Collections.Generic;

public class RuleEngineBase
{
    protected List<Rule> rules = new();

    public void AddRule(Rule rule)
    {
        rules.Add(rule);
    }

    public List<string> Execute(IEnumerable<Fact> facts)
    {
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
