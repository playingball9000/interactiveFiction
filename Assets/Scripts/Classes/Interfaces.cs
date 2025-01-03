public interface IClothing : IExaminable
{
    new string referenceName { get; set; }
    string description { get; set; }
    string color { get; set; }

}

public interface IExaminable
{
    string referenceName { get; set; }
    string GetDescription();
}

public interface IPlayerAction
{
    public string actionReferenceName { get; }
    public string tooFewMessage { get; }
    public string tooManyMessage { get; }
    public int minInputCount { get; }
    public int maxInputCount { get; }
    public abstract void Execute(string[] inputTextArray);
}


