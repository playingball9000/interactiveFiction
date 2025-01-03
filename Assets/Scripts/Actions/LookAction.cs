public class LookAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; }
    public int minInputCount { get; private set; } = 0;
    public int maxInputCount { get; private set; } = 9001;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_LOOK;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        DisplayTextHandler.invokeDisplayRoomText(WorldState.GetInstance().player.currentLocation);
    }
}
