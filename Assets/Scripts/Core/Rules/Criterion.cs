using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Criterion
{

    public string Description { get; }
    private readonly Func<IEnumerable<Fact>, bool> evaluator;

    public Criterion(string description, Func<IEnumerable<Fact>, bool> evaluator)
    {
        Description = description;
        this.evaluator = evaluator;
    }

    public static Criterion Create(string description, Func<IEnumerable<Fact>, bool> evaluator)
    {
        return new Criterion(description, evaluator);
    }

    public bool Evaluate(IEnumerable<Fact> facts) => evaluator(facts);

    public override string ToString()
    {
        return $"<b><color=#FFA07A>[Criterion]</color></b> {Description}";
    }

    public static Fact FindFirstFact(IEnumerable<Fact> facts, string key)
    {
        return facts.FirstOrDefault(f => f.key == key);
    }

    public static List<Fact> FindAllFacts(IEnumerable<Fact> facts, string key)
    {
        return facts.Where(f => f.key == key).ToList();
    }

    public static bool FactValueGreaterThan<T>(IEnumerable<Fact> facts, string key, T value) where T : IComparable
    {
        Fact fact = FindFirstFact(facts, key);
        return fact != null && fact.value is T typedValue && typedValue.CompareTo(value) > 0;
    }

    public static bool FactValueLessThan<T>(IEnumerable<Fact> facts, string key, T value) where T : IComparable
    {
        Fact fact = FindFirstFact(facts, key);
        return fact != null && fact.value is T typedValue && typedValue.CompareTo(value) < 0;
    }

    public static bool FactValueGreaterThanOrEqual<T>(IEnumerable<Fact> facts, string key, T value) where T : IComparable
    {
        Fact fact = FindFirstFact(facts, key);
        return fact != null && fact.value is T typedValue && typedValue.CompareTo(value) >= 0;
    }

    public static bool FactValueLessThanOrEqual<T>(IEnumerable<Fact> facts, string key, T value) where T : IComparable
    {
        Fact fact = FindFirstFact(facts, key);
        return fact != null && fact.value is T typedValue && typedValue.CompareTo(value) <= 0;
    }

    public static bool FactValueNotEquals(IEnumerable<Fact> facts, string key, object value)
    {
        Fact fact = FindFirstFact(facts, key);
        return fact != null && !fact.value.Equals(value);
    }

    // Use Exists and NotExists for checking multiple key match like in_inventory or in_room_npc where there are multiple values for a key
    public static bool FactDoesNotExist(IEnumerable<Fact> facts, string key, object value)
    {
        List<Fact> matchingFacts = FindAllFacts(facts, key);
        Fact fact = matchingFacts.FirstOrDefault(fact => fact.value.Equals(value));
        return fact == null;
    }

    public static bool FactExists(IEnumerable<Fact> facts, string key, object value)
    {
        List<Fact> matchingFacts = FindAllFacts(facts, key);
        Fact fact = matchingFacts.FirstOrDefault(fact => fact.value.Equals(value));
        return fact != null;
    }
}
