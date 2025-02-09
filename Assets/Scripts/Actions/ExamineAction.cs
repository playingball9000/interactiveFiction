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

    //TODO: How do i handle something like "Bar of soap" without being "soap bar"

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
     */
    void IPlayerAction.Execute(ActionInput actionInput)
    {
        List<IExaminable> examinables = WorldState.GetInstance().player.currentLocation.GetExaminableThings();
        examinables.AddRange(WorldState.GetInstance().player.GetInventory());

        List<IExaminable> pm = ActionUtil.ProcessMainClauseFromEnd(actionInput.mainClause, examinables);

        ActionUtil.MatchZeroOneAndMany(
             pm,
             () => StoryTextHandler.invokeUpdateStoryDisplay(CANT_ACTION_MESSAGE),
             thing => StoryTextHandler.invokeUpdateStoryDisplay(thing.GetDescription()),
             things => StoryTextHandler.invokeUpdateStoryDisplay("Are you trying to examine " + StringUtil.CreateOrSeparatedString(things.Select(thing => thing.GetDisplayName())))
         );
    }
}
