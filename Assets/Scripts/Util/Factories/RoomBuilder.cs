public class RoomBuilder
{
    private readonly Room room;

    public RoomBuilder(string name, LocationCode code)
    {
        room = new Room { displayName = name, internalCode = code };
    }

    public RoomBuilder WithDescription(string desc)
    {
        room.description = desc;
        return this;
    }

    public RoomBuilder WithExit(ExitDirection direction, LocationCode target)
    {
        // Set location code here
        room.exits.Add(new Exit { exitDirection = direction, locationCode = target });
        return this;
    }

    public Room Build()
    {
        // Adds to registry implicitly
        RoomRegistry.Register(room);
        return room;
    }

    public static void FinalizeRooms()
    {
        // For each room, for each exit: get either the room or location it points to from set locationCode
        // This is so I can set the exits to point to something before the room it points to has been created
        RoomRegistry.GetAllRooms().ForEach(r =>
        {
            r.exits.ForEach(e =>
            {
                e.targetDestination = GetLocationByCode(e.locationCode);
            });
        });
    }

    public static ILocation GetLocationByCode(LocationCode locationCode)
    {
        Room room = RoomRegistry.GetRoom(locationCode);
        return room != null ? room : AreaRegistry.GetArea(locationCode);
    }

}
