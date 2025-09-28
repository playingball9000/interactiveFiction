using System;
using System.Collections.Generic;
using System.Linq;

public class ActionPhraseSynonyms
{
    public static ActionPhraseSynonyms instance;

    public static ActionPhraseSynonyms getInstance()
    {
        if (instance == null)
        {
            instance = new ActionPhraseSynonyms();
        }
        return instance;
    }

    List<ActionPhrase> phrases = new() {
        new ("enter", new() { "go", "walk", "move", "step", "get" }, new() { "in", "into", "inside" }),
        new ("equip", new() { "put" }, new() { "on" }),
        new ("unequip", new() { "take" }, new() { "off" }),
        new ("get", new() { "pick" }, new() { "up" }),
    };

    public string[] Parse(string[] inputTextArray)
    {
        if (inputTextArray.Length >= 2)
        {
            foreach (ActionPhrase phrase in phrases)
            {
                bool firstMatches = phrase.first.Contains(inputTextArray[0]);
                bool secondMatches = phrase.second.Contains(inputTextArray[1]);

                if (firstMatches && secondMatches)
                {
                    inputTextArray = new string[] { phrase.action }.Concat(inputTextArray.Skip(2)).ToArray();
                    break;
                }
                else if (inputTextArray[0].Equals("pick"))
                {
                    // matches "pick x y up"
                    string item = "";
                    if (inputTextArray[^1].Equals("up"))
                    {
                        item = string.Join(" ", inputTextArray.Skip(1).Take(inputTextArray.Length - 2));
                    }
                    string normalized = $"get {item}";
                    inputTextArray = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                }
            }
        }
        return inputTextArray;
    }

    class ActionPhrase
    {
        public string action { get; set; }
        public List<string> first { get; set; } = new();
        public List<string> second { get; set; } = new();

        public ActionPhrase(string action, List<string> first, List<string> second)
        {
            this.action = action;
            this.first = first;
            this.second = second;
        }
    }
}
