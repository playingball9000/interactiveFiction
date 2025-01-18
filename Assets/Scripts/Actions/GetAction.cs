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
        ContainerBase itemInThisContainer = null;

        //TODO: Exact match, blah blah blah
        string target = inputTextArray[1];

        List<IItem> roomItems = player.currentLocation.roomItems;
        List<IItem> items = ActionUtil.FindItemsFieldContainsString(roomItems, item => item.referenceName, target);

        // TODO: perhaps its better to make a dict of (ItemLocation -> list of items / containers). this is fine for now.
        if (!items.Any())
        {
            itemLocation = ItemLocation.container;
            // Check in open containers to get the item. INTELLIGENCE
            List<IContainer> openContainers = roomItems.OfType<IContainer>()
                .Where(container => container.isOpen)
                .ToList();

            foreach (IContainer container in openContainers)
            {
                List<IItem> matchedItems = ActionUtil.FindItemsFieldContainsString(container.contents, item => item.referenceName, target);
                if (matchedItems.Count == 1)
                {
                    itemInThisContainer = (ContainerBase)container;
                    items.Add(matchedItems[0]);
                    break;
                }
                else
                {
                    // if 0 or many matches so the MatchZeroOneAndMany() will take care of it.
                    items.AddRange(matchedItems);
                }

            }
            ;
        }

        ActionUtil.MatchZeroOneAndMany<IItem>(
            items,
            () => StoryTextHandler.invokeUpdateTextDisplay("You can't get that"),
            item =>
            {
                StoryTextHandler.invokeUpdateTextDisplay(item.pickUpNarration);
                if (item.isGettable)
                {
                    player.AddToInventory(item);

                    switch (itemLocation)
                    {
                        case ItemLocation.container:
                            itemInThisContainer.RemoveItem(item);
                            break;
                        case ItemLocation.room:
                            player.currentLocation.RemoveItem(item);
                            break;
                    }
                }
            },
            items => StoryTextHandler.invokeUpdateTextDisplay(
                "Are you trying to get " + string.Join(" or ", items.Select(item => item.referenceName)))
        );
    }
}
