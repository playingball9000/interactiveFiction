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
        Player player = PlayerContext.Get;

        // roomItems is itself a type of IStorage that holds the items in the room
        List<IStorage> storages = new List<IStorage>
        {
            player.currentRoom.roomItems
        };
        // GetRoomContainers gets the containers in the room like bags
        storages.AddRange(player.currentRoom.GetRoomContainers());

        // We want to search all items in the room and accessible containers
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
                "Are you trying to get " + StringUtil.CreateOrSeparatedString(items.Select(item => item.GetDisplayName())))
        );
    }
}
