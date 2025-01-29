using System.Collections.Generic;
using System.Linq;

public class OpenAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What are you trying to open?";
    public string tooManyMessage { get; private set; } = "Try the following: open [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    string IPlayerAction.actionReferenceName { get; } = ActionConstants.ACTION_OPEN;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        List<ContainerBase> roomContainers = ActionUtil.FindContainersInRoom(WorldState.GetInstance().player.currentLocation, actionInput.mainClause);
        ActionUtil.MatchZeroOneAndMany(
            roomContainers,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't open that"),
            container =>
            {
                if (container.isOpen)
                {
                    StoryTextHandler.invokeUpdateStoryDisplay("That's already open");
                }
                // else if (container.isLocked)
                // {
                //     // TODO: maybe auto unlock if you have key or code? INTELLIGENCE
                //     StoryTextHandler.invokeUpdateStoryDisplay("Container is locked");
                // }
                else
                {
                    // TODO: Might be nice to have custom open text for each container

                    StoryTextHandler.invokeUpdateStoryDisplay("You open " + container.referenceName + "\nContents: " + container.ContentsToString());
                    container.isOpen = true;
                }
            },
            containers => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to open " + string.Join(" or ", containers.Select(item => item.referenceName)))
        );
    }
}
