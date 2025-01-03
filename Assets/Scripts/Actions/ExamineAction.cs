using System.Collections.Generic;
using System.Linq;

public class ExamineAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What do you want to examine?";
    public string tooManyMessage { get; private set; } = "Try the following: examine [target] OR examine [target] [object]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_EXAMINE;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        string target = inputTextArray[1];
        if (inputTextArray.Length == 2)
        {
            //TODO: try and find exact match first

            List<IExaminable> examinableThings = ActionUtil.GetInstance().FindItemsFieldContainsString(WorldState.GetInstance().player.currentLocation.npcs, npc => npc.referenceName, target).ToList<IExaminable>();

            ActionUtil.GetInstance().MatchZeroOneAndMany<IExaminable>(
                 examinableThings,
                 () => DisplayTextHandler.invokeUpdateTextDisplay("You can't examine that"),
                 thing => DisplayTextHandler.invokeUpdateTextDisplay(thing.GetDescription()),
                 things => DisplayTextHandler.invokeUpdateTextDisplay("Be more specific about what you are examining ie. 'red ring' instead of 'ring'")
             );
        }
        else if (inputTextArray.Length == 3)
        {
            string item = inputTextArray[2];
            List<NPC> matchedNpcsInRoom = ActionUtil.GetInstance().FindItemsFieldContainsString(WorldState.GetInstance().player.currentLocation.npcs, npc => npc.referenceName, target);

            if (!matchedNpcsInRoom.Any())
            {
                DisplayTextHandler.invokeUpdateTextDisplay("You can't examine that");
            }
            else if (matchedNpcsInRoom.Count >= 1)
            {
                List<IClothing> listOfAllClothes = matchedNpcsInRoom.SelectMany(npc => npc.clothes).ToList();
                List<IExaminable> examinableThings = ActionUtil.GetInstance().FindItemsFieldContainsString(listOfAllClothes, clothingItem => clothingItem.referenceName, item).ToList<IExaminable>();
                ActionUtil.GetInstance().MatchZeroOneAndMany<IExaminable>(
                    examinableThings,
                    () => DisplayTextHandler.invokeUpdateTextDisplay("You can't examine that"),
                    thing => DisplayTextHandler.invokeUpdateTextDisplay(thing.GetDescription()),
                    things => DisplayTextHandler.invokeUpdateTextDisplay("Be more specific about what you are examining ie. 'red ring' instead of 'ring'")
                );
            }
        }
    }
}
