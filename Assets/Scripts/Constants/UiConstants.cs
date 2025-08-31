using System.Collections.Generic;

public class UiConstants
{
    public const string TEXT_COLOR_PLAYER_NAME = "#8CC4CC";
    public const string TEXT_COLOR_PLAYER_TEXT = "#ADD8E6";
    public const string TEXT_COLOR_PLAYER_ACTION = "#7895A1";
    public const string TEXT_COLOR_NPC_NAME = "#9C8C64";
    public const string TEXT_COLOR_NPC_TEXT = "#94AC84";

    public const string TEXT_COLOR_STORY_TEXT = "#D8C79C";
    public const string TEXT_COLOR_STORY_NPC = "#4CAF50";
    public const string TEXT_COLOR_STORY_ITEM = "#FFC107";
    public const string TEXT_COLOR_DEATH_TEXT = "#FF4444";
}

public class Intro
{
    public static List<string> introParagraphs = new()
    {
        @"The labyrinth... the abyss... the void... the endless...

To call it a mere chasm or canyon would not do it justice.
        ",
        @"The entrance, which stretches over 3000 meters, and the bottom, which has yet to be discovered.

In between lie treasures capable of breaking the rules of reality as we know it. Many seek to uncover its secrets.",

        @"Those who would challenge the depths are called...
        
        <color=#FFD700>Trail Blazers</color>

The massive city of Belsk spans the northern opening. This is where your adventure begins..."
    };

    public static string introStory = @"You stand aboard the deck of the <i>Skypiercer</i>, an airship drifting within the first layer of the abyss. It just so happens that today is your graduation exam and the first step to becoming an official <color=#FFD700>Trail Blazer</color>. This is your first time on an airship and you have a bit of time before touchdown, the perfect opportunity to explore.";

}

public enum TextEffect
{
    None,
    Typewriter
}