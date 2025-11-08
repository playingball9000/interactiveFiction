using System.Collections.Generic;
using System.Linq;

public class TalkAction : IPlayerAction
{
    public string tooManyMessage { get; private set; } = "Try the following: talk [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Talk;


    void IPlayerAction.Execute(ActionInput actionInput)
    {
        var npcs = PlayerContext.Get.currentRoom.npcs;
        List<INPC> npcMatch = ActionUtil.FindMatchingNpcs(actionInput.mainClause, npcs);

        ActionUtil.MatchZeroOneAndMany(
            npcMatch,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't talk to that"),
            npc =>
            {
                if (npc is ComplexNPC complexNPC)
                {
                    StoryTextHandler.invokeUpdateStoryDisplay("You strike up a conversation...");
                    DialogueParser.invokeStartDialogue(complexNPC);
                }
                EventManager.Raise(GameEvent.ActionPerformed);
            },
            npcs => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to talk to " + StringUtil.CreateOrSeparatedString(npcs.Select(npc => npc.GetDisplayName())))
        );
    }
}
