using System.Collections.Generic;
using System.Linq;

public class TalkAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "Who do you want to talk to?";
    public string tooManyMessage { get; private set; } = "Try the following: talk [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_TALK;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        List<IExaminable> npcs = ActionUtil.ProcessMainClauseFromEnd(actionInput.mainClause, WorldState.GetInstance().player.currentLocation.npcs.Cast<IExaminable>().ToList());

        ActionUtil.MatchZeroOneAndMany(
            npcs.Cast<NPC>().ToList(),
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't talk to that"),
            npc =>
            {
                DialogueParser.invokeStartDialogue(npc);
            },
            npcs => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to talk to " + string.Join(" or ", npcs.Select(npc => npc.referenceName)))
        );
    }
}
