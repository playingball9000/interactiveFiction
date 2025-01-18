using System.Collections.Generic;
using System.Linq;

public class OpenAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What are you trying to open?";
    public string tooManyMessage { get; private set; } = "Try the following: open [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 2;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_OPEN;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        Player player = WorldState.GetInstance().player;
        List<IItem> roomItems = player.currentLocation.roomItems;

        List<IContainer> roomContainers = roomItems.OfType<IContainer>()
                .ToList();

        //TODO: Exact match, blah blah blah
        string target = inputTextArray[1];

        List<IContainer> containers = ActionUtil.FindItemsFieldContainsString(roomContainers, item => item.referenceName, target);

        ActionUtil.MatchZeroOneAndMany<IContainer>(
            containers,
            () => StoryTextHandler.invokeUpdateTextDisplay("You can't open that"),
            container =>
            {
                if (container.isOpen)
                {
                    StoryTextHandler.invokeUpdateTextDisplay("That's already open");
                }
                else if (container.isLocked)
                {
                    // TODO: maybe auto unlock if you have key or code? INTELLIGENCE
                    StoryTextHandler.invokeUpdateTextDisplay("Container is locked");
                }
                else
                {
                    // TODO: Might be nice to have custom open text for each container
                    StoryTextHandler.invokeUpdateTextDisplay("You open " + container.referenceName);
                    container.isOpen = true;
                }
            },
            containers => StoryTextHandler.invokeUpdateTextDisplay(
                "Are you trying to open " + string.Join(" or ", containers.Select(item => item.referenceName)))
        );
    }
}
