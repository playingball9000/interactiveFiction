using System;
using System.Collections.Generic;
using System.Linq;

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
        IEnumerable<T> filteredItems,
        Action noMatchAction,
        Action<T> singleMatchAction,
        Action<IEnumerable<T>> multipleMatchesAction)
    {

        if (!filteredItems.Any())
        {
            noMatchAction();
        }
        else if (filteredItems.Count() == 1)
        {
            singleMatchAction(filteredItems.First());
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
        exactMatch = source.Where(item => propertySelector(item).ToLower().Contains(searchString.ToLower())).ToList();
        // LoggingUtil.LogList(source, "source");

        return exactMatch;
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

    public static List<Exit> FindPossibleExits(List<Exit> exits, List<string> mainClause)
    {
        List<Exit> pe = exits.ToList();
        int processingCount = 1;

        while (mainClause.Count > 0 && processingCount < 3)
        {
            string word = DequeueFirstElement(mainClause);
            pe = FindItemsFieldContainsString(pe, e => e.targetRoom.displayName, word);

            if (!pe.Any() || pe.Count == 1)
            {
                break;
            }
            processingCount++;
        }
       ;
        return pe;
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


    public static List<NPC> FindMatchingNpcs(List<string> mainClause, List<NPC> npcs)
    {
        //TODO: Probably should make it match multiple words like adj

        // Honestly just hard code checking hte first and 2nd words... any more complicated is not worth it
        string firstWord = mainClause[0];
        string secondWord = mainClause[0];

        List<NPC> possibleMatches = FindItemsFieldContainsString(npcs, item => item.adjective, firstWord);
        if (!possibleMatches.Any())
        {
            possibleMatches = FindItemsFieldContainsString(npcs, item => item.referenceName, firstWord);
        }
        if (possibleMatches.Count <= 1)
        {
            return possibleMatches;
        }

        // if hasn't returned here, then possible matches are many so disambiguate
        return FindItemsFieldContainsString(possibleMatches, item => item.referenceName, secondWord);

    }

    private static readonly string[] unknownCommandResponses =
    {
        "You've discovered the secret command for doing absolutely nothing.",
        "Perhaps you're using a special dialect only understood by people of your intelligence.",
        "Better not, you'd probably end up inventing a new kind of disaster.",
        "It's entirely possible that worked in a dream you once had.",
        "There is apparently no crisis so imminent that will deter you from contemplating idiotic and frivolous actions.",
        "This is the dumbest idea you've had in weeks!!! STUPID STUPID STUPID. And yet the polished surface of your desk... It beckons.",
        "Ugh, what a terrible idea! The thought alone makes you sick to your stomach.",
        "You would only resort to such an embarrassing activity while no one was watching!!!",
        "What? No! That sounds incredibly dangerous!",
        "Now you're just being a pest. Which turnip truck did you just tumble out of, anyway?",
        "Pipe down, you. This is Rose's decision, not yours!",
        "What, you think this is some kind of game where you can just type anything and something cool happens?",
        "sigh.",
        "You briefly consider following through with this nonsense, however, even you have your limits.",
        "That would also be a preposterous waste of time!!!",
        "nah",
        "That is the sort of thing that only stupid idiots do in stupid idiot movies.",
        "You find this grisly abomination utterly detestable.",
        "This is incredibly silly, and you're not sure how it fits into your campaign.",
        "Oh God dammit, that's just what you need.",
        "You totally abjure the hell out of that idea.",
        "You refuse outright!",
        "It's just not going to happen buddy!",
        "This is COMPLETE BULLSHIT.",
        "Thank God your sanity has returned so you can entertain extremely rational, coherent thoughts like this one.",
        "This is incredibly dangerous!",
        "It is unfortunate. I guess. What are we talking about again?",
        "Seriously, just think for a second!"
    };

    public static string GetUnknownCommandResponse()
    {
        Random random = new();
        int index = random.Next(unknownCommandResponses.Length);
        return unknownCommandResponses[index];
    }
}
