using System.Collections.Generic;
using System.Linq;

public class DropAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What are you trying to drop?";
    public string tooManyMessage { get; private set; } = "Try the following: drop [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 2;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_DROP;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        string target = inputTextArray[1];

        List<IItem> items = ActionUtil.GetInstance().FindItemsFieldContainsString(WorldState.GetInstance().player.inventory, item => item.referenceName, target);

        ActionUtil.GetInstance().MatchZeroOneAndMany<IItem>(
            items,
            () => DisplayTextHandler.invokeUpdateTextDisplay("You can't drop that"),
            item =>
            {
                WorldState.GetInstance().player.RemoveFromInventory(item);
                WorldState.GetInstance().player.currentLocation.AddItem(item);
            },
            items => DisplayTextHandler.invokeUpdateTextDisplay(
                "Are you trying to drop " + string.Join(" or ", items.Select(npc => npc.referenceName)))
        );
    }
}
