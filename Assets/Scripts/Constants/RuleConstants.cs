public static class RuleConstants
{
    public const string CONCEPT_ON_MOVE = "onMove";
    public const string CONCEPT_ON_GIVE = "onGive";

    public const string CONCEPT_ON_CARD_COMPLETE = "onCardComplete";



    public const string RULE_ENGINE_GENERAL = "rule_engine_general";
    public const string RULE_ENGINE_AREA = "rule_engine_area";

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