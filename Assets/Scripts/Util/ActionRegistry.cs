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
            new KeyValuePair<string, IPlayerAction>("n", moveAction),
            new KeyValuePair<string, IPlayerAction>("north", moveAction),
            new KeyValuePair<string, IPlayerAction>("s", moveAction),
            new KeyValuePair<string, IPlayerAction>("south", moveAction),
            new KeyValuePair<string, IPlayerAction>("e", moveAction),
            new KeyValuePair<string, IPlayerAction>("east", moveAction),
            new KeyValuePair<string, IPlayerAction>("w", moveAction),
            new KeyValuePair<string, IPlayerAction>("west", moveAction),
        };
        return keyValuePairs;
    }
}
