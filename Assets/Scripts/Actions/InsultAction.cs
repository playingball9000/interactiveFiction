using System.Collections.Generic;
using System.Linq;

public class InsultAction : IPlayerAction
{
    private static string actionVerb = "insult";
    public string tooManyMessage { get; private set; } = $"Try the following: {actionVerb} [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Insult;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        var npcs = PlayerContext.Get.currentRoom.npcs.Cast<IExaminable>();
        List<IExaminable> npcMatch = ActionUtil.ProcessMainClauseFromEnd(actionInput.mainClause, npcs);

        ActionUtil.MatchZeroOneAndMany(
            npcMatch.Cast<INPC>().ToList(),
            () => StoryTextHandler.invokeUpdateStoryDisplay($"You can't {actionVerb} that"),
            npc =>
            {
                //TODO: fill this in
                StoryTextHandler.invokeUpdateStoryDisplay("They don't care");
            },
            npcs => StoryTextHandler.invokeUpdateStoryDisplay(
                $"Are you trying to {actionVerb} " + StringUtil.CreateOrSeparatedString(npcs.Select(npc => npc.GetDisplayName())))
        );
    }
}
