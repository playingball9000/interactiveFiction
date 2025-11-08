
using System.Collections.Generic;
using System.Linq;

public class MoveAction : IPlayerAction
{
    public string tooManyMessage { get; private set; } = "If you're trying to move, try: n,s,e,w,enter";
    public int minInputCount { get; private set; } = 0;
    public int maxInputCount { get; private set; } = 4;
    // Move is just the direction you are moving in, No ref action name.
    PlayerAction IPlayerAction.playerActionCode { get; }

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        Player player = PlayerContext.Get;
        string movement = actionInput.actionTaken.ToString();
        List<Exit> roomExits = player.currentRoom.exits
            .Where(e => StringUtil.EqualsIgnoreCase(e.exitDirection.ToString(), movement))
            .ToList();

        // Enter is special because you need to enter "somewhere"
        if (StringUtil.EqualsIgnoreCase(movement, ExitDirection.Enter.ToString()))
        {
            roomExits = ActionUtil.FindPossibleExits(roomExits, actionInput.mainClause);
        }

        ActionUtil.MatchZeroOneAndMany(
            roomExits,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't go that way"),
            exit =>
            {
                if (exit.isTargetAccessible())
                {
                    QueryRunner.RunPreMoveFacts(player.currentLocation);
                    player.currentLocation = exit.targetDestination;
                    // Display room text description used to be here but was moved to rules engine
                    QueryRunner.RunPostMoveFacts(player.currentLocation);
                    player.playerMemory.Update(RuleKey.RoomVisited, player.currentRoom.internalCode);
                    player.playerMemory.Delete(RuleKey.TurnsInCurrentRoom);
                }
                else
                {
                    StoryTextHandler.invokeUpdateStoryDisplay(exit.getNotAccessibleReason());
                }
            },
            exits => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to go to " + StringUtil.CreateOrSeparatedString(exits.Select(item => item.targetDestination.displayName)))
        );
    }
}
