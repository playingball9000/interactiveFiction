using System.Collections.Generic;
using System.Linq;

public class Memory
{
    private Dictionary<string, object> memory = new();

    public void Update(string key, object value)
    {
        if (memory.ContainsKey(key))
        {
            if (memory[key] is int existingIntValue && value is int intValue)
            {
                memory[key] = existingIntValue + intValue;
            }
            else
            {
                memory[key] = value;
            }
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
        if (memory.ContainsKey(key))
        {
            memory.Remove(key);
        }
    }

    public void Clear()
    {
        memory.Clear();
    }
}