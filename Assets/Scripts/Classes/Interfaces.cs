using System.Collections.Generic;

public interface IClothing : IExaminable, IItem
{
    bool isWearable { get; set; }
    string color { get; set; }

}

public interface IExaminable
{
    string referenceName { get; set; }
    string adjective { get; set; }
    string description { get; set; }
    string GetDisplayName();
    string GetDescription();
}

public interface IItem : IExaminable
{
    bool isGettable { get; set; }
    string pickUpNarration { get; }
    public string internalCode { get; set; }
    //TODO: probably should contain a list of disambiguations, but priority check 1) exact match 2) partial match 3) disambiguation match
}

public interface IPlayerAction
{
    public string actionReferenceName { get; }
    public string tooFewMessage { get; }
    public string tooManyMessage { get; }
    public int minInputCount { get; }
    public int maxInputCount { get; }
    public abstract void Execute(ActionInput actionInput);
}

public interface IStorage
{
    List<IItem> contents { get; set; }

    void AddItem(IItem item);
    void RemoveItem(IItem item);
    bool ContainsItem(IItem item);
    List<IItem> GetContents();
    string ContentsToString();
    bool isAccessible();
}

public interface ILockable
{
    bool isLocked { get; set; }
}

public interface IOpenable
{
    bool isOpen { get; set; }
}