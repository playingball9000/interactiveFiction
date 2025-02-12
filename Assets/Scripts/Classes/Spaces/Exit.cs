using System.Collections.Generic;

[System.Serializable]
public class Exit : ILockable
{
    public ExitDirection exitDirection;
    public Room targetRoom { get; set; }

    // I found it easier for now, to put the key code on Exit instead of vice versa
    public bool isLocked { get; set; } = false;
    public string lockedText { get; set; }
    public string keyInternalCode { get; set; } = "";

    public bool isTargetAccessible()
    {
        return !isLocked;
    }

    public string getNotAccessibleReason()
    {
        if (isLocked)
        {
            return $"Path to {targetRoom.displayName} is locked.";
        }
        return $"Path to {targetRoom.displayName} is not accessible.";
    }

    public string getNotAccessibleTag()
    {
        if (isLocked)
        {
            return $"-Locked-";
        }
        return $"-Inaccessible-";
    }

    public override string ToString()
    {
        string toString = $@"
            exitDirection: {exitDirection.ToString()}
            targetRoomName: {targetRoom.displayName}
            ";
        return toString;
    }
}

public enum ExitDirection
{
    North,
    Northwest,
    Northeast,
    South,
    Southwest,
    Southeast,
    East,
    West,
    Up,
    Down,
    Enter

}

public static class ExitHelper
{
    private static readonly Dictionary<ExitDirection, ExitDirection> OppositeDirections = new()
    {
        { ExitDirection.North, ExitDirection.South },
        { ExitDirection.South, ExitDirection.North },
        { ExitDirection.East, ExitDirection.West },
        { ExitDirection.West, ExitDirection.East },
        { ExitDirection.Northeast, ExitDirection.Southwest },
        { ExitDirection.Southwest, ExitDirection.Northeast },
        { ExitDirection.Northwest, ExitDirection.Southeast },
        { ExitDirection.Southeast, ExitDirection.Northwest },
        { ExitDirection.Up, ExitDirection.Down },
        { ExitDirection.Down, ExitDirection.Up },
        { ExitDirection.Enter, ExitDirection.Enter },
    };

    public static ExitDirection GetOpposite(ExitDirection direction)
    {
        return OppositeDirections.TryGetValue(direction, out var opposite) ? opposite : direction;
    }
}
