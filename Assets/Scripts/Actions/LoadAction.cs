public class LoadAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; }
    public int minInputCount { get; private set; } = 1;
    public int maxInputCount { get; private set; } = 1;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_LOADGAME;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        GameState gameState = SaveManager.LoadGame();
        if (gameState == null)
        {
            DisplayTextHandler.invokeUpdateTextDisplay("No save game found");
            return;
        }
        else
        {
            WorldState.SetInstance(gameState.worldState);
        }
    }
}
