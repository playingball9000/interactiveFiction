using System.Collections.Generic;
using System.Linq;

public class DropAction : IPlayerAction
{
    private static string actionVerb = "drop";
    public string tooManyMessage { get; private set; } = "Try the following: drop [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Drop;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        Player player = PlayerContext.Get;
        var inventory = player.GetInventory().OfType<IExaminable>();
        List<IExaminable> items = ActionUtil.ProcessMainClauseFromEnd(actionInput.mainClause, inventory);

        ActionUtil.MatchZeroOneAndMany(
            items.OfType<IItem>().ToList(),
            () => StoryTextHandler.invokeUpdateStoryDisplay($"You can't {actionVerb} that"),
            item =>
            {
                StoryTextHandler.invokeUpdateStoryDisplay($"You {actionVerb} " + item.GetDisplayName());
                player.RemoveFromInventory(item);
                player.currentRoom.AddItem(item);
                EventManager.Raise(GameEvent.ActionPerformed);
            },
            items => StoryTextHandler.invokeUpdateStoryDisplay(
                $"Are you trying to {actionVerb} " + StringUtil.CreateOrSeparatedString(items.Select(item => item.GetDisplayName())))
        );
    }
}
