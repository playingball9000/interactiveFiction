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
        List<IItem> roomItems = player.currentLocation.roomItems;
        ContainerBase containerHoldingItem = null;

        string itemBeingGiven = inputTextArray[1];

        // Check player's inventory first for giving
        List<IItem> items = ActionUtil.FindItemsFieldContainsString(player.inventory, item => item.referenceName, itemBeingGiven);
        // Check the room next if it's just sitting there. INTELLIGENCE
        if (!items.Any())
        {
            items = ActionUtil.FindItemsFieldContainsString(roomItems, item => item.referenceName, itemBeingGiven);
            itemLocation = ItemLocation.room;
        }

        if (!items.Any())
        {
            itemLocation = ItemLocation.container;
            // Check in open containers to get the item. INTELLIGENCE
            (containerHoldingItem, items) = ActionUtil.FindItemsInContainers(roomItems, itemBeingGiven);
        }

        ActionUtil.MatchZeroOneAndMany<IItem>(
            items,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't give that"),
            item =>
            {
                string target = inputTextArray.Length == 3
                ? inputTextArray[2] : (inputTextArray.Length == 4 && inputTextArray[2] == "to")
                ? inputTextArray[3] : null;

                List<NPC> npcs = ActionUtil.FindItemsFieldContainsString(player.currentLocation.npcs, npc => npc.referenceName, target);

                ActionUtil.MatchZeroOneAndMany<NPC>(
                    npcs,
                    () => StoryTextHandler.invokeUpdateStoryDisplay("Who do you want to give that to?"),
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
                                case ItemLocation.container:
                                    containerHoldingItem.RemoveItem(item);
                                    break;
                            }
                        }
                        else
                        {
                            StoryTextHandler.invokeUpdateStoryDisplay("Not an acceptable item to give to that person");
                        }
                    },
                    npcs => StoryTextHandler.invokeUpdateStoryDisplay(
                        "Are you trying to give the item to " + string.Join(" or ", npcs.Select(npc => npc.referenceName)))
                );
            },
            items => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to give " + string.Join(" or ", items.Select(item => item.referenceName)))
        );


    }
}
