
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
        string toString = $@"
            -exitDirection: {exitDirection.ToString()}
            -targetRoomName: {targetDestination.displayName}
            ";
        return toString;
    }
}

