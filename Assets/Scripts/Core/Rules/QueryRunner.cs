using System.Collections.Generic;

//TODO: I feel like this should be some sort of interceptor or events driven thing...
public static class QueryRunner
{

    // 1) get concept facts 2) get player related 3) get world state facts
    public static void RunMoveFacts(Room movedFrom)
    {
        List<Fact> facts = new()
        {
            new Fact { key = RuleConstants.KEY_CONCEPT, value = RuleConstants.CONCEPT_ON_MOVE },
        };

        facts.AddRange(movedFrom.GetRoomFacts());
        facts.AddRange(WorldState.GetInstance().player.GetPlayerFacts());

        LoggingUtil.LogList(facts);

        RuleEngine.Execute(facts);
    }

    public static void RunGiveFacts()
    {
        List<Fact> facts = new()
        {
            new Fact { key = RuleConstants.KEY_CONCEPT, value = RuleConstants.CONCEPT_ON_GIVE },
        };

        facts.AddRange(WorldState.GetInstance().player.currentLocation.GetRoomFacts());
        facts.AddRange(WorldState.GetInstance().player.GetPlayerFacts());

        RuleEngine.Execute(facts);
    }
}
