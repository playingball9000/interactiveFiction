using System.Collections.Generic;
using System.Linq;

public class GiveAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "Who do you want to give to?";
    public string tooManyMessage { get; private set; } = "Try the following: give [item] [target]";
    public int minInputCount { get; private set; } = 3;
    public int maxInputCount { get; private set; } = 4;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_GIVE;

    void IPlayerAction.Execute(string[] inputTextArray)
    {

        string toGive = inputTextArray[1];

        List<IItem> items = ActionUtil.FindItemsFieldContainsString(WorldState.GetInstance().player.inventory, item => item.referenceName, toGive);

        ActionUtil.MatchZeroOneAndMany<IItem>(
            items,
            () => StoryTextHandler.invokeUpdateTextDisplay("You can't give that"),
            item =>
            {
                string target = inputTextArray.Length == 3
                ? inputTextArray[2] : (inputTextArray.Length == 4 && inputTextArray[2] == "to")
                ? inputTextArray[3] : null;

                List<NPC> npcs = ActionUtil.FindItemsFieldContainsString(WorldState.GetInstance().player.currentLocation.npcs, npc => npc.referenceName, target);

                ActionUtil.MatchZeroOneAndMany<NPC>(
                    npcs,
                    () => StoryTextHandler.invokeUpdateTextDisplay("You can't give to them"),
                    npc =>
                    {
                        WorldState.GetInstance().player.RemoveFromInventory(item);
                        npc.GetGiveReaction(item);
                    },
                    npcs => StoryTextHandler.invokeUpdateTextDisplay(
                        "Are you trying to give to " + string.Join(" or ", npcs.Select(npc => npc.referenceName)))
                );
            },
            items => StoryTextHandler.invokeUpdateTextDisplay(
                "Are you trying to give " + string.Join(" or ", items.Select(item => item.referenceName)))
        );


    }
}
