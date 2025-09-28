using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public static class RoomIncidentLoader
{

    private static Dictionary<string, string> storySnippets;

    public static void LoadFlavor()
    {
        TextAsset yamlFile = Resources.Load<TextAsset>("Json/roomIncidentText");

        if (yamlFile == null)
        {
            Debug.LogError("Could not find yaml!");
            return;
        }

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var yamlObject = deserializer.Deserialize<object>(yamlFile.text);

        var serializer = new SerializerBuilder()
            .JsonCompatible()
            .Build();

        string json = serializer.Serialize(yamlObject);

        storySnippets = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        RoomIncidentRegistry.Load(storySnippets);
    }
}
