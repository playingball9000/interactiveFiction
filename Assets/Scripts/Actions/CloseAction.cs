using System.Collections.Generic;
using System.Linq;

public class CloseAction : IPlayerAction
{
    private static string actionVerb = "close";

    public string tooManyMessage { get; private set; } = $"Try the following: {actionVerb} [target]";
    public int minInputCount { get; private set; } = 2;
    public int maxInputCount { get; private set; } = 3;

    PlayerAction IPlayerAction.playerActionCode { get; } = PlayerAction.Close;

    void IPlayerAction.Execute(ActionInput actionInput)
    {
        List<ContainerBase> roomContainers = ActionUtil.FindContainersInRoom(PlayerContext.Get.currentRoom, actionInput.mainClause);

        ActionUtil.MatchZeroOneAndMany(
            roomContainers,
            () => StoryTextHandler.invokeUpdateStoryDisplay($"You can't {actionVerb} that"),
            container =>
            {
                if (container.isOpen)
                {
                    // TODO: Might be nice to have custom close text for each container
                    StoryTextHandler.invokeUpdateStoryDisplay($"You {actionVerb} " + container.GetDisplayName());
                    container.isOpen = false;
                    EventManager.Raise(GameEvent.ActionPerformed);
                }
                else
                {
                    StoryTextHandler.invokeUpdateStoryDisplay(container.GetDisplayName() + " is already closed");
                }
            },
            containers => StoryTextHandler.invokeUpdateStoryDisplay(
                $"Are you trying to {actionVerb} " + StringUtil.CreateOrSeparatedString(containers.Select(item => item.GetDisplayName())))
        );
    }
}
