
using System.Collections.Generic;
using System.Linq;

public class MoveAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; } = "If you're trying to move, try: n,s,e,w,enter";
    public int minInputCount { get; private set; } = 0;
    public int maxInputCount { get; private set; } = 4;
    // Move is just the direction you are moving in, No ref action name.
    string IPlayerAction.playerActionCode { get; }

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        string movement = actionInput.actionTaken;
        List<Exit> roomExits = WorldState.GetInstance().player.currentLocation.exits.Where(e => StringUtil.EqualsIgnoreCase(e.exitDirection.ToString(), movement)).ToList();
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
                    QueryRunner.RunMoveFacts(WorldState.GetInstance().player.currentLocation);
                    WorldState.GetInstance().player.currentLocation = exit.targetRoom;
                    WorldState.GetInstance().player.currentLocation.DisplayRoomStoryText();
                }
                else
                {
                    StoryTextHandler.invokeUpdateStoryDisplay(exit.getNotAccessibleReason());
                }
            },
            exits => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to go to " + StringUtil.CreateOrSeparatedString(exits.Select(item => item.targetRoom.displayName)))
        );
    }
}
