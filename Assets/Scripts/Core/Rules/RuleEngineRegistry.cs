using System.Collections.Generic;

using static FactExtensions;
using static RuleConstants;
using static RuleKey;
using static RuleConcept;

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

        // Rules execute in the order they were added
        engine.AddRule(Rule.Create("Leave with book")
            .AddCriteria(Criterion.Create("", facts => Criterion.FactExists(facts, Concept, OnMove)))
            .AddCriteria(Criterion.Create("", facts => Criterion.FactExists(facts, InInventory, "item_old_book")))
            .AddCriteria(Criterion.Create("", facts => Criterion.FactExists(facts, InRoomNpc, "npc_kate")))
            .SetAction(() =>
            {
                StoryTextHandler.invokeUpdateStoryDisplay(
                    TmpTextTagger.Color(@"As you leave, the woman calls after you, ""Hey! That's my book!""", UiConstants.TEXT_COLOR_NPC_TEXT),
                    TextEffect.Typewriter);
            }));
        engine.AddRule(Rule.Create("First time in room with Mary")
        .WhenAll(
            FactExists(Concept, OnMove),
            FactExists(InRoomNpc, NpcCode.Mary_Hearth),
            FactNotExists(RoomVisited, LocationCode.AirshipCabin_r)
        )
        .Do(() =>
        {
            StoryTextHandler.invokeUpdateStoryDisplay(RoomIncidentRegistry.GetFlavorText("maryIntro"), TextEffect.Incidental);
        }));

        engine.AddRule(Rule.Create("Examine railing in airship deck")
        .WhenAll(
            FactExists(Concept, OnExamine),
            FactExists(ActionTargetExaminable, "railing"),
            FactIsFalse(ExaminableExamined),
            FactExists(CurrentLocation, LocationCode.AirshipDeck_r)

        )
        .Do(() =>
        {
            StoryTextHandler.invokeUpdateStoryDisplay("Whoa! Don't fall over!", TextEffect.Incidental);
        }));

        engine.AddRule(Rule.Create("3 Turns airship deck")
        .WhenAll(
            FactExists(CurrentLocation, LocationCode.AirshipDeck_r),
            FactExists(TurnsInCurrentRoom, 3)
        )
        .Do(() =>
        {
            StoryTextHandler.invokeUpdateStoryDisplay("A small bird flies by...", TextEffect.Incidental);
            PlayerContext.Get.currentRoom.AddItem(ItemFactory.CreateTickleItem(
            "feather",
            "brown",
            "A soft brown feather you got from a passing bird."
            ));
        }));

        engine.AddRule(Rule.Create("Player moves from Room to Area")
            .WhenAll(
                FactExists(Concept, OnMove),
                FactIsTrue(ActionPlayerMovedToArea)
            )
            .Do(() =>
            {
                GameController.invokeShowExploreCanvas();
            }));

        engine.AddRule(Rule.Create("Player moves from Room to Room")
            .WhenAll(
                FactExists(Concept, OnMove),
                FactIsTrue(ActionPlayerMovedToRoom)
            )
            .Do(() =>
            {
                //TODO: Probably should be an event that does stuff
                PlayerContext.Get.currentRoom.DisplayRoomStoryText();
            }));



        Register(RULE_ENGINE_GENERAL, engine);
    }
}
