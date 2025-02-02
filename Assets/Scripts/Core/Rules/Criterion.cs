using System;
using System.Collections.Generic;
using System.Linq;

public static class Criterion
{
    public static Fact FindFact(IEnumerable<Fact> facts, string key)
    {
        return facts.FirstOrDefault(f => f.key == key);
    }

    public static bool FactValueEquals(IEnumerable<Fact> facts, string key, object value)
    {
        Fact fact = FindFact(facts, key);
        return fact != null && fact.value.Equals(value);
    }

    public static bool FactValueGreaterThan<T>(IEnumerable<Fact> facts, string key, T value) where T : IComparable
    {
        Fact fact = FindFact(facts, key);
        return fact != null && fact.value is T typedValue && typedValue.CompareTo(value) > 0;
    }

    public static bool FactValueLessThan<T>(IEnumerable<Fact> facts, string key, T value) where T : IComparable
    {
        Fact fact = FindFact(facts, key);
        return fact != null && fact.value is T typedValue && typedValue.CompareTo(value) < 0;
    }

    public static bool FactValueGreaterThanOrEqual<T>(IEnumerable<Fact> facts, string key, T value) where T : IComparable
    {
        Fact fact = FindFact(facts, key);
        return fact != null && fact.value is T typedValue && typedValue.CompareTo(value) >= 0;
    }

    public static bool FactValueLessThanOrEqual<T>(IEnumerable<Fact> facts, string key, T value) where T : IComparable
    {
        Fact fact = FindFact(facts, key);
        return fact != null && fact.value is T typedValue && typedValue.CompareTo(value) <= 0;
    }

    public static bool FactValueNotEquals(IEnumerable<Fact> facts, string key, object value)
    {
        Fact fact = FindFact(facts, key);
        return fact != null && !fact.value.Equals(value);
    }

    public static bool FactDoesNotExist(IEnumerable<Fact> facts, string key, object value)
    {
        Fact fact = FindFact(facts, key);
        return fact == null;
    }
}
