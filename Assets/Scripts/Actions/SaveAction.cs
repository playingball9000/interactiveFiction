public class SaveAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; }
    public int minInputCount { get; private set; } = 1;
    public int maxInputCount { get; private set; } = 1;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_SAVEGAME;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        GameState gameState = new GameState();
        //gameState.player = WorldState.GetInstance().player;
        //LoggingUtil.Log(gameState.player);
        gameState.test = 10;

        //Some things aren't serializable like scriptable and monobehavior

        SaveManager.SaveGame(gameState);

        gameState = SaveManager.LoadGame();
        LoggingUtil.Log(gameState.test);
        //WorldState.SetInstance(gameState.worldState);
        //DebugUtil.printPlayer();

    }
}
