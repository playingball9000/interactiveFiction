using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Room
{
    public string displayName;
    public string description;
    public string internalCode { get; set; }

    public List<Exit> exits = new();
    public List<NPC> npcs = new();
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
            otherRooms.Add($"{exit.targetRoom.displayName} [{(exit.isTargetAccessible() ? exit.exitDirection : exit.getNotAccessibleTag())}]");
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
        npcs.ForEach(npc => roomFacts.Add(new Fact { key = RuleConstants.KEY_IN_ROOM_NPC, value = npc.internalCode }));
        roomItems.contents.ForEach(item => roomFacts.Add(new Fact { key = RuleConstants.KEY_IN_ROOM_ITEM, value = item.internalCode }));

        return roomFacts;
    }

    public void RemoveItem(IItem item)
    {
        roomItems.contents.Remove(item);
    }

    public void AddItem(IItem item)
    {
        roomItems.contents.Add(item);
    }

    public List<IItem> GetRoomItems()
    {
        return roomItems.contents;
    }

    public List<ContainerBase> GetRoomContainers()
    {
        return roomItems.contents.OfType<ContainerBase>().ToList();
    }

    public void AddNpc(NPC npc)
    {
        npc.currentLocation = this;
        this.npcs.Add(npc);
    }

    public override string ToString()
    {
        string npcNames = StringUtil.CreateCommaSeparatedString(npcs.Select(npc => npc.referenceName).ToList());
        string itemNames = StringUtil.CreateCommaSeparatedString(roomItems.contents.Select(item => item.referenceName).ToList());
        string exitsPaths = StringUtil.CreateCommaSeparatedString(exits.Select(ex => ex.ToString()).ToList());

        string toString = $@"
            roomName: {displayName}
            description: {description}
            npcNames: {npcNames}
            itemNames: {itemNames}
            exitsPaths: {exitsPaths}
        ";

        return toString;
    }
}
