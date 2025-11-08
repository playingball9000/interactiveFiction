using System.Collections.Generic;
using System.Linq;

public class ActionRegistry
{
    public static readonly Dictionary<PlayerAction, IPlayerAction> ActionsDict = new[]
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
        CreateAction<UnlockAction>(),
        CreateAction<TickleAction>(),
        CreateAction<InsultAction>(),
        CreateAction<BattleAction>(),
        CreateAction<WaitAction>(),
    }
    .Concat(CreateMoveActions<MoveAction>())
    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

    public static IPlayerAction Get(PlayerAction name)
    {
        return ActionsDict.TryGetValue(name, out var action) ? action : null;
    }

    private static KeyValuePair<PlayerAction, IPlayerAction> CreateAction<T>() where T : IPlayerAction, new()
    {
        IPlayerAction action = new T();
        return new KeyValuePair<PlayerAction, IPlayerAction>(action.playerActionCode, action);
    }

    private static KeyValuePair<PlayerAction, IPlayerAction>[] CreateMoveActions<T>() where T : IPlayerAction, new()
    {
        IPlayerAction moveAction = new T();
        KeyValuePair<PlayerAction, IPlayerAction>[] keyValuePairs = new KeyValuePair<PlayerAction, IPlayerAction>[]
        {
            new(PlayerAction.North, moveAction),
            new(PlayerAction.NorthEast, moveAction),
            new(PlayerAction.NorthWest, moveAction),
            new(PlayerAction.South, moveAction),
            new(PlayerAction.SouthEast, moveAction),
            new(PlayerAction.SouthWest, moveAction),
            new(PlayerAction.East, moveAction),
            new(PlayerAction.West, moveAction),
            new(PlayerAction.Enter, moveAction),
        };
        return keyValuePairs;
    }
}
