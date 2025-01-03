using System;

public static class TmpTextTagger
{
    /// <summary>
    /// Wraps the given text in a bold tag.
    /// </summary>
    public static string Bold(string text)
    {
        return $"<b>{text}</b>";
    }

    /// <summary>
    /// Wraps the given text in an italic tag.
    /// </summary>
    public static string Italic(string text)
    {
        return $"<i>{text}</i>";
    }

    /// <summary>
    /// Wraps the given text in a color tag.
    /// </summary>
    /// <param name="text">The text to color.</param>
    /// <param name="color">The color name or hex code (e.g., "red", "#FF0000").</param>
    public static string Color(string text, string color)
    {
        return $"<color={color}>{text}</color>";
    }

    /// <summary>
    /// Wraps the given text in a size tag.
    /// </summary>
    /// <param name="text">The text to resize.</param>
    /// <param name="size">The size of the text.</param>
    public static string Size(string text, int size)
    {
        return $"<size={size}>{text}</size>";
    }

    /// <summary>
    /// Combines multiple tags for a given text.
    /// </summary>
    /// <param name="text">The text to style.</param>
    /// <param name="tags">An array of functions that apply tags.</param>

    /*
     * Example:
     *  string fancyText = TMPTextTagger.ApplyMultipleTags(
            "Fancy Text",
            TMPTextTagger.Bold,
            text => TMPTextTagger.Color(text, "#00FF00"),
            text => TMPTextTagger.Size(text, 36)
        );
     * 
     */
    public static string ApplyMultipleTags(string text, params Func<string, string>[] tags)
    {
        foreach (var tag in tags)
        {
            text = tag(text);
        }
        return text;
    }
}
