[System.Serializable]
public class WorldState
{
    public Player player;
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
