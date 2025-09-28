using System.Collections.Generic;
using System.Linq;

public static class RoomRegistry
{
    private static Dictionary<LocationCode, Room> rooms = new();

    public static void Register(Room room)
    {
        rooms.Add(room.internalCode, room);
    }

    public static Room GetRoom(LocationCode internalCode)
    {
        return rooms.TryGetValue(internalCode, out var room) ? room : null;
    }

    public static List<Room> GetAllRooms()
    {
        return rooms.Values.ToList();
    }

    public static Dictionary<LocationCode, Room> GetDict()
    {
        return rooms;
    }

    public static void Load(Dictionary<LocationCode, Room> r)
    {
        rooms = r;
    }

    public static void ClearRegistry()
    {
        rooms.Clear();
    }
}
