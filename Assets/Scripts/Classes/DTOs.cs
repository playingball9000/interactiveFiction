using System.Collections.Generic;
using Newtonsoft.Json;

public class ExitDTO
{
    [JsonProperty("direction")]
    public string Direction { get; set; }

    [JsonProperty("leadsTo")]
    public string LeadsTo { get; set; }
}

public class SceneryDTO
{
    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("adjective")]
    public string Adjective { get; set; }

    [JsonProperty("aliases")]
    public List<string> Aliases { get; set; }
}

public class RoomDTO
{
    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("internalCode")]
    public string InternalCode { get; set; }

    [JsonProperty("exits")]
    public List<ExitDTO> Exits { get; set; }

    [JsonProperty("roomScenery")]
    public List<SceneryDTO> RoomScenery { get; set; }
}

public class RoomCollectionDTO
{
    [JsonProperty("rooms")]
    public List<RoomDTO> Rooms { get; set; }
}



public class Root
{
    [JsonProperty("dictionary")]
    public List<LocationEntry> Entries { get; set; }
}

public class LocationEntry
{
    [JsonProperty("locationCode")]
    public string LocationCode { get; set; }

    [JsonProperty("keylist")]
    public List<string> KeyList { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }
}