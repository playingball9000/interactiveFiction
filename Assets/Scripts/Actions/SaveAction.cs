public class SaveAction : IPlayerAction
{
    public string tooManyMessage { get; private set; }
    public int minInputCount { get; private set; } = 1;
    public int maxInputCount { get; private set; } = 1;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.SaveGame;


    void IPlayerAction.Execute(ActionInput actionInput)
    {
        SaveManager.SaveGame(WorldState.GetInstance());
        StoryTextHandler.invokeUpdateStoryDisplay("Game saved successfully");
    }
}
