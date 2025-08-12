public static class RuleConstants
{
    public const string RULE_ENGINE_GENERAL = "rule_engine_general";
    public const string RULE_ENGINE_AREA = "rule_engine_area";

}

public enum RuleConcepts
{
    OnGive,
    OnMove,
    OnCardComplete
}


public enum RuleKey
{
    Concept,
    CurrentRoom,
    CurrentArea,
    PlayerEquipment,
    PlayerAbilities,
    InInventory,
    InRoomNpc,
    InRoomItem,

    CardCompleted,

    ActionTargetNpc,
    ActionMovedToLocation,
    ActionMovedFromLocation,
    ActionPlayerMovedToArea,
    ActionPlayerMovedToRoom
}