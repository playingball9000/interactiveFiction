public class LoadAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; }
    public int minInputCount { get; private set; } = 1;
    public int maxInputCount { get; private set; } = 1;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.LoadGame;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        WorldState worldState = SaveManager.LoadGame();
        if (worldState == null)
        {
            StoryTextHandler.invokeUpdateStoryDisplay("No save game found");
            return;
        }
        else
        {
            StoryTextHandler.invokeUpdateStoryDisplay("Game has been loaded");
            WorldState.SetInstance(worldState);
        }
    }
}
