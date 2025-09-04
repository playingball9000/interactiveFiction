using System.Collections.Generic;
using System.Linq;

//TODO: I feel like this should be some sort of interceptor or events driven thing...
public static class QueryRunner
{

    // 1) get concept facts 2) get player related 3) get world state facts
    public static void RunPreMoveFacts(ILocation movedFrom)
    {
        List<Fact> facts = new()
        {
            new Fact { key = RuleKey.Concept, value = RuleConcept.OnMove },
            new Fact { key = RuleKey.ActionMovedFromLocation, value = movedFrom.internalCode }
        };
        if (movedFrom is Room)
        {
            Room r = (Room)movedFrom;
            facts.AddRange(r.GetRoomFacts());
        }
        facts.AddRange(PlayerContext.Get.GetPlayerFacts());

        Log.LogList(facts);

        RuleEngineRegistry.Get(RuleConstants.RULE_ENGINE_GENERAL).Execute(facts);
    }

    public static void RunPostMoveFacts(ILocation movedTo)
    {
        List<Fact> facts = new()
        {
            new Fact { key = RuleKey.Concept, value = RuleConcept.OnMove },
            new Fact { key = RuleKey.ActionMovedToLocation, value = movedTo.internalCode }
        };

        if (movedTo is Room)
        {
            Room r = (Room)movedTo;
            facts.AddRange(r.GetRoomFacts());
            facts.Add(new Fact { key = RuleKey.ActionPlayerMovedToRoom, value = true });
        }
        else
        {
            facts.Add(new Fact { key = RuleKey.ActionPlayerMovedToArea, value = true });
        }
        facts.AddRange(PlayerContext.Get.GetPlayerFacts());

        RuleEngineRegistry.Get(RuleConstants.RULE_ENGINE_GENERAL).Execute(facts);
    }

    // public static void RunGiveFacts()
    // {
    //     List<Fact> facts = new()
    //     {
    //         new Fact { key = RuleConstants.KEY_CONCEPT, value = RuleConstants.CONCEPT_ON_GIVE },
    //     };

    //     facts.AddRange(WorldState.GetInstance().player.currentRoom.GetRoomFacts());
    //     facts.AddRange(WorldState.GetInstance().player.GetPlayerFacts());

    //     RuleEngine.Execute(facts);
    // }

    public static void RunCardCompleteFacts(List<Card> completedCards, List<Card> lockedCards)
    {
        List<Fact> facts = new()
        {
            Fact.Create(RuleKey.Concept, RuleConcept.OnCardComplete)
        };

        facts.AddRange(completedCards.Select(c => Fact.Create(RuleKey.CardCompleted, c.internalCode)).ToList());
        facts.AddRange(PlayerContext.Get.GetPlayerFacts());

        // Set isLocked to NOT isUnlocked basically
        lockedCards.ForEach(c => c.isLocked = !CardUtil.IsCardUnlocked(c, facts));
        // Log.LogList(facts);

    }



}
