
using System.Linq;

public class UnlockAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What are you trying to unlock?";
    public string tooManyMessage { get; private set; } = "Try the following: unlock [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 4;
    string IPlayerAction.playerActionCode { get; } = ActionConstants.ACTION_UNLOCK;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        Player player = WorldState.GetInstance().player;
        var roomExits = ActionUtil.FindPossibleExits(player.currentRoom.exits, actionInput.mainClause);

        ActionUtil.MatchZeroOneAndMany(
            roomExits,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't unlock that"),
            exit =>
            {
                if (!exit.isLocked)
                {
                    StoryTextHandler.invokeUpdateStoryDisplay("That exit way is not locked.");
                }
                else
                {
                    bool matchingKey = player.inventory.contents.OfType<KeyBase>().Any(key => key.internalCode == exit.keyInternalCode);

                    if (matchingKey)
                    {
                        StoryTextHandler.invokeUpdateStoryDisplay("You unlock the way to " + exit.targetDestination.displayName);

                        exit.isLocked = false;
                        Exit exitBack = exit.DestinationRoom.exits.FirstOrDefault(e => e.targetDestination == player.currentRoom);
                        if (exitBack != null)
                        {
                            exitBack.isLocked = false;
                        }
                    }
                    else
                    {
                        StoryTextHandler.invokeUpdateStoryDisplay("You do not have the right key for " + exit.targetDestination.displayName);
                    }
                }
            },
            exits => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to unlock " + StringUtil.CreateOrSeparatedString(exits.Select(item => item.targetDestination.displayName)))
        );
    }
}
