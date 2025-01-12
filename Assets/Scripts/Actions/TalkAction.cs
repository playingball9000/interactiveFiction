using System.Collections.Generic;
using System.Linq;

public class TalkAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "Who do you want to talk to?";
    public string tooManyMessage { get; private set; } = "Try the following: talk [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_TALK;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        string target = inputTextArray.Length == 2 ? inputTextArray[1] : (inputTextArray.Length == 3 && inputTextArray[1] == "to") ? inputTextArray[2] : null;

        List<NPC> npcs = ActionUtil.GetInstance().FindItemsFieldContainsString(WorldState.GetInstance().player.currentLocation.npcs, npc => npc.referenceName, target);

        ActionUtil.GetInstance().MatchZeroOneAndMany<NPC>(
            npcs,
            () => DisplayTextHandler.invokeUpdateTextDisplay("You can't talk to that"),
            npc =>
            {
                DialogueParser.invokeStartDialogue(npc);
            },
            npcs => DisplayTextHandler.invokeUpdateTextDisplay(
                "Are you trying to talk to " + string.Join(" or ", npcs.Select(npc => npc.referenceName)))
        );
    }
}
