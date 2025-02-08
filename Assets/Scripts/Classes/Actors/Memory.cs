using System.Collections.Generic;
using System.Linq;

public class Memory
{
    private Dictionary<string, object> memory = new();

    public void Update(string key, object value)
    {
        if (memory.TryGetValue(key, out var existingValue) && existingValue is int existingIntValue && value is int intValue)
        {
            memory[key] = existingIntValue + intValue;
        }
        else
        {
            memory[key] = value;
        }
    }

    public List<Fact> GetMemoryFacts()
    {
        return memory.Select(kvp => new Fact { key = kvp.Key, value = kvp.Value }).ToList();
    }

    public void Delete(string key)
    {
        memory.Remove(key);
    }

    public void Clear()
    {
        memory.Clear();
    }
}