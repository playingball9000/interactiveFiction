using System.Collections.Generic;

//TODO: I feel like this should be some sort of interceptor or events driven thing...
public static class QueryRunner
{

    // 1) get concept facts 2) get player related 3) get world state facts
    public static void RunPreMoveFacts(ILocation movedFrom)
    {
        List<Fact> facts = new()
        {
            new Fact { key = RuleConstants.KEY_CONCEPT, value = RuleConstants.CONCEPT_ON_MOVE },
            new Fact { key = RuleConstants.KEY_ACTION_MOVED_FROM_LOCATION, value = movedFrom.internalCode }
        };
        if (movedFrom is Room)
        {
            Room r = (Room)movedFrom;
            facts.AddRange(r.GetRoomFacts());
        }
        facts.AddRange(WorldState.GetInstance().player.GetPlayerFacts());

        // LoggingUtil.LogList(facts);

        RuleEngine.Execute(facts);
    }

    public static void RunPostMoveFacts(ILocation movedTo)
    {
        List<Fact> facts = new()
        {
            new Fact { key = RuleConstants.KEY_CONCEPT, value = RuleConstants.CONCEPT_ON_MOVE },
            new Fact { key = RuleConstants.KEY_ACTION_MOVED_TO_LOCATION, value = movedTo.internalCode }
        };

        if (movedTo is Room)
        {
            Room r = (Room)movedTo;
            facts.AddRange(r.GetRoomFacts());
            facts.Add(new Fact { key = RuleConstants.KEY_ACTION_PLAYER_MOVED_TO_ROOM, value = true });
        }
        else
        {
            facts.Add(new Fact { key = RuleConstants.KEY_ACTION_PLAYER_MOVED_TO_AREA, value = true });
        }
        facts.AddRange(WorldState.GetInstance().player.GetPlayerFacts());

        RuleEngine.Execute(facts);
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
}
