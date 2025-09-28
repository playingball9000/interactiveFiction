using System.Collections.Generic;
using System.Linq;

public class TickleAction : IPlayerAction
{
    private static string actionVerb = "tickle";
    public string tooManyMessage { get; private set; } = $"Try the following: {actionVerb} [target] [body part]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 4;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Tickle;

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
                StoryTextHandler.invokeUpdateStoryDisplay("They're not ticklish");
            },
            npcs => StoryTextHandler.invokeUpdateStoryDisplay(
                $"Are you trying to {actionVerb} " + StringUtil.CreateOrSeparatedString(npcs.Select(npc => npc.GetDisplayName())))
        );
    }
}
