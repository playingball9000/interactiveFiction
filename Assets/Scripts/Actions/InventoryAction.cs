public class InventoryAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; } = "To check your inventory: i, inventory";
    public int minInputCount { get; private set; } = 0;
    public int maxInputCount { get; private set; } = 1;

    string IPlayerAction.playerActionCode { get; } = ActionConstants.ACTION_INVENTORY;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        string inventoryString = WorldState.GetInstance().player.GetInventoryString();
        string relationsString = WorldState.GetInstance().player.GetRelationshipsToString();
        StoryTextHandler.invokeUpdateStoryDisplay(inventoryString + "\n\n" + relationsString);
    }
}
