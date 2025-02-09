
[System.Serializable]
public class Fact
{
    public string key { get; set; }
    public object value { get; set; }

    public override string ToString()
    {
        return key + " : " + value;
    }
}