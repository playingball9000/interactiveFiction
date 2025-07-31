using System.Collections.Generic;
using System.Linq;

public class ExamineAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What do you want to examine?";
    public string tooManyMessage { get; private set; } = "Try the following: examine [target] OR examine [target] [object]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 5;
    string IPlayerAction.playerActionCode { get; } = ActionConstants.ACTION_EXAMINE;

    private string CANT_ACTION_MESSAGE = "You can't examine that";

    /*
     * TODO: What this needs to do in the future is something like
     * 
     woman
     bag
     thin woman
     old bag
     woman bag
     woman old bag
     thin woman old bag

     * 
     * Really, i should look at the last item, exact match first, then partial match.
     If one match, that's it, if no matches, that's also it.
     If multiple matches, then start to disambiguate
        If no previous word, then ask user to be more specific
        Get previous word, check adjectives list for match
        if still ambiguous, check npc list for match, then item list for match
        and then check npc adj finally

        i can probably modify ProcessMainClauseFromEnd() to take in # of times to process. mostly 2, but this is 4 or until hte mainclause runs out
     * 

     New 5/1 - maybe the thing to do is construct a searchable string from each item. so you get the examinables from an npc
     so its like "name" + "adj" + "item"

     Or maybe start with a check for the first word in the clause, if it's an npc, go from there.
     */
    void IPlayerAction.Execute(ActionInput actionInput)
    {
        Player player = PlayerContext.Get;
        List<IExaminable> examinables = player.currentRoom.GetExaminableThings();
        examinables.AddRange(player.GetInventory());

        List<IExaminable> pm = ActionUtil.ProcessMainClauseFromEnd(actionInput.mainClause, examinables);

        ActionUtil.MatchZeroOneAndMany(
             pm,
             () => StoryTextHandler.invokeUpdateStoryDisplay(CANT_ACTION_MESSAGE),
             thing => StoryTextHandler.invokeUpdateStoryDisplay(thing.GetDescription()),
             things => StoryTextHandler.invokeUpdateStoryDisplay("Are you trying to examine " + StringUtil.CreateOrSeparatedString(things.Select(thing => thing.GetDisplayName())))
         );
    }
}
