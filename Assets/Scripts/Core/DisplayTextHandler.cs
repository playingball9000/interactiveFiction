using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DisplayTextHandler : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    List<string> storyLog = new List<string>();

    // Delegates for other functions to use to invoke this. Probably not the right way but oh well.
    public delegate void UpdateTextDisplayDelegate(string text);
    public static UpdateTextDisplayDelegate invokeUpdateTextDisplay;

    public delegate void DisplayRoomTextDelegate(Room room);
    public static DisplayRoomTextDelegate invokeDisplayRoomText;

    private void OnEnable()
    {
        DisplayTextHandler.invokeUpdateTextDisplay += UpdateTextDisplay;
        DisplayTextHandler.invokeDisplayRoomText += DisplayRoomText;
    }

    private void OnDisable()
    {
        DisplayTextHandler.invokeUpdateTextDisplay -= UpdateTextDisplay;
        DisplayTextHandler.invokeDisplayRoomText -= DisplayRoomText;
    }

    public void UpdateTextDisplay(string text)
    {
        text = TmpTextTagger.Color(text, UiConstants.TEXT_COLOR_STORY_TEXT);
        storyLog.Add(text + "\n");
        string logAsText = string.Join("\n", storyLog.ToArray());

        displayText.text = logAsText;
    }

    public void DisplayRoomText(Room room)
    {
        List<string> interactionDescriptionsInRoom = GetRoomInteractionDescriptions(room);

        string joinedInteractionDescriptions = string.Join("\n", interactionDescriptionsInRoom.ToArray());

        string combinedText = room.roomName + "\n" + room.description + "\n" + joinedInteractionDescriptions;

        UpdateTextDisplay(combinedText);
    }

    // TODO: should move this to the room object
    private List<string> GetRoomInteractionDescriptions(Room room)
    {
        List<string> interactionDescriptionsInRoom = new List<string>();

        List<string> npcNames = room.npcs.Select(npc => npc.referenceName).ToList();

        if (npcNames?.Any() == true)
        {
            interactionDescriptionsInRoom.Add("The following people are here: " + string.Join(", ", npcNames) + "\n");
        }

        foreach (Exit exit in room.exits)
        {
            interactionDescriptionsInRoom.Add(exit.exitDescription);
        }
        return interactionDescriptionsInRoom;
    }

}
