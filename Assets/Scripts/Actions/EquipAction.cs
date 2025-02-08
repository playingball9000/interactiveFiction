using System.Collections.Generic;
using System.Linq;

public class EquipAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What are you trying to equip?";
    public string tooManyMessage { get; private set; } = "Try the following: equip [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    string IPlayerAction.playerActionCode { get; } = ActionConstants.ACTION_EQUIP;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        //TODO: do i need all these toList() everywhere
        Player player = WorldState.GetInstance().player;
        List<IExaminable> items = ActionUtil.ProcessMainClauseFromEnd(actionInput.mainClause, player.GetInventory().Cast<IExaminable>().ToList());
        List<WearableBase> wearables = items.OfType<WearableBase>().ToList();

        ActionUtil.MatchZeroOneAndMany(
            wearables,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't equip that"),
            item =>
            {
                StoryTextHandler.invokeUpdateStoryDisplay("You equip " + item.GetDisplayName());

                List<IWearable> conflictingPieces = player.equipment.FindAll(e => e.layer == item.layer && e.slotsTaken.Intersect(item.slotsTaken).Any()).ToList();
                player.equipment.RemoveAll(e => conflictingPieces.Any(e2 => e2.internalCode == e.internalCode));
                player.AddToInventory(conflictingPieces.Cast<IItem>().ToList());

                player.RemoveFromInventory(item);
                player.equipment.Add(item);

            },
            items => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to equip " + StringUtil.CreateOrSeparatedString(items.Select(item => item.GetDisplayName())))
        );
    }
}
