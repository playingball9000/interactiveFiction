using System.Collections.Generic;
using System.Linq;

public class PutAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "Try the following: put [target] [destination]";
    public string tooManyMessage { get; private set; } = "Try the following: put [target] [destination]";
    public int minInputCount { get; private set; } = 3;
    public int maxInputCount { get; private set; } = 4;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Put;


    void IPlayerAction.Execute(ActionInput actionInput)
    {
        Player player = PlayerContext.Get;

        List<IStorage> storages = new List<IStorage>
        {
            player.currentRoom.roomItems,
            player.inventory
        };
        storages.AddRange(player.currentRoom.GetContainersInRoom());

        var (containerHoldingItem, items) = ActionUtil.FindItemsInAccessibleStorages(storages, actionInput.mainClause);

        // Not gettable means not puttable. Also can't put containers in other containers for now.
        items = items
            .Where(i => i.isGettable)
            .Where(i => i is not ContainerBase)
            .ToList();

        ActionUtil.MatchZeroOneAndMany(
            items,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't put that"),
            item =>
            {
                List<ContainerBase> containers = ActionUtil.FindContainersInRoom(player.currentRoom, actionInput.mainClause);

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
                        "Are you trying to put in " + StringUtil.CreateOrSeparatedString(containers.Select(item => item.GetDisplayName())))
                );
            },
            items => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to put " + StringUtil.CreateOrSeparatedString(items.Select(item => item.GetDisplayName())))
        );



    }
}
