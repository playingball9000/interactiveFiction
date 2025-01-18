using System.Collections.Generic;
using System.Linq;

public class GiveAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "Who do you want to give to?";
    public string tooManyMessage { get; private set; } = "Try the following: give [item] [target]";
    public int minInputCount { get; private set; } = 3;
    public int maxInputCount { get; private set; } = 4;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_GIVE;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        Player player = WorldState.GetInstance().player;
        ItemLocation itemLocation = ItemLocation.inventory;
        string itemBeingGiven = inputTextArray[1];
        //TODO: this needs exact match also

        // Check player's inventory first for giving
        List<IItem> items = ActionUtil.FindItemsFieldContainsString(player.inventory, item => item.referenceName, itemBeingGiven);
        // Check the room next if it's just sitting there. INTELLIGENCE
        if (!items.Any())
        {
            items = ActionUtil.FindItemsFieldContainsString(player.currentLocation.roomItems, item => item.referenceName, itemBeingGiven);
            itemLocation = ItemLocation.room;
        }
        //TODO: Check open containers

        ActionUtil.MatchZeroOneAndMany<IItem>(
            items,
            () => StoryTextHandler.invokeUpdateTextDisplay("You can't give that"),
            item =>
            {
                string target = inputTextArray.Length == 3
                ? inputTextArray[2] : (inputTextArray.Length == 4 && inputTextArray[2] == "to")
                ? inputTextArray[3] : null;

                List<NPC> npcs = ActionUtil.FindItemsFieldContainsString(player.currentLocation.npcs, npc => npc.referenceName, target);

                ActionUtil.MatchZeroOneAndMany<NPC>(
                    npcs,
                    () => StoryTextHandler.invokeUpdateTextDisplay("Who do you want to give that to?"),
                    npc =>
                    {
                        bool accepted = npc.GetGiveReaction(item);
                        if (accepted)
                        {
                            switch (itemLocation)
                            {
                                case ItemLocation.inventory:
                                    player.RemoveFromInventory(item);
                                    break;
                                case ItemLocation.room:
                                    player.currentLocation.RemoveItem(item);
                                    break;
                            }
                        }
                        else
                        {
                            StoryTextHandler.invokeUpdateTextDisplay("Not an acceptable item to give to that person");
                        }
                    },
                    npcs => StoryTextHandler.invokeUpdateTextDisplay(
                        "Are you trying to give the item to " + string.Join(" or ", npcs.Select(npc => npc.referenceName)))
                );
            },
            items => StoryTextHandler.invokeUpdateTextDisplay(
                "Are you trying to give " + string.Join(" or ", items.Select(item => item.referenceName)))
        );


    }
}
