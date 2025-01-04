using System.Collections.Generic;
using System.Linq;

public class InventoryAction : IPlayerAction
{
    public string tooFewMessage { get; private set; }
    public string tooManyMessage { get; private set; }
    public int minInputCount { get; private set; } = 0;
    public int maxInputCount { get; private set; } = 9001;

    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_INVENTORY;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        List<IItem> inventory = WorldState.GetInstance().player.inventory;
        if (inventory.Any() == true)
        {
            List<string> itemNames = inventory.Select(item => item.referenceName).ToList();
            DisplayTextHandler.invokeUpdateTextDisplay("Inventory:\n - " + string.Join(" - ", itemNames) + "\n");

        }
        else
        {
            DisplayTextHandler.invokeUpdateTextDisplay("You have nothing in your inventory");
        }
    }
}
