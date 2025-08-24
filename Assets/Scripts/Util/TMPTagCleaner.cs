using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class TMPTagCleaner
{
    private static readonly HashSet<string> selfClosingTags = new HashSet<string>
    {
        "br", "sprite", "quad", "img"
    };

    public static string Clean(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Opening tags with optional attributes
        var openTagPattern = new Regex(@"<([a-zA-Z0-9]+)(=[^<>]*)?>", RegexOptions.IgnoreCase);
        // Closing tags
        var closeTagPattern = new Regex(@"</([a-zA-Z0-9]+)>", RegexOptions.IgnoreCase);

        var removals = new List<(int start, int length)>();
        var stack = new Stack<(string tag, int index, int length)>();

        // Collect both open + close matches
        var matches = new List<(Match match, bool isClose)>();
        foreach (Match m in openTagPattern.Matches(input))
            matches.Add((m, false));
        foreach (Match m in closeTagPattern.Matches(input))
            matches.Add((m, true));

        // Sort by position
        matches.Sort((a, b) => a.match.Index.CompareTo(b.match.Index));

        foreach (var (m, isClose) in matches)
        {
            if (isClose)
            {
                string tagName = m.Groups[1].Value.ToLower();
                // Find matching opening tag in stack
                var tempStack = new Stack<(string tag, int index, int length)>();
                bool found = false;

                while (stack.Count > 0)
                {
                    var top = stack.Pop();
                    if (top.tag == tagName)
                    {
                        found = true;
                        // put back everything above it
                        while (tempStack.Count > 0) stack.Push(tempStack.Pop());
                        break;
                    }
                    else
                    {
                        tempStack.Push(top);
                    }
                }

                if (!found)
                {
                    // Orphan closing tag
                    removals.Add((m.Index, m.Length));
                    // restore stack
                    while (tempStack.Count > 0) stack.Push(tempStack.Pop());
                }
            }
            else
            {
                string tagName = m.Groups[1].Value.ToLower();
                if (!selfClosingTags.Contains(tagName))
                {
                    stack.Push((tagName, m.Index, m.Length));
                }
            }
        }

        // Remove any unclosed openings left in stack
        while (stack.Count > 0)
        {
            var bad = stack.Pop();
            removals.Add((bad.index, bad.length));
        }

        // Remove backwards so indices don't shift
        removals.Sort((a, b) => b.start.CompareTo(a.start));
        foreach (var (start, length) in removals)
            input = input.Remove(start, length);

        return input;
    }
}
