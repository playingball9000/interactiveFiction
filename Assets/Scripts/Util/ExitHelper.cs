
using System;
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
    private static readonly Dictionary<string, ExitDirection> _map =
        new Dictionary<string, ExitDirection>(StringComparer.OrdinalIgnoreCase)
        {
            { "n", ExitDirection.North },
            { "north", ExitDirection.North },
            { "nw", ExitDirection.Northwest },
            { "northwest", ExitDirection.Northwest },
            { "ne", ExitDirection.Northeast },
            { "northeast", ExitDirection.Northeast },
            { "s", ExitDirection.South },
            { "south", ExitDirection.South },
            { "sw", ExitDirection.Southwest },
            { "southwest", ExitDirection.Southwest },
            { "se", ExitDirection.Southeast },
            { "southeast", ExitDirection.Southeast },
            { "e", ExitDirection.East },
            { "east", ExitDirection.East },
            { "w", ExitDirection.West },
            { "west", ExitDirection.West },
            { "u", ExitDirection.Up },
            { "up", ExitDirection.Up },
            { "d", ExitDirection.Down },
            { "down", ExitDirection.Down },
            { "enter", ExitDirection.Enter }
        };

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


    public static ExitDirection GetExitDirectionEnum(string input)
    {
        return _map.TryGetValue(input, out var exitDirection) ? exitDirection : throw new ArgumentException("RoomContextAction loader error");
    }
}