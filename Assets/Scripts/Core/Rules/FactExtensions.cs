using System;
using System.Collections.Generic;

public static class FactExtensions
{
    // Using FactExists here becauase you can have the same key with different values.
    public static Criterion FactIsEqual(string key, object value)
        => Criterion.Create($"{key} == {value}", facts => Criterion.FactExists(facts, key, value));

    public static Criterion FactIsTrue(string key)
        => Criterion.Create($"{key} == true", facts => Criterion.FactExists(facts, key, true));
}
