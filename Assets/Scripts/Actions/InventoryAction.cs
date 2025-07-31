public class InventoryAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; } = "To check your inventory: i, inventory";
    public int minInputCount { get; private set; } = 0;
    public int maxInputCount { get; private set; } = 1;

    string IPlayerAction.playerActionCode { get; } = ActionConstants.ACTION_INVENTORY;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        Player player = PlayerContext.Get;
        string moneyString = $"Money: {player.money}";
        string inventoryString = player.GetInventoryString();
        string equipString = player.GetEquipmentToString();
        string relationsString = player.GetRelationshipsToString();
        StoryTextHandler.invokeUpdateStoryDisplay(
            moneyString + "\n" +
            inventoryString + "\n\n" +
            equipString + "\n\n" +
            relationsString
            );
    }
}
