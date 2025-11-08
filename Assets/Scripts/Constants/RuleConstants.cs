public static class RuleConstants
{
    public const string RULE_ENGINE_GENERAL = "rule_engine_general";
    public const string RULE_ENGINE_AREA = "rule_engine_area";

}

public enum RuleConcept
{
    OnGive,
    OnMove,
    OnExamine,
    OnCardComplete
}


public enum RuleKey
{
    Concept,
    CurrentLocation,
    CurrentRoom,
    CurrentArea,
    PlayerEquipment,
    PlayerAbilities,
    InInventory,
    InRoomNpc,
    InRoomItem,

    RoomVisited,
    ExaminableExamined,

    CardCompleted,

    ActionTargetNpc,
    ActionTargetExaminable,
    ActionMovedToLocation,
    ActionMovedFromLocation,
    ActionPlayerMovedToArea,
    ActionPlayerMovedToRoom,

    TurnsInCurrentRoom
}