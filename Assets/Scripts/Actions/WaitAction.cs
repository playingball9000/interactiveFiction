public class WaitAction : IPlayerAction
{
    public string tooManyMessage { get; private set; } = "Just 'wait' is good enough.";
    public int minInputCount { get; private set; } = 0;
    public int maxInputCount { get; private set; } = 1;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Wait;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        StoryTextHandler.invokeUpdateStoryDisplay("You stand around for a bit...");
        EventManager.Raise(GameEvent.ActionPerformed);
    }
}
