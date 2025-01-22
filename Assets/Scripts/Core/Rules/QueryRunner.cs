using System.Collections.Generic;

//TODO: I feel like this should be some sort of interceptor or events driven thing...
public static class QueryRunner
{

    // 1) get concept facts 2) get player related 3) get world state facts
    public static void RunMoveFacts(Room movedFrom)
    {
        List<Fact> facts = new()
        {
            new Fact { key = "concept", value = "onMove" },
        };

        facts.AddRange(movedFrom.GetRoomFacts());
        facts.AddRange(WorldState.GetInstance().player.GetPlayerFacts());

        RuleEngine.Execute(facts);
    }
}
