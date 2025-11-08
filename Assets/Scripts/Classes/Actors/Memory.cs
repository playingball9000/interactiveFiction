using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Memory
{
    private Dictionary<RuleKey, object> memory = new()
    {
        { RuleKey.RoomVisited, new HashSet<LocationCode>() },
    };

    public void Update(RuleKey key, object value)
    {
        if (key == RuleKey.RoomVisited && memory.TryGetValue(key, out object obj) && obj is HashSet<LocationCode> roomsVisited && value is LocationCode room)
        {
            roomsVisited.Add(room);
        }
        else if (memory.TryGetValue(key, out var existingValue) && existingValue is int existingIntValue && value is int intValue)
        {
            // using int as value increments by default
            memory[key] = existingIntValue + intValue;
        }
        else
        {
            memory[key] = value;
        }
    }

    public List<Fact> GetMemoryFacts()
    {
        List<Fact> memoryFacts = memory
            .Where(m => m.Key != RuleKey.RoomVisited)
            .Select(kvp => new Fact { key = kvp.Key, value = kvp.Value })
            .ToList();

        var roomsVisited = memory[RuleKey.RoomVisited] as HashSet<LocationCode>;
        memoryFacts.AddRange(roomsVisited.Select(rv => new Fact { key = RuleKey.RoomVisited, value = rv }));

        return memoryFacts;
    }

    public void Delete(RuleKey key)
    {
        memory.Remove(key);
    }

    public void Clear()
    {
        memory.Clear();
    }
}