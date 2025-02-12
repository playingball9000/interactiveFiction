using System.Collections.Generic;

public static class RoomFactory
{
    // DO I need this?
    public static Room CreateRoom(
       string displayName,
       string description,
       string internalCode,
       List<Exit> exits = null,
       List<NPC> npcs = null,
       RoomItems roomItems = null,
       List<IExaminable> roomScenery = null)
    {
        return new Room
        {
            displayName = displayName,
            description = description,
            internalCode = internalCode,
            exits = exits ?? new List<Exit>(),
            npcs = npcs ?? new List<NPC>(),
            roomItems = roomItems ?? new RoomItems(),
            roomScenery = roomScenery ?? new List<IExaminable>()
        };
    }

    public static void LinkRooms(Room from, Room to, ExitDirection direction, bool isLocked = false, string lockedText = "", string keyInternalCode = "")
    {
        from.exits.Add(new Exit { exitDirection = direction, targetRoom = to, isLocked = isLocked, lockedText = lockedText, keyInternalCode = keyInternalCode });
    }

    public static void LinkRoomsTwoWay(Room from, Room to, ExitDirection direction, bool isLocked = false, string lockedText = "", string keyInternalCode = "")
    {
        LinkRooms(from, to, direction, isLocked, lockedText, keyInternalCode);
        LinkRooms(to, from, ExitHelper.GetOpposite(direction), isLocked, lockedText, keyInternalCode);
    }
}
