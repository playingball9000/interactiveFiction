using System.Collections.Generic;

[System.Serializable]
public class Exit
{
    public ExitDirection exitDirection;
    public Room targetRoom { get; set; }

    // I found it easier for now, to put the key code on Exit instead of vice versa
    public bool isTargetAccessible { get; set; } = true;
    public string lockedText { get; set; }
    public string keyInternalCode { get; set; } = "";

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
