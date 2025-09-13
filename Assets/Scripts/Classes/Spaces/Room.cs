using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Room : ILocation
{
    public string displayName { get; set; }
    public string description;
    public LocationCode internalCode { get; set; }

    public List<Exit> exits = new();
    public List<INPC> npcs = new();
    public RoomItems roomItems = new();
    public List<IExaminable> roomScenery = new();

    public List<string> GetRoomInteractionDescriptions()
    {
        List<string> interactionDescriptionsInRoom = new();

        if (npcs.Any())
        {
            List<string> npcNames = npcs.Select(npc => npc.GetDisplayName()).ToList();
            interactionDescriptionsInRoom.Add("The following people are here: " + TmpTextTagger.Color(StringUtil.CreateCommaSeparatedString(npcNames), UiConstants.TEXT_COLOR_STORY_NPC));
        }
        if (roomItems.contents.Any())
        {
            interactionDescriptionsInRoom.Add(roomItems.ContentsToString());
        }
        return interactionDescriptionsInRoom;
    }


    public string GetExitDescriptions()
    {
        List<string> otherRooms = new();

        foreach (Exit exit in exits)
        {
            otherRooms.Add($"{exit.targetDestination.displayName} [{(exit.isTargetAccessible() ? exit.exitDirection : exit.getNotAccessibleTag())}]");
        }
        return string.Join("\n", otherRooms.ToArray());
    }

    public List<IExaminable> GetExaminableThings()
    {
        List<IExaminable> examinableThings = npcs.ToList<IExaminable>();
        examinableThings.AddRange(roomItems.contents.ToList<IExaminable>());
        examinableThings.AddRange(roomScenery);

        examinableThings.AddRange(roomItems.contents.OfType<ContainerBase>()
                                    .Where(container => container.isAccessible())
                                    .SelectMany(openContainer => openContainer.contents)
                                    .ToList());

        return examinableThings;
    }

    public void DisplayRoomStoryText()
    {
        List<string> interactionDescriptionsInRoom = GetRoomInteractionDescriptions();

        string joinedInteractionDescriptions = "\n" +
            (interactionDescriptionsInRoom.Any() ? (string.Join("\n", interactionDescriptionsInRoom.ToArray()) + "\n") : "");
        joinedInteractionDescriptions = TmpTextTagger.LineHeight(joinedInteractionDescriptions, 160);

        string exitDescription = GetExitDescriptions();

        string combinedText = TmpTextTagger.Bold(displayName) + "\n"
            + description
            + joinedInteractionDescriptions
            + exitDescription;

        StoryTextHandler.invokeUpdateStoryDisplay(combinedText);
    }

    public List<Fact> GetRoomFacts()
    {
        List<Fact> roomFacts = new();
        roomFacts.AddRange(npcs
            .OfType<ComplexNPC>()
            .Select(npc => new Fact { key = RuleKey.InRoomNpc, value = npc.internalCode }));
        roomFacts.AddRange(roomItems.contents
            .Select(item => new Fact { key = RuleKey.InRoomItem, value = item.internalCode }));
        return roomFacts;
    }


    public List<IItem> GetRoomItems()
    {
        return roomItems.contents;
    }

    public List<ContainerBase> GetContainersInRoom()
    {
        return roomItems.contents.OfType<ContainerBase>().ToList();
    }

    public override string ToString()
    {
        string npcNames = StringUtil.CreateCommaSeparatedString(npcs.Select(npc => npc.displayName).ToList());
        string itemNames = StringUtil.CreateCommaSeparatedString(roomItems.contents.Select(item => item.displayName).ToList());
        string exitsPaths = StringUtil.CreateCommaSeparatedString(exits.Select(ex => ex.ToString()).ToList());

        string toString =
            $"<b><color=#8B4513>[Room]</color></b>\n" +
            $"  • Name: <b>{displayName}</b>\n" +
            $"  • Code: {internalCode}\n" +
            $"  • Description: {description}\n" +
            $"  • NPCs: {npcNames}\n" +
            $"  • Items: {itemNames}\n" +
            $"  • Exits: {exitsPaths}\n";

        return toString;
    }
}
