[System.Serializable]
public class Exit
{
    public ExitDirection exitDirection;
    public string exitDescription;
    public Room targetRoom;

    public override string ToString()
    {
        string toString = $@"
exitDirection: {exitDirection.ToString()}
targetRoomName: {targetRoom.roomName}
";
        return toString;
    }
}

public enum ExitDirection
{
    north,
    south,
    east,
    west
}