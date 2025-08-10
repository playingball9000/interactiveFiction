
public static class FactExtensions
{
    // Using FactExists here becauase you can have the same key with different values.
    public static Criterion FactExists(RuleKey key, object value)
        => Criterion.Create($"{key} == {value}", facts => Criterion.FactExists(facts, key, value));

    public static Criterion FactIsTrue(RuleKey key)
        => Criterion.Create($"{key} == true", facts => Criterion.FactExists(facts, key, true));
}
