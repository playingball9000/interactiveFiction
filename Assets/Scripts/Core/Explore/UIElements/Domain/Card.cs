using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public string title;
    public CardCode internalCode;
    public float baseTimeToComplete;

    public bool isLocked = true;
    public bool isComplete = false;
    public int completionCount = 0;

    public float minimumTime = .02f;

    List<ThresholdStage> scalingStages = new List<ThresholdStage>
    {
        new ThresholdStage(0, 0.85f),  // First completions = rapid improvement (15% faster per)
        new ThresholdStage(3, 0.9f),   // Growth slows slightly
        new ThresholdStage(6, 0.93f),  // Steady improvement
        new ThresholdStage(10, 0.95f), // Slight tapering
        new ThresholdStage(15, 0.98f), // Small gains
        new ThresholdStage(20, 0.99f), // Minimal gain, near mastery
    };

    public Card(string title, float baseTimeToComplete, CardCode internalCode)
    {
        this.title = title;
        this.baseTimeToComplete = baseTimeToComplete;
        this.internalCode = internalCode;

    }

    public float GetCurrentTimeToComplete()
    {
        float time = baseTimeToComplete;

        ThresholdStage activeStage = scalingStages[0];

        foreach (var stage in scalingStages)
        {
            if (completionCount >= stage.minCompletionCount)
            {
                activeStage = stage;
            }
            else
            {
                break;
            }
        }

        int scaledCompletions = completionCount - activeStage.minCompletionCount;
        float scaledTime = time * Mathf.Pow(activeStage.scalingFactor, scaledCompletions);
        return Mathf.Max(minimumTime, scaledTime);
    }

    public void MarkCompleted()
    {
        completionCount++;
        isComplete = true;
    }

    public void ResetCard()
    {
        isComplete = false;
    }

    public override string ToString()
    {
        return $"<b><color=#FFD700>[Card]</color></b>\n" +
               $"  • Title: <b>{title}</b>\n" +
               $"  • Code: {internalCode}\n" +
               $"  • Base Time: {baseTimeToComplete:F1}s\n" +
               $"  • Current Time: {GetCurrentTimeToComplete():F1}s\n" +
               $"  • Completions: {completionCount}\n" +
               $"  • Locked: {(isLocked ? "<color=red>Yes</color>" : "<color=green>No</color>")}\n" +
               $"  • Complete: {(isComplete ? "<color=green>Yes</color>" : "<color=grey>No</color>")}\n";
    }
}
