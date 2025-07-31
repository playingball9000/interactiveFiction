
[System.Serializable]
public class Exit : Portal
{
    public ILocation targetDestination { get; set; }

    public Room DestinationRoom => targetDestination as Room;
    public Area DestinationArea => targetDestination as Area;

    public override string getNotAccessibleReason()
    {
        if (isLocked)
        {
            return $"Path to {targetDestination.displayName} is locked.";
        }
        return $"Path to {targetDestination.displayName} is not accessible.";
    }

    public override string ToString()
    {
        string toString =
            $"<b><color=#8B4513>[Room]</color></b>\n" +
            $"  • Direction: <b>{exitDirection}</b>\n" +
            $"  • Target Room: {targetDestination.displayName}\n";
        return toString;
    }
}

