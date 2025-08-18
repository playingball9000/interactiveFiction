using System.Collections.Generic;
using System.Linq;

public class UnequipAction : IPlayerAction
{
    public string tooManyMessage { get; private set; } = "Try the following: unequip [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 4;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Unequip;


    void IPlayerAction.Execute(ActionInput actionInput)
    {
        //TODO: how to unequip npcs?
        Player player = PlayerContext.Get;
        List<IExaminable> items = ActionUtil.ProcessMainClauseFromEnd(actionInput.mainClause, player.equipment.Cast<IExaminable>());
        List<WearableBase> wearables = items.OfType<WearableBase>().ToList();

        ActionUtil.MatchZeroOneAndMany(
            wearables,
            () => StoryTextHandler.invokeUpdateStoryDisplay("There is nothing like that to take off"),
            item =>
            {
                StoryTextHandler.invokeUpdateStoryDisplay("You take off " + item.GetDisplayName());
                player.AddToInventory(item);
                player.equipment.Remove(item);
            },
            items => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to take off " + StringUtil.CreateOrSeparatedString(items.Select(item => item.GetDisplayName())))
        );
    }
}
