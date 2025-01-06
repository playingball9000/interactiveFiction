using System.Collections.Generic;

[System.Serializable]
public class WorldState
{
    public Player player;
    public List<Room> rooms = new List<Room>();
    public List<NPC> npcs = new List<NPC>();

    private static WorldState instance;

    public static WorldState GetInstance()
    {
        if (instance == null)
        {
            instance = new WorldState();
        }
        return instance;
    }

    public static void SetInstance(WorldState worldState)
    {
        instance = worldState;
    }
}
