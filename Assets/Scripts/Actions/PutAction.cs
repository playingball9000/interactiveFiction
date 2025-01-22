using System.Collections.Generic;
using System.Linq;

public class PutAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "Try the following: put [target] [destination]";
    public string tooManyMessage { get; private set; } = "Try the following: put [target] [destination]";
    public int minInputCount { get; private set; } = 3;
    public int maxInputCount { get; private set; } = 4;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_PUT;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        Player player = WorldState.GetInstance().player;
        List<IItem> roomItems = player.currentLocation.roomItems;

        ItemLocation itemLocation = ItemLocation.inventory;
        string itemBeingPut = inputTextArray[1];

        //TODO: Finding the actionable items around the player seems like something to go into a Util, give, put, etc

        // Check player's inventory first for giving
        List<IItem> items = ActionUtil.FindItemsFieldContainsString(player.inventory, item => item.referenceName, itemBeingPut);

        //TODO: Not gettable probably also means not puttable.

        // Check the room next if it's just sitting there. INTELLIGENCE
        if (!items.Any())
        {
            items = ActionUtil.FindItemsFieldContainsString(player.currentLocation.roomItems, item => item.referenceName, itemBeingPut);
            itemLocation = ItemLocation.room;
        }

        //TODO: Check open containers cuz you can move from one container to another

        ActionUtil.MatchZeroOneAndMany<IItem>(
            items,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't put that"),
            item =>
            {
                string target = inputTextArray.Length == 3
                ? inputTextArray[2] : (inputTextArray.Length == 4 && (inputTextArray[2] == "in" || inputTextArray[2] == "into"))
                ? inputTextArray[3] : null;

                // TODO: Should I make it smart and open openable containers automatically?
                // TODO: Should I even have an 'open' option: I could have all containers auto open, but if locked, then contents hidden?
                List<IContainer> openRoomContainers = roomItems.OfType<IContainer>()
                    .Where(container => container.isOpen)
                    .ToList();
                List<IContainer> containers = ActionUtil.FindItemsFieldContainsString(openRoomContainers, item => item.referenceName, target);

                ActionUtil.MatchZeroOneAndMany<IContainer>(
                    containers,
                    () => StoryTextHandler.invokeUpdateStoryDisplay("You can't put into that"),
                    container =>
                    {
                        switch (itemLocation)
                        {
                            case ItemLocation.inventory:
                                player.RemoveFromInventory(item);
                                break;
                            case ItemLocation.room:
                                player.currentLocation.RemoveItem(item);
                                break;
                        }
                        StoryTextHandler.invokeUpdateStoryDisplay($"You put {item.referenceName} in {container.referenceName}");
                        container.AddItem(item);

                    },
                    containers => StoryTextHandler.invokeUpdateStoryDisplay(
                        "Are you trying to put in " + string.Join(" or ", containers.Select(item => item.referenceName)))
                );
            },
            items => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to put " + string.Join(" or ", items.Select(item => item.referenceName)))
        );



    }
}
