using System.Collections.Generic;
using System.Linq;

public class TalkAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "Who do you want to talk to?";
    public string tooManyMessage { get; private set; } = "Try the following: talk [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    string IPlayerAction.playerActionCode { get; } = ActionConstants.ACTION_TALK;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        var npcs = PlayerContext.Get.currentRoom.npcs.Cast<IExaminable>();
        List<IExaminable> npcMatch = ActionUtil.ProcessMainClauseFromEnd(actionInput.mainClause, npcs);

        ActionUtil.MatchZeroOneAndMany(
            npcMatch.Cast<NPC>().ToList(),
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't talk to that"),
            npc =>
            {
                StoryTextHandler.invokeUpdateStoryDisplay("You strike up a conversation...");
                DialogueParser.invokeStartDialogue(npc);
            },
            npcs => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to talk to " + StringUtil.CreateOrSeparatedString(npcs.Select(npc => npc.GetDisplayName())))
        );
    }
}
