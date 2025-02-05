public class LookAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; } = "Look is for viewing the space, try 'Examine' for investigating things";
    public int minInputCount { get; private set; } = 0;
    public int maxInputCount { get; private set; } = 1;
    string IPlayerAction.playerActionCode { get; } = ActionConstants.ACTION_LOOK;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        WorldState.GetInstance().player.currentLocation.DisplayRoomStoryText();
    }
}
