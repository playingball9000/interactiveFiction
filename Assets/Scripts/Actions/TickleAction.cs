using System.Collections.Generic;
using System.Linq;

public class TickleAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "Who do you want to tickle?";
    public string tooManyMessage { get; private set; } = "Try the following: tickle [target] [body part]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 4;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Tickle;


    void IPlayerAction.Execute(ActionInput actionInput)
    {
        var npcs = PlayerContext.Get.currentRoom.npcs.Cast<IExaminable>();
        List<IExaminable> npcMatch = ActionUtil.ProcessMainClauseFromEnd(actionInput.mainClause, npcs);

        ActionUtil.MatchZeroOneAndMany(
            npcMatch.Cast<NPC>().ToList(),
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't tickle that"),
            npc =>
            {
                //TODO: fill this in
                StoryTextHandler.invokeUpdateStoryDisplay("They're not ticklish");
            },
            npcs => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to tickle " + StringUtil.CreateOrSeparatedString(npcs.Select(npc => npc.GetDisplayName())))
        );
    }
}
