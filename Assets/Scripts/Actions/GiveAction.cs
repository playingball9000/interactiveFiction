using System.Collections.Generic;
using System.Linq;

public class GiveAction : IPlayerAction
{
    public string tooManyMessage { get; private set; } = "Try the following: give [item] [target]";
    public int minInputCount { get; private set; } = 3;
    public int maxInputCount { get; private set; } = 5;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Give;


    //TODO: Should probably handle "give man the bar" phrasing as well
    void IPlayerAction.Execute(ActionInput actionInput)
    {
        Player player = PlayerContext.Get;

        // You can give from any "storage" in the room, might want to change that...
        List<IStorage> storages = new List<IStorage>
        {
            player.currentRoom.roomItems,
            player.inventory
        };

        storages.AddRange(player.currentRoom.GetContainersInRoom());

        var (containerHoldingItem, items) = ActionUtil.FindItemsInAccessibleStorages(storages, actionInput.mainClause);

        ActionUtil.MatchZeroOneAndMany(
            items,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't give that"),
            item =>
            {
                List<IExaminable> npcs = ActionUtil.ProcessMainClauseFromEnd(actionInput.mainClause, player.currentRoom.npcs.Cast<IExaminable>().ToList());

                ActionUtil.MatchZeroOneAndMany(
                    npcs.Cast<INPC>().ToList(),
                    () => StoryTextHandler.invokeUpdateStoryDisplay("Who do you want to give that to?"),
                    npc =>
                    {
                        bool accepted = npc.GetGiveReaction(item);
                        if (accepted)
                        {
                            containerHoldingItem.RemoveItem(item);
                            EventManager.Raise(GameEvent.ActionPerformed);
                        }
                        else
                        {
                            StoryTextHandler.invokeUpdateStoryDisplay("Not an acceptable item to give to that person");
                        }
                    },
                    npcs => StoryTextHandler.invokeUpdateStoryDisplay(
                        "Are you trying to give the item to " + StringUtil.CreateOrSeparatedString(npcs.Select(npc => npc.GetDisplayName())))
                );
            },
            items => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to give " + StringUtil.CreateOrSeparatedString(items.Select(item => item.GetDisplayName())))
        );


    }
}
