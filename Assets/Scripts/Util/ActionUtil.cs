using System;
using System.Collections.Generic;
using System.Linq;

public enum ItemLocation
{
    inventory,
    room,
    container,
}

public static class ActionUtil
{
    /// <summary>
    /// Takes in a list of any type <T> and handles checking how many items are in that list: 0, 1, or >1 and lets the user do appropriate actions.
    /// </summary>
    /// <param name="filteredItems"></param>
    /// <param name="noMatchAction"></param>
    /// <param name="singleMatchAction"></param>
    /// <param name="multipleMatchesAction"></param>
    /// <typeparam name="T"></typeparam>
    public static void MatchZeroOneAndMany<T>(
        List<T> filteredItems,
        Action noMatchAction,
        Action<T> singleMatchAction,
        Action<List<T>> multipleMatchesAction)
    {

        if (!filteredItems.Any())
        {
            noMatchAction();
        }
        else if (filteredItems.Count == 1)
        {
            singleMatchAction(filteredItems[0]);
        }
        else
        {
            multipleMatchesAction(filteredItems);
        }
    }

    /// <summary>
    /// Searches a list of objects for a field value that matches the searchString
    /// </summary>
    /// <param name="source">list to search ex. player.currentLocation.npcs </param>
    /// <param name="propertySelector">field name on object to check ex. npc => npc.referenceName</param>
    /// <param name="searchString">word you are looking for ex. man</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> FindItemsFieldContainsString<T>(IEnumerable<T> source, Func<T, string> propertySelector, string searchString)
    {
        // Look for exact match first
        T exactMatch = source.SingleOrDefault(item => propertySelector(item).ToLower() == searchString.ToLower());

        if (exactMatch != null)
        {
            return new List<T>() { exactMatch };
        }
        return source.Where(item => propertySelector(item).ToLower().Contains(searchString.ToLower())).ToList();
    }

    /// <summary>
    /// Searches containers for an item that matches the searchString
    /// </summary>
    /// <param name="itemsToSearch">Takes in a list of IItems since containers are IItems</param>
    /// <param name="searchString"></param>
    /// <returns>The containing container and the item(s) matched</returns>
    public static (ContainerBase, List<IItem>) FindItemsInContainers(List<IItem> itemsToSearch, string searchString)
    {
        ContainerBase containerHoldingItem = null;
        List<IItem> finalMatchedItems = new List<IItem>();

        List<IContainer> openContainers = itemsToSearch.OfType<IContainer>()
                .Where(container => container.isOpen)
                .ToList();

        foreach (IContainer container in openContainers)
        {
            // Technically matches the first item name it matches in soonest container it searches, but shouldn't matter
            List<IItem> matchedItems = FindItemsFieldContainsString(container.contents, item => item.referenceName, searchString);
            if (matchedItems.Count == 1)
            {
                containerHoldingItem = (ContainerBase)container;
                finalMatchedItems.Add(matchedItems[0]);
                break;
            }
            else
            {
                // if 0 or many matches so the MatchZeroOneAndMany() will take care of it.
                finalMatchedItems.AddRange(matchedItems);
            }
        }
        ;
        return (containerHoldingItem, finalMatchedItems);
    }


    private static readonly string[] unknownCommandResponses =
    {
        "I'm sorry, Dave, I can't do that. Mostly because I'm not HAL.",
        "That's an interesting idea. Let me file it in my *totally hypothetical suggestions* drawer.",
        "You might as well have typed *turnup salsa*. It makes about as much sense.",
        "Did you mean to type that, or did a monkey just escape with your keyboard?",
        "Ah, I see you've discovered the secret command for doing absolutely nothing.",
        "There are 42 ways to be confused, and you've just unlocked a new one.",
        "The universe is vast, but unfortunately, I have no idea what you want.",
        "If I had a million dollars for every time that command worked, I'd still be broke.",
        "The improbability of that command working approaches infinite tea-making failure.",
        "Perhaps you're using a special dialect only understood by hyperintelligent shades of the color blue.",
        "In an alternate universe, that command would start a conga line. Sadly, this isn't that universe.",
        "I'm not sure what you meant, but congratulations on confusing even me!",
        "The mice are laughing at you. Just thought you'd like to know.",
        "That command looks suspiciously like middle school poetry. Please don't do that again.",
        "I could try that, but I'd probably end up inventing a new kind of disaster.",
        "I'll add that to my list of strange human behaviors. It's getting very long.",
        "That's a bold move, Cotton. Let's see not do that.",
        "Was that a command or a sneeze? Hard to tell.",
        "The ship's computer would like a word with you mostly to say No.",
        "It's entirely possible that command worked in a dream you once had."
    };

    public static string GetUnknownCommandResponse()
    {
        Random random = new Random();
        int index = random.Next(unknownCommandResponses.Length);
        return unknownCommandResponses[index];
    }
}
