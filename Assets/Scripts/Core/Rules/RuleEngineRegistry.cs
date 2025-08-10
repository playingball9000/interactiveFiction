using System.Collections.Generic;

using static FactExtensions;
using static RuleConstants;

public static class RuleEngineRegistry
{
    private static Dictionary<string, RuleEngineBase> engines = new();

    static RuleEngineRegistry()
    {
        setupGeneralEngine();
    }

    public static void Register(string name, RuleEngineBase engine)
    {
        engines[name] = engine;
    }

    public static RuleEngineBase Get(string name)
    {
        return engines.TryGetValue(name, out var engine) ? engine : null;
    }

    private static void setupGeneralEngine()
    {
        RuleEngineBase engine = new();

        engine.AddRule(Rule.Create("Leave with book")
            .AddCriteria(Criterion.Create("", facts => Criterion.FactExists(facts, RuleKey.Concept, RuleConstants.CONCEPT_ON_MOVE)))
            .AddCriteria(Criterion.Create("", facts => Criterion.FactExists(facts, RuleKey.InInventory, "item_old_book")))
            .AddCriteria(Criterion.Create("", facts => Criterion.FactExists(facts, RuleKey.InRoomNpc, "npc_kate")))
            .SetAction(() =>
            {
                StoryTextHandler.invokeUpdateStoryDisplay(
                    TmpTextTagger.Color(@"As you leave, the woman calls after you, ""Hey! That's my book!""", UiConstants.TEXT_COLOR_NPC_TEXT),
                    UiConstants.EFFECT_TYPEWRITER);
            }));

        engine.AddRule(Rule.Create("Player moves from Room to Area")
            .WhenAll(
                FactExists(RuleKey.Concept, CONCEPT_ON_MOVE),
                FactIsTrue(RuleKey.ActionPlayerMovedToArea)
            )
            .Do(() =>
            {
                // EventManager.Raise(GameEvent.EnterArea);
                GameController.invokeShowExploreCanvas();

            }));

        engine.AddRule(Rule.Create("Player moves from Room to Room")
            .WhenAll(
                FactExists(RuleKey.Concept, CONCEPT_ON_MOVE),
                FactIsTrue(RuleKey.ActionPlayerMovedToRoom)
            )
            .Do(() =>
            {
                //TODO: Probably should be an event that does stuff
                PlayerContext.Get.currentRoom.DisplayRoomStoryText();
            }));

        Register(RULE_ENGINE_GENERAL, engine);
    }
}
