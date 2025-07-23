
using System.Collections.Generic;

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