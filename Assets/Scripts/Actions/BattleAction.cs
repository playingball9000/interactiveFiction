using System.Collections.Generic;
using System.Linq;

public class BattleAction : IPlayerAction
{
    private static string actionVerb = "battle";
    public string tooManyMessage { get; private set; } = $"Try the following: {actionVerb} [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Battle;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        var npcs = PlayerContext.Get.currentRoom.npcs.Cast<IExaminable>();
        List<IExaminable> npcMatch = ActionUtil.ProcessMainClauseFromEnd(actionInput.mainClause, npcs);

        ActionUtil.MatchZeroOneAndMany(
            npcMatch.Cast<NPC>().ToList(),
            () => StoryTextHandler.invokeUpdateStoryDisplay($"You can't {actionVerb} that"),
            npc =>
            {
                //TODO: fill this in
                StoryTextHandler.invokeUpdateStoryDisplay("They don't want to battle");
            },
            npcs => StoryTextHandler.invokeUpdateStoryDisplay(
                $"Are you trying to {actionVerb} " + StringUtil.CreateOrSeparatedString(npcs.Select(npc => npc.GetDisplayName())))
        );
    }
}
