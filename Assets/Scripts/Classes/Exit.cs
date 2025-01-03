[System.Serializable]
public class Exit
{
    public ExitDirection exitDirection;
    public string exitDescription;
    public Room targetRoom;
}

public enum ExitDirection
{
    north,
    south,
    east,
    west
}