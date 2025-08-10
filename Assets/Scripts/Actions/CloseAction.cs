using System.Collections.Generic;
using System.Linq;
using UnityEngine.Animations;

public class CloseAction : IPlayerAction
{
    public string tooFewMessage { get; private set; } = "What are you trying to close?";
    public string tooManyMessage { get; private set; } = "Try the following: close [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;
    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Close;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        List<ContainerBase> roomContainers = ActionUtil.FindContainersInRoom(PlayerContext.Get.currentRoom, actionInput.mainClause);

        ActionUtil.MatchZeroOneAndMany(
            roomContainers,
            () => StoryTextHandler.invokeUpdateStoryDisplay("You can't close that"),
            container =>
            {
                if (container.isOpen)
                {
                    // TODO: Might be nice to have custom close text for each container
                    StoryTextHandler.invokeUpdateStoryDisplay("You close " + container.GetDisplayName());
                    container.isOpen = false;
                }
                else
                {
                    StoryTextHandler.invokeUpdateStoryDisplay(container.GetDisplayName() + " is already closed");
                }
            },
            containers => StoryTextHandler.invokeUpdateStoryDisplay(
                "Are you trying to close " + StringUtil.CreateOrSeparatedString(containers.Select(item => item.GetDisplayName())))
        );
    }
}
