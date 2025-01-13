using System.Collections.Generic;
using System.Linq;

public class ExamineAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What do you want to examine?";
    public string tooManyMessage { get; private set; } = "Try the following: examine [target] OR examine [target] [object]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_EXAMINE;



    /*
     * What this needs to do in the future is
     * 1. look for a direct object match (item or npc or scenery ) ie. woman, bag
     * 2. look for a modifier match (item or npc) ie. thin woman, black bag
     * 3. look for sub item ie. bag (item)
     * 4. look for sub item modifier ie. black bag (full sentence: examine thin woman black bag)
     * 
     * in other words find the npc or item first, if there are more words after npc, assume its a sub item
     * most things will be 2 words for the foreseeable future
     * 
     */
    void IPlayerAction.Execute(string[] inputTextArray)
    {
        string target = inputTextArray[1];
        if (inputTextArray.Length == 2)
        {
            //TODO: try and find exact match first

            List<IExaminable> potentialMatches = ActionUtil.FindItemsFieldContainsString(WorldState.GetInstance().player.currentLocation.npcs, npc => npc.referenceName, target).ToList<IExaminable>();
            potentialMatches.AddRange(ActionUtil.FindItemsFieldContainsString(WorldState.GetInstance().player.currentLocation.roomItems, item => item.referenceName, target).ToList<IExaminable>());
            potentialMatches.AddRange(ActionUtil.FindItemsFieldContainsString(WorldState.GetInstance().player.currentLocation.roomScenery, item => item.referenceName, target).ToList<IExaminable>());
            potentialMatches.AddRange(ActionUtil.FindItemsFieldContainsString(WorldState.GetInstance().player.inventory, item => item.referenceName, target).ToList<IExaminable>());

            ActionUtil.MatchZeroOneAndMany<IExaminable>(
                 potentialMatches,
                 () => StoryTextHandler.invokeUpdateTextDisplay("You can't examine that"),
                 thing => StoryTextHandler.invokeUpdateTextDisplay(thing.GetDescription()),
                 things => StoryTextHandler.invokeUpdateTextDisplay("Be more specific about what you are examining ie. 'red ring' instead of 'ring'")
             );
        }
        else if (inputTextArray.Length == 3)
        {
            string objectName = string.Join(" ", inputTextArray, 1, inputTextArray.Length - 1);

            string item = inputTextArray[2];
            List<NPC> matchedNpcsInRoom = ActionUtil.FindItemsFieldContainsString(WorldState.GetInstance().player.currentLocation.npcs, npc => npc.referenceName, target);

            if (!matchedNpcsInRoom.Any())
            {
                StoryTextHandler.invokeUpdateTextDisplay("You can't examine that");
            }
            else if (matchedNpcsInRoom.Count >= 1)
            {
                List<IClothing> listOfAllClothes = matchedNpcsInRoom.SelectMany(npc => npc.clothes).ToList();
                List<IExaminable> potentialMatches = ActionUtil.FindItemsFieldContainsString(listOfAllClothes, clothingItem => clothingItem.referenceName, item).ToList<IExaminable>();
                ActionUtil.MatchZeroOneAndMany<IExaminable>(
                    potentialMatches,
                    () => StoryTextHandler.invokeUpdateTextDisplay("You can't examine that"),
                    thing => StoryTextHandler.invokeUpdateTextDisplay(thing.GetDescription()),
                    things => StoryTextHandler.invokeUpdateTextDisplay("Be more specific about what you are examining ie. 'red ring' instead of 'ring'")
                );
            }
        }
    }
}
