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
        new ("enter", new() { "go", "walk", "move", "step" }, new() { "in", "into" }),
        new ("equip", new() { "put" }, new() { "on" }),
        new ("unequip", new() { "take" }, new() { "off" }),
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
