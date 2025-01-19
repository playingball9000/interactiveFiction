using System.Collections.Generic;
using System.Linq;

public class CloseAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What are you trying to close?";
    public string tooManyMessage { get; private set; } = "Try the following: close [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 2;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_CLOSE;

    void IPlayerAction.Execute(string[] inputTextArray)
    {
        Player player = WorldState.GetInstance().player;
        List<IItem> roomItems = player.currentLocation.roomItems;

        List<IContainer> roomContainers = roomItems.OfType<IContainer>().ToList();

        string target = inputTextArray[1];

        List<IContainer> containers = ActionUtil.FindItemsFieldContainsString(roomContainers, item => item.referenceName, target);

        ActionUtil.MatchZeroOneAndMany<IContainer>(
            containers,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't close that"),
            container =>
            {
                if (container.isOpen)
                {
                    // TODO: Might be nice to have custom close text for each container
                    StoryTextHandler.invokeUpdateStoryDisplay("You close " + container.referenceName);
                    container.isOpen = false;
                }
                else
                {
                    StoryTextHandler.invokeUpdateStoryDisplay("That's already closed");
                }
            },
            containers => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to close " + string.Join(" or ", containers.Select(item => item.referenceName)))
        );
    }
}
