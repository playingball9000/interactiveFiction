using System.Collections.Generic;
using System.Linq;

public class DropAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What are you trying to drop?";
    public string tooManyMessage { get; private set; } = "Try the following: drop [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    string IPlayerAction.playerActionCode { get; } = ActionConstants.ACTION_DROP;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        Player player = PlayerContext.Get;
        var inventory = player.GetInventory().OfType<IExaminable>();
        List<IExaminable> items = ActionUtil.ProcessMainClauseFromEnd(actionInput.mainClause, inventory);

        ActionUtil.MatchZeroOneAndMany(
            items.OfType<IItem>().ToList(),
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't drop that"),
            item =>
            {
                StoryTextHandler.invokeUpdateStoryDisplay("You drop " + item.GetDisplayName());
                player.RemoveFromInventory(item);
                player.currentRoom.AddItem(item);
            },
            items => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to drop " + StringUtil.CreateOrSeparatedString(items.Select(item => item.GetDisplayName())))
        );
    }
}
