using System.Collections.Generic;
using System.Linq;

public class PutAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "Try the following: put [target] [destination]";
    public string tooManyMessage { get; private set; } = "Try the following: put [target] [destination]";
    public int minInputCount { get; private set; } = 3;
    public int maxInputCount { get; private set; } = 4;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_PUT;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        Player player = WorldState.GetInstance().player;
        List<IItem> roomItems = player.currentLocation.GetRoomItems();

        List<IStorage> storages = new List<IStorage>
        {
            player.currentLocation.roomItems,
            player.inventory
        };

        var (containerHoldingItem, items) = ActionUtil.FindItemsInAccessibleStorages(storages, actionInput.mainClause);
        items = items.Where(i => i.isGettable == true).ToList();
        //TODO: Check open containers cuz you can move from one container to another
        ActionUtil.MatchZeroOneAndMany(
            items,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't put that"),
            item =>
            {
                List<ContainerBase> containers = ActionUtil.FindContainersInRoom(player.currentLocation, actionInput.mainClause);

                ActionUtil.MatchZeroOneAndMany(
                    containers,
                    () => StoryTextHandler.invokeUpdateStoryDisplay("You can't put into that"),
                    container =>
                    {
                        StoryTextHandler.invokeUpdateStoryDisplay($"You put {item.GetDisplayName()} in {container.GetDisplayName()}");
                        containerHoldingItem.RemoveItem(item);
                        container.AddItem(item);
                    },
                    containers => StoryTextHandler.invokeUpdateStoryDisplay(
                        "Are you trying to put in " + string.Join(" or ", containers.Select(item => item.GetDisplayName())))
                );
            },
            items => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to put " + string.Join(" or ", items.Select(item => item.GetDisplayName())))
        );



    }
}
