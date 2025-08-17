using System.Collections.Generic;

public static class PlayerFactory
{
    // Must be after Areas and Rooms are made and added to respective registries
    public static Player CreateNew(string name, LocationCode startingRoom)
    {
        Player player = new Player
        {
            playerName = name,
            description = "An eager adventurer.",
            currentLocation = RoomBuilder.GetLocationByCode(startingRoom),
            money = 0,
            // equipment left empty
            // inventory left empty
        };

        var startingStats = new Dictionary<Stat, float>()
        {
            { Stat.Health, 72 },
            { Stat.Food, 21 },
            { Stat.Water, 10 },
            { Stat.Strength, 12 },
            { Stat.Agility, 8 },
            { Stat.Intelligence, 6 }
        };

        player.stats.InitializeBaseStats(startingStats);
        return player;
    }
}
