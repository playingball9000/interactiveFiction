using System.Collections.Generic;

public interface IClothing : IExaminable, IItem
{
    new string referenceName { get; set; }
    string color { get; set; }

}

public interface IExaminable
{
    string referenceName { get; set; }
    string description { get; set; }
    string GetDescription();
}

public interface IItem : IExaminable
{
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

public interface IContainer
{
    void AddItem(IItem item);
    void RemoveItem(IItem item);
    bool ContainsItem(IItem item);
    List<IItem> GetItems();
}

