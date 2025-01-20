using System.Collections.Generic;

public static class QueryRunner
{

    // 1) get concept facts 2) get player related 3) get world state facts
    public static void RunMoveFacts(Room movedFrom)
    {
        var facts = new List<Fact>
            {
                new Fact { key = "concept", value = "OnMove" },
            };

        movedFrom.npcs.ForEach(npc => facts.Add(new Fact { key = "in_room_npc", value = npc.referenceName }));

        WorldState.GetInstance().player.inventory.ForEach(item => facts.Add(new Fact { key = "in_inventory", value = item.referenceName }));

        RuleEngine.Execute(facts);
    }


}
