using System.Collections.Generic;

//TODO: I feel like this should be some sort of interceptor or events driven thing...
public static class QueryRunner
{

    // 1) get concept facts 2) get player related 3) get world state facts
    public static void RunPreMoveFacts(Room movedFrom)
    {
        List<Fact> facts = new()
        {
            new Fact { key = RuleConstants.KEY_CONCEPT, value = RuleConstants.CONCEPT_ON_MOVE },
            new Fact { key = RuleConstants.KEY_ACTION_MOVED_FROM_ROOM, value = movedFrom.internalCode }
        };

        facts.AddRange(movedFrom.GetRoomFacts());
        facts.AddRange(WorldState.GetInstance().player.GetPlayerFacts());

        // LoggingUtil.LogList(facts);

        RuleEngine.Execute(facts);
    }

    public static void RunPostMoveFacts(Room movedTo)
    {
        List<Fact> facts = new()
        {
            new Fact { key = RuleConstants.KEY_CONCEPT, value = RuleConstants.CONCEPT_ON_MOVE },
            new Fact { key = RuleConstants.KEY_ACTION_MOVED_TO_ROOM, value = movedTo.internalCode }
        };

        facts.AddRange(movedTo.GetRoomFacts());
        facts.AddRange(WorldState.GetInstance().player.GetPlayerFacts());

        // LoggingUtil.LogList(facts);

        RuleEngine.Execute(facts);
    }

    // public static void RunGiveFacts()
    // {
    //     List<Fact> facts = new()
    //     {
    //         new Fact { key = RuleConstants.KEY_CONCEPT, value = RuleConstants.CONCEPT_ON_GIVE },
    //     };

    //     facts.AddRange(WorldState.GetInstance().player.currentLocation.GetRoomFacts());
    //     facts.AddRange(WorldState.GetInstance().player.GetPlayerFacts());

    //     RuleEngine.Execute(facts);
    // }
}
