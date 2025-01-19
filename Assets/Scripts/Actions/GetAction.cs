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
        Player player = WorldState.GetInstance().player;
        ItemLocation itemLocation = ItemLocation.room;
        ContainerBase containerHoldingItem = null;

        string target = inputTextArray[1];

        List<IItem> roomItems = player.currentLocation.roomItems;
        List<IItem> items = ActionUtil.FindItemsFieldContainsString(roomItems, item => item.referenceName, target);

        // TODO: perhaps its better to make a dict of (ItemLocation -> list of items / containers). this is fine for now.
        if (!items.Any())
        {
            itemLocation = ItemLocation.container;
            // Check in open containers to get the item. INTELLIGENCE
            (containerHoldingItem, items) = ActionUtil.FindItemsInContainers(roomItems, target);
        }

        ActionUtil.MatchZeroOneAndMany<IItem>(
            items,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't get that"),
            item =>
            {
                StoryTextHandler.invokeUpdateStoryDisplay(item.pickUpNarration);
                if (item.isGettable)
                {
                    player.AddToInventory(item);

                    switch (itemLocation)
                    {
                        case ItemLocation.container:
                            containerHoldingItem.RemoveItem(item);
                            break;
                        case ItemLocation.room:
                            player.currentLocation.RemoveItem(item);
                            break;
                    }
                }
            },
            items => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to get " + string.Join(" or ", items.Select(item => item.referenceName)))
        );
    }
}
