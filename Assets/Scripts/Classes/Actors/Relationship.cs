using System;

public class Relationship
{
    public int points { get; set; }

    public void IncreaseRelations(int amount)
    {
        points += amount;
    }

    public void DecreaseRelations(int amount)
    {
        // prevent negative (for now)
        points = Math.Max(0, points - amount);
    }
    public int GetLevel()
    {
        // Rounds down by default
        return points / 200;
    }

}