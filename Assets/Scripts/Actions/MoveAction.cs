public class MoveAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; }
    public int minInputCount { get; private set; } = 0;
    public int maxInputCount { get; private set; } = 9001;
    // Move is just the direction you are moving in, No ref action name.
    string IPlayerAction.actionReferenceName { get; }

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        Exit exit = WorldState.GetInstance().player.currentLocation.exits.Find(e => e.exitDirection.ToString() == inputTextArray[0]);
        if (exit != null)
        {
            WorldState.GetInstance().player.currentLocation = exit.targetRoom;
            DisplayTextHandler.invokeDisplayRoomText(exit.targetRoom);

        }
        else
        {
            DisplayTextHandler.invokeUpdateTextDisplay("You can't go that way");
        }
    }
}
