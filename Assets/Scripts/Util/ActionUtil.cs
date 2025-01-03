using System;
using System.Collections.Generic;
using System.Linq;

public class ActionUtil
{
    private static ActionUtil instance;

    public static ActionUtil GetInstance()
    {
        if (instance == null)
        {
            instance = new ActionUtil();
        }
        return instance;
    }
    /**
     * Takes in a list of any type <T> and handles checking how many items
     * are in that list: 0, 1, or >1 and lets the user do appropriate actions.
     * 
     */
    public void MatchZeroOneAndMany<T>(
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

    /**
     * source - list to search 
     *  ex. player.currentLocation.npcs
     * propertySelector - field name on object to check
     *  ex. npc => npc.npcName
     * searchString - word you are looking for
     *  ex. man
     * 
     */
    public List<T> FindItemsFieldContainsString<T>(IEnumerable<T> source, Func<T, string> propertySelector, string searchString)
    {
        return source.Where(item => propertySelector(item).ToLower().Contains(searchString.ToLower())).ToList();
    }
}
