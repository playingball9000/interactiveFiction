public class MoveAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; } = "If you're trying to move, try just the direction: n,s,e,w";
    public int minInputCount { get; private set; } = 0;
    public int maxInputCount { get; private set; } = 2;
    // Move is just the direction you are moving in, No ref action name.
    string IPlayerAction.actionReferenceName { get; }

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        Exit exit = WorldState.GetInstance().player.currentLocation.exits.Find(e => e.exitDirection.ToString() == inputTextArray[0]);
        if (exit != null)
        {
            WorldState.GetInstance().player.currentLocation = exit.targetRoom;
            StoryTextHandler.invokeDisplayRoomText(exit.targetRoom);
        }
        else
        {
            StoryTextHandler.invokeUpdateTextDisplay("You can't go that way");
        }
    }
}
