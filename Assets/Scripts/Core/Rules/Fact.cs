
[System.Serializable]
public class Fact
{
    public RuleKey key { get; set; }
    public object value { get; set; }

    public static Fact Create(RuleKey key, object value)
    {
        return new Fact { key = key, value = value };
    }

    public override string ToString()
    {
        string valueType = value != null ? value.GetType().Name : "null";
        string displayValue = value != null ? value.ToString() : "<i>null</i>";

        return $"<b><color=#87CEFA>[Fact]</color></b>\n" +
               $"  • Key: <b>{key}</b>\n" +
               $"  • Value: {displayValue}\n" +
               $"  • Type: <i>{valueType}</i>";
    }
}