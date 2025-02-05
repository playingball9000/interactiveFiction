using System.Collections.Generic;
using System.Linq;

public class GetAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What are you trying to get?";
    public string tooManyMessage { get; private set; } = "Try the following: get [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    string IPlayerAction.playerActionCode { get; } = ActionConstants.ACTION_GET;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        Player player = WorldState.GetInstance().player;

        List<IStorage> storages = new List<IStorage>
        {
            player.currentLocation.roomItems
        };
        storages.AddRange(player.currentLocation.GetRoomContainers());

        var (containerHoldingItem, items) = ActionUtil.FindItemsInAccessibleStorages(storages, actionInput.mainClause);

        ActionUtil.MatchZeroOneAndMany(
            items,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't get that"),
            item =>
            {
                StoryTextHandler.invokeUpdateStoryDisplay(item.pickUpNarration);
                if (item.isGettable)
                {
                    player.AddToInventory(item);
                    containerHoldingItem.RemoveItem(item);
                }
            },
            items => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to get " + string.Join(" or ", items.Select(item => item.GetDisplayName())))
        );
    }
}
