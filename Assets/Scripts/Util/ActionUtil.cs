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
        List<T> exactMatch = FindItemsFieldEqualsString(source, propertySelector, searchString);

        if (exactMatch.Count == 1)
        {
            return exactMatch;
        }
        return source.Where(item => propertySelector(item).ToLower().Contains(searchString.ToLower())).ToList();
    }

    public static List<T> FindItemsFieldEqualsString<T>(IEnumerable<T> source, Func<T, string> propertySelector, string searchString)
    {
        List<T> matches = source.Where(x => propertySelector(x).ToLower() == searchString.ToLower()).ToList();
        return matches;
    }


    public static (IStorage, List<IItem>) FindItemsInAccessibleStorages(List<IStorage> containersToSearch, List<string> mainClause)
    {
        IStorage containerHoldingItem = null;
        List<IItem> pm = new();

        List<IStorage> openContainers = containersToSearch
                .Where(container => container.isAccessible())
                .ToList();
        int processingCount = 1;

        // Iterates from the front
        while (mainClause.Count > 0 && processingCount < 3)
        {
            string word = DequeueFirstElement(mainClause);
            foreach (IStorage container in openContainers)
            {
                // Technically matches the first item name it matches in soonest container it searches, but shouldn't matter
                if (processingCount == 1)
                {
                    pm = FindItemsFieldContainsString(container.contents, item => item.adjective, word);
                    if (!pm.Any())
                    {
                        pm = FindItemsFieldContainsString(container.contents, item => item.referenceName, word);
                    }
                }
                else if (processingCount == 2)
                {
                    // Second time through, I want to disambiguate by adjectives
                    pm = FindItemsFieldContainsString(pm, item => item.referenceName, word);
                }

                if (pm.Count == 1)
                {
                    containerHoldingItem = container;
                    processingCount = processingCount + 9001;
                    break;
                }
            }
            processingCount++;
        }
    ;
        return (containerHoldingItem, pm);
    }

    public static List<ContainerBase> FindContainersInRoom(Room room, List<string> mainClause)
    {
        List<IExaminable> openContainers = room.GetRoomItems().OfType<ContainerBase>()
            .Cast<IExaminable>()
            .ToList();
        // Searches from end becuse you "put stuff in x"
        List<IExaminable> pm = ProcessMainClauseFromEnd(mainClause, openContainers);
        return pm.Cast<ContainerBase>().ToList();
    }

    /// <summary>
    /// Processes mainClause from the last item and modifies it as each element is processed
    /// </summary>
    /// <param name="mainClause"></param>
    /// <param name="examinables"></param>
    /// <returns>List of items matching the words in mainClause</returns>
    public static List<IExaminable> ProcessMainClauseFromEnd(List<string> mainClause, List<IExaminable> examinables)
    {
        /*
            I just need to walk myself through this logic.
            - Process 2 at a time from the end because the player will only be actioning on
            the pattern of [adjective] [object] or just [object].
            - For the case where it's [object] [object] like 'put BOOK in BAG', I think it's
            safe to assume direct object will never match the adjective of the indirect object and
            even if it does, then it should error out anyway because then you will be left with nothing
            left in the main clause after the processed element is removed or popped.
        */
        List<IExaminable> pm = new();
        int processingCount = 1;
        while (mainClause.Count > 0 && processingCount < 3)
        {
            // Process the last element in the list
            string word = PopLastElement(mainClause);

            if (processingCount == 1)
            {
                pm = FindItemsFieldContainsString(examinables, item => item.referenceName, word);
                if (!pm.Any())
                {
                    pm = FindItemsFieldContainsString(examinables, item => item.adjective, word);
                }
            }
            else if (processingCount == 2)
            {
                // Second time through, I want to disambiguate by adjectives
                pm = FindItemsFieldContainsString(pm, item => item.adjective, word);
            }
            if (!pm.Any() || pm.Count == 1)
            {
                // Break if no elements or one element, if many elements, continue looping
                break;
            }
            processingCount++;
        }
        return pm;
    }

    public static List<IExaminable> ProcessMainClauseFromStart(List<string> mainClause, List<IExaminable> examinables)
    {
        /*
            - Process 2 at a time from the start because the player will only be actioning on
            the pattern of [adjective] [object] or just [object].
            - Since the input could be "put bar in duffel bag", the word bag matches and leaves duffel, 
            which will fail if we keep going backwards
        */
        List<IExaminable> pm = new();
        int processingCount = 1;
        while (mainClause.Count > 0 && processingCount < 3)
        {
            // Process the first element in the list
            string word = DequeueFirstElement(mainClause);

            if (processingCount == 1)
            {
                pm = FindItemsFieldContainsString(examinables, item => item.adjective, word);
                if (!pm.Any())
                {
                    pm = FindItemsFieldContainsString(examinables, item => item.referenceName, word);
                }
            }
            else if (processingCount == 2)
            {
                // Second time through, I want to disambiguate by referenceName
                pm = FindItemsFieldContainsString(pm, item => item.referenceName, word);
            }
            if (!pm.Any() || pm.Count == 1)
            {
                // Break if no elements or one element, if many elements, continue looping
                break;
            }
            processingCount++;
        }
        return pm;
    }

    public static string PopLastElement(List<string> stringList)
    {
        string lastElement = stringList[stringList.Count - 1];
        stringList.RemoveAt(stringList.Count - 1);
        return lastElement;
    }

    public static string DequeueFirstElement(List<string> stringList)
    {
        string firstElement = stringList[0];
        stringList.RemoveAt(0);
        return firstElement;
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
        Random random = new();
        int index = random.Next(unknownCommandResponses.Length);
        return unknownCommandResponses[index];
    }
}
