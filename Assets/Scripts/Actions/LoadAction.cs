public class LoadAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; }
    public int minInputCount { get; private set; } = 1;
    public int maxInputCount { get; private set; } = 1;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_LOADGAME;

    void IPlayerAction.Execute(string[] inputTextArray)
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
