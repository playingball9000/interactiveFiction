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
        CreateAction<EquipAction>(),
        CreateAction<UnequipAction>(),
    }
    .Concat(CreateMoveActions<MoveAction>())
    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


    private static KeyValuePair<string, IPlayerAction> CreateAction<T>() where T : IPlayerAction, new()
    {
        IPlayerAction action = new T();
        return new KeyValuePair<string, IPlayerAction>(action.playerActionCode, action);
    }

    private static KeyValuePair<string, IPlayerAction>[] CreateMoveActions<T>() where T : IPlayerAction, new()
    {
        IPlayerAction moveAction = new T();
        KeyValuePair<string, IPlayerAction>[] keyValuePairs = new KeyValuePair<string, IPlayerAction>[]
        {
            new(ExitDirection.North.ToString(), moveAction),
            new(ExitDirection.Northeast.ToString(), moveAction),
            new(ExitDirection.Northwest.ToString(), moveAction),
            new(ExitDirection.South.ToString(), moveAction),
            new(ExitDirection.Southeast.ToString(), moveAction),
            new(ExitDirection.Southwest.ToString(), moveAction),
            new(ExitDirection.East.ToString(), moveAction),
            new(ExitDirection.West.ToString(), moveAction),
            new(ExitDirection.Enter.ToString(), moveAction),
        };
        return keyValuePairs;
    }
}
