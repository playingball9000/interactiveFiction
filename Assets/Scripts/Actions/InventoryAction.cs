public class InventoryAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; } = "To check your inventory: i, inventory";
    public int minInputCount { get; private set; } = 0;
    public int maxInputCount { get; private set; } = 1;

    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_INVENTORY;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        string inventoryString = WorldState.GetInstance().player.GetInventoryString();
        if (inventoryString.Length > 0)
        {
            StoryTextHandler.invokeUpdateTextDisplay(inventoryString);
        }
        else
        {
            StoryTextHandler.invokeUpdateTextDisplay("You have nothing in your inventory");
        }
    }
}
