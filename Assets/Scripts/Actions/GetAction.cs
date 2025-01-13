using System.Collections.Generic;
using System.Linq;

public class GetAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What are you trying to get?";
    public string tooManyMessage { get; private set; } = "Try the following: get [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 2;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_GET;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        string target = inputTextArray[1];

        List<IItem> items = ActionUtil.FindItemsFieldContainsString(WorldState.GetInstance().player.currentLocation.roomItems, item => item.referenceName, target);

        ActionUtil.MatchZeroOneAndMany<IItem>(
            items,
            () => StoryTextHandler.invokeUpdateTextDisplay("You can't get that"),
            item =>
            {
                WorldState.GetInstance().player.AddToInventory(item);
                WorldState.GetInstance().player.currentLocation.RemoveItem(item);
            },
            items => StoryTextHandler.invokeUpdateTextDisplay(
                "Are you trying to get " + string.Join(" or ", items.Select(npc => npc.referenceName)))
        );
    }
}
