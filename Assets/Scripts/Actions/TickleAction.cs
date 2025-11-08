using System.Collections.Generic;
using System.Linq;

public class TickleAction : IPlayerAction
{
    private static string actionVerb = "tickle";
    public string tooManyMessage { get; private set; } = $"Try the following: {actionVerb} [target] [body part]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 4;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Tickle;
    /*
    Really, the full pattern is [adj?] [noun] [part] [adj?] [noun]
    But I don't foresee a scenario needing that first adj (or even the 2nd adj if I'm clever)
    so really I need to account for [noun] [part] for default and [noun] [part] [noun] 

    */

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        Player player = PlayerContext.Get;

        List<string> mainClause = actionInput.mainClause;
        var npcs = PlayerContext.Get.currentRoom.npcs;
        List<ITickleable> npcMatch = ActionUtil.FindMatchingNpcs(mainClause, npcs).OfType<ITickleable>().ToList();

        // assuming part is in 2nd spot
        string bodyPart = mainClause[1];




        ActionUtil.MatchZeroOneAndMany(
            npcMatch,
            () => StoryTextHandler.invokeUpdateStoryDisplay($"You can't {actionVerb} that"),
            npc =>
            {

                BodyPart part = BodyPartsSynonyms.Get(bodyPart);

                if (part != BodyPart.Unknown)
                {
                    if (npc.availableSpots.Contains(part))
                    {

                        if (mainClause.Count > 2)
                        {
                            List<TickleToolBase> itemMatch = ActionUtil.FindMatchingItemsInStorageFromEnd(player.inventory, mainClause).OfType<TickleToolBase>().ToList();

                            ActionUtil.MatchZeroOneAndMany(
                                itemMatch,
                                () => StoryTextHandler.invokeUpdateStoryDisplay($"You can't {actionVerb} with that object"),
                                item =>
                                {
                                    bool reaction = npc.GetTickleReaction(bodyPart, item);

                                },
                                items => StoryTextHandler.invokeUpdateStoryDisplay(
                                    $"Are you trying to {actionVerb} with " + StringUtil.CreateOrSeparatedString(items.Select(npc => npc.GetDisplayName())))
                            );

                        }
                        bool reaction = npc.GetTickleReaction(bodyPart);

                    }
                    else
                    {
                        StoryTextHandler.invokeUpdateStoryDisplay($"You can't tickle {npc.GetDisplayName()} on that spot.");
                    }
                }
                else
                {
                    // Probably want to check if it's a tool in which case, can process
                }


                EventManager.Raise(GameEvent.ActionPerformed);
            },
            npcs => StoryTextHandler.invokeUpdateStoryDisplay(
                $"Are you trying to {actionVerb} " + StringUtil.CreateOrSeparatedString(npcs.Select(npc => npc.GetDisplayName())))
        );
    }
}
