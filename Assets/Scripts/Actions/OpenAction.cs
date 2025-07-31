using System.Collections.Generic;
using System.Linq;

public class OpenAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What are you trying to open?";
    public string tooManyMessage { get; private set; } = "Try the following: open [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    string IPlayerAction.playerActionCode { get; } = ActionConstants.ACTION_OPEN;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        List<ContainerBase> roomContainers = ActionUtil.FindContainersInRoom(PlayerContext.Get.currentRoom, actionInput.mainClause);
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
                    StoryTextHandler.invokeUpdateStoryDisplay("You open " + container.referenceName + "\nContents: " + container.ContentsToString());
                    container.isOpen = true;
                }
            },
            containers => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to open " + StringUtil.CreateOrSeparatedString(containers.Select(item => item.GetDisplayName())))
        );
    }
}
