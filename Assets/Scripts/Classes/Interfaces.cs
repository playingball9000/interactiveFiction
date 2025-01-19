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
    bool isGettable { get; set; }
    string pickUpNarration { get; }
    //TODO: probably should contain a list of disambiguations, but priority check 1) exact match 2) partial match 3) disambiguation match
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

public interface IContainer : IExaminable, IItem
{
    bool isLocked { get; set; }
    bool isOpen { get; set; }
    List<IItem> contents { get; set; }


    void AddItem(IItem item);
    void RemoveItem(IItem item);
    bool ContainsItem(IItem item);
    List<IItem> GetItems();
}

