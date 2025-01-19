using System.Collections.Generic;

public class StringListMax
{
    private readonly List<string> logs;
    private readonly int maxCount;

    public StringListMax(int maxCount)
    {
        this.maxCount = maxCount;
        logs = new List<string>();
    }

    public void Add(string log)
    {
        logs.Add(log);
        if (logs.Count > maxCount)
        {
            logs.RemoveAt(0);
        }
    }

    public string GetLogsString()
    {
        return string.Join("\n", logs);
    }

    public int Count => logs.Count;

    // Gets an item at the specified index
    public string Get(int index)
    {
        if (index < 0 || index >= logs.Count)
        {
            throw new System.ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }
        return logs[index];
    }

    public void Clear()
    {
        logs.Clear();
    }

    public IReadOnlyList<string> Items => logs.AsReadOnly();
}
