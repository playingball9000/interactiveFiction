using System.Collections.Generic;

[System.Serializable]
public class Card
{
    public string title;
    public string internalCode;
    public float timeToComplete; // In seconds

    public bool isLocked = true;
    public bool isComplete = false;
    public Rule unlockRule;

    public Card(string title, float timeToComplete, string internalCode)
    {
        this.title = title;
        this.timeToComplete = timeToComplete;
        this.unlockRule = CardRulesRegistry.GetRule(internalCode);
        this.internalCode = internalCode;

        isLocked = unlockRule != null;
    }

    public void RefreshIsLocked(List<Fact> cardFacts)
    {
        isLocked = !unlockRule.Evaluate(cardFacts);
    }

    public override string ToString()
    {
        return $"<b><color=#FFD700>[Card]</color></b>\n" +
               $"  • Title: <b>{title}</b>\n" +
               $"  • Code: {internalCode}\n" +
               $"  • Time to Complete: {timeToComplete:F1}s\n" +
               $"  • Locked: {(isLocked ? "<color=red>Yes</color>" : "<color=green>No</color>")}\n" +
               $"  • Complete: {(isComplete ? "<color=green>Yes</color>" : "<color=grey>No</color>")}\n" +
               $"  • Rule: {(unlockRule != null ? unlockRule.GetType().Name : "<i>None</i>")}";
    }

}
