using System.Collections.Generic;

public static class RoomRegistry
{
    private static Dictionary<string, Room> rooms = new();

    public static void Register(Room room)
    {
        rooms[room.internalCode] = room;
    }

    public static Room GetRoom(string internalCode)
    {
        return rooms.TryGetValue(internalCode, out var room) ? room : null;
    }

    public static IEnumerable<Room> GetAllRooms()
    {
        return rooms.Values;
    }

    public static void ClearRegistry()
    {
        rooms.Clear();
    }
}
