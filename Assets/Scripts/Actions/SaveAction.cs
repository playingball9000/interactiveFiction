public class SaveAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; }
    public int minInputCount { get; private set; } = 1;
    public int maxInputCount { get; private set; } = 1;
    string IPlayerAction.playerActionCode { get; } = ActionConstants.ACTION_SAVEGAME;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        SaveManager.SaveGame(WorldState.GetInstance());
        StoryTextHandler.invokeUpdateStoryDisplay("Game saved successfully");
    }
}
