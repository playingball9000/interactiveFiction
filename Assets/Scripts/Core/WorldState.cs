using System.Collections.Generic;

[System.Serializable]
public class WorldState
{
    public Player player;
    public Dictionary<LocationCode, Room> roomData = new();
    public Dictionary<LocationCode, Area> areaData = new();
    public Dictionary<CardCode, Card> cardData = new();

    public List<NPC> npcs = new();

    public bool FLAG_dialogWindowActive = false;
    public bool FLAG_showIntro = false;

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

public static class PlayerContext
{
    public static Player Get => WorldState.GetInstance().player;
}