using System.Collections.Generic;
using System.Linq;

public class ActionRegistry
{
    public static readonly Dictionary<string, IPlayerAction> ActionsDict = new[]
    {
        CreateAction<LookAction>(),
        CreateAction<TalkAction>(),
        CreateAction<ExamineAction>(),
        CreateAction<GetAction>(),
        CreateAction<DropAction>(),
        CreateAction<InventoryAction>(),
        CreateAction<SaveAction>(),
        CreateAction<LoadAction>(),
        CreateAction<GiveAction>(),
        CreateAction<OpenAction>(),
        CreateAction<CloseAction>(),
        CreateAction<PutAction>(),
    }
    .Concat(CreateMoveActions<MoveAction>())
    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


    private static KeyValuePair<string, IPlayerAction> CreateAction<T>() where T : IPlayerAction, new()
    {
        IPlayerAction action = new T();
        return new KeyValuePair<string, IPlayerAction>(action.actionReferenceName, action);
    }

    private static KeyValuePair<string, IPlayerAction>[] CreateMoveActions<T>() where T : IPlayerAction, new()
    {
        IPlayerAction moveAction = new T();
        KeyValuePair<string, IPlayerAction>[] keyValuePairs = new KeyValuePair<string, IPlayerAction>[]
        {
            new("n", moveAction),
            new("north", moveAction),
            new("s", moveAction),
            new("south", moveAction),
            new("e", moveAction),
            new("east", moveAction),
            new("w", moveAction),
            new("west", moveAction),
        };
        return keyValuePairs;
    }
}
