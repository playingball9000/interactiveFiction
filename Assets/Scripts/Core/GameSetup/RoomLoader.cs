using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json; // Make sure you have Newtonsoft.Json imported (via NuGet or Unity Package Manager)

public static class RoomLoader
{
    public static void LoadRooms()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Json/rooms");

        if (jsonFile == null)
        {
            Debug.LogError("RoomLoader: Could not find Json/rooms.json in Resources!");
            return;
        }

        // Deserialize using Newtonsoft.Json
        RoomCollectionDTO dto = JsonConvert.DeserializeObject<RoomCollectionDTO>(jsonFile.text);

        if (dto?.Rooms == null)
        {
            Debug.LogError("RoomLoader: No rooms found in JSON file.");
            return;
        }

        foreach (var roomDTO in dto.Rooms)
        {
            List<IExaminable> scenery = new List<IExaminable>();

            if (roomDTO.RoomScenery != null)
            {
                foreach (var pm in roomDTO.RoomScenery)
                {
                    scenery.Add(new Scenery
                    {
                        displayName = pm.DisplayName,
                        adjective = pm.Adjective ?? "",
                        description = pm.Description,
                        aliases = pm.Aliases ?? new List<string>(),
                    });
                }
            }

            Room room = new Room
            {
                displayName = roomDTO.DisplayName,
                description = roomDTO.Description,
                internalCode = Enum.Parse<LocationCode>(roomDTO.InternalCode),
                roomScenery = scenery
            };

            if (roomDTO.Exits != null)
            {
                foreach (var exitDTO in roomDTO.Exits)
                {
                    var direction = ExitHelper.GetExitDirectionEnum(exitDTO.Direction);
                    var leadsTo = Enum.Parse<LocationCode>(exitDTO.LeadsTo);
                    room.exits.Add(new Exit { exitDirection = direction, locationCode = leadsTo });
                }
            }

            RoomRegistry.Register(room);
        }
    }

    public static void LoadRoomContextActions()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Json/roomContextActions");

        var root = JsonConvert.DeserializeObject<Root>(jsonFile.text);
        var final = new Dictionary<LocationCode, Dictionary<string, Action>>();

        foreach (var entry in root.Entries)
        {
            LocationCode code = Enum.Parse<LocationCode>(entry.LocationCode);
            if (!final.ContainsKey(code))
            {
                final[code] = new Dictionary<string, Action>();
            }

            foreach (var key in entry.KeyList)
            {
                final[code][key] = () => { StoryTextHandler.invokeUpdateStoryDisplay(entry.Value); };
            }
        }
        RoomContextRegistry.Load(final);
    }
}
